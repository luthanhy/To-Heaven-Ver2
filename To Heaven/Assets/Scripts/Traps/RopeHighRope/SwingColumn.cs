using UnityEngine;

public class SwingColumn : MonoBehaviour
{
    public float swingSpeed = 2f; // Tốc độ đung đưa
    public float swingAngle = 30f; // Góc tối đa đung đưa

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime * swingSpeed;
        float angle = Mathf.Sin(timer) * swingAngle; // Dùng hàm Sin để tính góc đung đưa
        transform.rotation = Quaternion.Euler(new Vector3(angle, 0, 0)); // Xoay theo trục X
    }
}
