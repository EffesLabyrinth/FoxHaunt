using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerManager manager;
    Rigidbody rb;
    Camera cam;

    //xy Movement
    Vector2 direction;

    //JumpAndFallAdjustment
    bool jumpTrigger;
    int startJumpCount;

    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultipler;

    [SerializeField] Transform groundCheckPos;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask whatIsGround;

    //dash
    bool dashTrigger;
    int startDashCount;
    Vector2 dashDirection;
    [SerializeField] float dashDuration;
    float startDashDuration;

    //attack
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask whatIsEnemy;
    [SerializeField] GameObject[] attackPattern;
    void Awake()
    {
        manager = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        ExecuteAction();
        JumpFallAdjustment();
        TimerUpdate();
        
    }
    void GetInput()
    {
        //xy movement
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GroundCheck())
            {
                if (manager.stat.GetMaxJumpCount() > 0)
                {
                    jumpTrigger = true;
                    startJumpCount = manager.stat.GetMaxJumpCount() - 1;
                    startDashCount = manager.stat.GetMaxDashCount();
                }
            }
            else if (startJumpCount > 0)
            {
                jumpTrigger = true;
                startJumpCount--;
            }
        }
        //dash
        if (Input.GetMouseButtonDown(1))
        {
            if (GroundCheck())
            {
                dashTrigger = true;
                startDashCount = manager.stat.GetMaxDashCount() - 1;
            }
            else if (startDashCount > 0)
            {
                dashTrigger = true;
                startDashCount--;
            }
        }
        if (Input.GetMouseButtonDown(0) && !dashTrigger && startDashDuration <= 0) 
        {
            Attack();
        }
    }
    void ExecuteAction()
    {
        if (startDashDuration <= 0) Move();
        if (jumpTrigger)
        {
            jumpTrigger = false;
            Jump();
        }
        if (dashTrigger)
        {
            startDashDuration = dashDuration;
            dashTrigger = false;
            Dash();
        }
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
    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, manager.stat.GetJumpPower(), rb.velocity.z);
    }
    void Move()
    {
        direction = direction.normalized * manager.stat.GetMoveSpeed();
        rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.y);
    }
    void Dash()
    {
        dashDirection = (Vector2)Input.mousePosition - new Vector2(Screen.width / 2, Screen.height / 2);
        dashDirection = dashDirection.normalized * manager.stat.GetDashSpeed();

        rb.velocity = new Vector3(dashDirection.x, rb.velocity.y, dashDirection.y);
        manager.anim.DashAfterEffect(dashDuration);
    }
    void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius, whatIsEnemy);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100, whatIsGround);
        Vector3 attackDir;
        Vector3 attackPos;
        if (colliders.Length > 0)
        {
            GameObject temp = Instantiate(attackPattern[Random.Range(0, attackPattern.Length)], colliders[0].transform.position, Quaternion.identity);
            temp.transform.forward = (colliders[0].transform.position - transform.position).normalized;
        }
        else
        {
            if ((hit.point - transform.position).sqrMagnitude > attackRadius * attackRadius)
            {
                attackDir = (hit.point - transform.position).normalized;
                attackPos = transform.position + attackDir * attackRadius;
            }
            else
            {
                attackPos = hit.point;
            }
            GameObject temp = Instantiate(attackPattern[Random.Range(0, attackPattern.Length)], attackPos, Quaternion.identity);
            temp.transform.forward = (attackPos - transform.position).normalized;
        }
    }
    bool GroundCheck()
    {
        return (Physics.OverlapSphere(groundCheckPos.position, groundCheckRadius, whatIsGround)).Length > 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (GroundCheck())
            {
                startJumpCount = manager.stat.GetMaxJumpCount();
                startDashCount = manager.stat.GetMaxDashCount();
            }
        }
    }
    void TimerUpdate()
    {
        if (startDashDuration > 0) startDashDuration -= Time.deltaTime;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,attackRadius);
    }
}
