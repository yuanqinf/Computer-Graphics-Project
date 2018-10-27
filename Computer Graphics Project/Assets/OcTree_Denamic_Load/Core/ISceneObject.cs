using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Detector for the object
/// </summary>
public interface ISceneObject
{
    /// <summary>
    /// The bounding box of this object
    /// </summary>
    Bounds Bounds { get; }

    /// <summary>
    /// Show the object when this function is called
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    bool OnShow(Transform parent);

    /// <summary>
    /// Hide the object when this funciton is called
    /// </summary>
    void OnHide();

    
}

public interface ISOLinkedListNode
{
    LinkedListNode<T> GetLinkedListNode<T>() where T : ISceneObject;

    void SetLinkedListNode<T>(LinkedListNode<T> node);
}