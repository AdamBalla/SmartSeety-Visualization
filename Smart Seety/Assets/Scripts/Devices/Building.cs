using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Device
{
    private void Start()
    {
        base.Start();
        
        temperature.sensor.valueMaximumAllowedDifference = 10;

        sensors = (int)(Sensors.ACCELEROMETER | Sensors.TEMPERATURE | Sensors.BAROMETRIC_PRESSURE);
    }
}
