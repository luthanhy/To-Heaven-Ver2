using UnityEngine;

public class RockPathGenerator : MonoBehaviour
{
    public GameObject stepPrefab; // Đối tượng bậc thang hoặc đá
    public int numberOfSteps = 10; // Số lượng bậc thang
    public float stepHeight = 1f; // Chiều cao cơ bản mỗi bậc
    public float stepDepth = 1f; // Khoảng cách tiến về phía trước cơ bản mỗi bậc
    public float horizontalRange = 1f; // Phạm vi ngẫu nhiên trên trục X (trái/phải)
    public float heightVariation = 0.2f; // Độ thay đổi chiều cao giữa các bậc
    public float depthVariation = 0.2f; // Độ thay đổi khoảng cách tiến về phía trước giữa các bậc

    void Start()
    {
        GeneratePath();
    }

    void GeneratePath()
    {
        Vector3 startPosition = transform.position;
        float accumulatedHeight = 0f;
        float accumulatedDepth = 0f;

        for (int i = 0; i < numberOfSteps; i++)
        {
            // Tạo vị trí ngẫu nhiên trên trục X cho mỗi bước
            float randomX = Random.Range(-horizontalRange, horizontalRange);

            // Tạo chiều cao và khoảng cách tiến về phía trước ngẫu nhiên cho mỗi bước
            float currentStepHeight = stepHeight + Random.Range(-heightVariation, heightVariation);
            float currentStepDepth = stepDepth + Random.Range(-depthVariation, depthVariation);

            accumulatedHeight += currentStepHeight;
            accumulatedDepth += currentStepDepth;

            Vector3 position = startPosition + new Vector3(randomX, accumulatedHeight, accumulatedDepth);
            Instantiate(stepPrefab, position, Quaternion.identity);
        }
    }
}
