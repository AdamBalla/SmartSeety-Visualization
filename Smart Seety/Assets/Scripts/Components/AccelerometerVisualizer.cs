using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerVisualizer : MonoBehaviour
{
    private Accelerometer accelerometer;

    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        accelerometer = ((dynamic)GetComponentInParent<Device>()).accelerometer;
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(accelerometer.sensor.value);
    }
}
