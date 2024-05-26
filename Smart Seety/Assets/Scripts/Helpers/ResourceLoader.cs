using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;

public static class ResourceLoader
{
    public static List<Device> devices = GameObject.FindObjectsOfType<Device>().ToList();
    public static GameObject camera = GameObject.Find("FlyCamera");
    public static GameObject deviceAnchor = Resources.Load<GameObject>("Prefabs/DeviceAnchor");

    public static Material temperatureMaterial = new Material(Resources.Load<Material>("Materials/TemperatureMaterial"));

    public static Material greenMaterial = new Material(Resources.Load<Material>("Materials/TrafficLightGreenMaterial"));
    public static Material yellowMaterial = new Material(Resources.Load<Material>("Materials/TrafficLightYellowMaterial"));
    public static Material redMaterial = new Material(Resources.Load<Material>("Materials/TrafficLightRedMaterial"));
    public static Material onMaterial = new Material(Resources.Load<Material>("Materials/LampOnMaterial"));
    public static Material offMaterial = new Material(Resources.Load<Material>("Materials/LampOffMaterial"));

    public static List<Material> lampMaterials = new List<Material> { onMaterial, offMaterial, offMaterial };
    public static List<Material> trafficLightCarsMaterial = new List<Material> { greenMaterial, yellowMaterial, redMaterial };
    public static List<Material> trafficLightPedestriansMaterial = new List<Material> { redMaterial, redMaterial, greenMaterial };

    static ResourceLoader()
    {

    }
}
