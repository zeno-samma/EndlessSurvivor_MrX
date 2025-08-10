using System;
using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class PlayerStats : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private void OnEnable()
        {
            EventBus.Subscribe<UpgradeChosenEvent>(OnUpgradeChosen);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<UpgradeChosenEvent>(OnUpgradeChosen);
        }

        private void OnUpgradeChosen(UpgradeChosenEvent value)
        {
            UpgradeData chosenUpgrade = value.selectedUpgrade;

            // Dùng switch...case để xem người chơi đã chọn loại nâng cấp nào
            switch (chosenUpgrade.type)
            {
                case UpgradeType.AttackDamage:
                    // Truy cập đến script quản lý sát thương và cộng thêm
                    // Ví dụ: weapon.damage += chosenUpgrade.value;
                    Debug.Log("Đã tăng Attack Damage thêm: " + chosenUpgrade.value);
                    break;

                case UpgradeType.MaxHealth:
                    // Truy cập đến PlayerHealth và cộng thêm
                    // Ví dụ: playerHealth.maxHealth += chosenUpgrade.value;
                    Debug.Log("Đã tăng Max Health thêm: " + chosenUpgrade.value);
                    break;

                case UpgradeType.FireRate:
                    // Truy cập đến WeaponManager và giảm cooldown
                    // Ví dụ: weaponManager.fireRate -= chosenUpgrade.value;
                    Debug.Log("Đã giảm Fire Rate đi: " + chosenUpgrade.value);
                    break;
            }
        }

    }
}

