using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Monitor : MonoBehaviour
{
    const string NA = "---";
    const float graphTop = 175;
    const float graphSpacing = 250;
    RenderTexture rt;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject go = ResourceLoader.camera.GetComponent<FlyCamera>().selectedObject;
        if (go != null)
        {
            float graphDisplacement = graphTop;

            Device sd = go.GetComponentInParent<Device>();
            int s = sd.sensors;

            var graphs = transform.parent.Find("Graphs");

            var gyroscopeGraph = graphs.Find("GyroscopeGraph");
            bool hasGyroscope = (s & (int)Sensors.GYROSCOPE) == (int)Sensors.GYROSCOPE;
            gyroscopeGraph.gameObject.SetActive(hasGyroscope);
            if (hasGyroscope)
            {
                var pos = gyroscopeGraph.transform.localPosition;
                gyroscopeGraph.transform.localPosition = new Vector3(pos.x, graphDisplacement, pos.z);
                var series = gyroscopeGraph.Find("Series");
                var series1 = series.Find("Series1").GetComponent<WMG_Series>();
                var series2 = series.Find("Series2").GetComponent<WMG_Series>();
                var series3 = series.Find("Series3").GetComponent<WMG_Series>();
                series1.pointValues = sd.gyroscopeSeries.Select(ss => ss.x).Zip(sd.timeSeries, (a, b) => new Vector2(b, a)).ToList();
                series2.pointValues = sd.gyroscopeSeries.Select(ss => ss.y).Zip(sd.timeSeries, (a, b) => new Vector2(b, a)).ToList();
                series3.pointValues = sd.gyroscopeSeries.Select(ss => ss.z).Zip(sd.timeSeries, (a, b) => new Vector2(b, a)).ToList();
                series1.seriesName = "X: " + sd.gyroscope.sensor.value.x.ToString();
                series2.seriesName = "Y: " + sd.gyroscope.sensor.value.y.ToString();
                series3.seriesName = "Z: " + sd.gyroscope.sensor.value.z.ToString();

                graphDisplacement -= graphSpacing;
            }
            else
                ;

            var accelerometerGraph = graphs.Find("AccelerometerGraph");
            bool hasAccelerometer = (s & (int)Sensors.ACCELEROMETER) == (int)Sensors.ACCELEROMETER;
            accelerometerGraph.gameObject.SetActive(hasAccelerometer);
            if (hasAccelerometer)
            {
                var pos = accelerometerGraph.transform.localPosition;
                accelerometerGraph.transform.localPosition = new Vector3(pos.x, graphDisplacement, pos.z);
                var series = accelerometerGraph.Find("Series");
                var series1 = series.Find("Series1").GetComponent<WMG_Series>();
                var series2 = series.Find("Series2").GetComponent<WMG_Series>();
                var series3 = series.Find("Series3").GetComponent<WMG_Series>();
                series1.pointValues = sd.accelerometerSeries.Select(ss => ss.x).Zip(sd.timeSeries, (a, b) => new Vector2(b, a)).ToList();
                series2.pointValues = sd.accelerometerSeries.Select(ss => ss.y).Zip(sd.timeSeries, (a, b) => new Vector2(b, a)).ToList();
                series3.pointValues = sd.accelerometerSeries.Select(ss => ss.z).Zip(sd.timeSeries, (a, b) => new Vector2(b, a)).ToList();
                series1.seriesName = "X: " + sd.accelerometer.sensor.value.x.ToString();
                series2.seriesName = "Y: " + sd.accelerometer.sensor.value.y.ToString();
                series3.seriesName = "Z: " + sd.accelerometer.sensor.value.z.ToString();

                graphDisplacement -= graphSpacing;
            }
            else
                ;

            var magnetometerGraph = graphs.Find("MagnetometerGraph");
            bool hasMagnetometer = (s & (int)Sensors.MAGNETOMETER) == (int)Sensors.MAGNETOMETER;
            magnetometerGraph.gameObject.SetActive(hasMagnetometer);
            if (hasMagnetometer)
            {
                var pos = magnetometerGraph.transform.localPosition;
                magnetometerGraph.transform.localPosition = new Vector3(pos.x, graphDisplacement, pos.z);
                var series = magnetometerGraph.Find("Series").GetComponent<WMG_Series>();
                series.pointValues = sd.magnetometerSeries.Zip(sd.timeSeries, (a, b) => new Vector2(b, a)).ToList();
                series.seriesName = "Value: " + sd.magnetometer.sensor.value.ToString();

                graphDisplacement -= graphSpacing;
            }
            else
                ;

            var temperatureGraph = graphs.Find("TemperatureGraph");
            bool hasTemperature = (s & (int)Sensors.TEMPERATURE) == (int)Sensors.TEMPERATURE;
            temperatureGraph.gameObject.SetActive(hasTemperature);
            if (hasTemperature)
            {
                var pos = temperatureGraph.transform.localPosition;
                temperatureGraph.transform.localPosition = new Vector3(pos.x, graphDisplacement, pos.z);
                var series = temperatureGraph.Find("Series").GetComponent<WMG_Series>();
                series.pointValues = sd.temperatureSeries.Zip(sd.timeSeries, (a, b) => new Vector2(b, a)).ToList();
                series.seriesName = "Value: " + sd.temperature.sensor.value.ToString();

                graphDisplacement -= graphSpacing;
            }
            else
                ;

            var barometricPressureGraph = graphs.Find("BarometricPressureGraph");
            bool hasBarometricPressure = (s & (int)Sensors.BAROMETRIC_PRESSURE) == (int)Sensors.BAROMETRIC_PRESSURE;
            barometricPressureGraph.gameObject.SetActive(hasBarometricPressure);
            if (hasBarometricPressure)
            {
                var pos = barometricPressureGraph.transform.localPosition;
                barometricPressureGraph.transform.localPosition = new Vector3(pos.x, graphDisplacement, pos.z);
                var series = barometricPressureGraph.Find("Series").GetComponent<WMG_Series>();
                series.pointValues = sd.barometricPressureSeries.Zip(sd.timeSeries, (a, b) => new Vector2(b, a)).ToList();
                series.seriesName = "Value: " + sd.barometricPressure.sensor.value.ToString();

                graphDisplacement -= graphSpacing;
            }
            else
                ;

            var humidityGraph = graphs.Find("HumidityGraph");
            bool hasHumidity = (s & (int)Sensors.HUMIDITY) == (int)Sensors.HUMIDITY;
            humidityGraph.gameObject.SetActive(hasHumidity);
            if (hasHumidity)
            {
                var pos = humidityGraph.transform.localPosition;
                humidityGraph.transform.localPosition = new Vector3(pos.x, graphDisplacement, pos.z);
                var series = humidityGraph.Find("Series").GetComponent<WMG_Series>();
                series.pointValues = sd.humiditySeries.Zip(sd.timeSeries, (a, b) => new Vector2(b, a)).ToList();
                series.seriesName = "Value: " + sd.humidity.sensor.value.ToString();

                graphDisplacement -= graphSpacing;
            }
            else
                ;

            
            var joystick = graphs.Find("Joystick");
            joystick.gameObject.SetActive((s & (int)Sensors.JOYSTICK) == (int)Sensors.JOYSTICK);
            if ((s & (int)Sensors.JOYSTICK) == (int)Sensors.JOYSTICK)
            {
                var pos = joystick.transform.localPosition;
                joystick.transform.localPosition = new Vector3(pos.x, graphDisplacement, pos.z);
                joystick.Find("Up").GetComponent<TextMesh>().text = sd.joystick.up ? "●" : "○";
                joystick.Find("Middle").GetComponent<TextMesh>().text = sd.joystick.middle ? "●" : "○";
                joystick.Find("Down").GetComponent<TextMesh>().text = sd.joystick.down ? "●" : "○";
                joystick.Find("Left").GetComponent<TextMesh>().text = sd.joystick.left ? "●" : "○";
                joystick.Find("Right").GetComponent<TextMesh>().text = sd.joystick.right ? "●" : "○";
            }
            else
                ;

            if ((s & (int)Sensors.CAMERA) == (int)Sensors.CAMERA)
            {
                transform.Find("Sensors").Find("Preview").gameObject.SetActive(true && sd.GetComponentInChildren<Camera>().targetTexture != null);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (sd.GetComponentInChildren<Camera>().targetTexture != null)
                    {
                        rt = sd.GetComponentInChildren<Camera>().targetTexture;
                        sd.GetComponentInChildren<Camera>().targetTexture = null;
                        transform.Find("Sensors").Find("Preview").gameObject.SetActive(false);
                    }
                    else
                    {
                        sd.GetComponentInChildren<Camera>().targetTexture = rt;
                        transform.Find("Sensors").Find("Preview").gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                transform.Find("Sensors").Find("Preview").gameObject.SetActive(false);
            }
        }
    }
}
