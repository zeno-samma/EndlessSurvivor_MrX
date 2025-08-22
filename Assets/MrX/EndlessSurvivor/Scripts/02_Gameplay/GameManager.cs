using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace MrX.EndlessSurvivor
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Ins;
        // [SerializeField] private int currentScore;
        // private PlayerData loadedPlayerData;
        // [SerializeField] private PlayerInfo playerInfo; // Kéo đối tượng Hero trong Scene vào đây

        private string saveFilePath;
        // private bool isDataDirty = false; // << "CỜ BẨN"
        public GameState CurrentState { get; private set; }
        private void OnEnable()
        {
            EventBus.Subscribe<PlayerSpawnedEvent>(OnPlayerSpawned);
            EventBus.Subscribe<PlayerDiedEvent>(GameOver);
            EventBus.Subscribe<PlayerLeveledUpEvent>(OnPlayerLeveledUp);
            EventBus.Subscribe<UpgradeChosenEvent>(OnUpgradeChosen);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<PlayerSpawnedEvent>(OnPlayerSpawned);
            EventBus.Unsubscribe<PlayerDiedEvent>(GameOver);
            EventBus.Unsubscribe<PlayerLeveledUpEvent>(OnPlayerLeveledUp);
            EventBus.Unsubscribe<UpgradeChosenEvent>(OnUpgradeChosen);
        }

        private void OnUpgradeChosen(UpgradeChosenEvent value)
        {
            UpdateGameState(GameState.PLAYING);
        }

        private void OnPlayerLeveledUp(PlayerLeveledUpEvent value)
        {

            Debug.Log("2.Lắng nghe vào UPGRADEPHASE");
            UpdateGameState(GameState.UPGRADEPHASE);

        }

        private void OnPlayerSpawned(PlayerSpawnedEvent value)
        {
            // Nhận Transform từ sự kiện và lưu lại
            // this.playerInfo = value.playerObject.GetComponent<PlayerInfo>();
            // // Debug.Log("GameManager đã nhận được tham chiếu đến PlayerHealth thành công!");
            // if (playerInfo != null && loadedPlayerData != null)
            // {
            //     Debug.Log("Ok");
            //     this.playerInfo.ApplyLoadedData(loadedPlayerData);
            // }
        }

        void Awake()
        {
            // Ra lệnh cho game chạy ở 60 FPS
            Application.targetFrameRate = 60;
            saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
            // Singleton Pattern
            if (Ins != null && Ins != this)
            {
                // Nếu log này xuất hiện, đây chính là lỗi của bạn
                Debug.LogError("GAME MANAGER BI HUY NHAM!", this.gameObject);
                Destroy(gameObject);
                return; // Thêm return để chắc chắn
            }
            else
            {
                Debug.Log("Game Manager khoi tao thanh cong", this.gameObject);
                Ins = this;
                DontDestroyOnLoad(gameObject); // Giữ GameManager tồn tại giữa các scene
            }
            // Chỉ đọc dữ liệu từ file và lưu lại, KHÔNG áp dụng cho player
            LoadDataFromFile();
        }
        // Hàm công khai để các script khác có thể "báo hiệu" có thay đổi
        // public void MarkDataAsDirty()
        // {
        //     isDataDirty = true;
        // }
        void Start()
        {
            Debug.Log("--- GameManager START CALLED! ---");
            // Khi game vừa bắt đầu, phát nhạc loading/menu
            // Kiểm tra AudioManager
            if (AudioManager.Instance == null)
            {
                Debug.LogError("LOI: AudioManager.Instance BI NULL!");
                return; // Dừng lại ở đây nếu có lỗi
            }
            Debug.Log("AudioManager OK.");
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
            // SceneManager.LoadScene("MainMenu");
            // Kiểm tra SceneLoader
            if (SceneLoader.Instance == null)
            {
                Debug.LogError("LOI: SceneLoader.Instance BỊ NULL!");
                return; // Dừng lại ở đây nếu có lỗi
            }
            Debug.Log("SceneLoader OK. Chuan bi tai MainMenu...");
            SceneLoader.Instance.LoadScene("MainMenu");
            // Bắt đầu game bằng trạng thái khởi tạo
            UpdateGameState(GameState.PREPAIR);
        }

        public void UpdateGameState(GameState newState)
        {
            // Tránh gọi lại logic nếu không có gì thay đổi
            if (newState == CurrentState)
            {
                Debug.Log("Trùng");
                return;
            }
            CurrentState = newState;
            // Xử lý logic đặc biệt ngay khi chuyển sang state mới
            switch (newState)
            {
                case GameState.PREPAIR:
                    // Debug.Log("code chuẩn bị game");
                    // ... code chuẩn bị game ...
                    // Sau khi chuẩn bị xong, tự động chuyển sang Playing
                    // EventBus.Publish(new InitialUIDataReadyEvent { defScore = Pref.coins });//Phát thông báo lần đầu để ui cập nhật lên màn hình đầu game.
                    break;
                case GameState.PLAYING:
                    Time.timeScale = 1f;
                    break;
                case GameState.PAUSE:
                    Time.timeScale = 0f;
                    break;
                case GameState.UPGRADEPHASE:
                    Debug.Log("3.vào UPGRADEPHASE");
                    Time.timeScale = 0f; // Dừng game để người chơi nâng cấp
                    break;
                case GameState.GAMEOVER:
                    Time.timeScale = 0f; // Dừng game
                    break;
            }

            // 4. Phát đi "báo cáo" về trạng thái mới cho các hệ thống khác lắng nghe
            // OnStateChanged?.Invoke(newState);
            EventBus.Publish(new StateUpdatedEvent { CurState = newState });//Phát thông báo lần đầu thay đổi state
            Debug.Log("Game state changed to: " + newState);
        }
        public void SaveGame()
        {

            // Chỉ thực hiện lưu nếu có thay đổi
            // if (!isDataDirty) return;
            // Debug.Log("Data was dirty, SAVING GAME...");
            // PlayerData dataToSave = playerInfo.GetDataToSave();
            // dataToSave.version = Application.version; // << LƯU PHIÊN BẢN HIỆN TẠI
            // dataToSave.gold = currentScore;

            // string json = JsonUtility.ToJson(dataToSave, true);
            // File.WriteAllText(saveFilePath, json);
            // Debug.Log("Lưu game với version: " + dataToSave.version);
            // // Sau khi lưu xong, reset cờ
            // isDataDirty = false;
        }

        public void LoadDataFromFile()
        {
            // if (File.Exists(saveFilePath))
            // {
            //     string json = File.ReadAllText(saveFilePath);
            //     loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);

            //     // --- LOGIC SO SÁNH PHIÊN BẢN ---
            //     if (loadedPlayerData.version != Application.version)
            //     {
            //         // Phiên bản của file save khác với phiên bản của game
            //         // -> Đây là bản build mới -> Reset dữ liệu
            //         Debug.Log("Phát hiện phiên bản trò chơi mới (" + Application.version + "). Dữ liệu lưu cũ từ phiên bản " + loadedPlayerData.version + " sẽ được đặt lại.");
            //         ResetAndCreateNewData();
            //     }
            //     else
            //     {
            //         // Cùng phiên bản, tải dữ liệu bình thường
            //         currentScore = loadedPlayerData.gold;
            //         Debug.Log("Đã tải trò chơi từ phiên bản: " + loadedPlayerData.version);
            //     }
            // }
            // else
            // {
            //     // Không có file save, tạo dữ liệu mới
            //     Debug.Log("Không tìm thấy tệp lưu, đang tạo dữ liệu mới.");
            //     playerInfo.ApplyLoadedData(new PlayerData());
            //     ResetAndCreateNewData();
            // }
        }

        // Hàm tạo dữ liệu mới và lưu lại ngay lập tức
        private void ResetAndCreateNewData()
        {
            // currentScore = 0;
            SaveGame(); // Gọi SaveGame để tạo file save mới với phiên bản hiện tại và score = 0
        }
        // Hàm đặc biệt của Unity
        void OnApplicationQuit()
        {
            Debug.Log("Application quitting...");
            SaveGame(); // Gọi hàm save lần cuối
        }
        public void PlayGame()///1.Sau khi ấn nút play
        {

            // Khi vào màn chơi, đổi sang nhạc gameplay
            AudioManager.Instance.PlayMusic(AudioManager.Instance.gameplayMusic);
            SceneLoader.Instance.LoadScene("Gameplay");
            UpdateGameState(GameState.PLAYING);
            EventBus.Publish(new EnemySpawnedEvent { });//Phát thông báo lần đầu thay đổi state
            // ActivePlayer();
        }
        public void ActivePlayer()
        {

            // if (m_curPlayer)
            //     Destroy(m_curPlayer.gameObject);

            // var shopItem = ShopController.Ins.items;
            // if (shopItem == null || shopItem.Length <= 0) return;

            // var newPlayerPb = shopItem[Pref.curPlayerId].playerPrefabs;
            // if (newPlayerPb)
            // {
            //     m_curPlayer = Instantiate(newPlayerPb, new Vector3(-6f, -1.7f, 0f), Quaternion.identity);
            // }
        }
        // public void AddScore(EnemyDiedEvent value)//Test UI
        // {
        //     // Debug.Log("DieGM");
        //     m_score += value.dieScore;
        //     // Debug.Log(value.dieScore);
        //     Pref.coins += value.dieScore;
        //     // Thay vì tự phát event, nó "gửi thông báo" đến EventBus
        //     EventBus.Publish(new ScoreUpdatedEvent { newScore = Pref.coins });//Phát thông báo kèm điểm khi tiêu diệt một enemy
        // }
        public void GameOver(PlayerDiedEvent value)//							   
        {

            UpdateGameState(GameState.GAMEOVER);
        }


















































    }
}