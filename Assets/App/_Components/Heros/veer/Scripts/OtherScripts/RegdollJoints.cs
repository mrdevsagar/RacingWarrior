using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode] 
public class RegdollJoints : MonoBehaviour
{
    public bool IsPlayerLeftFacing;
    public List<JointData> jointDataList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateJointLimits();
    }

    // This method updates the limits of each joint based on the IsPlayerLeftFacing flag
    private void UpdateJointLimits()
    {
        foreach (JointData jointData in jointDataList)
        {
            if (jointData.hingeJoint == null) continue;

            JointAngleLimits2D jointLimit = jointData.hingeJoint.limits;
            jointLimit.min = IsPlayerLeftFacing ? jointData.vectorB.x : jointData.vectorA.x;
            jointLimit.max = IsPlayerLeftFacing ? jointData.vectorB.y : jointData.vectorA.y;

            jointData.hingeJoint.limits = jointLimit;
            jointData.hingeJoint.useLimits = true;
        }
    }

    // OnValidate is called in the editor whenever values are changed
    private void OnValidate()
    {
        if (!Application.isPlaying) // Prevents running during play mode
        {
            UpdateJointLimits();
        }
    }


}
