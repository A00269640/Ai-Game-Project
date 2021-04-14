using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public GameObject ChestAliveRenderer;
    public GameObject ChestDestroyedRenderer;

    public bool collided;

    void Start()
    {
        ChestAliveRenderer.SetActive(true);
        ChestDestroyedRenderer.SetActive(false);
        collided = false;
    }

    //On collision with Hitbox enacts following
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Hitbox"))
        {
            ChestAliveRenderer.SetActive(false);
            ChestDestroyedRenderer.SetActive(true);
            if (collided == false)
            {
                GlobalVars.score = GlobalVars.score + 200;
            }
            collided = true;
        }
    }
}
