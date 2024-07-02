using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [Header("Raycast Check")]
    [SerializeField] private Transform checkRaycastPoint;
    [SerializeField] private float distanceCheckRaycast;
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private LayerMask brickLayerMask;
    [Header("Brick Spawn")]
    [SerializeField] private GameObject brickPrefabs;

    [SerializeField] private Transform brickParent;
    [SerializeField] private float distanceBetweenBrick;
    [SerializeField] private Transform playerVisual;
    
    private List<Transform> bricksList;
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

        if (Input.GetMouseButtonUp(0) && !isMoving)
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

    private void AddBricks()
    {
        var brick = Instantiate(brickPrefabs, brickParent);
        if (bricksList == null)
        {
            bricksList = new List<Transform>();
        }
        bricksList.Add(brick.transform);
        UpdateVisualBricks();
    }
    private void UpdateVisualBricks()
    {
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < bricksList.Count; i++)
        {
            bricksList[i].localPosition = pos;
            pos += Vector3.up * distanceBetweenBrick;
        }

        playerVisual.transform.localPosition = pos;
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
            CollectBrick();
        }
    }

    private void CollectBrick()
    {
        RaycastHit hit;
        Physics.Raycast(checkRaycastPoint.position, moveDirection,out hit, distanceCheckRaycast, brickLayerMask);
        if (hit.collider != null)
        {
            Debug.Log("Get brick");
            var brick = hit.collider.GetComponentInParent<Brick>();
            if (brick != null && brick.CanGetBrick())
            {
                brick.PlayerGetBrick();
                AddBricks();
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
