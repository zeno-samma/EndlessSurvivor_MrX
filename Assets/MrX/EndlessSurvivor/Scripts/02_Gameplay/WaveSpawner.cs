using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class WaveSpawner : MonoBehaviour
    {
        [Header("Stage & Spawn Configuration")]
        [Tooltip("Kéo file ScriptableObject chứa kịch bản của cả màn chơi vào đây.")]
        [SerializeField] private StageDataSO currentStageData;

        [Tooltip("Danh sách các điểm có thể spawn kẻ địch.")]
        [SerializeField] private Transform[] spawnPoints;
        [Tooltip("Thời gian đếm ngược chờ giữa các wave.")]
        [SerializeField] private float countdownBetweenWaves = 5f;
        // [SerializeField] private int m_CD_Nextwave = 5;
        // Thêm biến tổng số wave
        [SerializeField] public int TotalWavesInStage = 20;
        // public int CurrentWave { get; private set; }
        // public int enemyCount;// Dùng UniRx để tự động thông báo cho UI
        public ReactiveProperty<int> CurrentWaveNumber { get; private set; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> EnemiesKilledThisWave { get; private set; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<float> EnemyPoint { get; private set; } = new ReactiveProperty<float>(0f);
        public ReactiveProperty<int> TotalEnemiesInWave { get; private set; } = new ReactiveProperty<int>(0);
        public enum SpawnState
        {
            SPAWNING,      // Trạng thái đang tạo địch
            WAITING,       // Trạng thái đang chờ người chơi diệt hết địch
            COUNTING_DOWN  // Trạng thái đang đếm ngược tới wave tiếp theo
        }
        public SpawnState m_state;
        private void OnEnable()
        {

            // Đăng ký lắng nghe sự thay đổi trạng thái từ GameManager
            EventBus.Subscribe<StateUpdatedEvent>(OnGameStateChanged);//Lắng nghe trạng thái game do gamemanager quản lý
            // EventBus.Subscribe<EnemySpawnedEvent>(OnEnemySpawned);//Lắng nghe trạng thái game do gamemanager quản lý
            EventBus.Subscribe<EnemyDiedEvent>(OnEnemyDied); // << THÊM DÒNG NÀY
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<StateUpdatedEvent>(OnGameStateChanged);
            // EventBus.Unsubscribe<EnemySpawnedEvent>(OnEnemySpawned);
            EventBus.Unsubscribe<EnemyDiedEvent>(OnEnemyDied); // << THÊM DÒNG NÀY
        }

        private void OnEnemyDied(EnemyDiedEvent value)
        {
            if (m_state == SpawnState.WAITING || m_state == SpawnState.SPAWNING)
            {
                // Debug.Log($"EnemiesKilledThisWave: {EnemiesKilledThisWave.Value}");
                EnemiesKilledThisWave.Value++; // Tăng biến đếm khi có enemy chết
                EnemyPoint.Value = EnemiesKilledThisWave.Value * 0.2f;
                // Debug.Log($"EnemyPoint: {EnemyPoint.Value}");
            }
        }
        #region Unity Lifecycle & Event Subscription
        void Start()
        {
            if (currentStageData == null)
            {
                Debug.LogError("CHƯA GÁN StageDataSO CHO WAVESPAWNER!", this);
                this.enabled = false; // Vô hiệu hóa script này nếu thiếu file config
                return;
            }
            m_state = SpawnState.COUNTING_DOWN;
            SetupUniRxSubscriptions();
            StartNextWave();
        }
        void Update()
        {
            if (m_state == SpawnState.WAITING)
            {
                if (EnemyManager.Ins.activeEnemies.Count == 0)
                {
                    // Nếu không còn kẻ thù nào, wave đã hoàn thành!
                    if (CurrentWaveNumber.Value > 0 && CurrentWaveNumber.Value % 3 == 0)
                    {
                        // UpgradePhase();
                        // Debug.Log("UpgradePhase");
                        WaveCompleted();//Nếu không có UpgradePhase thì lập tức vào dòng này
                    }
                    else
                    {
                        WaveCompleted();
                    }
                }
            }

        }
        #endregion
        #region Core Spawning Logic

        public void StartNextWave()
        {
            // Lấy index của wave hiện tại (CurrentWaveNumber bắt đầu từ 1, index list bắt đầu từ 0)
            int waveIndex = CurrentWaveNumber.Value;

            if (waveIndex >= TotalWavesInStage)
            {
                Debug.Log("🎉 ĐÃ HOÀN THÀNH TẤT CẢ CÁC WAVE! Xử lý logic chiến thắng tại đây.");
                // TODO: Thêm logic chiến thắng
                return;
            }

            // Chuyển sang wave tiếp theo
            CurrentWaveNumber.Value++;

            // Lấy dữ liệu của wave từ ScriptableObject
            WaveData waveToSpawn = currentStageData.waves[waveIndex];

            // Reset các biến đếm cho wave mới
            EnemiesKilledThisWave.Value = 0;
            TotalEnemiesInWave.Value = waveToSpawn.GetTotalEnemiesInWave();

            // Bắt đầu coroutine để spawn quái
            StartCoroutine(SpawnWaveCoroutine(waveToSpawn));
        }

        private IEnumerator SpawnWaveCoroutine(WaveData waveData)
        {
            m_state = SpawnState.SPAWNING;
            Debug.Log($"Bắt đầu Wave {CurrentWaveNumber.Value}: '{waveData.waveName}' với {TotalEnemiesInWave.Value} kẻ địch.");

            // Vòng lặp chính: duyệt qua từng nhóm quái được định nghĩa trong wave
            foreach (var group in waveData.enemyGroups)
            {
                // Chờ trước khi bắt đầu nhóm mới (nếu có)
                if (group.delayBeforeSpawningGroup > 0)
                {
                    yield return new WaitForSeconds(group.delayBeforeSpawningGroup);
                }

                // Vòng lặp phụ: spawn từng con quái trong nhóm
                for (int i = 0; i < group.count; i++)
                {
                    SpawnAndRegister(group.enemyName);

                    // Chờ giữa mỗi lần spawn (nếu có)
                    if (group.spawnInterval > 0)
                    {
                        yield return new WaitForSeconds(group.spawnInterval);
                    }
                }
            }

            // Sau khi spawn hết tất cả các nhóm, chuyển sang trạng thái chờ
            m_state = SpawnState.WAITING;
            Debug.Log("Đã spawn xong. Chuyển sang trạng thái chờ người chơi dọn dẹp...");
        }

        private void SpawnAndRegister(string enemyName)
        {
            if (spawnPoints.Length == 0) return;

            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform randomSpawnPoint = spawnPoints[randomIndex];

            GameObject enemyObj = PoolManager.Ins.GetFromPool(enemyName, randomSpawnPoint.position);
            if (enemyObj == null)
            {
                Debug.LogWarning($"Không tìm thấy enemy có tên '{enemyName}' trong Pool!");
                return;
            }

            Enemy enemyScript = enemyObj.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                EnemyManager.Ins.RegisterEnemy(enemyScript);
            }
        }

        #endregion

        #region State & Event Handling

        void WaveCompleted()
        {
            Debug.Log($"✅ Wave {CurrentWaveNumber.Value} đã hoàn thành!");
            m_state = SpawnState.COUNTING_DOWN;

            // Bắt đầu đếm ngược cho wave tiếp theo
            StartCoroutine(CountdownCoroutine(countdownBetweenWaves));
        }

        private IEnumerator CountdownCoroutine(float duration)
        {
            Debug.Log($"Đếm ngược {duration} giây cho wave tiếp theo...");
            float timer = duration;
            while (timer > 0)
            {
                // Có thể phát đi event countdown để UI hiển thị nếu cần
                MessageBroker.Default.Publish(new WaveCountdownTickMessage { RemainingTime = timer });
                timer -= Time.deltaTime;
                yield return null;
            }

            StartNextWave();
        }



        private void OnGameStateChanged(StateUpdatedEvent evt)
        {
            if (evt.CurState == GameState.GAMEOVER)
            {
                StopAllCoroutines();
                this.enabled = false; // Ngừng hoạt động của spawner khi game over
                Debug.Log("WaveSpawner đã ngừng hoạt động do GAMEOVER.");
            }
        }

        private void SetupUniRxSubscriptions()
        {
            // UniRx để theo dõi tiến trình của wave hiện tại cho UI
            Observable.CombineLatest(
                EnemiesKilledThisWave,
                TotalEnemiesInWave,
                (killed, total) =>
                {
                    if (total <= 0) return 0f;
                    return (float)killed / total;
                })
                .Subscribe(currentProgress =>
                {
                    // Phát đi sự kiện chứa tiến trình của wave hiện tại
                    EventBus.Publish(new WaveProgressUpdatedEvent
                    {
                        progressPercentage = currentProgress,
                        currentWaveNumber = CurrentWaveNumber.Value
                    });
                })
                .AddTo(this);
        }

        #endregion
    }
}
