using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEAttackCircleScript : MonoBehaviour
{    
    void Start()
    {   
        foreach (Transform childTransform in transform)
        {
            childTransform.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        Destroy(gameObject,5);
    }
}
