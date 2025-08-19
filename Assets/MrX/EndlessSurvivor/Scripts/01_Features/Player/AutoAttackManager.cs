using UnityEngine;
using System.Collections.Generic; // Cần để dùng List
using System.Linq; // Cần để dùng LINQ cho tiện

namespace MrX.EndlessSurvivor
{

    public class AutoAttackManager : MonoBehaviour
    {
        private Transform currentTarget;
        private float findTargetTimer;
        private float findTargetInterval = 0.25f; // Tần suất quét tìm mục tiêu (4 lần/giây)

        // Tham chiếu đến các bộ phận khác của Player
        private WeaponManager weaponManager;
        public PlayerInfo playerInfo; // Tham chiếu đến script Aim
        // private WeaponAim weaponAim; // Giả sử bạn có script này để xoay vũ khí

        void Awake()
        {
            // Lấy các component cần thiết
            weaponManager = GetComponentInChildren<WeaponManager>();
            playerInfo = GetComponent<PlayerInfo>();
        }

        void Update()
        {
            // Script này chỉ hoạt động khi game đang ở trạng thái COMBAT
            // (Giả sử GameManager dùng UniRx, nếu không bạn có thể kiểm tra bằng cách khác)
            // if (GameManager.Ins.CurrentState.Value != GameManager.GameState.COMBAT) return;

            // Quét tìm mục tiêu theo một tần suất cố định để tối ưu
            findTargetTimer -= Time.deltaTime;
            if (findTargetTimer <= 0f)
            {
                FindClosestEnemy();
                findTargetTimer = findTargetInterval;
            }

            // Nếu đã có mục tiêu, thực hiện tấn công
            if (currentTarget != null)
            {
                // 1. Manager tính toán hướng đi cho mỗi con Enemy
                Vector3 direction = (currentTarget.position - this.transform.position).normalized;
                // Xoay Player
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Giả sử sprite của bạn hướng lên
                // Ra lệnh cho các bộ phận khác hành động
                // weaponAim.AimAt(currentTarget);
                weaponManager.Shoot(direction, playerInfo.CurrentDamage.Value);
                // Debug.Log("Tấn công");
            }
            else if (currentTarget == null)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Giả sử sprite của bạn hướng lên
            }
        }

        void FindClosestEnemy()
        {
            // Tự tìm tất cả các GameObject đang hoạt động có tag "Enemy"
            GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemyObjects.Length == 0)
            {
                currentTarget = null;
                return;
            }

            Transform closestTarget = null;
            float minDistance = 5f; // Khoảng cách quét tối đa

            // Duyệt qua mảng các đối tượng tìm được
            foreach (GameObject enemyObject in enemyObjects)
            {
                float distance = Vector3.Distance(transform.position, enemyObject.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTarget = enemyObject.transform;
                }
            }

            currentTarget = closestTarget;
        }
    }
}