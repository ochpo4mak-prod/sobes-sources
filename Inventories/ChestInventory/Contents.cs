using System;
using UnityEngine;
using Unity.Mathematics;
using Assets.Scripts.Table;

[Serializable] 
public struct ContentToIcon
{
    public EChestType Type;
    public Sprite Sprite;
    public int2 Size;
}

public readonly struct ChestContent
{
    public ChestContent(ENodeContent content, RectInt rect)
    {
        Content = content;
        Rect = rect;
    }

    public readonly ENodeContent Content;
    public readonly RectInt Rect;
}

public enum EChestType
{
    Wood,
    Iron,
    Gold
}