using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class RangedEnemy : BaseEnemy
    {
        [Header("Ranged Specific Stats")]
        [SerializeField] private float stoppingDistance = 5f; // Khoảng cách sẽ dừng lại để bắn
        [SerializeField] private float fireRate = 2f; // Tần suất bắn (giây/viên)
        [SerializeField] private GameObject projectilePrefab; // Prefab của viên đạn

        private float fireTimer; // Bộ đếm thời gian để bắn
        [Header("AI Behavior")]
        [SerializeField] private float timeBetweenDecisions = 2f; // Cứ mỗi 2 giây sẽ ra quyết định mới
        private float decisionTimer;

        private Vector2 strafeDirection; // Hướng di chuyển ngang sẽ được chọn ngẫu nhiên
        // Override: Viết đè lên phương thức của lớp cha (BaseEnemy)
        protected override void Move()
        {
            // Tính khoảng cách tới người chơi
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            // Nếu ở ngoài tầm bắn, tiến lại gần
            if (distanceToPlayer > stoppingDistance)
            {
                // Di chuyển về hướng người chơi
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
                // 1. Manager tính toán hướng đi cho mỗi con Enemy
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                // Xoay Player
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Giả sử sprite của bạn hướng lên
                // Reset timer để khi vào tầm là ra quyết định ngay
                decisionTimer = 0;
            }
            // Nếu đã ở trong tầm bắn, dừng lại và tấn công
            else
            {
                // Cập nhật bộ đếm thời gian
                fireTimer += Time.deltaTime;
                // Đếm ngược timer ra quyết định
                decisionTimer -= Time.deltaTime;
                if (decisionTimer <= 0)
                {
                    // Đã đến lúc ra quyết định mới
                    ChooseNewAction();
                    // Reset lại timer cho lần quyết định sau
                    decisionTimer = timeBetweenDecisions;
                }

                // Thực hiện hành động di chuyển ngang
                PerformStrafe();
                if (fireTimer >= fireRate)
                {
                    // Attack();
                    fireTimer = 0f; // Reset bộ đếm
                }
            }
        }
        // Hàm để chọn một hành động mới một cách ngẫu nhiên
        private void ChooseNewAction()
        {
            // Chọn một trong 3 hướng: -1 (trái), 0 (đứng im), 1 (phải)
            int randomChoice = Random.Range(-1, 2);

            // Tính toán vector di chuyển ngang (vuông góc với hướng tới người chơi)
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            Vector2 perpendicularDirection = new Vector2(-directionToPlayer.y, directionToPlayer.x);
            // // Xoay Player
            // float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Giả sử sprite của bạn hướng lên
            // Lưu lại hướng di chuyển đã chọn
            strafeDirection = perpendicularDirection * randomChoice;
        }

        // Hàm để thực hiện việc di chuyển ngang
        private void PerformStrafe()
        {
            // Di chuyển theo hướng đã được chọn trong ChooseNewAction()
            transform.Translate(strafeDirection * moveSpeed * Time.deltaTime);
        }
        protected override void Attack()
        {
            if (projectilePrefab == null)
            {
                Debug.LogError("Projectile Prefab is not set on " + gameObject.name);
                return;
            }

            // --- LOGIC QUAN TRỌNG ĐỂ XOAY VIÊN ĐẠN ĐÚNG HƯỚNG ---
            // 1. Tính toán vector hướng từ kẻ địch tới người chơi
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            // 2. Tính toán góc xoay từ vector hướng đó
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            // 3. Dùng Instantiate để tạo ra viên đạn tại vị trí của kẻ địch
            // và với góc xoay đã tính toán
            // GameObject RangedEnemyObj = PoolManager.Ins.GetFromPool("EnemyProjectile", transform.position);
            // Projectile ProjectileScript = RangedEnemyObj.GetComponent<Projectile>();
            // 4. "Ra lệnh" cho viên đạn bay theo hướng đã tính
            // ProjectileScript.SetDirection(direction);
            // Instantiate(projectilePrefab, transform.position, rotation);
            Debug.Log(gameObject.name + " is attacking the player!");
        }
    }
}

