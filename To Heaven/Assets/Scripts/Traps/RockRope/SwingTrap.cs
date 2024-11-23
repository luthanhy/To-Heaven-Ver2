using UnityEngine;

public class SwingTrap : MonoBehaviour
{
    public float swingSpeed = 2f;  // Tốc độ đung đưa
    public float swingAngle = 45f; // Góc đung đưa tối đa

    private float currentAngle = 0f;
    private bool swingingForward = true;

    void Update()
    {
        float angleChange = swingSpeed * Time.deltaTime;
        if (swingingForward)
        {
            currentAngle += angleChange;
            if (currentAngle >= swingAngle)
            {
                swingingForward = false; // Đổi hướng
            }
        }
        else
        {
            currentAngle -= angleChange;
            if (currentAngle <= -swingAngle)
            {
                swingingForward = true; // Đổi hướng
            }
        }

        // Xoay quanh trục Z
        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
