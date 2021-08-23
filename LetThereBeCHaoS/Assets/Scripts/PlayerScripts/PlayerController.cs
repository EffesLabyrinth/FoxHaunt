using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerManager manager;
    Rigidbody rb;

    //xy Movement
    Vector2 direction;

    //JumpAndFallAdjustment
    bool jumpTrigger;
    int startJumpCount;

    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultipler;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask whatIsGround;

    void Awake()
    {
        manager = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        ExecuteAction();
        JumpFallAdjustment();
    }
    void GetInput()
    {
        //xy movement
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        direction = direction.normalized * manager.stat.GetMoveSpeed();
        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((Physics.OverlapSphere(groundCheck.position, groundCheckRadius, whatIsGround)).Length > 0)
            {
                if (manager.stat.GetMaxJumpCount() > 0)
                {
                    jumpTrigger = true;
                    startJumpCount = manager.stat.GetMaxJumpCount() - 1;
                }
            }
            else if (startJumpCount > 0)
            {
                jumpTrigger = true;
                startJumpCount--;
            }
        }
    }
    void ExecuteAction()
    {
        if (jumpTrigger)
        {
            jumpTrigger = false;
            rb.velocity = new Vector3(direction.x, manager.stat.GetJumpPower(), direction.y);
        }
        else rb.velocity = new Vector3(direction.x,rb.velocity.y,direction.y);
    }
    void JumpFallAdjustment()
    {
        if (rb.velocity.y < 0) rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0)
        {
            if (!Input.GetKey(KeyCode.Space))
                rb.velocity += Vector3.up * Physics.gravity.y * lowJumpMultipler * Time.deltaTime;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
