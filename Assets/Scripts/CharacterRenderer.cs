using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterRenderer : MonoBehaviour
{
    //Initializes arrays of animations
    public static readonly string[] staticDirections = { "Static N", "Static NW", "Static W", "Static SW", "Static S", "Static SE", "Static E", "Static NE" };
    public static readonly string[] runDirections = { "Run N", "Run NW", "Run W", "Run SW", "Run S", "Run SE", "Run E", "Run NE" };
    public static readonly string[] attackDirections = { "Attack N", "Attack NW", "Attack W", "Attack SW", "Attack S", "Attack SE", "Attack E", "Attack NE" };
    Animator animator;
    int lastDirection;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //Sets attack animation to corresponding mouse direction input
    public void SetAttackDirection(Vector2 attackdirection)
    {
        string[] attackDirArray = attackDirections;
        lastDirection = DirectionToIndex(attackdirection, 8);
        animator.Play(attackDirArray[lastDirection]);
    }

    //Sets walking animation to corresponding direction input
    public void SetDirection(Vector2 direction)
    {

        string[] directionArray = null;



        if (direction.magnitude < .01f)
        {
            directionArray = staticDirections;
        }
        else
        {
            directionArray = runDirections;
            lastDirection = DirectionToIndex(direction, 8);
        }


        animator.Play(directionArray[lastDirection]);

    }

    //Changes direction to an index in terms of "slice count*", to be used with calling corresponding animation (*4 being 4 directions of movement/attack, 8 being 8 directions etc..)
    public static int DirectionToIndex(Vector2 dir, int sliceCount)
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
