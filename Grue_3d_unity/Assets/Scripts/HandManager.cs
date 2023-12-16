using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{


 
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("HAND Collided with " + other.name);

        if (other.gameObject.tag == "LoadBottom")
        {
            Debug.Log("HAND Collided with LoadBottom");
            GameObject p = other.gameObject.transform.parent.gameObject;

            // if other name is "TopLoad" => 3 pièce 
            if (p.name == "TopLoad")
            {
                // set load parent to hand
                p.gameObject.transform.parent = this.gameObject.transform;
                p.gameObject.transform.localPosition = new Vector3(0, 0, 0f);
                p.gameObject.transform.localRotation = Quaternion.Euler(180.0f, 0.0f, 0.002f);
            }
            else
            {
                // set load parent to hand
                p.gameObject.transform.parent = this.gameObject.transform;
                p.gameObject.transform.localPosition = new Vector3(0, 0, -0.085f);
                p.gameObject.transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            }

            
            
        }
    }
}
