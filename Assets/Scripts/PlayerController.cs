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

    [Header("Bridge fill")]
    [SerializeField] private LayerMask bridgeLayerMask;

    private Animator animator;
    
    private List<Transform> bricksList;
    private Rigidbody rigidbodyy;

    private Vector2 startTouchPos;

    private Vector2 endTouchPos;
    private Vector3 moveDirection;
    private PlayerMoveDirection moveDirectionEnum;
    private bool isMoving;
    private static readonly int JumpAnimName = Animator.StringToHash("Jump");
    private static readonly int WinAnimName = Animator.StringToHash("Win");
    private bool isStart;

    private void Awake()
    {
        rigidbodyy = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //ResetStart();
        GameController.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameController.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState obj)
    {
        if (obj == GameState.InGame)
        {
            Invoke(nameof(ResetStart), 0.5f);
            transform.position = LevelManager.Instance.GetSpawnPositionOfPlayer();
            ChangeToIdle();
        }
    }

    void ResetStart()
    {
        isStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.Instance.IsInState(GameState.InGame) || !isStart)
            return;
        HandleUserInput();
    }

    private void FixedUpdate()
    {
        if (!GameController.Instance.IsInState(GameState.InGame) || !isStart)
            return;
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
        //convert direction to 4 directions left, right, forward, back
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
        //Assign movedirection
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
        rigidbodyy.velocity = direction;
    }
    private void CheckCollide()
    {
        if (isMoving)
        {
            if (CheckCollideWithWall())
            {
                //Debug.Log("CollideWall");
                FixPlayerPosition();
                ChangeToIdle();
            }
            FillBridge();
            CollectBrick();
        }
    }

    #region HandleBrick
    private void AddBricks()
    {
        var brick = LazyPool.Instance.GetObj(brickPrefabs);
        brick.transform.SetParent(brickParent);
        animator.SetBool(JumpAnimName, true);
        if (bricksList == null)
        {
            bricksList = new List<Transform>();
        }
        bricksList.Add(brick.transform);
        UpdateVisualBricks();
    }

    private void RemoveBrick()
    {
        if (bricksList.Count > 0)
        {
       
            LazyPool.Instance.AddObjectToPool(bricksList[bricksList.Count-1].gameObject);
           
            bricksList.RemoveAt(bricksList.Count-1);
            
            UpdateVisualBricks();
        }
    }

    private void ClearAllBrick()
    {
        foreach (var brick in bricksList)
        {
            LazyPool.Instance.AddObjectToPool(brick.gameObject);
        }
        bricksList.Clear();
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
    
    private void CollectBrick()
    {
        RaycastHit hit;
        Physics.Raycast(checkRaycastPoint.position, moveDirection,out hit, distanceCheckRaycast, brickLayerMask);
        if (hit.collider != null)
        {
           // Debug.Log("Get brick");
            var brick = hit.collider.GetComponent<Brick>();
            if (brick != null && brick.CanGetBrick())
            {
                brick.PlayerGetBrick();
                AddBricks();
            }
        }
    }
    #endregion

    #region HandleBridge
    private bool CanFillBridge()
    {
        return bricksList.Count > 0;
    }
    private void FillBridge()
    {
        
        RaycastHit hit;
        Physics.Raycast(checkRaycastPoint.position, moveDirection,out hit, distanceCheckRaycast, bridgeLayerMask);
        if (hit.collider != null)
        {
            //Debug.Log("collide bridge");
            var bridge = hit.collider.GetComponent<Bridge>();
            if (bridge == null)
            {
                Debug.LogError("bridge null roi T.T");
                return;
            }

            if (!bridge.CanFill()) // bridge is already fill
            {
                return;
            } 
            //if bridge is not fill and player doesn't have enough brick to fill
            if (!CanFillBridge())
            {
                FixPlayerPosition();
                ChangeToIdle();
                return;
            }
            //fill bridge
            // Debug.Log("Fill bridge");
           
            bridge.PlayerFillBridge();
            RemoveBrick();
          
        }
    }
    

    #endregion
   
    
   
    //fix player position to the center of tile
    //size of each tile is (1,1)
    void FixPlayerPosition()
    {
        // Debug.Log(transform.position + " " + transform.position.x/0.5f + " "+transform.position.z/0.5f);
        float numberX = (float)Math.Round(transform.position.x / 0.5f);
        float numberY = (float)Math.Round(transform.position.z / 0.5f);
//        Debug.Log(numberX + " " + numberY);
        transform.position = new Vector3(numberX * 0.5f, 0, numberY * 0.5f);
    }
    private void ChangeToIdle()
    {
        //fix position to the center
        isMoving = false;
        Move(Vector3.zero);
        animator.SetBool(JumpAnimName, false);
        animator.SetBool(WinAnimName, false);
    }
    private void ChangeToWinState()
    {
        isMoving = false;
        isStart = false;
        Move(Vector3.zero);
        animator.SetBool(JumpAnimName, false);
        animator.SetBool(WinAnimName, true);
        GameController.Instance.currentGameScore = bricksList.Count;
        ClearAllBrick();
        GameController.Instance.ChangeState(GameState.FinishGame);
    }
   

   
    private bool CheckCollideWithWall()
    {
        return Physics.Raycast(checkRaycastPoint.position, moveDirection, distanceCheckRaycast, wallLayerMask);
    }
    private void BounceToDirection(Vector3 direction)
    {
        moveDirection = direction;
        
        isMoving = true;
        Move(moveDirection*moveSpeed);
    }
   
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(checkRaycastPoint.position, checkRaycastPoint.position + Vector3.right*distanceCheckRaycast);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WinPos"))
        {
            ChangeToWinState();
        }

        if (other.CompareTag("BouncePlatform"))
        {
           // Debug.Log("Bounce");
            FixPlayerPosition();
            var bounce = other.GetComponent<BounceObj>();
            BounceToDirection(bounce.GetDirectionBounce(moveDirection));
        }
    }

   
   
   
}

public enum PlayerMoveDirection
{
    Left,
    Right,
    Forward,
    BackWard
}
