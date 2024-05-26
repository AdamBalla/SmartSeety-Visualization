using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humidity
{
    public Sensor1 sensor;

    public Humidity()
    {
        sensor = new Sensor1();

        sensor.valueMin = 30;
        sensor.valueNormal = 40;
        sensor.valueMax = 50;
    }
}
