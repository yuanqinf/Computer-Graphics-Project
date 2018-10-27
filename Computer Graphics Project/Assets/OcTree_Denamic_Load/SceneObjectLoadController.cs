using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controller to load objects into scene
/// </summary>
public class SceneObjectLoadController : MonoBehaviour
{

    private WaitForEndOfFrame m_WaitForFrame;

    
    private SceneSeparateTree<SceneObject> m_QuadTree;

    /// <summary>
    /// refresh technique
    /// only update when it is the refreshtime.
    /// </summary>
    private float m_RefreshTime;


    private float m_DestroyRefreshTime;
    
    private Vector3 m_OldRefreshPosition;
    private Vector3 m_OldDestroyRefreshPosition;

    /// <summary>
    /// loaded objects
    /// </summary>
    private List<SceneObject> m_LoadedObjectList;

    /// <summary>
    /// a queue where the objects are waiting to be deleted
    /// </summary>
    //private Queue<SceneObject> m_PreDestroyObjectQueue;
    private Queue<SceneObject> m_PreDestroyObjectQueue;

    private TriggerHandle<SceneObject> m_TriggerHandle;

    private bool m_IsTaskRunning;

    private bool m_IsInitialized;

    private int m_MaxCreateCount;
    private int m_MinCreateCount;
    private float m_MaxRefreshTime;
    private float m_MaxDestroyTime;
    private bool m_Asyn;

    private IDetector m_CurrentDetector;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="center">character bounding box center</param>
    /// <param name="size">size of the bounding box</param>
    /// <param name="maxCreateCount"></param>
    /// <param name="minCreateCount"></param>
    /// <param name="maxRefreshTime"></param>
    /// <param name="maxDestroyTime"></param>
    /// <param name="quadTreeDepth"></param>
    public void Init(Vector3 center, Vector3 size, int maxCreateCount, int minCreateCount, float maxRefreshTime, float maxDestroyTime, SceneSeparateTreeType treeType , int quadTreeDepth = 5)
    {
        if (m_IsInitialized)
            return;
        m_QuadTree = new SceneSeparateTree<SceneObject>(treeType, center, size, quadTreeDepth);
        m_LoadedObjectList = new List<SceneObject>();
        //m_PreDestroyObjectQueue = new Queue<SceneObject>();
        m_PreDestroyObjectQueue = new Queue<SceneObject>();
        m_TriggerHandle = new TriggerHandle<SceneObject>(this.TriggerHandle); 

        m_MaxCreateCount = Mathf.Max(0, maxCreateCount);
        m_MinCreateCount = Mathf.Clamp(minCreateCount, 0, m_MaxCreateCount);
        m_MaxRefreshTime = maxRefreshTime;
        m_MaxDestroyTime = maxDestroyTime;
       

        m_IsInitialized = true;

        m_RefreshTime = maxRefreshTime;
    }


    /// <param name="center"></param>
    /// <param name="size"></param>
    public void Init(Vector3 center, Vector3 size, SceneSeparateTreeType treeType)
    {
        Init(center, size,20, 15, 1, 1, treeType);

    }


    void OnDestroy()
    {
        if (m_QuadTree)
            m_QuadTree.Clear();
        m_QuadTree = null;

        if (m_LoadedObjectList != null)
            m_LoadedObjectList.Clear();
        
        m_LoadedObjectList = null;
        m_TriggerHandle = null;
    }

    /// <summary>
    /// Add objects into the quad tree
    /// </summary>
    /// <param name="obj"></param>
    public void AddSceneBlockObject(ISceneObject obj)
    {
        if (!m_IsInitialized)
            return;
        if (m_QuadTree == null)
            return;
        if (obj == null)
            return;
       
        SceneObject sbobj = new SceneObject(obj);
        m_QuadTree.Add(sbobj);
        // Create the object 
        if (m_CurrentDetector != null && m_CurrentDetector.IsDetected(sbobj.Bounds))
        {
            // add to the object loaded list
            m_LoadedObjectList.Add(sbobj);
            // create the object
            CreateObject(sbobj);

        }
    }

    /// <summary>
    /// Refresh detector
    /// </summary>
    /// <param name="detector">the detector</param>
    public void RefreshDetector(IDetector detector)
    {
        if (!m_IsInitialized)
            return;
        // only when the position changes, it refreshes
        if (m_OldRefreshPosition != detector.Position)
        {
            m_RefreshTime += Time.deltaTime;
            //Also the refresh interval needs to be reached too
            if (m_RefreshTime > m_MaxRefreshTime)
            {
                m_OldRefreshPosition = detector.Position;
                m_RefreshTime = 0;
                m_CurrentDetector = detector;
                // detect this character bounding box with the nodes in quadtree
                m_QuadTree.Trigger(detector, m_TriggerHandle);
                //mark the out of bound objects
                MarkOutofBoundsObjs();
                
            }
            if (m_PreDestroyObjectQueue != null)
            //if (m_PreDestroyObjectList != null && m_PreDestroyObjectList.Count >= m_MaxCreateCount)
            {
                m_DestroyRefreshTime += Time.deltaTime;
                if (m_DestroyRefreshTime > m_MaxDestroyTime)
                {
                    m_OldDestroyRefreshPosition = detector.Position;
                    m_DestroyRefreshTime = 0;
                    //Delete out of bound objects
                    DestroyOutOfBoundsObjs();
                }
            }
        }

    }

