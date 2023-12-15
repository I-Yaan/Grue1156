using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

// ----------------- EoManager -----------------
// manage construction of Eolienne
public class EoManager : MonoBehaviour
{


    public GameObject EoliennePreFab;
    public GameObject TopEoliennePreFab;
    public GameObject LoadManager;

    public GameObject EoLine;

    public GameObject FullEoPreFab;


    public GameObject EolienneBase;

    public int maxLoad = 3;

    public float EoDistance = 0.5f;

    private uint step = 0;


    private bool needToMove = false;
    private Vector3 newPos;

    void Update()
    {

        if (needToMove)
        {
            // MoveTowards EoLine of 0.2 on z axis
            EoLine.transform.position = Vector3.MoveTowards(EoLine.transform.position, newPos, 0.008f);
            // stop when EoLine is at LoadManager position
            if (EoLine.transform.position == newPos)
            {
                needToMove = false;
            }
        }
        // if childcount of his parent > 6 delete all childs with EoliennePart as tag
        if (transform.parent.childCount > maxLoad)
        {

            step = 0;
            // add full Eo pre fab to EoLine 
            GameObject newFullEo = Instantiate(FullEoPreFab, new Vector3(0, 0, 0), Quaternion.identity);
            newFullEo.transform.parent = EoLine.transform;

            // on the same position as EoManager parent positon
            newFullEo.transform.position = transform.parent.transform.localPosition;

            
            newFullEo.transform.localRotation = Quaternion.identity;
            newFullEo.name = "FullEo";

            needToMove = true;
            newPos = new Vector3(EoLine.transform.position.x, EoLine.transform.position.y, EoLine.transform.position.z + EoDistance);


            // delete all childs with EoliennePart as tag

            foreach (Transform child in this.gameObject.transform.parent)
            {
                if (child.gameObject.tag == "EoliennePart")
                {
                    Destroy(child.gameObject);
                }
            }
            ResetHitBox();
        }
    }

    void ResetHitBox()
    {
        // reset hitbox
        step = 0;

        this.gameObject.transform.localPosition = new Vector3(0, 0.6f, 0);

    }
    void NewLoad()
    {

           // create new load

        if (step == 2)
        {
            // create new load on top of the Eolienne
            GameObject newTopLoad = Instantiate(TopEoliennePreFab, new Vector3(0, 0, 0), Quaternion.identity);
            newTopLoad.transform.parent = EolienneBase.transform;
            newTopLoad.transform.localPosition = new Vector3(0, 1.5f + (2.3f * step), 0);
            newTopLoad.transform.localRotation = Quaternion.identity;

            newTopLoad.name = "TopLoad" + step;
            // trigger loadManager to add new load
            

        }
        else
        {
            GameObject newLoad = Instantiate(EoliennePreFab, new Vector3(0, 0, 0), Quaternion.identity);
            newLoad.transform.parent = EolienneBase.transform;
            newLoad.transform.localPosition = new Vector3(0, 1.5f +(2.3f * step) , 0);
            newLoad.transform.localRotation = Quaternion.identity;
        
            newLoad.name = "Load" + step;
        }
            
        if (step ==1)
        {
            LoadManager.GetComponent<LoadManager>().AddNewLoad(true);
        }else
        {
            LoadManager.GetComponent<LoadManager>().AddNewLoad();
        }
        step++;

        // move EoManager to load height up
        this.gameObject.transform.localPosition += new Vector3(0, 2.3f, 0);
        // trigger loadManager to add new load


        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Eolienne Collided with " + other.name);

        // if collided with load bottom Object
        if (other.gameObject.tag == "LoadBottom")
        {
            Debug.Log("Eolienne Collided with LoadBottom");
            // Destroy the load in the hand
            Destroy(other.gameObject.transform.parent.gameObject);
            // create new load on top of the Eolienne
            NewLoad();
            
        }
    }
}
