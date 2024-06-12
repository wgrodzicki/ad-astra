using UnityEngine;

/// <summary>
/// Abstract class for all objects that can be affected by interactive switches.
/// </summary>
public abstract class SwitchableObject : MonoBehaviour
{
    public bool IsSwitching { get; protected set; }
    public virtual void Switch(bool value)
    {
        IsSwitching = value;
    }
}
