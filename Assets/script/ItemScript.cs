using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    void Start()
    {
        gameObject.tag = "Item";
    }

    void Update()
    {
        transform.position += new Vector3(0.05f, 0);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
