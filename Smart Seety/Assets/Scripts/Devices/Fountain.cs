using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : Device
{
    bool state, prevState;

    List<ParticleSystem> particleSystems;
    List<Light> lights;
    List<AudioSource> audioSources;

    // Start is called before the first frame update
    void Start()
    {
        sensors = (int)Sensors.JOYSTICK;
        base.Start();
        AnchorDisplacement = new Vector3(-80, -20, 0);
        joystick = new Joystick();

        state = false;
        prevState = false;

        particleSystems = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());
        lights = new List<Light>(GetComponentsInChildren<Light>());
        audioSources = new List<AudioSource>(GetComponentsInChildren<AudioSource>());
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        prevState = state;
        state = joystick.middle;
        if (prevState != state && state)
        {
            particleSystems.ForEach(particleSystem =>
            {
                if (particleSystem.isPlaying)
                    particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
                else
                    particleSystem.Play();
            });
            audioSources.ForEach(audioSource => audioSource.enabled = !audioSource.enabled);
            lights.ForEach(light => light.enabled = !light.enabled);
        }
    }

    void Switch()
    {
        state = !state;
    }
}
