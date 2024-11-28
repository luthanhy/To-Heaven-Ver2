using UnityEngine;

public class StoneController : MonoBehaviour
{
    public float disappearDelay = 2.0f; // Thời gian trước khi đá biến mất
    public GameObject fractureEffectPrefab; // Prefab hiệu ứng tan vỡ

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            Invoke(nameof(TriggerEffect), disappearDelay);
        }
    }

    private void TriggerEffect()
    {
        // Hiển thị hiệu ứng tan vỡ
        if (fractureEffectPrefab != null)
        {
            Instantiate(fractureEffectPrefab, transform.position, transform.rotation);
        }

        // Ẩn hoặc xóa hòn đá
        Destroy(gameObject);
    }
}
