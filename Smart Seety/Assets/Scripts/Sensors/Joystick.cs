using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick
{
    public bool up;
    public bool left;
    public bool down;
    public bool right;
    public bool middle;

    public Joystick()
    {
        up = false;
        left = false;
        down = false;
        right = false;
        middle = false;
    }

    public void Copy(Joystick joystick)
    {
        joystick.up = up;
        joystick.left = left;
        joystick.down = down;
        joystick.right = right;
        joystick.middle = middle;
    }
}
