using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2Renderer : MonoBehaviour
{
    //Initializes arrays of animations
    public static readonly string[] staticDirections = { "e1_Static N", "e1_Static NW", "e1_Static W", "e1_Static SW", "e1_Static S", "e1_Static SE", "e1_Static E", "e1_Static NE" };
    public static readonly string[] runDirections = { "e1_Run N", "e1_Run NW", "e1_Run W", "e1_Run SW", "e1_Run S", "e1_Run SE", "e1_Run E", "e1_Run NE" };
    public static readonly string[] attackDirections = { "e1_Attack N", "e1_Attack NW", "e1_Attack W", "e1_Attack SW", "e1_Attack S", "e1_Attack SE", "e1_Attack E", "e1_Attack NE" };

    Animator animator;
    int lastDirection;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //Sets animation to corresponding direction input
    public void enemSetDirection(Vector2 direction)
    {

        string[] directionArray = null;



        if (direction.magnitude < .01f)
        {
            directionArray = staticDirections;
        }
        else
        {

            directionArray = runDirections;
            lastDirection = enemDirectionToIndex(direction, 8);
        }


        animator.Play(directionArray[lastDirection]);

    }

    public void enemSetIdle(Vector2 direction)
    {
        string[] directionArray = null;

        directionArray = staticDirections;

        lastDirection = enemDirectionToIndex(direction, 8);

        animator.Play(directionArray[lastDirection]);
    }

    //Sets attack animation to corresponding direction input
    public void enemSetAttackDirection(Vector2 direction)
    {
        string[] directionArray = null;


        directionArray = attackDirections;
        lastDirection = enemDirectionToIndex(direction, 8);
        

        //tell the animator to play the requested state

        animator.Play(directionArray[lastDirection]);

    }

    //Changes direction to an index in terms of "slice count*", to be used with calling corresponding animation (*4 being 4 directions of movement/attack, 8 being 8 directions etc..)
    public static int enemDirectionToIndex(Vector2 dir, int sliceCount)
    {
        Vector2 normDir = dir.normalized;
        float step = 360f / sliceCount;
        float halfstep = step / 2;
        float angle = Vector2.SignedAngle(Vector2.up, normDir);
        angle += halfstep;
        if (angle < 0)
        {
            angle += 360;
        }
        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }

}
