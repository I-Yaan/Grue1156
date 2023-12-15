using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// -----------------------------------------------------------------------------------�
// FABRIK Algorithm
public class FABRIKManager : MonoBehaviour
{
    // Vos autres variables membres

    public List<Joint> jointsChain = new List<Joint>(); // Liste de tous les joints de votre bras
    public Transform target; // La cible que le bras essaie d'atteindre

    public GameObject loadManager;
    public GameObject Eolienne_target;

    public Joint joint0;
    public Joint joint1;
    public Joint joint2;
    public Joint joint3;
    public float tolerance = 0.1f; // Tol�rance pour d�terminer si la cible est atteinte

    void Start()
    {
        // Assignez tous les joints dans l'ordre requis
        // Exemple : jointsChain.Add(joint1);
        //           jointsChain.Add(joint2);
        //           ...
        //           jointsChain.Add(jointN);
        jointsChain.Add(joint0);
        jointsChain.Add(joint1);
        jointsChain.Add(joint2);
        jointsChain.Add(joint3);


    }

    void Update()
    {
        FABRIKAlgorithm();
    }

    void FABRIKAlgorithm()
    {
        // Phase forward : d�placer le dernier joint vers la cible
        jointsChain[jointsChain.Count - 1].transform.position = target.position;

        // It�ration de la phase backward
        for (int i = jointsChain.Count - 2; i >= 0; i--)
        {
            // Calcul du vecteur entre les joints
            Vector3 direction = jointsChain[i].transform.position - jointsChain[i + 1].transform.position;
            float distance = direction.magnitude;

            // V�rification de la tol�rance pour �viter les divisions par z�ro
            if (distance > tolerance)
            {
                direction.Normalize();

                // D�placer le joint vers l'autre joint dans la cha�ne
                jointsChain[i].transform.position = jointsChain[i + 1].transform.position + direction * distance;
            }
        }

        // Phase forward : d�placer le premier joint vers sa position initiale
        jointsChain[0].transform.position = transform.position;

        // It�ration de la phase forward
        for (int i = 0; i < jointsChain.Count - 1; i++)
        {
            // Calcul du vecteur entre les joints
            Vector3 direction = jointsChain[i + 1].transform.position - jointsChain[i].transform.position;
            float distance = direction.magnitude;

            // V�rification de la tol�rance pour �viter les divisions par z�ro
            if (distance > tolerance)
            {
                direction.Normalize();

                // D�placer le joint vers l'autre joint dans la cha�ne
                jointsChain[i + 1].transform.position = jointsChain[i].transform.position + direction * distance;
            }
        }
    }
}
