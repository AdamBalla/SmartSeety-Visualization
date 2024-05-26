using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : Device
{
    // Start is called before the first frame update
    void Start()
    {
        AnchorDisplacement = new Vector3(-10, 0, +5f);
        base.Start();
        sensors = (int)(Sensors.HUMIDITY | Sensors.TEMPERATURE);
    }
}
