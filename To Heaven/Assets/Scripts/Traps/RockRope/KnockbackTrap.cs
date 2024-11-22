using UnityEngine;

public class KnockbackTrap : MonoBehaviour
{
    public float knockbackForce = 50f; // Lực hất văng (tăng giá trị để đẩy xa hơn)
    public float upwardForce = 10f; // Lực đẩy lên (để tạo hiệu ứng văng lên trên)

    private void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra nếu va chạm với người chơi
        if (collision.gameObject.CompareTag("Player"))
        {
            // Lấy Rigidbody của người chơi
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Tính toán hướng đẩy (từ đá tới người chơi)
                Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;

                // Thêm lực đẩy theo hướng knockback và lực nâng lên
                Vector3 force = knockbackDirection * knockbackForce + Vector3.up * upwardForce;
                playerRb.AddForce(force, ForceMode.Impulse);

                Debug.Log("Người chơi bị đẩy văng ra rất xa!");
            }
        }
    }
}
