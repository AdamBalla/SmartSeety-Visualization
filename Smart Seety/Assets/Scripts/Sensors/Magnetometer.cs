using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetometer
{
    public Sensor1 sensor;

    public Magnetometer()
    {
        sensor = new Sensor1();

        sensor.value = 0;
    }
}
