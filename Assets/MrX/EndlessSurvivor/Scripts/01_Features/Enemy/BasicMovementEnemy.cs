using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class BasicMovementEnemy : BaseEnemy
    {
        [Header("Ranged Specific Stats")]
        // [SerializeField] private float stoppingDistance = 5f; // Khoảng cách sẽ dừng lại để bắn
        // [SerializeField] private float fireRate = 2f; // Tần suất bắn (giây/viên)
        [SerializeField] private GameObject projectilePrefab; // Prefab của viên đạn

        // private float fireTimer; // Bộ đếm thời gian để bắn
        // [Header("AI Behavior")]
        // [SerializeField] private float timeBetweenDecisions = 2f; // Cứ mỗi 2 giây sẽ ra quyết định mới
        // Override: Viết đè lên phương thức của lớp cha (BaseEnemy)
        protected override void Move()
        {
            if (playerTransform == null) return;

            // 1. Tính toán hướng tới người chơi (không thay đổi)
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

            // 2. Xoay cho "đầu" của enemy (trục Y) hướng về phía người chơi
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Dùng -90 độ vì sprite của bạn hướng lên

            // 3. Di chuyển về "phía trước" THEO HƯỚNG MẶT MỚI
            // transform.up chính là trục Y (phía trước) của enemy sau khi đã xoay
            transform.position += transform.up * moveSpeed * Time.deltaTime;
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
