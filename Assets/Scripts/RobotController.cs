using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    void Start()
    {
        foreach (Transform child in transform) 
        {
            child.gameObject.GetComponent<JointController>().UpdateChildren();
        }
    }
}
