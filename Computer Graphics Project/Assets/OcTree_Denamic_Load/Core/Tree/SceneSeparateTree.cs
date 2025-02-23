﻿using UnityEngine;
using System.Collections;

public delegate void TriggerHandle<T>(T trigger);

public enum SceneSeparateTreeType
{

    QuadTree,
}

public class SceneSeparateTree<T> where T : ISceneObject, ISOLinkedListNode
{

    public Bounds Bounds
    {
        get
        {
            if (m_Root != null)
                return m_Root.Bounds;
            return default(Bounds);
        }
    }

    public int MaxDepth
    {
        get { return m_MaxDepth; }
    }

    public SceneSeparateTreeType CurrentTreeType { get { return m_TreeType; } }

    /// <summary>
    /// The root of the quad tree
    /// </summary>
    private SceneSeparateTreeNode<T> m_Root;

    /// <summary>
    /// maximum depth so it dosen't get too deep
    /// </summary>
    private int m_MaxDepth;

    private SceneSeparateTreeType m_TreeType;

    ///Constructor
    /// <param name="treeType"></param>
    /// <param name="center"></param>
    /// <param name="size"></param>
    /// <param name="maxDepth"></param>
    public SceneSeparateTree(SceneSeparateTreeType treeType, Vector3 center, Vector3 size, int maxDepth)
    {
        this.m_TreeType = treeType;
        this.m_MaxDepth = maxDepth;
        if (treeType == SceneSeparateTreeType.QuadTree)
            this.m_Root = new SceneSeparateTreeNode<T>(new Bounds(center, size), 0, 4);
        else
            this.m_Root = new SceneSeparateTreeNode<T>(new Bounds(center, size), 0, 8);
    }

    public void Add(T item)
    {
        m_Root.Insert(item, 0, m_MaxDepth);
    }

    public void Clear()
    {
        m_Root.Clear();
    }

    public bool Contains(T item)
    {
        return m_Root.Contains(item);
    }

    public void Remove(T item)
    {
         m_Root.Remove(item);
    }

    public void Trigger(IDetector detector, TriggerHandle<T> handle)
    {
        if (handle == null)
            return;
        m_Root.Trigger(detector, handle);
    }

    public static implicit operator bool(SceneSeparateTree<T> tree)
    {
        return tree != null;
    }

#if UNITY_EDITOR
    public void DrawTree(Color treeMinDepthColor, Color treeMaxDepthColor, Color objColor, Color hitObjColor, int drawMinDepth, int drawMaxDepth, bool drawObj)
    {
        if (m_Root != null)
            m_Root.DrawNode(treeMinDepthColor, treeMaxDepthColor, objColor, hitObjColor, drawMinDepth, drawMaxDepth, drawObj, m_MaxDepth);
    }
#endif
}
