using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

public class GeneralDevice : Device
{
    public Gyroscope gyroscope;
    public Accelerometer accelerometer;
    public Magnetometer magnetometer;
    public Temperature temperature;
    public BarometricPressure barometricPressure;
    public Humidity humidity;
    public Joystick joystick;

    public void Start()
    {
        base.Start();
        sensors = (int)(Sensors.GYROSCOPE | Sensors.ACCELEROMETER | Sensors.MAGNETOMETER | Sensors.TEMPERATURE | Sensors.BAROMETRIC_PRESSURE | Sensors.HUMIDITY | Sensors.JOYSTICK);
    }

    public void Update()
    {
        base.Update();
    }
}
