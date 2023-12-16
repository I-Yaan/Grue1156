using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// ----------------- IKC -----------------
// Inverse Kinematic Algorithm wich can be enable or disable

public class IKC : MonoBehaviour
{

    public Joint joint0;
    public Joint joint1;
    public Joint joint2;
    public Joint joint3;

    public GameObject hand;

    public GameObject loadManager;

    public GameObject Eolienne_target;

    private GameObject current_target;

    private GameObject current_end;

    public float m_threshold = 0.05f;

    public float m_rate = 5.0f;

    public int m_steps = 20;

    // array of 2 first joints
    private Joint[] joints ;

    // array of array of joints
    private Joint[][] jointsArray;

    private int step = 0;

    private bool isIkEnable = false;

    private bool getLoad = false;
    private bool setLoad = false;


    public void EnableIk()
    {
        isIkEnable = true;
    }
    public void DisableIk()
    {
        isIkEnable = false;
    }

    public bool IsLoadHanded()
    {
        return getLoad;
    }
    public bool IsLoadSet()
    {
        return setLoad;
    }

    // Start is called before the first frame update
    void Start()
    {
        joints = new Joint[4];
        joints[0] = joint0;
        joints[1] = joint1;
        joints[2] = joint2;
        joints[3] = joint3;

    }

    // Update is called once per frame
    void Update()
    {
        if (isIkEnable)
        {
            // if m_end the hand have a child 
            if (hand.transform.childCount > 0)
            {
                getLoad = true;
                setLoad = false;
                // the end is now the bottom of the load
                current_end = hand.transform.GetChild(0).transform.GetChild(1).gameObject;

                // the target is now the top of the Eolienne
                current_target = Eolienne_target;

            }
            // if loadManager have a child
            else if (loadManager.transform.childCount > 0)
            {
                getLoad = false;
                setLoad = true;

                // the end is now the hand
                current_end = hand;

                // the target is now the load bottom
                current_target = loadManager.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
                //Debug.Log("current_target" + current_target.name + current_target.transform.position);

            }
        
            for (int i = 0; i < m_steps; ++i)
            {
                if (GetDistance(current_end.transform.position, current_target.transform.localPosition) > m_threshold)
                {
                    Joint Current;
                    // for each joint in joints array
                    for (int j = 0; j < joints.Length; j++)
                    {
                        // rotate the joint
                        Current = joints[j];
                        // debug the name of the joint
                        //Debug.Log("Current" + Current.name);

                        float slope = CalculateSlope(Current);
                        Current.Rotate(-slope * m_rate);

                    }

                }
            }
        }
        
    }

    float CalculateSlope(Joint _joint)
    {
        float deltaTheta = 0.01f;
        float distance1 = GetDistance(current_end.transform.position, current_target.transform.position);

        _joint.Rotate(deltaTheta);

        float distance2 = GetDistance(current_end.transform.position, current_target.transform.position);
        // if m_end is under 0 in y axis
        bool isUnder = current_end.transform.position.y < 0;
        

        _joint.Rotate(-deltaTheta);


        return (distance2 - distance1) / deltaTheta;
    }

    float GetDistance(Vector3 _point1, Vector3 _point2)
    {
        return Vector3.Distance(_point1, _point2);
    }
}
