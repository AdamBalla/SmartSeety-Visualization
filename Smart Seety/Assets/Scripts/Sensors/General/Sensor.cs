using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Sensor<T>
{
    public T value;
    public T valueMin;
    public T valueNormal;
    public T valueMax;
    public T valueMaximumAllowedDifference;
}
