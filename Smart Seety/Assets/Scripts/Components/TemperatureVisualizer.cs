using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TemperatureVisualizer : MonoBehaviour
{
    Temperature temperature;

    List<Renderer> renderers;
    List<Color> colors;
    Material temperatureMaterial;

    float lerp;

    public float maxVisualization = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        temperature = ((dynamic)GetComponentInParent<Device>()).temperature;

        temperatureMaterial = new Material(ResourceLoader.temperatureMaterial);

        renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        colors = new List<Color>();
        renderers.ForEach(renderer => renderer.materials.ToList().ForEach(material => {
            if (material.HasProperty("color"))
                colors.Add(new Color(material.color.r, material.color.g, material.color.b));
            else
                colors.Add(Color.white);
        }));
    }

    // Update is called once per frame
    void Update()
    {
        lerp = Mathf.Min(Mathf.Abs((float)(temperature?.sensor.valueNormal ?? 0.0f) - (float)(temperature?.sensor.value ?? 0.0f)) / Mathf.Abs((float)(temperature?.sensor.valueMaximumAllowedDifference ?? 0.0f)), maxVisualization);
        temperatureMaterial.color = ((temperature?.sensor.value ?? 0.0f) > (temperature?.sensor.valueNormal ?? 0.0f)) ? Color.red : Color.blue;
        int i = 0;
        renderers.ForEach(renderer => renderer.materials.ToList().ForEach(material => { material.color = Color.Lerp(colors[i], temperatureMaterial.color, lerp); ++i; }));
    }
}
