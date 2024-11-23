using UnityEngine;

public class ActivatesSwingTrap : MonoBehaviour
{
    public GameObject trap; // Tham chiếu tới GameObject của bẫy
    private SwingTrap swingTrap;

    private void Start()
    {
        if (trap != null)
        {
            swingTrap = trap.GetComponent<SwingTrap>();
            if (swingTrap != null)
            {
                swingTrap.enabled = false; // Tắt bẫy ban đầu
            }
        }
        else
        {
            Debug.LogError("Trap không được gán trong Inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Người chơi đã vào vùng Trigger!");
            if (swingTrap != null)
            {
                swingTrap.enabled = true; // Kích hoạt bẫy
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Người chơi đã rời vùng Trigger!");
            if (swingTrap != null)
            {
                swingTrap.enabled = false; // Tắt bẫy
            }
        }
    }
}
