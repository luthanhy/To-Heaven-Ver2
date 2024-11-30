using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyLand : MonoBehaviour
{
    public float moveDistance = 5f; // Khoảng cách di chuyển sang bên phải
    public float moveSpeed = 2f; // Tốc độ di chuyển
    public bool moveBackAndForth = true; // Di chuyển qua lại

    private Vector3 startPosition;
    private bool movingRight = true;

    void Start()
    {
        // Lưu vị trí ban đầu của flyland
        startPosition = transform.position;
    }

    void Update()
    {
        // Tính toán vị trí đích
        Vector3 targetPosition;

        if (moveBackAndForth)
        {
            if (movingRight)
            {
                targetPosition = startPosition + new Vector3(moveDistance, 0, 0);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // Đổi hướng khi đến vị trí đích
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    movingRight = false;
                }
            }
            else
            {
                targetPosition = startPosition;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // Đổi hướng khi về vị trí ban đầu
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    movingRight = true;
                }
            }
        }
        else
        {
            // Chỉ di chuyển sang phải mà không quay về
            targetPosition = startPosition + new Vector3(moveDistance, 0, 0);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}