using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrafficLightPost : Device
{
    List<Transform> lightTransforms;
    List<Transform> lampTransforms;
    List<Transform> trafficLightCarsTransforms;
    List<Transform> trafficLightCarsInverseTransforms;
    List<Transform> trafficLightPedestriansTransforms;
    List<Transform> trafficLightPedestriansInverseTransforms;

    Joystick joystickState;
    Joystick prevJoystickState;

    int trafficLightState;
    bool lampState;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        sensors = (int)Sensors.JOYSTICK;

        lightTransforms = GetComponentsInChildren<Transform>(true).Where(t => t.name == "light").ToList();
        lampTransforms = GetComponentsInChildren<Transform>(true).Where(t => t.name == "lamp").ToList();
        trafficLightCarsTransforms = GetComponentsInChildren<Transform>(true).Where(t => t.name == "trafficLightCars").ToList();
        trafficLightCarsInverseTransforms = GetComponentsInChildren<Transform>(true).Where(t => t.name == "trafficLightCarsInverse").ToList();
        trafficLightPedestriansTransforms = GetComponentsInChildren<Transform>(true).Where(t => t.name == "trafficLightPedestrians").ToList();
        trafficLightPedestriansInverseTransforms = GetComponentsInChildren<Transform>(true).Where(t => t.name == "trafficLightPedestriansInverse").ToList();


        joystickState = new Joystick();
        prevJoystickState = new Joystick();
        joystick.Copy(joystickState);
        joystick.Copy(prevJoystickState);

        trafficLightState = 0;
        lampState = true;
        ChangeMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        joystickState.Copy(prevJoystickState);
        joystick.Copy(joystickState);
        if (prevJoystickState.middle != joystickState.middle && joystickState.middle)
        {
            lampState = !lampState;
            lightTransforms.ForEach(t => t.gameObject.SetActive(lampState));
            lampTransforms.ForEach(t => t.gameObject.GetComponent<Renderer>().material = lampState ? ResourceLoader.onMaterial : ResourceLoader.offMaterial);
        }

        if (prevJoystickState.up != joystickState.up && joystickState.up)
        {
            trafficLightState = ++trafficLightState % 3;
            ChangeMaterial();
        }

        if (prevJoystickState.down != joystickState.down && joystickState.down)
        {
            trafficLightState = --trafficLightState % 3;
            if (trafficLightState < 0)
                trafficLightState = 3 + trafficLightState;
            ChangeMaterial();
        }
    }

    private void ChangeMaterial()
    {
        trafficLightCarsTransforms.ForEach(t =>
        {
            t.gameObject.GetComponent<Renderer>().material = ResourceLoader.trafficLightCarsMaterial.ElementAt(trafficLightState);
        });
        trafficLightCarsInverseTransforms.ForEach(t =>
        {
            t.gameObject.GetComponent<Renderer>().material = ResourceLoader.trafficLightCarsMaterial.ElementAt(2 - trafficLightState);
        });
        trafficLightPedestriansTransforms.ForEach(t =>
        {
            t.gameObject.GetComponent<Renderer>().material = ResourceLoader.trafficLightPedestriansMaterial.ElementAt(trafficLightState);
        });
        trafficLightPedestriansInverseTransforms.ForEach(t =>
        {
            t.gameObject.GetComponent<Renderer>().material = ResourceLoader.trafficLightPedestriansMaterial.ElementAt(2 - trafficLightState);
        });
    }
}
