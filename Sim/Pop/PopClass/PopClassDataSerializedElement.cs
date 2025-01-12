using System;
using UnityEngine;

[Serializable]
public struct PopClassDataSerializedElement
{
    public PopClass Class;

    public string Name;
    public Sprite Icon;
    [TextArea] public string Description;
}