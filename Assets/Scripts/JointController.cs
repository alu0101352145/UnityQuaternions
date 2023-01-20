using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JointController : MonoBehaviour
{
    public List<GameObject> parentJoints;
    public float angle;
    public TextMeshProUGUI positionText;

    private Transform tf;

    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetKeyDown("t")) 
        {
            Debug.Log(gameObject.name + ": " + tf.position);
        }
    }

    public void UpdatePositionText() 
    {
        Vector3 oldPoint = gameObject.GetComponent<Transform>().localPosition;
        MyQuaternion oldPosition = new MyQuaternion(0, oldPoint.x, oldPoint.y, oldPoint.z);
        MyQuaternion finalPosition = new MyQuaternion(1, 0, 0, 0);

        List<MyQuaternion> conjugates = new List<MyQuaternion>();
        Vector3 finalVector;
        if (parentJoints.Count > 0) 
        {
            foreach (GameObject parentJoint in parentJoints)
            {
                float parentAngle = Mathf.Deg2Rad * -parentJoint.GetComponent<JointController>().angle;
                Vector3 direction = parentJoint.GetComponent<Transform>().up;

                MyQuaternion rotationQuaternion = new MyQuaternion(Mathf.Cos(parentAngle / 2), 
                                                                   Mathf.Sin(parentAngle / 2) * direction.x, 
                                                                   Mathf.Sin(parentAngle / 2) * direction.y, 
                                                                   Mathf.Sin(parentAngle / 2) * direction.z);
                MyQuaternion rotationConjugate = rotationQuaternion.Conjugate();
                conjugates.Insert(0, rotationConjugate);

                finalPosition = finalPosition * rotationQuaternion;
            }

            finalPosition = finalPosition * oldPosition;
            foreach (MyQuaternion conjugate in conjugates) 
            {
                finalPosition = finalPosition * conjugate;
            }
            finalVector = new Vector3(finalPosition.iPart, finalPosition.jPart, finalPosition.kPart);
            finalVector += parentJoints[parentJoints.Count - 1].GetComponent<Transform>().position;
        } else {
            finalVector = oldPoint;
        }
    
        positionText.text = finalVector.ToString();
    }

    public void UpdateAngle(string input) 
    {
        float newAngle = float.Parse(input);
        tf.Rotate(new Vector3(0, angle, 0));
        tf.Rotate(new Vector3(0, -newAngle, 0));
        angle = newAngle;

        UpdateChildren();
    }

    public void UpdateChildren() 
    {
        gameObject.GetComponent<JointController>().UpdatePositionText();
        foreach (Transform child in transform) 
        {
            if ((child.gameObject.tag == "Joint") || (child.gameObject.tag == "FinalEffector")) 
            {
                child.gameObject.GetComponent<JointController>().UpdateChildren();
            }
        }
    }
}
