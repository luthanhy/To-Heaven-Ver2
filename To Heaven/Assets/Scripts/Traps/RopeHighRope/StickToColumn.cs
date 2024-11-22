using UnityEngine;

public class StickToColumn : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Ki?m tra n?u nhân v?t va ch?m v?i c?t
        if (collision.gameObject.CompareTag("Player"))
        {
            // G?n nhân v?t làm con c?a c?t
            collision.transform.SetParent(transform);
            Debug.Log("Player stuck to column");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Khi nhân v?t r?i kh?i c?t
        if (collision.gameObject.CompareTag("Player"))
        {
            // H?y g?n cha-con
            collision.transform.SetParent(null);
            Debug.Log("Player detached from column");
        }
    }
}
