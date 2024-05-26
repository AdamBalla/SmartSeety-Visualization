using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temperature
{
    public Sensor1 sensor;

    public Temperature()
    {
        sensor = new Sensor1();

        sensor.valueMin = 20;
        sensor.valueNormal = 30;
        sensor.valueMax = 40;
        sensor.value = sensor.valueNormal;
    }
}
