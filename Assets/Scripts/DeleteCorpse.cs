using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCorpse : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    //Destroys Self After X Seconds
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
