using UnityEngine;

public class RockMovement : MonoBehaviour
{
    public float speed = 5f;
    public float verticalSpeed = 1f;
    public float maxDistance = 20f;
    private Vector3 startPosition;
    private bool isMovingForward = true;

    // Biến lưu vị trí và vận tốc
    private Vector3 lastPosition;
    private Vector3 platformVelocity;

    void Start()
    {
        startPosition = transform.position;
        lastPosition = transform.position;
    }

    void Update()
    {
        // Tính toán vector di chuyển tiến về phía trước và hướng lên trên
        Vector3 moveDirection = transform.right * speed * Time.deltaTime;
        Vector3 verticalMovement = transform.up * verticalSpeed * Time.deltaTime;
        Vector3 movement;

        if (isMovingForward)
        {
            // Cập nhật vị trí của đối tượng bằng cách cộng hai vector trên
            movement = moveDirection + verticalMovement;
            transform.position += movement;

            // Kiểm tra nếu mặt phẳng đã di chuyển quá khoảng cách tối đa
            if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
            {
                isMovingForward = false;
            }
        }
        else
        {
            // Di chuyển ngược lại về vị trí ban đầu
            movement = -(moveDirection + verticalMovement);
            transform.position += movement;

            // Kiểm tra nếu mặt phẳng đã quay lại vị trí ban đầu
            if (Vector3.Distance(startPosition, transform.position) <= 0.1f)
            {
                isMovingForward = true;
            }
        }

        // Tính toán vận tốc của mặt phẳng
        platformVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    public Vector3 GetPlatformVelocity()
    {
        return platformVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra va chạm với nhân vật
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Khi nhân vật rời khỏi vật thể, ngắt liên kết
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
