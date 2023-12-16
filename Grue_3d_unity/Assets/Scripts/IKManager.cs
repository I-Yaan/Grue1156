using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ----------------- IKManager -----------------
// Inverse Kinematic Algorithm
public class IKManager : MonoBehaviour
{

    public Joint m_root;

    public GameObject m_end;

    public GameObject loadManager;

    public GameObject Eolienne_target;

    private GameObject current_target;

    private GameObject current_end;

    public float m_threshold = 0.05f;

    public float m_rate = 5.0f;

    public int m_steps = 20;

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
    
    // Start is called before the first frame update
    void Start()
    {
        // assign m_target to the child of loadManager
        current_target = loadManager.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        // debug log all children of loadManager

        Debug.Log("dfdfd"+loadManager.transform.childCount);
        
    }

    // Update is called once per frame
    void Update()
    {

        // get current_target center positon of the capsule collider


        // if m_end the hand have a child 
        if (m_end.transform.childCount > 0)
        {
            // the end is now the bottom of the load

            current_end = m_end;
            //Debug.Log("current_end" + current_end.name + current_end.transform.position);
            // the target is now the top of the Eolienne
            current_target = Eolienne_target;
            //Debug.Log("current_target" + current_target.name + current_target.transform.position);
        }
        // if loadManager have a child
        else if (loadManager.transform.childCount > 0)
        {
            // the end is now the hand
            current_end = m_end;
            // the target is now the load bottom
            current_target =  loadManager.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
            //Debug.Log("current_target" + current_target.name + current_target.transform.position);

        }
        for (int i = 0; i < m_steps; ++i)
        {
            if (GetDistance(current_end.transform.position, current_target.transform.localPosition) > m_threshold)
            {
                Joint Current = m_root;
                while (Current != null)
                {
                    float slope = CalculateSlope(Current);
                    Current.Rotate(-slope * m_rate);
                    Current = Current.GetChild();
                }


            }
        }
    }

    float GetDistance(Vector3 _point1, Vector3 _point2)
    {
        return Vector3.Distance(_point1, _point2);
    }
}
