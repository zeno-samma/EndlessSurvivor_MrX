using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class PlayerAim : MonoBehaviour
    {
        // Biến này sẽ được các script khác đọc
        public Vector3 AimDirection { get; private set; }

        // Update is called once per frame
        void Update()
        {
            // Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // mousePosition.z = 0;

            // // Tính toán hướng MỘT LẦN
            // Vector3 direction = (mousePosition - transform.position).normalized;
            // AimDirection = direction; // Lưu lại hướng cho các script khác dùng

            // // Xoay Player
            // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Giả sử sprite của bạn hướng lên
        }
    }
}

