using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public enum GameState//Giá trị mặc định của enum là đầu tiên.
    {
        NONE, //Rỗng
        PREPAIR, //Đang chuẩn bị
        PLAYING,      // Đang chơi
        PAUSE,       // Dừng game
        UPGRADEPHASE,//Nâng cấp.
        GAMEOVER  // Thua cuộc
    }
    // Ví dụ một sự kiện không chứa dữ liệu
    public struct GameStartedEvent { }
    // Sự kiện này được phát đi khi Player đã sẵn sàng
    public struct PlayerSpawnedEvent
    {
        // public Transform PlayerTransform; // Mang theo thông tin về Transform của Player
        // public PlayerHealth HealthComponent;
        public GameObject playerObject;
    }
    public struct PlayerDiedEvent { }

    public struct StateUpdatedEvent
    {
        public GameState CurState;
    }
    public struct PlayerHealthChangedEvent
    {
        public float NewHealthPercentage;
    }
    public struct EnemyDiedEvent
    {
        public int diecoin;
        public GameObject deadEnemyObject; // << THÊM DÒNG NÀY
    }
    public struct InitialUIDataReadyEvent
    {
        // public int defHealth;
        // public int maxHealth;
        // public int defScore;
    }
    // Test asmdef;
}