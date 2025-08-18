using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class Projectile : MonoBehaviour
    {
        // --- CÁC BIẾN CẦN THIẾT ---
        [SerializeField] private float speed = 10f;
        // [SerializeField] private int damage = 1;
        [SerializeField] private float lifetime = 2f; // Thời gian tồn tại tối đa để tránh đạn bay vô tận
        private float timelast;

        // --- HÀM START ---
        void Start()
        {

        }
        void Update()
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            Desable();
        }
        public void SetDirection(Vector3 newDirection)
        {
            float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle); // Giả sử sprite của bạn hướng lên
        }
        void Desable()
        {
            if (Time.time > timelast)
            {
                timelast = Time.time + lifetime;
                gameObject.SetActive(false);
            }
        }
        // --- HÀM VA CHẠM (TRIGGER) ---
        private void OnTriggerEnter2D(Collider2D other)
        {
            // // Kiểm tra xem đã va chạm với ai
            // if (other.CompareTag("Player"))
            // {
            //     // Lấy component của Player và gọi hàm trừ máu
            //     // Player target = other.GetComponent<Player>();
            //     // target.TakeDamage(damage);

            //     // Sau khi gây sát thương, biến mất
            //     // Destroy(gameObject); hoặc ReturnToPool();
            // }
            // else if (other.CompareTag("Wall"))
            // {
            //     // Nếu chỉ va vào tường, biến mất ngay lập tức
            //     // Destroy(gameObject); hoặc ReturnToPool();
            // }
        }
    }

}
