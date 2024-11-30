using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Nhân vật mà camera sẽ theo dõi
    public Vector3 offset = new Vector3(0, 2, -5); // Khoảng cách giữa camera và nhân vật
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Khóa con trỏ chuột
        yRotation = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        // Nhận giá trị từ chuột
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Cập nhật góc xoay dựa trên chuyển động chuột
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f); // Giới hạn góc nhìn lên/xuống

        // Tính toán hướng xoay
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // Cập nhật vị trí và hướng nhìn của camera
        transform.position = target.position + rotation * offset;
        transform.LookAt(target.position);
    }
}
