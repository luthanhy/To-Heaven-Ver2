using UnityEngine;

public class FireObstacleController : MonoBehaviour
{
    public ParticleSystem fireEffect;      // Tham chiếu đến Particle System của hiệu ứng lửa
    public float fireDuration = 2f;        // Thời gian lửa phun ra
    public float fireInterval = 3f;        // Thời gian chờ giữa các lần phun lửa

    private float timer;
    private bool isFiring = false;

    void Start()
    {
        // Bắt đầu với lửa tắt
        fireEffect.Stop();
        timer = fireInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (!isFiring && timer <= 0f)
        {
            StartFire();
        }
        else if (isFiring && timer <= 0f)
        {
            StopFire();
        }
    }

    void StartFire()
    {
        isFiring = true;
        fireEffect.Play();
        timer = fireDuration;
    }

    void StopFire()
    {
        isFiring = false;
        fireEffect.Stop();
        timer = fireInterval;
    }

    // Xử lý va chạm với người chơi
    private void OnTriggerEnter(Collider other)
    {
        if (isFiring && other.CompareTag("Player"))
        {
            // Gây sát thương hoặc xử lý khi người chơi chạm vào lửa
            Debug.Log("Player bị trúng lửa!");
            // Bạn có thể thêm mã để giảm máu, hồi sinh, v.v.
        }
    }
}
