using UnityEngine;

public class RotateBoardAroundCenter : MonoBehaviour
{
    public Transform rotateBoard; // Tấm ván đá
    public Transform centerPoint; // Điểm trung tâm
    public float rotationSpeed = 10f; // Tốc độ quay
    public Vector3 rotationAxis = Vector3.forward; // Trục quay (mặc định là Z)

    void Update()
    {
        if (rotateBoard != null && centerPoint != null)
        {
            // Lưu khoảng cách giữa tấm ván và tâm quay
            Vector3 offset = rotateBoard.position - centerPoint.position;

            // Xoay offset xung quanh tâm theo trục được chỉ định
            offset = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, rotationAxis) * offset;

            // Cập nhật vị trí mới của tấm ván
            rotateBoard.position = centerPoint.position + offset;

            // Đảm bảo tấm ván giữ hướng phù hợp
            rotateBoard.rotation = Quaternion.LookRotation(rotationAxis, centerPoint.position - rotateBoard.position);
        }
    }
}
