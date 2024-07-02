using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObj : MonoBehaviour
{   //x = 1 right, x = -1 left
    //y - 1 upper, y = -1 lower
    [SerializeField] private Vector2 bouncePos;
    private Animator animator;
    private const string bounceAnim = "bounce";
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public Vector3 GetDirectionBounce(Vector3 towardDirection)
    {
        Vector3 result = Vector3.zero;
        animator.SetTrigger(bounceAnim);
        if (towardDirection.x > towardDirection.z)
        {
            result = new Vector3(0, 0, towardDirection.x * bouncePos.y * -1);
            return result;
        }

        result = new Vector3(towardDirection.z * bouncePos.x * -1, 0, 0f);
        return result;
    }
}
