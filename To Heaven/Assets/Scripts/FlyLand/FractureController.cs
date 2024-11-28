using UnityEngine;

public class FractureController : MonoBehaviour
{
    public GameObject fracturedObject; // Mô hình phân mảnh
    public GameObject intactObject;   // Mô hình nguyên vẹn

    public void TriggerFracture()
    {
        // Ẩn mô hình nguyên vẹn
        intactObject.SetActive(false);

        // Hiển thị mô hình phân mảnh
        fracturedObject.SetActive(true);

        // Kích hoạt Rigidbody cho từng mảnh
        Rigidbody[] pieces = fracturedObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in pieces)
        {
            rb.isKinematic = false; // Cho phép mảnh vỡ tương tác vật lý
            rb.AddExplosionForce(500f, transform.position, 5f); // Lực nổ
        }
    }
}
