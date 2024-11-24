using UnityEngine;

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

    private SwingColumn currentPlatform; // Tham chiếu đến cầu xoay hiện tại

    // Biến lưu vị trí bắt đầu để hồi sinh
    private Vector3 startingPosition;

    // Biến kiểm tra trạng thái rơi
    private bool isFalling = false;

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
        MoveWithPlatform();
        HandleFallingAndRespawn(); // Thêm hàm xử lý rơi và hồi sinh
    }

    // Kiểm tra xem nhân vật có đang trên mặt đất không
    void HandleGroundCheck()
    {
        isGrounded = controller.isGrounded;

        // Cập nhật tham số Animator cho trạng thái trên mặt đất
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Giá trị nhỏ để giữ nhân vật trên mặt đất
        }
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

            // Di chuyển nhân vật
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
        else
        {
            // Không có input: chuyển sang trạng thái idle
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalkingBackward", false);
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
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Di chuyển nhân vật cùng với cầu xoay
    void MoveWithPlatform()
    {
        if (currentPlatform != null)
        {
            Vector3 platformVelocity = currentPlatform.GetPlatformVelocity(transform.position);

            // Giới hạn vận tốc tối đa
            float maxPlatformSpeed = 5f; // Bạn có thể điều chỉnh giá trị này
            if (platformVelocity.magnitude > maxPlatformSpeed)
            {
                platformVelocity = platformVelocity.normalized * maxPlatformSpeed;
            }

            // Áp dụng vận tốc đã được giới hạn lên nhân vật
            controller.Move(platformVelocity * Time.deltaTime);

            // Mô phỏng ma sát bằng cách giảm vận tốc ngang của nhân vật
            Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);
            float friction = 5f; // Giá trị ma sát, bạn có thể điều chỉnh
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, friction * Time.deltaTime);

            velocity.x = horizontalVelocity.x;
            velocity.z = horizontalVelocity.z;
        }
    }

    // Xử lý va chạm với cầu xoay
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Kiểm tra nếu nhân vật đang đứng trên cầu xoay (kiểm tra góc va chạm)
        if (hit.collider.CompareTag("SwingPlatform"))
        {
            // Kiểm tra góc va chạm để xác định xem nhân vật có đang đứng trên cầu không
            if (hit.normal.y > 0.5f)
            {
                currentPlatform = hit.collider.GetComponent<SwingColumn>();
            }
        }
        else
        {
            // Nếu không va chạm với cầu xoay, đặt currentPlatform về null
            currentPlatform = null;
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
        else
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
}
