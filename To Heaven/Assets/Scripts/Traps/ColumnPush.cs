using UnityEngine;

public class ColumnPush : MonoBehaviour
{
    public float pushDistance = 2f;  // Khoảng cách trụ đẩy theo trục Y
    public float speed = 2f;         // Tốc độ di chuyển của trụ
    public float minWaitTime = 1f;   // Thời gian chờ tối thiểu giữa mỗi lần đẩy
    public float maxWaitTime = 4f;   // Thời gian chờ tối đa giữa mỗi lần đẩy

    private Vector3 originalPosition;  // Vị trí ban đầu của trụ
    private float waitTime;            // Thời gian chờ trước mỗi lần đẩy trụ

    void Start()
    {
        originalPosition = transform.position; // Lưu lại vị trí ban đầu của trụ
        waitTime = Random.Range(minWaitTime, maxWaitTime); // Thiết lập thời gian chờ ngẫu nhiên cho mỗi trụ
        StartCoroutine(PushColumn()); // Bắt đầu Coroutine để di chuyển trụ
    }

    System.Collections.IEnumerator PushColumn()
    {
        while (true)
        {
            // Chờ trước khi bắt đầu đẩy
            yield return new WaitForSeconds(waitTime);

            // Đẩy trụ lên theo trục Y
            Vector3 targetPosition = originalPosition + transform.up * pushDistance;
            yield return MoveToPosition(targetPosition);

            // Chờ trước khi thu lại trụ
            yield return new WaitForSeconds(waitTime);

            // Thu trụ lại vị trí ban đầu
            yield return MoveToPosition(originalPosition);
        }
    }

    System.Collections.IEnumerator MoveToPosition(Vector3 target)
    {
        // Di chuyển tới vị trí mục tiêu cho đến khi gần đủ
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null; // Đợi tới khung hình tiếp theo
        }
    }
}
