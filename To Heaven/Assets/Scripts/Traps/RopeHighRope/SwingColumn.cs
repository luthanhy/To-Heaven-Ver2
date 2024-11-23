using UnityEngine;

public class SwingColumn : MonoBehaviour
{
    public float rotationSpeed = 30f; // Tốc độ xoay (độ/giây)

    // Hàm tính vận tốc góc (rad/s)
    public Vector3 GetAngularVelocity()
    {
        return Vector3.up * rotationSpeed * Mathf.Deg2Rad;
    }

    // Hàm tính vận tốc tuyến tính tại một điểm do xoay
    public Vector3 GetPlatformVelocity(Vector3 point)
    {
        Vector3 angularVelocity = GetAngularVelocity();
        Vector3 r = point - transform.position;
        Vector3 velocity = Vector3.Cross(angularVelocity, r);
        return velocity;
    }

    void Update()
    {
        // Xoay cầu quanh trục Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
