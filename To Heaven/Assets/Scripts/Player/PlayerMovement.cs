﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public CharacterController controller;
    public Animator animator;

    [Header("Movement Settings")]
    public float speed = 6f;             // Tốc độ đi bộ
    public float runSpeed = 12f;         // Tốc độ chạy
    public float gravity = -9.81f * 2;   // Trọng lực
    public float jumpHeight = 3f;        // Chiều cao nhảy

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Rotation Settings")]
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private Vector3 velocity;
    private bool isGrounded;

    private SwingColumn currentSwingPlatform; // Tham chiếu đến cầu xoay hiện tại
    private PlaneMoveForward currentMovingPlatform; // Tham chiếu đến mặt phẳng di chuyển hiện tại

    // Biến lưu vị trí bắt đầu để hồi sinh
    private Vector3 startingPosition;

    // Biến kiểm tra trạng thái rơi
    private bool isFalling = false;

    // Biến lưu hướng di chuyển
    private Vector3 moveDirection;

    void Start()
    {
        // Lưu vị trí ban đầu của nhân vật
        startingPosition = transform.position;
    }

    void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleJump();
        ApplyGravity();
        HandleFallingAndRespawn();

        // Tính toán chuyển động tổng
        Vector3 totalMovement = (moveDirection + velocity) * Time.deltaTime;


        if (currentSwingPlatform != null)
        {
            Vector3 swingPlatformVelocity = currentSwingPlatform.GetPlatformVelocity(transform.position);

            if (!isGrounded)
            {
                // Loại bỏ vận tốc thẳng đứng của cầu xoay khi nhân vật đang trên không
                swingPlatformVelocity.y = 0;
            }

            // Giới hạn vận tốc tối đa
            float maxSwingSpeed = 5f; // Điều chỉnh giá trị này phù hợp
            if (swingPlatformVelocity.magnitude > maxSwingSpeed)
            {
                swingPlatformVelocity = swingPlatformVelocity.normalized * maxSwingSpeed;
            }

            totalMovement += swingPlatformVelocity * Time.deltaTime;
        }


        // Thêm chuyển động của mặt phẳng di chuyển (nếu có)
        if (currentMovingPlatform != null)
        {
            Vector3 movingPlatformVelocity = currentMovingPlatform.GetPlatformVelocity();

            if (!isGrounded)
            {
                // Loại bỏ vận tốc thẳng đứng của mặt phẳng khi nhân vật đang trên không
                movingPlatformVelocity.y = 0;
            }

            totalMovement += movingPlatformVelocity * Time.deltaTime;
        }

        // Di chuyển nhân vật
        controller.Move(totalMovement);
    }

    void HandleGroundCheck()
    {
        // Thực hiện Raycast xuống dưới để kiểm tra mặt đất
        RaycastHit hit;
        float raycastDistance = controller.height / 2 + 0.1f;

        if (Physics.SphereCast(transform.position, controller.radius, Vector3.down, out hit, raycastDistance, groundMask))
        {
            isGrounded = true;

            // Kiểm tra nếu đang đứng trên mặt phẳng di chuyển
            if (hit.collider.CompareTag("MovingPlatform"))
            {
                currentMovingPlatform = hit.collider.GetComponent<PlaneMoveForward>();
                currentSwingPlatform = null;
            }
            // Kiểm tra nếu đang đứng trên cầu xoay
            else if (hit.collider.CompareTag("SwingPlatform"))
            {
                currentSwingPlatform = hit.collider.GetComponent<SwingColumn>();
                currentMovingPlatform = null;
            }
            else
            {
                currentMovingPlatform = null;
                currentSwingPlatform = null;
            }
        }
        else
        {
            isGrounded = false;
            currentMovingPlatform = null;
            currentSwingPlatform = null;
        }

        // Áp dụng giá trị vận tốc Y nhỏ để giữ nhân vật trên mặt đất
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Cập nhật tham số Animator cho trạng thái trên mặt đất
        animator.SetBool("isGrounded", isGrounded);
    }

    // Xử lý chuyển động của nhân vật
    void HandleMovement()
    {
        // Lấy input từ người chơi
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Tính toán hướng di chuyển dựa trên camera
        Vector3 direction = new Vector3(x, 0f, z).normalized;

        // Chỉ tiến hành nếu có input
        if (direction.magnitude >= 0.1f)
        {
            // Tính góc xoay mục tiêu dựa trên hướng di chuyển và camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // Xoay nhân vật theo hướng di chuyển
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Xác định hướng di chuyển
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Xác định tốc độ hiện tại (đi bộ hoặc chạy)
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? runSpeed : speed;

            // Xác định trạng thái Animator dựa trên hướng di chuyển
            if (z < 0) // Di chuyển lùi
            {
                currentSpeed = speed; // Tốc độ đi bộ cho di chuyển lùi
                animator.SetBool("isWalkingBackward", true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
            else // Di chuyển tiến
            {
                animator.SetBool("isWalkingBackward", false);
                animator.SetBool("isWalking", !isRunning);
                animator.SetBool("isRunning", isRunning);
            }

            // Lưu hướng di chuyển
            moveDirection = moveDir.normalized * currentSpeed;
        }
        else
        {
            // Không có input: chuyển sang trạng thái idle
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalkingBackward", false);

            // Đặt hướng di chuyển về zero
            moveDirection = Vector3.zero;
        }
    }

    // Xử lý nhảy
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isFalling)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
        }
    }

    // Áp dụng trọng lực
    void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("SwingPlatform"))
        {
            // Nếu nhân vật va chạm với cầu xoay
            if (hit.normal.y > 0.5f)
            {
                currentSwingPlatform = hit.collider.GetComponent<SwingColumn>();
            }
        }
        else
        {
            currentSwingPlatform = null;
        }
    }


    // Hàm xử lý trạng thái rơi và hồi sinh
    void HandleFallingAndRespawn()
    {
        // Kiểm tra trạng thái rơi
        if (!isGrounded && velocity.y < 0)
        {
            if (!isFalling)
            {
                StartFalling();
            }
        }
        else if (isGrounded)
        {
            if (isFalling)
            {
                StopFalling();
            }
        }

        // Kiểm tra nếu nhân vật rơi xuống quá thấp
        if (transform.position.y < -30f)
        {
            Respawn();
        }
    }

    void StartFalling()
    {
        isFalling = true;
        animator.SetBool("isFalling", true);
    }

    void StopFalling()
    {
        isFalling = false;
        animator.SetBool("isFalling", false);
    }

    void Respawn()
    {
        // Đưa nhân vật về vị trí ban đầu
        controller.enabled = false;
        transform.position = startingPosition;
        controller.enabled = true;

        // Reset vận tốc
        velocity = Vector3.zero;

        // Đặt trạng thái rơi về false
        isFalling = false;
        animator.SetBool("isFalling", false);
    }

    public void LaunchPlayer(float forceUp, float forceForward)
    {
        velocity.y = Mathf.Sqrt(forceUp * -2f * gravity);
        Vector3 forwardDirection = transform.forward;
        Vector3 horizontalVelocity = forwardDirection * forceForward;
        moveDirection += horizontalVelocity;
        animator.SetTrigger("Launch");
    }
}
