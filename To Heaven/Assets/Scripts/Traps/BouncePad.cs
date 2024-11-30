using UnityEngine;

public class BouncingPad : MonoBehaviour
{
    public Transform springBase;  // Tham chiếu tới phần lò xo
    public float compressionDistance = 0.3f;  // Khoảng cách lún xuống khi nhân vật giẫm lên
    public float bounceForceUp = 15f;  // Lực bật lên trên của nhân vật
    public float bounceForceForward = 10f;  // Lực đẩy về phía trước của nhân vật
    public float compressionSpeed = 5f;  // Tốc độ lún xuống và bật lên

    private Vector3 initialPlatformPosition;
    private Vector3 initialSpringPosition;
    private bool isCompressing = false;
    private PlayerMovement player;

    void Start()
    {
        // Lưu lại vị trí ban đầu của tấm ván và lò xo
        initialPlatformPosition = transform.localPosition;
        if (springBase != null)
        {
            initialSpringPosition = springBase.localPosition;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu nhân vật giẫm lên
        if (other.CompareTag("Player"))
        {
            isCompressing = true;
            player = other.GetComponent<PlayerMovement>();

            // Áp dụng lực bật lên cho nhân vật sau một chút thời gian
            Invoke("LaunchPlayer", 0.2f);
        }
    }

    void LaunchPlayer()
    {
        if (player != null)
        {
            // Áp dụng lực bật lên và lực đẩy về phía trước
            player.LaunchPlayer(bounceForceUp, bounceForceForward);
        }
    }

    void Update()
    {
        if (isCompressing)
        {
            // Di chuyển tấm ván và lò xo xuống dưới
            Vector3 targetPlatformPosition = initialPlatformPosition - new Vector3(0, compressionDistance, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPlatformPosition, Time.deltaTime * compressionSpeed);

            if (springBase != null)
            {
                Vector3 targetSpringPosition = initialSpringPosition - new Vector3(0, compressionDistance, 0);
                springBase.localPosition = Vector3.Lerp(springBase.localPosition, targetSpringPosition, Time.deltaTime * compressionSpeed);
            }

            // Nếu đã lún đủ khoảng cách thì bật trở lại
            if (Vector3.Distance(transform.localPosition, targetPlatformPosition) < 0.01f)
            {
                isCompressing = false;
            }
        }
        else
        {
            // Bật tấm ván và lò xo trở lại vị trí ban đầu
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPlatformPosition, Time.deltaTime * compressionSpeed);
            
            if (springBase != null)
            {
                springBase.localPosition = Vector3.Lerp(springBase.localPosition, initialSpringPosition, Time.deltaTime * compressionSpeed);
            }
        }
    }
}