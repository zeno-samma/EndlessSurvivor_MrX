using UnityEngine;
using UniRx;

namespace MrX.EndlessSurvivor
{
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        // === DỮ LIỆU ===
        [SerializeField]private PlayerInfo playerInfo;

        public void TakeDamage(float damage)//Player nhận sát thương từ enemy
        {
            // Đảm bảo máu không âm
            if (playerInfo.CurrentHealth.Value < 0)
            {
                playerInfo.CurrentHealth.Value = 0;
            }
            // Kiểm tra nếu đã chết
            if (playerInfo.CurrentHealth.Value == 0)
            {
                // int coinBonus = UnityEngine.Random.Range(minCoinBonus, maxCoinBonus);
                Debug.Log("Phát event player chết");
                // gameObject.SetActive(false);
                EventBus.Publish(new PlayerDiedEvent { });
                return;
            }
            // Debug.Log("TakeDamage: " + damage);
            if (playerInfo.CurrentHealth.Value <= 0) return; // Nếu đã chết rồi thì không nhận thêm sát thương
            // currentHealth -= damage;
            float CurrentDamage = damage - playerInfo.CurrentDef.Value;
            if (CurrentDamage < 1)
            {
                CurrentDamage = 1;
            }
            playerInfo.CurrentHealth.Value -= CurrentDamage;
            Debug.Log($"currentHealth: {playerInfo.CurrentHealth.Value}");
        }
    }
}