    /// <summary>
    /// Trigger handle for quadtree
    /// </summary>
    /// <param name="data">objects that intersect current character bounding box</param>
    void TriggerHandle(SceneObject data)
    {
        if (data == null)
            return;
        if (data.Flag == SceneObject.CreateFlag.Old) //set its flag to new
        {
            data.Flag = SceneObject.CreateFlag.New;
        }
        else if (data.Flag == SceneObject.CreateFlag.OutofBounds)//If it is in the out of bound list, then we set it to new, and add it to the object list.
        {
            data.Flag = SceneObject.CreateFlag.New;
            //if (m_PreDestroyObjectList.Remove(data))
            {
                m_LoadedObjectList.Add(data);
            }
        }
        else if (data.Flag == SceneObject.CreateFlag.None) //If it is none, then we create it
        {
            // add to the object loaded list
            m_LoadedObjectList.Add(data);
            // create the object
            CreateObject(data);
        }
    }




    /// <summary>
    /// Mark the out of bound object
    /// </summary>
    void MarkOutofBoundsObjs()
    {
        if (m_LoadedObjectList == null)
            return;
        int i = 0;
        while (i < m_LoadedObjectList.Count)
        {
            if (m_LoadedObjectList[i].Flag == SceneObject.CreateFlag.Old)//If it is old, that means it doesn't intersect with the bounding box.
            {
                m_LoadedObjectList[i].Flag = SceneObject.CreateFlag.OutofBounds;
                //m_PreDestroyObjectList.Add(m_LoadedObjectList[i]);
                if (m_MinCreateCount == 0)//
                {
                    DestroyObject(m_LoadedObjectList[i]);
                }
                else
                {
                    //m_PreDestroyObjectQueue.Enqueue(m_LoadedObjectList[i]);
                    m_PreDestroyObjectQueue.Enqueue(m_LoadedObjectList[i]);//加add to the object load list
                }
                m_LoadedObjectList.RemoveAt(i);

            }
            else
            {
                m_LoadedObjectList[i].Flag = SceneObject.CreateFlag.Old;// flag everything else as old
                i++;
            }
        }
    }

    /// <summary>
    /// Delete out of bound objects
    /// </summary>
    void DestroyOutOfBoundsObjs()
    {
        while(m_PreDestroyObjectQueue.Count>m_MinCreateCount)
        {

            //var obj = m_PreDestroyObjectQueue.Dequeue();
            var obj = m_PreDestroyObjectQueue.Dequeue();
            if (obj == null)
                continue;
            if (obj.Flag == SceneObject.CreateFlag.OutofBounds)
            {
                DestroyObject(obj);
            }
        }
    }

    /// <summary>
    /// Create an object in the controller, so that it will be displayed
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="asyn"></param>
    private void CreateObject(SceneObject obj)
    {
        if (obj == null)
            return;
        if (obj.TargetObj == null)
            return;
        if (obj.Flag == SceneObject.CreateFlag.None)
        {
            if (obj.ProcessFlag == SceneObject.CreatingProcessFlag.IsPrepareDestroy)// If the object IsPrepareDestroy, then we change its flag to None
            {
                obj.ProcessFlag = SceneObject.CreatingProcessFlag.None;
                return;
            }
            obj.OnShow(transform);

            obj.Flag = SceneObject.CreateFlag.New;// falg it to new
        }
    }

    /// <summary>
    /// Delete an object
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="asyn"></param>
    private void DestroyObject(SceneObject obj)
    {
        if (obj == null)
            return;
        if (obj.Flag == SceneObject.CreateFlag.None)
            return;
        if (obj.TargetObj == null)
            return;

        if (obj.ProcessFlag == SceneObject.CreatingProcessFlag.IsPrepareCreate)//If it is IsprepareCreate, set it to None
        {
            obj.ProcessFlag = SceneObject.CreatingProcessFlag.None;
            return;
        }
        obj.OnHide();// run OnHide

        obj.Flag = SceneObject.CreateFlag.None;//Deleted object has None flag
    }





    


#if UNITY_EDITOR
    public int debug_DrawMinDepth = 0;
    public int debug_DrawMaxDepth = 5;
    public bool debug_DrawObj = true;
    void OnDrawGizmosSelected()
    {
        Color mindcolor = new Color32(0, 66, 255, 255);
        Color maxdcolor = new Color32(133, 165, 255, 255);
        Color objcolor = new Color32(0, 210, 255, 255);
        Color hitcolor = new Color32(255, 216, 0, 255);
        if (m_QuadTree != null)
            m_QuadTree.DrawTree(mindcolor, maxdcolor, objcolor, hitcolor, debug_DrawMinDepth, debug_DrawMaxDepth, debug_DrawObj);
    }
#endif
}
