using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerometer
{
    public Sensor3 sensor;

    public Accelerometer()
    {
        sensor = new Sensor3();

        sensor.valueMin = new Vector3(-2, -2, -2);
        sensor.valueNormal = Vector3.zero;
        sensor.valueMax = new Vector3(2, 2, 2);
        sensor.value = sensor.valueNormal;
    }
}