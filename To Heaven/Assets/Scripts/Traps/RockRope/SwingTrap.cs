using UnityEngine;

public class SwingTrap : MonoBehaviour
{
    public float swingSpeed = 1.5f; // Tốc độ đung đưa
    public float swingAngle = 45f; // Góc đung đưa tối đa (theo độ)
    
    private Quaternion startRotation; // Vị trí ban đầu
    private float time;

    void Start()
    {
        // Lưu trữ vị trí ban đầu
        startRotation = transform.localRotation;
    }

    void Update()
    {
        // Tính toán góc đung đưa theo thời gian (dùng hàm sin)
        float angle = Mathf.Sin(time * swingSpeed) * swingAngle;

        // Áp dụng góc quay cho quả cầu
        transform.localRotation = startRotation * Quaternion.Euler(angle, 0, 0);

        // Tăng thời gian
        time += Time.deltaTime;
    }
}