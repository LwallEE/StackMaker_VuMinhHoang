using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform checkRaycastPoint;
    [SerializeField] private float distanceCheckRaycast;
    [SerializeField] private LayerMask wallLayerMask;
    
    private Rigidbody rigidbody;

    private Vector2 startTouchPos;

    private Vector2 endTouchPos;
    private Vector3 moveDirection;
    private PlayerMoveDirection moveDirectionEnum;
    private bool isMoving;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleUserInput();
    }

    private void FixedUpdate()
    {
        CheckCollide();
    }

    private void HandleUserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endTouchPos = Input.mousePosition;
            isMoving = true;
            UpdateUserMoveDirection();
        }
    }

    private void UpdateUserMoveDirection()
    {
        var direction = (endTouchPos - startTouchPos).normalized;
        
        if (Math.Abs(direction.y) > Math.Abs(direction.x))
        {
            if (direction.y > 0)
            {
                moveDirectionEnum = PlayerMoveDirection.Forward;
            }
            else
            {
                moveDirectionEnum = PlayerMoveDirection.BackWard;
            }
        }
        else
        {
            if (direction.x > 0)
            {
                moveDirectionEnum = PlayerMoveDirection.Right;
            }
            else
            {
                moveDirectionEnum = PlayerMoveDirection.Left;
            }
        }
        //Debug.Log(moveDirectionEnum);
        if (moveDirectionEnum == PlayerMoveDirection.Left)
        {
            moveDirection = Vector3.left;
        }
        else if (moveDirectionEnum == PlayerMoveDirection.Right)
        {
            moveDirection = Vector3.right;
        }
        else if (moveDirectionEnum == PlayerMoveDirection.Forward)
        {
            moveDirection = Vector3.forward;
        }
        else
        {
            moveDirection = Vector3.back;
        }
        //Debug.Log(moveDirection);
        Move(moveDirection * moveSpeed);
    }

    private void Move(Vector3 direction)
    {
        //Debug.Log("Move " + direction);
        rigidbody.velocity = direction;
    }
    
    private void CheckCollide()
    {
        if (isMoving)
        {
            if (CheckCollideWithWall())
            {
                isMoving = false;
                Move(Vector3.zero);
            }
        }
    }

    private bool CheckCollideWithWall()
    {
        return Physics.Raycast(checkRaycastPoint.position, moveDirection, distanceCheckRaycast, wallLayerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(checkRaycastPoint.position, checkRaycastPoint.position + Vector3.right*distanceCheckRaycast);
    }
}

public enum PlayerMoveDirection
{
    Left,
    Right,
    Forward,
    BackWard
}
