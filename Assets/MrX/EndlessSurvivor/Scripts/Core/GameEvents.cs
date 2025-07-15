using UnityEngine;

namespace MrX.EndlessSurvivor
{
    // Ví dụ một sự kiện không chứa dữ liệu
    public struct GameStartedEvent { }
    // Sự kiện này được phát đi khi Player đã sẵn sàng
    public struct PlayerSpawnedEvent
    {
        public Transform PlayerTransform; // Mang theo thông tin về Transform của Player
        public PlayerHealth HealthComponent;
    }
    public struct PlayerDiedEvent { }

    public struct StateUpdatedEvent
    {
        public GameManager.GameState CurState;
    }
    public struct PlayerHealthChangedEvent
    {
        public float NewHealthPercentage;
    }
    public struct EnemyDiedEvent
    {
        public int diecoin;
    }
    public struct InitialUIDataReadyEvent
    {
        // public int defHealth;
        // public int maxHealth;
        // public int defScore;
    }

}