using UnityEngine;

public class RotateAroundCenter : MonoBehaviour
{
    public Transform centerPoint; // Điểm trung tâm để xoay quanh
    public float rotationSpeed = 100f; // Tốc độ xoay
    public bool clockwise = true; // Xoay theo chiều kim đồng hồ

    void Update()
    {
        if (centerPoint == null)
        {
            Debug.LogWarning("Center point is not set!");
            return;
        }

        // Tính toán hướng xoay (clockwise hoặc counterclockwise)
        float direction = clockwise ? -1f : 1f;

        // Xoay đối tượng quanh trục Z của centerPoint
        transform.RotateAround(centerPoint.position, Vector3.forward, rotationSpeed * direction * Time.deltaTime);
    }
}
