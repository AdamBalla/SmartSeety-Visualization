using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyroscope
{
    public Sensor3 sensor;

    public Gyroscope()
    {
        sensor = new Sensor3();

        sensor.value = Vector3.zero;
    }
}
