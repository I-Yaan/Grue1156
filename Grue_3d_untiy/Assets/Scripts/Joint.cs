using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------- Joint -----------------
// Joint class for each joint of the grue
public class Joint : MonoBehaviour
{
    // speed of rotation
    public float speed = 100.0f;

    public bool axeY = true;
    public bool axeZ = false;
    public bool axeX = false;

    public float minAngle = -180f;
    public float maxAngle = 180f;

    public Joint m_child;

    public Joint GetChild()
    {
        return m_child;
    }

    private bool needToRotate = false;
    private float targetAngle = 0f;
    private Vector3 oldPos;


    public void Rotate(float _angle)
    {
        if (axeY)
        {
            float newAngle = transform.localEulerAngles.y + _angle;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, newAngle, transform.localEulerAngles.z);
        }
        else if (axeZ)
        {
            float newAngle = transform.localEulerAngles.z + _angle;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, newAngle);
        }
        else if (axeX)
        {
            float newAngle = Mathf.Clamp(transform.localEulerAngles.x + _angle, minAngle, maxAngle);
            transform.localEulerAngles = new Vector3(newAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }

    public void RotateTo(float _angle)
    {
        needToRotate = true;
        targetAngle = _angle;
        oldPos = transform.localEulerAngles;
    }

    public int GetRoundedAngle()
    {
        if (axeY)
        {             
            return (360 + Mathf.RoundToInt(transform.localEulerAngles.y)) % 360;
        }
        else if (axeZ)
        {
            return (360 + Mathf.RoundToInt(transform.localEulerAngles.z)) % 360;
        }
        else if (axeX)
        {
            return (360 + Mathf.RoundToInt(transform.localEulerAngles.x)) % 360;
        }
        return 0;
    }

    public bool IsRotating()
    {
        return needToRotate;
    }
    void Update()
    {

        if (needToRotate)
        {
            Rotate(0.1f * speed * targetAngle);
            
            if (Mathf.Round(transform.localEulerAngles.y) == (360+Mathf.Round(oldPos.y + targetAngle))%360 && axeY)
            {
                needToRotate = false;
            }
            if (Mathf.Round(transform.localEulerAngles.z) == (360+Mathf.Round(oldPos.z + targetAngle))%360 && axeZ)
            {
                needToRotate = false;
            }
            if (Mathf.Round(transform.localEulerAngles.x) == Mathf.Round(oldPos.x + targetAngle) && axeX)
            {
                needToRotate = false;
            }

        }
       


    }
}

