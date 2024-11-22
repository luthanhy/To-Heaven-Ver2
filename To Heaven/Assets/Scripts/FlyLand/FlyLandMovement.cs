using UnityEngine;

public class FlyLandMovement : MonoBehaviour
{
    public float speed = 2f; // Tốc độ bay lên/xuống
    public float height = 2f; // Độ cao bay lên/xuống

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Lưu vị trí ban đầu
    }

    void Update()
    {
        // Thay đổi vị trí theo dạng sóng sin
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
