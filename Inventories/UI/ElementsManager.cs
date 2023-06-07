using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IElement
{
    int Id { get; }
    string ItemName { get; }
}

interface IElementsManager
{
    IElement[] Elements { get; }
    event Action<IElement> ElementAddedEvent;
    event Action<IElement> ElementRemovedEvent;
}

public class ElementsManager : IElementsManager
{
    public IElement[] Elements { get; }
    public event Action<IElement> ElementAddedEvent;
    public event Action<IElement> ElementRemovedEvent;

    private IList<IElement> _elements;

    void AddItem(IElement element)
    {

    }

    void RemoveItem(IElement element)
    {

    }
}