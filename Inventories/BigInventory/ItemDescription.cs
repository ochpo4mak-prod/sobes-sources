using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class ItemDescription
{
    private string GUID = Guid.NewGuid().ToString();
    public string Name;
    public Sprite Icon;
    public Size Size;
}

[Serializable] public struct Size
{
    public int y;
    public int x;
}