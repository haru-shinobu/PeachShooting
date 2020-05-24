using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextScript : MonoBehaviour
{
    void NewBorn(float textnum)
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-80, 80), Random.Range(200, 300), 0));
        GetComponent<TextMesh>().text = (textnum).ToString("F0");
        StartCoroutine(Dastroyer());
    }

    private IEnumerator Dastroyer()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
