using System.Collections;
using UnityEngine;

public class BreakPlatform : MonoBehaviour
{
    public GameObject[] fragments; // Danh sách các mảnh vỡ
    public Rigidbody playerRigidbody; // Rigidbody của người chơi
    public float fallDelay = 0.5f; // Thời gian trễ trước khi người chơi rơi
    public float fragmentFallDelay = 0.2f; // Thời gian trễ giữa các mảnh vỡ rơi

    private bool isActivated = false; // Kiểm tra xem mảnh đã rơi chưa

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated) // Kích hoạt nếu Player đứng lên
        {
            isActivated = true;
            StartCoroutine(BreakPlatformAndFall());
        }
    }

    IEnumerator BreakPlatformAndFall()
    {
        // Kích hoạt rơi từng mảnh với độ trễ
        foreach (GameObject fragment in fragments)
        {
            Rigidbody fragmentRigidbody = fragment.GetComponent<Rigidbody>();
            if (fragmentRigidbody != null)
            {
                fragmentRigidbody.isKinematic = false; // Cho phép mảnh rơi
                fragmentRigidbody.useGravity = true; // Bật trọng lực
            }
            yield return new WaitForSeconds(fragmentFallDelay);
        }

        // Sau khi các mảnh rơi, làm người chơi rơi xuống
        yield return new WaitForSeconds(fallDelay);
        playerRigidbody.useGravity = true; // Bật trọng lực cho người chơi
    }
}
