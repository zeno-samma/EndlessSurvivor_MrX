using System;
using UniRx;
using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class PlayerInfo : MonoBehaviour
    {
        public PlayerConfigSO playerConfig; //

        // Dữ liệu động của người chơi
        private float damageLevel;
        private int healthLevel;
        private int defLevel;
        private float speedLevel;
        private float cooldownLevel;
        private int currentGold;

        // --- Các thuộc tính (Properties) để tính toán chỉ số cuối cùng ---
        public float MaxDamage => playerConfig.initialDamage + (playerConfig.damageBonusPerLevel * damageLevel);
        public float MaxHealth => playerConfig.initialHealth + (playerConfig.healthBonusPerLevel * healthLevel);
        public int MaxDef => playerConfig.initialDef + (playerConfig.defBonusPerLevel * defLevel);
        public float MaxSpeed => playerConfig.initialMoveSpeed + (playerConfig.speedBonusPerLevel * speedLevel);
        public float MaxCooldown => playerConfig.initialCooldown - (playerConfig.cooldownReductionPerLevel * cooldownLevel);


        public ReactiveProperty<int> CurrentGold { get; private set; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<float> CurrentDamage { get; private set; } = new ReactiveProperty<float>(0f);
        public ReactiveProperty<float> CurrentHealth { get; private set; } = new ReactiveProperty<float>(1f);
        public ReactiveProperty<int> CurrentDef { get; private set; } = new ReactiveProperty<int>(10);
        public ReactiveProperty<float> CurrentSpeed { get; private set; } = new ReactiveProperty<float>(0f);
        public ReactiveProperty<float> CurrentCooldown { get; private set; } = new ReactiveProperty<float>(100f);
        // Hàm này sẽ được GameManager gọi khi load game xong
        void Awake()
        {
            CurrentDamage.Value = MaxDamage;
            CurrentHealth.Value = MaxHealth;
            CurrentDef.Value = MaxDef;
            CurrentSpeed.Value = MaxSpeed;
            CurrentCooldown.Value = MaxCooldown;
            Debug.Log($"MaxDamage: {CurrentDamage.Value}, MaxHealth: {CurrentHealth.Value}, MaxDef: {CurrentDef.Value}, MaxSpeed: {CurrentSpeed.Value}, MaxCooldown: {CurrentCooldown.Value}");
        }
        public void ApplyLoadedData(PlayerData data)
        {
            healthLevel = data.healthUpgradeLevel;
            damageLevel = data.damageUpgradeLevel;
            speedLevel = data.speedUpgradeLevel;
            cooldownLevel = data.cooldownUpgradeLevel;
            currentGold = data.gold;

            Debug.Log("Player data applied. Current Damage: " + MaxCooldown);
        }

        // Hàm này được GameManager gọi trước khi save game
        public PlayerData GetDataToSave()
        {
            PlayerData data = new PlayerData();
            data.healthUpgradeLevel = healthLevel;
            data.damageUpgradeLevel = damageLevel;
            data.speedUpgradeLevel = speedLevel;
            data.cooldownUpgradeLevel = cooldownLevel;
            data.gold = currentGold;
            return data;
        }

        // Ví dụ về việc nâng cấp
        public void UpgradeHealth()
        {
            // (Kiểm tra xem có đủ vàng không...)
            // healthLevel++;
            // (Trừ vàng...)
            // << BÁO HIỆU CHO GAMEMANAGER >>
            // GameManager.Ins.SaveGame();//Dùng cho ít lần thay đổi và các thay đổi quan trọng(Qua một chương, hoàn thành được thành tựu)
            // GameManager.Ins.MarkDataAsDirty();//Dùng cho trường hợp nhặt liên tục 10 coins
        }
    }
}

