using UnityEngine;

public class StickToSurface : MonoBehaviour
{
    private Transform originalParent; // Để lưu trạng thái cha ban đầu của nhân vật

    private void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra nếu nhân vật chạm vào cột
        if (collision.gameObject.CompareTag("Player"))
        {
            // Gắn nhân vật vào cột
            collision.transform.SetParent(transform);

            // Đặt vị trí nhân vật sát với bề mặt cột
            Vector3 closestPoint = collision.contacts[0].point; // Lấy điểm va chạm gần nhất
            collision.transform.position = closestPoint;

            // Xoay nhân vật theo hướng bề mặt của cột
            Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);
            collision.transform.rotation = surfaceRotation;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Khi nhân vật rời khỏi cột
        if (collision.gameObject.CompareTag("Player"))
        {
            // Hủy cha và trả nhân vật về vị trí tự do
            collision.transform.SetParent(null);
        }
    }
}
