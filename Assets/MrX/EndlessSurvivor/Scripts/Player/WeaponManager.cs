using System;
using System.Numerics;
using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class WeaponManager : MonoBehaviour
    {
        private Camera mainCam;
        // private Vector3 mousePos;
        [SerializeField] private GameObject bulletPrefabs;
        [SerializeField] private Transform firePos;
        [SerializeField] private float shotDelay = 0.15f;
        [SerializeField] private int maxAmo = 24;
        private PlayerAim playerAim; // Tham chiếu đến script Aim
        public int currentAmo;
        private float nextShot;
        void Awake()
        {
            // Lấy tham chiếu đến script cha
            playerAim = GetComponentInParent<PlayerAim>();
        }
        void Start()
        {
            currentAmo = maxAmo;
            mainCam = Camera.main; // Lấy camera chính của game
        }
        // public bool IsComponentNull()
        // {
        //     return m_anim == null;
        // }
        void Update()
        {
            // Reload();
        }
        public void Shoot(UnityEngine.Vector3 currentTarget)
        {
            // Đọc hướng trực tiếp, không cần tính toán lại
            // Vector3 shootDirection = playerAim.AimDirection;
            if (Time.time > nextShot)
            {
                // Khi game vừa bắt đầu, phát nhạc loading/menu
                // AudioManager.Instance.PlaySFX(AudioManager.Instance.shootSFX);
                nextShot = Time.time + shotDelay;
                GameObject bulletObj = PoolManager.Ins.GetFromPool("PlayerBullet", firePos.position);
                Bullet bulletScript = bulletObj.GetComponent<Bullet>();
                // 4. "Ra lệnh" cho viên đạn bay theo hướng đã tính
                bulletScript.SetDirection(currentTarget);
                currentAmo--;
            }
        }
        void Reload()
        {
            if (Input.GetMouseButtonDown(1) && currentAmo <= 0)
            {
                currentAmo = maxAmo;
            }
        }
    }
}

