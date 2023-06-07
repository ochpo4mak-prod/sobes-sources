using System;
using UnityEngine;
using Unity.Mathematics;
using Assets.Scripts.Table;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TableContentViews", menuName = "ScriptableObjects/TableContentViewsObject", order = 1)]
public class TableContentViewsObject : ScriptableObject
{
    [SerializeField] private List<ContentToIconPair> contents;

    public ContentToIconPair[] Contents => contents.ToArray();
}

[Serializable]
public struct ContentToIconPair
{
    public ENodeContent Content;
    public Sprite Sprite;
    public int2 Size;
}