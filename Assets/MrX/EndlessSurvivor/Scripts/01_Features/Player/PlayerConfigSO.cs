using UnityEngine;

namespace MrX.EndlessSurvivor
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Survivor/Player Config")]
    public class PlayerConfigSO : ScriptableObject
    {
        
        public int initialGold = 0;
        public float initialDamage = 10f;
        public int initialHealth = 100;
        public int initialDef = 100;
        public float initialMoveSpeed = 5f;
        public float initialCooldown = 2f;
        public int healthBonusPerLevel = 50;
        public int defBonusPerLevel = 50;
        public float speedBonusPerLevel = 1f;
        public float damageBonusPerLevel = 2f;
        public float cooldownReductionPerLevel = 0.2f;
        // Có thể thêm prefab của người chơi ở đây nếu muốn
        // public GameObject playerPrefab;
    }
}
