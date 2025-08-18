using UnityEngine;

namespace MrX.EndlessSurvivor
{
    // abstract class không thể được gắn trực tiếp vào GameObject.
    // Nó phải được kế thừa bởi một class khác.
    public abstract class BaseEnemy : MonoBehaviour
    {
        [Header("Base Stats")]
        [SerializeField] protected float maxHealth = 10f;
        [SerializeField] protected float moveSpeed = 2f;
        [SerializeField] protected int damage = 1;
        [SerializeField] protected int experienceDropped = 5;

        protected float currentHealth;
        protected Transform playerTransform; // Để lưu vị trí người chơi

        // Awake được gọi trước Start
        protected virtual void Awake()
        {
            currentHealth = maxHealth;
        }

        // Start được gọi sau Awake
        protected virtual void Start()
        {
            // Tìm và lưu vị trí của người chơi để tối ưu
            // Giả sử người chơi có tag là "Player"
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Update được gọi mỗi frame
        protected virtual void Update()
        {
            // Nếu không tìm thấy người chơi thì không làm gì cả
            if (playerTransform == null) return;

            Move();
        }

        // Phương thức trừu tượng: Bất kỳ class nào kế thừa BaseEnemy
        // BẮT BUỘC phải viết lại logic cho phương thức này.
        protected abstract void Move();
        protected abstract void Attack();

        // Phương thức public để các đối tượng khác (viên đạn) có thể gọi
        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            // TODO: Thả ra điểm kinh nghiệm
            // TODO: Tạo hiệu ứng nổ/chết
            Destroy(gameObject);
        }
    }
}