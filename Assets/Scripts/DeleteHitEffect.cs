using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteHitEffect : MonoBehaviour
{

    public float timer;

    void Start()
    {
        timer = 0.5f;
    }

    void Update()
    {
        StartCoroutine("deleteEffect");    
    }

    //Destroys Self After X Seconds
    private IEnumerator deleteEffect()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
