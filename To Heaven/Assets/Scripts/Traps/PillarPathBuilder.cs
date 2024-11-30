using UnityEngine;

public class PillarPathBuilder : MonoBehaviour
{
    public GameObject pillarPrefab;          // Prefab của trụ
    public int pillarCount = 20;             // Số lượng trụ
    public float distanceBetweenPillars = 3f; // Khoảng cách giữa các trụ (theo phương ngang)
    public float heightIncrement = 0.5f;     // Độ tăng chiều cao giữa các trụ
    public float maxTiltAngle = 15f;         // Góc nghiêng tối đa (độ)

    void Start()
    {
        if (pillarPrefab == null)
        {
            Debug.LogError("Pillar Prefab is not assigned!");
            return;
        }

        Vector3 currentPosition = transform.position;
        Vector3 direction = transform.forward.normalized; // Hướng đi của đường

        for (int i = 0; i < pillarCount; i++)
        {
            // Tạo góc nghiêng ngẫu nhiên
            float tiltAngle = Random.Range(-maxTiltAngle, maxTiltAngle);

            // Áp dụng góc nghiêng vào trục Z để nghiêng trái phải
            Quaternion tiltRotation = Quaternion.Euler(tiltAngle, 0, 0);

            // Tạo trụ
            GameObject pillar = Instantiate(pillarPrefab, currentPosition, tiltRotation, transform);

            // Cập nhật vị trí cho trụ tiếp theo
            currentPosition += direction * distanceBetweenPillars;
            currentPosition.y += heightIncrement; // Tăng chiều cao
        }
    }
}
