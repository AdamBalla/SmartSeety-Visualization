using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Newtonsoft.Json;
using System.Dynamic;
//using Firebase.Database;
//using Firebase.Database.Query;
using System.Threading;
using System.Net.Http;

[Flags]
public enum Sensors
{
    NONE = 0,
    GYROSCOPE = 1,
    ACCELEROMETER = 2,
    MAGNETOMETER = 4,
    TEMPERATURE = 8,
    BAROMETRIC_PRESSURE = 16,
    HUMIDITY = 32,
    JOYSTICK = 64,
    CAMERA = 128,
    DISPLAY = 256
}

public enum Statuses
{
    NONE = 0,
    DISCONNECTED = 1,
    CONNECTED = 2,
    HIGH_ACCELERATION = 4,
    LOW_TEMPERATURE = 8,
    HIGH_TEMPERATURE = 16,
    LOW_BAROMETRICPRESSURE = 32,
    HIGH_BAROMETRICPRESSURE = 64,
    LOW_HUMIDITY = 128,
    HIGH_HUMIDITY = 256
}

public struct RGB
{
    public float r;
    public float b;
    public float g;

    public RGB(float r, float g, float b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }
}

public struct Status
{
    public int code;
    public RGB color;
    public string message;

    public Status(int code, RGB color, string message)
    {
        this.code = code;
        this.color = color;
        this.message = message;
    }
}

public class Device : MonoBehaviour
{
    Timer timer;

    [HideInInspector]
    public Gyroscope gyroscope = new Gyroscope();
    [HideInInspector]
    public Accelerometer accelerometer = new Accelerometer();
    [HideInInspector]
    public Magnetometer magnetometer = new Magnetometer();
    [HideInInspector]
    public Temperature temperature = new Temperature();
    [HideInInspector]
    public BarometricPressure barometricPressure = new BarometricPressure();
    [HideInInspector]
    public Humidity humidity = new Humidity();
    [HideInInspector]
    public Joystick joystick = new Joystick();
    [HideInInspector]
    public Camera0 camera0 = new Camera0();
    [HideInInspector]
    public Display0 display = new Display0();

    public List<Vector3> gyroscopeSeries;
    public List<Vector3> accelerometerSeries;
    public List<float> magnetometerSeries;
    public List<float> temperatureSeries;
    public List<float> barometricPressureSeries;
    public List<float> humiditySeries;

    public List<int> timeSeries;

    private const int seriesCapacity = 25;

    public Vector3 AnchorDisplacement;
    public bool connected = false;
    public bool selected = false;
    public int countDown = 100;

    private int statusCode;
    private HashSet<int> statusCodes = new HashSet<int>();
    private List<Status> statuses;

    [HideInInspector]
    public int sensors = (int)Sensors.NONE;

    GameObject DeviceAnchor;

