using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Device
{
    private int speed = 0;

    Joystick joystickState;
    Joystick prevJoystickState;

    // Start is called before the first frame update
    void Start()
    {
        AnchorDisplacement = Vector3.up * -5;
        base.Start();
        sensors = (int)(Sensors.GYROSCOPE | Sensors.BAROMETRIC_PRESSURE | Sensors.JOYSTICK | Sensors.CAMERA);

        joystickState = new Joystick();
        prevJoystickState = new Joystick();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        joystickState.Copy(prevJoystickState);
        joystick.Copy(joystickState);

        if (prevJoystickState.up != joystickState.up && joystickState.up)
        {
            speed = Mathf.Min(++speed, 3);
        }
        if (prevJoystickState.down != joystickState.down && joystickState.down)
        {
            speed = Mathf.Max(--speed, 0);
        }

        transform.localRotation = Quaternion.Euler(new Vector3(gyroscope.sensor.value.y, gyroscope.sensor.value.x, gyroscope.sensor.value.z));
        transform.position += speed * -transform.right;
        //transform.eulerAngles = new Vector3(-gyroscope.sensor.value.y, gyroscope.sensor.value.x, 0);
    }
}
