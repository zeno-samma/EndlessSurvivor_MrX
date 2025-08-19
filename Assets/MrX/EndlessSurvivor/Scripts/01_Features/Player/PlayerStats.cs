using System;
using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class PlayerStats : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public PlayerInfo playerInfo;
        private void OnEnable()
        {
            EventBus.Subscribe<UpgradeChosenEvent>(OnUpgradeChosen);
            EventBus.Subscribe<EnemyDiedEvent>(OnEnemyDied);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<UpgradeChosenEvent>(OnUpgradeChosen);
            EventBus.Unsubscribe<EnemyDiedEvent>(OnEnemyDied);
        }

        private void OnEnemyDied(EnemyDiedEvent value)
        {
            playerInfo.CurrentGold.Value += value.diecoin;
        }

        private void OnUpgradeChosen(UpgradeChosenEvent value)
        {
            UpgradeData chosenUpgrade = value.selectedUpgrade;
            // Dùng switch...case để xem người chơi đã chọn loại nâng cấp nào
            switch (chosenUpgrade.type)
            {
                case UpgradeType.AttackDamage:
                    // Truy cập đến script quản lý sát thương và cộng thêm
                    playerInfo.CurrentDamage.Value += chosenUpgrade.value;
                    Debug.Log("Đã tăng Attack Damage thêm: " + chosenUpgrade.value);
                    break;

                case UpgradeType.MaxHealth:
                    // Truy cập đến PlayerHealth và cộng thêm
                    playerInfo.CurrentHealth.Value += chosenUpgrade.value;
                    Debug.Log("Đã tăng Max Health thêm: " + chosenUpgrade.value);
                    break;
                case UpgradeType.Def:
                    // Truy cập đến PlayerHealth và cộng thêm
                    playerInfo.CurrentDef.Value += (int)chosenUpgrade.value;
                    Debug.Log("Đã tăng Max Def thêm: " + chosenUpgrade.value);
                    break;

                case UpgradeType.MoveSpeed:
                    // Truy cập đến PlayerHealth và cộng thêm
                    playerInfo.CurrentSpeed.Value += chosenUpgrade.value;
                    Debug.Log("Đã tăng Max Health thêm: " + chosenUpgrade.value);
                    break;

                case UpgradeType.FireRate:
                    // Truy cập đến WeaponManager và giảm cooldown
                    playerInfo.CurrentHealth.Value -= chosenUpgrade.value;
                    Debug.Log("Đã giảm Fire Rate đi: " + chosenUpgrade.value);
                    break;
            }
        }

    }
}

