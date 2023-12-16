using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// -----------------------------------------------------------------------------------µ
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
    public float tolerance = 0.1f; // Tolérance pour déterminer si la cible est atteinte

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
        // Phase forward : déplacer le dernier joint vers la cible
        jointsChain[jointsChain.Count - 1].transform.position = target.position;

        // Itération de la phase backward
        for (int i = jointsChain.Count - 2; i >= 0; i--)
        {
            // Calcul du vecteur entre les joints
            Vector3 direction = jointsChain[i].transform.position - jointsChain[i + 1].transform.position;
            float distance = direction.magnitude;

            // Vérification de la tolérance pour éviter les divisions par zéro
            if (distance > tolerance)
            {
                direction.Normalize();

                // Déplacer le joint vers l'autre joint dans la chaîne
                jointsChain[i].transform.position = jointsChain[i + 1].transform.position + direction * distance;
            }
        }

        // Phase forward : déplacer le premier joint vers sa position initiale
        jointsChain[0].transform.position = transform.position;

        // Itération de la phase forward
        for (int i = 0; i < jointsChain.Count - 1; i++)
        {
            // Calcul du vecteur entre les joints
            Vector3 direction = jointsChain[i + 1].transform.position - jointsChain[i].transform.position;
            float distance = direction.magnitude;

            // Vérification de la tolérance pour éviter les divisions par zéro
            if (distance > tolerance)
            {
                direction.Normalize();

                // Déplacer le joint vers l'autre joint dans la chaîne
                jointsChain[i + 1].transform.position = jointsChain[i].transform.position + direction * distance;
            }
        }
    }
}
