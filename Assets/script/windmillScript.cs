using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windmillScript : MonoBehaviour
{
    Transform trs;
    public float RotateSpeed = 1;
    Vector3 movepos;
    float timer = 0;
    Vector3 PPos;
    void Start()
    {
        PPos = GameObject.FindWithTag("Player").transform.position;
        trs = gameObject.GetComponent<Transform>();
        movepos = new Vector2(0.1f,0);
    }

    void FixedUpdate()
    {
        Vector3.Lerp(transform.position,PPos,timer);
        trs.Rotate(trs.forward, -RotateSpeed);
        timer += Time.deltaTime;
        if (2 < timer)
            trs.position += movepos;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.SendMessage("Damage",5);
            Destroy(gameObject);
        }
        if (collision.tag == "Attack")
            collision.transform.Rotate(Vector3.forward,transform.rotation.z);
    }

    void BounceBack()
    {

    }

    void StartWeponState(int val)
    {

    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
