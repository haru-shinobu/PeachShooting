using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDeleteScript : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(ParticleWorking());
    }

    IEnumerator ParticleWorking()
    {
        var particle = GetComponent<ParticleSystem>();

        yield return new WaitWhile(() => particle.IsAlive(true));
        Destroy(gameObject);
    }
}
