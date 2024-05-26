using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banner : Device
{
    Texture2D texture;
    Material material;

    byte[] image;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        sensors = (int)Sensors.DISPLAY;

        texture = new Texture2D(1920, 1080);
        material = new Material(Shader.Find("Diffuse"));
        material.mainTexture = texture;
        GetComponent<Renderer>().material = material;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (display.picture != string.Empty)
        {
            var disp = display.picture.Substring(2, display.picture.Length - 3);
            image = Convert.FromBase64String(disp);
            texture.LoadImage(image);
        }
    }
}