    // Start is called before the first frame update
    public void Start()
    {
        timer = new Timer(ClearList);
        timer.Change(0, 15000);

        statusCode = (int)Statuses.DISCONNECTED;
        
        gyroscope = new Gyroscope();
        accelerometer = new Accelerometer();
        magnetometer = new Magnetometer();
        temperature = new Temperature();
        barometricPressure = new BarometricPressure();
        humidity = new Humidity();
        joystick = new Joystick();
        camera0 = new Camera0();
        display = new Display0();

        timeSeries = Enumerable.Repeat(0, seriesCapacity).ToList();
        gyroscopeSeries = Enumerable.Repeat(gyroscope.sensor.value, seriesCapacity).ToList();
        accelerometerSeries = Enumerable.Repeat(accelerometer.sensor.value, seriesCapacity).ToList();
        magnetometerSeries = Enumerable.Repeat(magnetometer.sensor.value, seriesCapacity).ToList();
        temperatureSeries = Enumerable.Repeat(temperature.sensor.value, seriesCapacity).ToList();
        barometricPressureSeries = Enumerable.Repeat(barometricPressure.sensor.value, seriesCapacity).ToList();
        humiditySeries = Enumerable.Repeat(humidity.sensor.value, seriesCapacity).ToList();

        DeviceAnchor = Instantiate<GameObject>(ResourceLoader.deviceAnchor, transform);
        DeviceAnchor.GetComponentInChildren<TextMesh>().text = transform.name;
        DeviceAnchor.name = "Anchor";
        Vector3 BoundMin;
        Vector3 BoundMax;
        Vector3 pos = Vector3.zero;
        float size = 0.0f;
        if (transform.Find("AnchorPlaceholder") == null)
        {
            FindBoundaries(out BoundMin, out BoundMax);
            size = (BoundMax.y - BoundMin.y) / 10;
            pos = new Vector3(0, BoundMax.y, 0) + AnchorDisplacement;
        }
        else
        {
            pos = transform.Find("AnchorPlaceholder").localPosition;
            size = transform.Find("AnchorPlaceholder").localScale.y;
        }
        
        DeviceAnchor.transform.localPosition = pos;
        DeviceAnchor.transform.localScale = new Vector3(size, size, size);

        statuses = new List<Status>()
        {
            new Status((int)Statuses.DISCONNECTED, new RGB(1, 0, 0), "Disc."),
            new Status((int)Statuses.CONNECTED, new RGB(0, 1, 0), "Conn."),
            new Status((int)Statuses.HIGH_ACCELERATION, new RGB(0, 0, 1), "H Acc."),
            new Status((int)Statuses.LOW_TEMPERATURE, new RGB(1, 0, 1), "L Tem."),
            new Status((int)Statuses.HIGH_TEMPERATURE, new RGB(0, 1, 1), "H Tem."),
            new Status((int)Statuses.LOW_BAROMETRICPRESSURE, new RGB(0.5f, 0, 0.5f), "L Pre."),
            new Status((int)Statuses.HIGH_BAROMETRICPRESSURE, new RGB(0, 0.5f, 0.5f), "H Pre."),
            new Status((int)Statuses.LOW_HUMIDITY, new RGB(0.5f, 0, 0.25f), "L Hum."),
            new Status((int)Statuses.HIGH_HUMIDITY, new RGB(0, 0.5f, 0.25f), "H Hum.")
        };
    }

    // Update is called once per frame
    public void Update()
    {
        DeviceAnchor.transform.LookAt(ResourceLoader.camera.transform);

        if (countDown == 0)
        {
            connected = false;
            SetStatus((int)Statuses.DISCONNECTED);
        }
        countDown = Mathf.Max(-1, --countDown);

        Color anchorColor = selected ? Color.blue : (connected ? Color.green : Color.red);
        DeviceAnchor.GetComponentsInChildren<Renderer>().ToList().ForEach(r => r.material.color = anchorColor);
    }

    private void FindBoundaries(out Vector3 boundMin, out Vector3 boundMax)
    {
        Vector3 min = Vector3.positiveInfinity;
        Vector3 max = Vector3.negativeInfinity;
        GetComponentsInChildren<Renderer>().ToList().ForEach(c =>
        {
            if (c.bounds.min.x < min.x) min.x = c.bounds.min.x;
            if (c.bounds.min.y < min.y) min.y = c.bounds.min.y;
            if (c.bounds.min.z < min.z) min.z = c.bounds.min.z;
            if (c.bounds.max.x > max.x) max.x = c.bounds.max.x;
            if (c.bounds.max.y > max.y) max.y = c.bounds.max.y;
            if (c.bounds.max.z > max.z) max.z = c.bounds.max.z;
        });

        boundMin = min;
        boundMax = max;
    }

