using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A simple wrapper for objects
/// </summary>
public class SceneObject : ISceneObject, ISOLinkedListNode
{
    /// <summary>
    /// Flags 
    /// </summary>
    public enum CreateFlag
    {
        /// <summary>
        /// Not created
        /// </summary>
        None,
        /// <summary>
        /// new object
        /// </summary>
        New,
        /// <summary>
        /// old object
        /// </summary>
        Old,
        /// <summary>
        /// waiting to be deleted object
        /// </summary>
        OutofBounds,
    }

    /// <summary>
    /// Flag that mark the process stage
    /// </summary>
    public enum CreatingProcessFlag
    {
        None,
        /// <summary>
        /// waiting to be loaded
        /// </summary>
        IsPrepareCreate,
        /// <summary>
        /// waiting to be destroyed
        /// </summary>
        IsPrepareDestroy,
    }

    /// <summary>
    /// Bounding box information of the object
    /// </summary>
    public Bounds Bounds
    {
        get { return m_TargetObj.Bounds; }
    }




    public ISceneObject TargetObj
    {
        get { return m_TargetObj; }
    }

    public CreateFlag Flag { get; set; }

    public CreatingProcessFlag ProcessFlag { get; set; }

    private ISceneObject m_TargetObj;



    private System.Object m_Node;

    public SceneObject(ISceneObject obj)
    {

        m_TargetObj = obj;
    }

    public LinkedListNode<T> GetLinkedListNode<T>() where T : ISceneObject
    {
        return (LinkedListNode<T>)m_Node;
    }

    public void SetLinkedListNode<T>(LinkedListNode<T> node)
    {
        m_Node = node;
    }

    public void OnHide()
    {

        m_TargetObj.OnHide();
    }

    public bool OnShow(Transform parent)
    {
        return m_TargetObj.OnShow(parent);
    }

#if UNITY_EDITOR
    public void DrawArea(Color color, Color hitColor)
    {
        if (Flag == CreateFlag.New || Flag == CreateFlag.Old)
        {
            m_TargetObj.Bounds.DrawBounds(hitColor);
        }
        else 
        m_TargetObj.Bounds.DrawBounds(color);
    }
#endif
}
