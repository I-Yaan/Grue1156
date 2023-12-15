using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// ----------------- GrueManager -----------------
// move the grue with user keybord input

public class GrueManager : MonoBehaviour
{

   


    public GameObject baseJoint;
    public GameObject firstArmJoint;
    public GameObject secondArmJoint;

    private float baseSliderValue;
    private float firstArmSliderValue;
    private float secondArmSliderValue;

    public float firstArmMaxAngle = 90.0f;
    public float firstArmMinAngle = -135.0f;

    public float secondArmMaxAngle = 90.0f;
    public float secondArmMinAngle = -135.0f;

    private float baseAngle = 180.0f;
    private float firstArmAngle = 0.0f;
    private float secondArmAngle = 0.0f;


    public float baseSpeed = 1.0f;
    public float firstArmSpeed = 1.0f;
    public float secondArmSpeed = 1.0f;


    // Start is called before the first frame updateµ
    void Start()
    {
        
    }
   
  

    void ProcessMovement()
    {
        baseAngle = baseSliderValue;
        firstArmAngle = firstArmSliderValue;
        secondArmAngle = secondArmSliderValue;

        firstArmAngle = Mathf.Clamp(firstArmAngle, firstArmMinAngle, firstArmMaxAngle);
        secondArmAngle = Mathf.Clamp(secondArmAngle, secondArmMinAngle, secondArmMaxAngle);

        baseJoint.transform.localEulerAngles = new Vector3(baseJoint.transform.localEulerAngles.x, baseJoint.transform.localEulerAngles.y, baseAngle);

        firstArmJoint.transform.localEulerAngles = new Vector3(firstArmJoint.transform.localEulerAngles.x, firstArmAngle, firstArmJoint.transform.localEulerAngles.z);
        secondArmJoint.transform.localEulerAngles = new Vector3(secondArmJoint.transform.localEulerAngles.x, secondArmAngle, secondArmJoint.transform.localEulerAngles.z);
    }

    void CheckInput()
    {         
        if (Input.GetKey(KeyCode.A))
        {
            baseSliderValue += baseSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            baseSliderValue -= baseSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            firstArmSliderValue += firstArmSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            firstArmSliderValue -= firstArmSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            secondArmSliderValue += secondArmSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            secondArmSliderValue -= secondArmSpeed * Time.deltaTime;
        }   
    }

    // Update is called once per frame
    void Update()
    {

        CheckInput();
        ProcessMovement();


    }
}
