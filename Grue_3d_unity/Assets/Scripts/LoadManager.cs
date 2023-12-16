using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    public GameObject LoadPreFab;
    public GameObject TopEoLoadPreFab;
    public void AddNewLoad(bool top = false)
    {
        if (top)
        {
            // create new load
            GameObject newLoad = Instantiate(TopEoLoadPreFab, new Vector3(0, 0, 0), Quaternion.identity);
            newLoad.transform.parent = this.gameObject.transform;
            newLoad.transform.localPosition = new Vector3(0, 0.03f, 0.08f);
            newLoad.transform.localScale = new Vector3(1f, 1f, 1f);
            newLoad.transform.localRotation = Quaternion.Euler(0, 180.0f, 90.0f);

            newLoad.name = "TopLoad";
        }
        else { 
            // create new load
            GameObject newLoad = Instantiate(LoadPreFab, new Vector3(0, 0, 0), Quaternion.identity);
            newLoad.transform.parent = this.gameObject.transform;
            newLoad.transform.localPosition = new Vector3(0, 0.01f, 0);
            newLoad.transform.localRotation = Quaternion.Euler(0, -90.0f, 90.0f);
            newLoad.name = "Load";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        AddNewLoad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
