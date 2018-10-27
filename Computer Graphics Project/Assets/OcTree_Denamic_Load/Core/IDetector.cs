using UnityEngine;
using System.Collections;

/// <summary>
/// Detector that detects the intersection
/// </summary>
public interface IDetector
{
    /// <summary>
    /// whether the detection is successful
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    bool IsDetected(Bounds bounds);

    /// <summary>
    /// detector position
    /// </summary>
    Vector3 Position { get; }
}
