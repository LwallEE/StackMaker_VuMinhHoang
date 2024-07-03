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
        float rever = 1f;
        animator.SetTrigger(bounceAnim);
        if (Math.Abs(towardDirection.x) > Math.Abs(towardDirection.z))
        {
            if (towardDirection.x > 0) rever = -1f;
            result = new Vector3(0, 0, towardDirection.x * bouncePos.y * rever);
            return result;
        }

        if (towardDirection.z > 0) rever = -1f;
        result = new Vector3(towardDirection.z * bouncePos.x*rever, 0, 0f);
        //Debug.Log(result);
        return result;
    }
}
