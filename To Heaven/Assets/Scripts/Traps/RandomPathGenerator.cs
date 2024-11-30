using UnityEngine;

public class RandomPathGenerator : MonoBehaviour
{
    public GameObject modelPrefab; // Prefab của model
    public int numberOfModels = 20; // Số lượng model
    public float verticalStep = 1f; // Khoảng cách đi lên (Y)
    public float forwardStep = 2f; // Khoảng cách tiến về phía trước (Z)
    public Vector2 rotationRange = new Vector2(-15f, 15f); // Góc xoay dọc
    public Vector2 tiltRange = new Vector2(-15f, 15f); // Góc nghiêng ngang
    public Transform startPoint; // Điểm bắt đầu

    void Start()
    {
        Vector3 spawnPosition = startPoint != null ? startPoint.position : transform.position;
        Quaternion spawnRotation = Quaternion.identity;

        for (int i = 0; i < numberOfModels; i++)
        {
            // Tạo model tại vị trí hiện tại
            GameObject model = Instantiate(modelPrefab, spawnPosition, spawnRotation);

            // Random hóa góc nghiêng và xoay
            float randomTilt = Random.Range(tiltRange.x, tiltRange.y); // Nghiêng ngang
            float randomRotation = Random.Range(rotationRange.x, rotationRange.y); // Xoay dọc
            model.transform.Rotate(randomTilt, 0, randomRotation); // Chỉnh đúng thứ tự trục xoay

            // Cập nhật vị trí cho model tiếp theo
            spawnPosition += -model.transform.forward * forwardStep; // Tiến tới phía trước
            spawnPosition += Vector3.up * verticalStep;             // Đi lên trên
        }
    }
}
