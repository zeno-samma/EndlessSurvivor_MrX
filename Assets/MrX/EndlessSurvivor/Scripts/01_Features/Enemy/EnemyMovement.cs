using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class EnemyMovement : MonoBehaviour
    {
        public float moveSpeed;

        // Hàm này nhận lệnh từ "bộ não"
        public void Move(Vector3 direction)
        {
            // Chỉ thực hiện di chuyển, không suy nghĩ
            transform.position += direction * moveSpeed * Time.deltaTime;
            // Xoay Player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Giả sử sprite của bạn hướng lên
        }
    }
}
