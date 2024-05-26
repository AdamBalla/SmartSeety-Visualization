using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarometricPressure
{
    public Sensor1 sensor;

    public BarometricPressure()
    {
        sensor = new Sensor1();

        sensor.valueMin = 950;
        sensor.valueNormal = 1000;
        sensor.valueMax = 1050;
        sensor.value = sensor.valueNormal;
    }
}
