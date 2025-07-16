using MRX.DefenseGameV1;
using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1;//
        [SerializeField] private float desableDelay = 4f;
        private float timelast;
        private Vector3 moveDirection;

        // Một hàm public để WeaponController có thể "ra lệnh"
        public void SetDirection(Vector3 newDirection)
        {
            moveDirection = newDirection;
            float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle); // Giả sử sprite của bạn hướng lên
        }

        // Update is called once per frame
        void Update()
        {
            // Chỉ cần di chuyển theo hướng đã được thiết lập
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            Desable();
        }
        void Desable()
        {
            if (Time.time > timelast)
            {
                timelast = Time.time + desableDelay;
                gameObject.SetActive(false);
            }
        }
        void OnTriggerEnter2D(Collider2D colTaget)
        {
            if (colTaget.CompareTag(Const.ENEMY_TAG))
            {
                // Khi game vừa bắt đầu, phát nhạc loading/menu
                AudioManager.Instance.PlaySFX(AudioManager.Instance.bulletHitSFX);
                // 2. Lấy script "Enemy" từ chính đối tượng vừa va chạm
                Enemy enemy = colTaget.GetComponent<Enemy>();

                // 3. Kiểm tra để chắc chắn là đã lấy được script (tránh lỗi null)
                if (enemy != null)
                {
                    // 4. Gọi thẳng hàm TakeDamage của chính con enemy đó
                    enemy.Health.TakeDamageEnemy(10);
                }
                gameObject.SetActive(false);
            }
        }
    }
}

