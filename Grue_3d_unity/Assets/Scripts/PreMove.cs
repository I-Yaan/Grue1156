using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// make preset movement and adjuste them with IKC script 
public class PreMove : MonoBehaviour
{
    public IKC IKC;
    public Joint m_base;
    public Joint joint1;
    public Joint joint2;
    public Joint joint3;
   

    public int[] loadToEo_stage1 = { 45, -50, 79, 45 };
    public int[] loadToEo_stage2 = { 45, -45, 85, 40 };
    public int[] loadToEo_stage3 = { 45, -40, 89, 35 };

    private int m_base_angle = 0;
    private int joint1_angle = 0;
    private int joint2_angle = 0;
    private int joint3_angle = 0;

    public GameObject m_end;

    public GameObject loadManager;

    public GameObject Eolienne_target;

    private GameObject current_target;

    private GameObject current_end;

    private uint step = 0;
    // Start is called before the first frame update

    private bool IsRotating()
    {
        return m_base.IsRotating() || joint1.IsRotating() || joint2.IsRotating() || joint3.IsRotating();
    }
    private void StartToLoad()
    {
      
        m_base.RotateTo(-45);
        joint1.RotateTo(-20);
        joint2.RotateTo(-145);    
        
        step = 1;

    }
    private void LoadToEo(uint stage)
    {
        if (stage == 1)
        {
            m_base.RotateTo(loadToEo_stage1[0]);
            joint1.RotateTo(loadToEo_stage1[1]);
            joint2.RotateTo(loadToEo_stage1[2]);
            joint3.RotateTo(loadToEo_stage1[3]);
            
        }
        else if (stage == 2)
        {
            m_base.RotateTo(loadToEo_stage2[0]);
            joint1.RotateTo(loadToEo_stage2[1]);
            joint2.RotateTo(loadToEo_stage2[2]);
            joint3.RotateTo(loadToEo_stage2[3]);
        }
        else if (stage == 3)
        {
            m_base.RotateTo(loadToEo_stage3[0]);
            joint1.RotateTo(loadToEo_stage3[1]);
            joint2.RotateTo(loadToEo_stage3[2]);
            joint3.RotateTo(loadToEo_stage3[3]);
        }
    }

    private void EoToLoad(uint stage)
    {
        if (stage == 1)
        {
            m_base.RotateTo(-loadToEo_stage1[0]);
            joint1.RotateTo(-loadToEo_stage1[1]);
            joint2.RotateTo(-loadToEo_stage1[2]);
            joint3.RotateTo(-loadToEo_stage1[3]);
        }
        else if (stage == 2)
        {
            m_base.RotateTo(-loadToEo_stage2[0]);
            joint1.RotateTo(-loadToEo_stage2[1]);
            joint2.RotateTo(-loadToEo_stage2[2]);
            joint3.RotateTo(-loadToEo_stage2[3]);

        }
        else if (stage == 3)
        {
            m_base.RotateTo(-loadToEo_stage3[0]);
            joint1.RotateTo(-loadToEo_stage3[1]);
            joint2.RotateTo(-loadToEo_stage3[2]);
            joint3.RotateTo(-loadToEo_stage3[3]);

        }

    }


    void Start()
    {
        //current_target = loadManager.transform.GetChild(0).gameObject;
        
        Debug.Log("doing rotation now");
        
    }
    void Update()
    {
        if (step == 0)
        {
            StartToLoad();
            step = 1;

        }
        else if (step == 1 && !IsRotating())
        {
            // call functon EnableIk from IKC
            IKC.EnableIk();

            // when hand grab the load
            if (IKC.IsLoadHanded())
            {
                step = 2;
                IKC.DisableIk();
                //first load to the Eolienne platform
                LoadToEo(1);
                Debug.Log("target reached");
            }
        }
        else if(step == 2 && !IsRotating())
        {
            IKC.EnableIk();
            if (IKC.IsLoadSet())
            {
                step = 3;
                IKC.DisableIk();
                EoToLoad(1);
                Debug.Log("target reached");
            }
        }
        else if (step == 3 && !IsRotating())
        {
            IKC.EnableIk();
            if (IKC.IsLoadHanded())
            {
                step = 4;
                IKC.DisableIk();
                LoadToEo(2);
                Debug.Log("target reached");
            }
        }
        else if (step == 4 && !IsRotating())
        {
            IKC.EnableIk();
            if (IKC.IsLoadSet())
            {
                step = 5;
                IKC.DisableIk();
                EoToLoad(2);
                Debug.Log("target reached");
            }
        }
        else if (step == 5 && !IsRotating())
        {
            IKC.EnableIk();
            if (IKC.IsLoadHanded())
            {
                step = 6;
                IKC.DisableIk();
                LoadToEo(3);
                Debug.Log("target reached");
            }
        }
        else if (step == 6 && !IsRotating())
        {
            IKC.EnableIk();
            if (IKC.IsLoadSet())
            {
                step = 1;
                IKC.DisableIk();
                EoToLoad(3);
                Debug.Log("target reached new step " + step);
            }
        }

    }
}
