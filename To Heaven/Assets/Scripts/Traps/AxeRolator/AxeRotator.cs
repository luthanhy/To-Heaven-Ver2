using UnityEngine;

public class AxeRotator : MonoBehaviour
{
    // Tốc độ quay của rìu
    public float rotationSpeed = 200f;

    // Trục quay (X, Y, Z)
    public Vector3 rotationAxis = Vector3.right; // Quay theo chiều ngang (trục X)

    // Trục quay (Pivot Point)
    public Transform pivotPoint;

    void Update()
    {
        // Nếu pivotPoint được gán, tiến hành quay quanh trục
        if (pivotPoint != null)
        {
            // Quay quanh pivotPoint theo rotationAxis
            transform.RotateAround(pivotPoint.position, rotationAxis, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Pivot Point chưa được gán!");
        }
    }
}