    public void SetValues(Device copyDevice)
    {
        if (!connected)
        {
            connected = true;
            SetStatus((int)Statuses.CONNECTED);
        }
        countDown = 100;

        if ((sensors & (int)Sensors.GYROSCOPE) == (int)Sensors.GYROSCOPE)
        {
            gyroscope.sensor.value = copyDevice.gyroscope.sensor.value;

            gyroscopeSeries.Add(gyroscope.sensor.value);
            gyroscopeSeries.RemoveAt(0);
        }

        if ((sensors & (int)Sensors.ACCELEROMETER) == (int)Sensors.ACCELEROMETER)
        {
            accelerometer.sensor.value = copyDevice.accelerometer.sensor.value;

            if (accelerometer.sensor.value.sqrMagnitude > accelerometer.sensor.valueMax.sqrMagnitude)
            {
                SetStatus((int)Statuses.HIGH_ACCELERATION);
            }

            accelerometerSeries.Add(accelerometer.sensor.value);
            accelerometerSeries.RemoveAt(0);
        }

        if ((sensors & (int)Sensors.MAGNETOMETER) == (int)Sensors.MAGNETOMETER)
        {
            magnetometer.sensor.value = copyDevice.magnetometer.sensor.value;

            magnetometerSeries.Add(magnetometer.sensor.value);
            magnetometerSeries.RemoveAt(0);
        }

        if ((sensors & (int)Sensors.TEMPERATURE) == (int)Sensors.TEMPERATURE)
        {
            temperature.sensor.value = copyDevice.temperature.sensor.value;

            if (temperature.sensor.value > temperature.sensor.valueMax)
            {
                SetStatus((int)Statuses.HIGH_TEMPERATURE);
            }
            else if(temperature.sensor.value < temperature.sensor.valueMin)
            {
                SetStatus((int)Statuses.LOW_TEMPERATURE);
            }

            temperatureSeries.Add(temperature.sensor.value);
            temperatureSeries.RemoveAt(0);
        }

        if ((sensors & (int)Sensors.BAROMETRIC_PRESSURE) == (int)Sensors.BAROMETRIC_PRESSURE)
        {
            barometricPressure.sensor.value = copyDevice.barometricPressure.sensor.value;

            if (barometricPressure.sensor.value > barometricPressure.sensor.valueMax)
            {
                SetStatus((int)Statuses.HIGH_BAROMETRICPRESSURE);
            }
            else if (barometricPressure.sensor.value < barometricPressure.sensor.valueMin)
            {
                SetStatus((int)Statuses.LOW_TEMPERATURE);
            }

            barometricPressureSeries.Add(barometricPressure.sensor.value);
            barometricPressureSeries.RemoveAt(0);
        }

        if ((sensors & (int)Sensors.HUMIDITY) == (int)Sensors.HUMIDITY)
        {
            humidity.sensor.value = copyDevice.humidity.sensor.value;

            if (humidity.sensor.value > humidity.sensor.valueMax)
            {
                SetStatus((int)Statuses.HIGH_HUMIDITY);
            }
            else if (humidity.sensor.value < humidity.sensor.valueMin)
            {
                SetStatus((int)Statuses.LOW_HUMIDITY);
            }

            humiditySeries.Add(humidity.sensor.value);
            humiditySeries.RemoveAt(0);
        }

        if ((sensors & (int)Sensors.JOYSTICK) == (int)Sensors.JOYSTICK)
        {
            joystick = copyDevice.joystick;
        }

        if ((sensors & (int)Sensors.DISPLAY) == (int)Sensors.DISPLAY)
        {
            display = copyDevice.display;
        }

        SetStatus((int)Statuses.CONNECTED);
    }

    public Status GetStatus()
    {
        return statuses.Where(s => s.code == statusCode).FirstOrDefault();
    }

    async private void SetStatus(int newStatusCode)
    {
        statusCode = newStatusCode;

        if (!statusCodes.Contains(statusCode) && InitilaizingDataBase.client != null)
        {
            HttpResponseMessage response = await InitilaizingDataBase.client.PutAsJsonAsync(InitilaizingDataBase.DATABASE_URL + "devices/" + gameObject.name + "/status/.json", GetStatus());
        }

        statusCodes.Add(statusCode);
    }

    private void ClearList(object a)
    {
        statusCodes.Clear();
    }
}
