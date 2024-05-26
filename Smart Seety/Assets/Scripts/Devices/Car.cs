using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Car : Device
{
    private const float speedStep = 0.01f;
    private float speed = 0;
    private float calibration;

    Joystick joystickState;
    Joystick prevJoystickState;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        sensors = (int)(Sensors.GYROSCOPE | Sensors.JOYSTICK | Sensors.CAMERA | Sensors.MAGNETOMETER);
        gyroscope.sensor.value.z = 0;

        joystickState = new Joystick();
        prevJoystickState = new Joystick();

        calibration = 0;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        joystickState.Copy(prevJoystickState);
        joystick.Copy(joystickState);

        if (prevJoystickState.left != joystickState.left && joystickState.left)
        {
            transform.Find("LeftIndex").GetComponent<Light>().enabled = !transform.Find("LeftIndex").GetComponent<Light>().enabled;
            transform.Find("RightIndex").GetComponent<Light>().enabled = false;
        }
        if (prevJoystickState.right != joystickState.right && joystickState.right)
        {
            transform.Find("RightIndex").GetComponent<Light>().enabled = !transform.Find("RightIndex").GetComponent<Light>().enabled;
            transform.Find("LeftIndex").GetComponent<Light>().enabled = false;
        }

        if (joystick.middle)
        {
            calibration = gyroscope.sensor.value.z;
        }

        if (joystickState.up)
        {
            speed = Mathf.Min(speed += speedStep, 3);
        }
        if (joystickState.down)
        {
            speed = Mathf.Max(speed -= speedStep, -1);
        }

        if (prevJoystickState.middle != joystickState.middle && joystickState.middle)
        {
            GetComponentsInChildren<Light>(true).Where(light => light.type == LightType.Spot).ToList().ForEach(light => light.enabled = !light.enabled);
        }

        transform.localPosition = transform.localPosition + transform.forward * speed;
        transform.Rotate(Vector3.up, (gyroscope.sensor.value.z - calibration) / 50);

        if (!connected && name != "--Car--")
        {
            ResourceLoader.devices.Remove(GetComponent<Device>());
            Destroy(gameObject);
        }
    }
}
