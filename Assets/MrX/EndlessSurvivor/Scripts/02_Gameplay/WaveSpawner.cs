using System.Collections;
using UniRx;
using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private int m_CD_Nextwave = 5;
        // Thêm biến tổng số wave
        [SerializeField] private int totalWavesInStage = 5;
        public int CurrentWave { get; private set; }
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
            EventBus.Subscribe<StateUpdatedEvent>(SpawnEnemiesState);//Lắng nghe trạng thái game do gamemanager quản lý
            EventBus.Subscribe<EnemySpawnedEvent>(OnEnemySpawned);//Lắng nghe trạng thái game do gamemanager quản lý
            EventBus.Subscribe<EnemyDiedEvent>(OnEnemyDied); // << THÊM DÒNG NÀY
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<StateUpdatedEvent>(SpawnEnemiesState);
            EventBus.Unsubscribe<EnemySpawnedEvent>(OnEnemySpawned);
            EventBus.Unsubscribe<EnemyDiedEvent>(OnEnemyDied); // << THÊM DÒNG NÀY
        }

        private void OnEnemyDied(EnemyDiedEvent value)
        {
            // Debug.Log($"EnemiesKilledThisWave: {EnemiesKilledThisWave.Value}");
            EnemiesKilledThisWave.Value++; // Tăng biến đếm khi có enemy chết
            EnemyPoint.Value = EnemiesKilledThisWave.Value * 0.2f;
            // Debug.Log($"EnemyPoint: {EnemyPoint.Value}");
        }

        void Start()
        {
            m_state = SpawnState.COUNTING_DOWN;
            StartNextWave();
            // Dùng UniRx để lắng nghe sự thay đổi của chính nó
            Observable.CombineLatest(
                    EnemiesKilledThisWave,
                    TotalEnemiesInWave,
                    CurrentWaveNumber,
                    (killed, total, waveNum) => // "Công thức trộn" mới
                    {
                        if (total <= 0 || totalWavesInStage <= 0) return 0f;

                        // 1. Tính tiến trình của các wave đã hoàn thành
                        // Ví dụ: đang ở wave 3 -> (3-1)/5 = 0.4 (40%)
                        float progressOfPreviousWaves = (float)(waveNum - 1) / totalWavesInStage;

                        // 2. Tính tiến trình nhỏ của wave hiện tại
                        // Ví dụ: giết 2/10 quái ở wave 3 -> (2/10) * (1/5) = 0.04 (4%)
                        float progressOfCurrentWave = ((float)killed / total) / totalWavesInStage;

                        // 3. Cộng dồn lại
                        return progressOfPreviousWaves + progressOfCurrentWave;
                    })
                    .Subscribe(overallProgress =>
                    {
                        // Phát đi sự kiện chứa tiến trình tổng
                        EventBus.Publish(new WaveProgressUpdatedEvent { progressPercentage = overallProgress });
                    })
                    .AddTo(this);
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
        private void SpawnEnemiesState(StateUpdatedEvent Value)
        {
            if (Value.CurState == GameState.GAMEOVER)
            {
                // Debug.Log("Vào đây");
                StopAllCoroutines();
                Time.timeScale = 0f;
            }
        }
        private void OnEnemySpawned(EnemySpawnedEvent Value)
        {
            Debug.Log("GameStart...!");
            StartNextWave();

        }
        // Hàm được gọi khi wave được xác nhận là đã sạch
        void WaveCompleted()
        {
            Debug.Log("Wave " + CurrentWaveNumber.Value + " đã hoàn thành!");
            // EventBus.Publish(new CountdownNextWave {cooldownDuration = m_CD_Nextwave});
            // Chuyển sang trạng thái đếm ngược cho wave tiếp theo
            m_state = SpawnState.COUNTING_DOWN;
            // Bắt đầu bộ đếm ngược (ví dụ, gọi hệ thống CountdownTimer đã thiết kế)
            StartNewCountdown(m_CD_Nextwave);
        }
        private void StartNewCountdown(float duration)////duration thời gian cd mỗi wave enemy
        {
            StartCoroutine(CountdownCoroutine(duration));
        }
        private IEnumerator CountdownCoroutine(float duration)
        {
            float timer = duration;

            // Bắt đầu vòng lặp đếm ngược
            while (timer > 0)
            {
                // Giảm thời gian
                // Debug.Log($"CurrentTime: {timer}");
                MessageBroker.Default.Publish(new WaveCountdownTickMessage { RemainingTime = timer });
                timer -= Time.deltaTime;
                yield return null;
            }

            // Phát event báo hiệu đã hoàn thành
            // Debug.Log("Countdown Finished!");
            StartNextWave();
            yield break; // Kết thúc coroutine cho wave này
        }
        public void StartNextWave()
        {
            CurrentWaveNumber.Value++;

            Debug.Log("Chuẩn bị cho Wave: " + CurrentWaveNumber.Value);

            // DÙNG SWITCH...CASE ĐỂ QUYẾT ĐỊNH LOGIC CHO TỪNG WAVE
            switch (CurrentWaveNumber.Value)
            {
                case 1:
                    // --- Điều kiện cho Wave 1 ---"Làm Quen Vũ Khí"
                    Debug.Log("Kịch bản Wave 1: 3 x Lính Thí Mạng (Cấp 1)."); //cách nhau 4s
                    StartCoroutine(SpawnMixedWave_01());//count, level,Delaytime
                    m_state = SpawnState.SPAWNING;
                    break;
                case 2:
                    // --- Điều kiện cho Wave 2 ---"Sức Mạnh Của Đánh Xuyên"Chia làm 2 cụm, mỗi cụm 3 con đi sát nhau. Thời gian nghỉ giữa 2 cụm là 3 giây.
                    Debug.Log("Kịch bản Wave 2: 4 x Lính Thí Mạng (Cấp 1)."); //cách nhau 2.5s
                    StartCoroutine(SpawnMixedWave_02()); // Gọi một coroutine có kịch bản phức tạp hơn
                    m_state = SpawnState.SPAWNING;
                    break;

                case 3:
                    // --- Điều kiện cho Wave 3 ---"Thử Thách Bức Tường Thịt"2 Thiết Giáp xuất hiện trước. Sau 2 giây, 3 Sát Thủ xuất hiện ngay phía sau và di chuyển cùng tốc độ.
                    Debug.Log("Kịch bản Wave 3: 2 x Kẻ Cản Trở (Cấp 2) và 2 x Lính Thí Mạng (Cấp 3).");
                    StartCoroutine(SpawnMixedWave_03()); // Gọi một coroutine có kịch bản phức tạp hơn
                    m_state = SpawnState.SPAWNING;
                    break;

                case 4:
                    // --- Điều kiện cho Wave 4 ---
                    Debug.Log("Kịch bản Wave 4: 1 x Mini-Boss, 1 x Kẻ Cản Trở (Cấp 2), 4 x Kẻ Rỉa Máu (Cấp 3).!");
                    StartCoroutine(SpawnMixedWave_04()); // Gọi một coroutine có kịch bản phức tạp hơn
                    m_state = SpawnState.SPAWNING;
                    break;

                case 5:
                    // --- Điều kiện cho Wave 4 ---
                    Debug.Log("Kịch bản Wave 4: 1 x Mini-Boss!");
                    StartCoroutine(SpawnMixedWave_05()); // Gọi một coroutine có kịch bản phức tạp hơn
                    m_state = SpawnState.SPAWNING;
                    break;

                default:
                    Debug.Log("Kịch bản Wave " + CurrentWaveNumber.Value + ": Thử thách tăng dần!");
                    break;
            }
        }

        private IEnumerator SpawnMixedWave_05()
        {
            m_state = SpawnState.SPAWNING;
            EnemiesKilledThisWave.Value = 0;

            // TÍNH TOÁN VÀ ĐẶT TỔNG SỐ ENEMY CHỈ MỘT LẦN
            int totalEnemies = 1; // GiantSlimeBlue(5) + GiantSpirit(2)
            TotalEnemiesInWave.Value = totalEnemies;
            // --- Đợt 1: Spawn 1 GiantSpirit ---
            for (int i = 0; i < 1; i++)
            {
                SpawnAndRegister("GiantBamboo");
                yield return new WaitForSeconds(1f);
            }
            m_state = SpawnState.WAITING;
            Debug.Log("Đã spawn xong wave 5, đang chờ người chơi dọn dẹp...");
        } 

        private void SpawnAndRegister(string enemyName)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform randomSpawnPoint = spawnPoints[randomIndex];
            GameObject enemyObj = PoolManager.Ins.GetFromPool(enemyName, randomSpawnPoint.position);
            Enemy enemyScript = enemyObj.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                EnemyManager.Ins.RegisterEnemy(enemyScript);
            }
        }
        // private IEnumerator SpawnEnemies(string name, int count, float spawnInterval)
        // {
        //     m_state = SpawnState.SPAWNING;
        //     EnemiesKilledThisWave.Value = 0;
        //     TotalEnemiesInWave.Value = count;
        //     for (int i = 0; i < count; i++)
        //     {
        //         // Lấy một chỉ số ngẫu nhiên từ 0 đến số lượng điểm spawn
        //         int randomIndex = Random.Range(0, spawnPoints.Length);

        //         // Lấy Transform của điểm spawn ngẫu nhiên đó
        //         Transform randomSpawnPoint = spawnPoints[randomIndex];
        //         // Lấy object từ pool và lưu nó vào một biến
        //         GameObject enemyObj = PoolManager.Ins.GetFromPool(name, randomSpawnPoint.position);
        //         // Lấy script Enemy từ object đó
        //         Enemy enemyScript = enemyObj.GetComponent<Enemy>();
        //         // Đăng ký lính mới với Manager
        //         if (enemyScript != null)
        //         {
        //             EnemyManager.Ins.RegisterEnemy(enemyScript);
        //         }
        //         // Debug.Log("Spawn");
        //         yield return new WaitForSeconds(spawnInterval);
        //     }
        //     m_state = SpawnState.WAITING;
        //     Debug.Log("Đã spawn xong, đang chờ người chơi dọn dẹp...");
        //     yield break; // Kết thúc coroutine cho wave này
        // }
        private IEnumerator SpawnMixedWave_01()
        {
            m_state = SpawnState.SPAWNING;
            EnemiesKilledThisWave.Value = 0;

            // TÍNH TOÁN VÀ ĐẶT TỔNG SỐ ENEMY CHỈ MỘT LẦN
            int totalEnemies = 5; // GiantSlimeGreen(5) + GiantFlam(2)
            TotalEnemiesInWave.Value = totalEnemies;

            // --- Đợt 1: Spawn 5 GiantSlimeGreen ---
            for (int i = 0; i < 5; i++)
            {
                SpawnAndRegister("GiantSlimeGreen");
                yield return new WaitForSeconds(2.5f);
            }
            m_state = SpawnState.WAITING;
            Debug.Log("Đã spawn xong wave 3, đang chờ người chơi dọn dẹp...");
        }        // Hàm công khai để bắt đầu đếm ngược từ một script khác
        // }
        private IEnumerator SpawnMixedWave_02()
        {
            m_state = SpawnState.SPAWNING;
            EnemiesKilledThisWave.Value = 0;

            // TÍNH TOÁN VÀ ĐẶT TỔNG SỐ ENEMY CHỈ MỘT LẦN
            int totalEnemies = 5; // GiantSlimeGreen(5) + GiantFlam(2)
            TotalEnemiesInWave.Value = totalEnemies;

            // --- Đợt 1: Spawn 5 GiantSlimeGreen ---
            for (int i = 0; i < 3; i++)
            {
                SpawnAndRegister("GiantSlimeGreen");
                yield return new WaitForSeconds(2.5f);
            }

            // --- Đợt 2: Spawn 2 GiantFlam ---
            for (int i = 0; i < 2; i++)
            {
                SpawnAndRegister("GiantFlam");
                yield return new WaitForSeconds(1f);
            }

            m_state = SpawnState.WAITING;
            Debug.Log("Đã spawn xong wave 3, đang chờ người chơi dọn dẹp...");
        }        // Hàm công khai để bắt đầu đếm ngược từ một script khác
        // }
        private IEnumerator SpawnMixedWave_03()
        {
            m_state = SpawnState.SPAWNING;
            EnemiesKilledThisWave.Value = 0;

            // TÍNH TOÁN VÀ ĐẶT TỔNG SỐ ENEMY CHỈ MỘT LẦN
            int totalEnemies = 5 + 2; // GiantSlimeGreen(5) + GiantFlam(2)
            TotalEnemiesInWave.Value = totalEnemies;

            // --- Đợt 1: Spawn 5 GiantSlimeGreen ---
            for (int i = 0; i < 5; i++)
            {
                SpawnAndRegister("GiantSlimeGreen");
                yield return new WaitForSeconds(2.5f);
            }

            // --- Đợt 2: Spawn 2 GiantFlam ---
            for (int i = 0; i < 2; i++)
            {
                SpawnAndRegister("GiantFlam");
                yield return new WaitForSeconds(1f);
            }

            m_state = SpawnState.WAITING;
            Debug.Log("Đã spawn xong wave 3, đang chờ người chơi dọn dẹp...");
        }        // Hàm công khai để bắt đầu đếm ngược từ một script khác
        private IEnumerator SpawnMixedWave_04()
        {
            m_state = SpawnState.SPAWNING;
            EnemiesKilledThisWave.Value = 0;

            // TÍNH TOÁN VÀ ĐẶT TỔNG SỐ ENEMY CHỈ MỘT LẦN
            int totalEnemies = 5 + 2; // GiantSlimeBlue(5) + GiantSpirit(2)
            TotalEnemiesInWave.Value = totalEnemies;

            // --- Đợt 1: Spawn 5 GiantSlimeBlue ---
            for (int i = 0; i < 5; i++)
            {
                SpawnAndRegister("GiantSlimeBlue");
                yield return new WaitForSeconds(2.5f);
            }

            // --- Đợt 2: Spawn 2 GiantSpirit ---
            for (int i = 0; i < 2; i++)
            {
                SpawnAndRegister("GiantSpirit");
                yield return new WaitForSeconds(1f);
            }

            m_state = SpawnState.WAITING;
            Debug.Log("Đã spawn xong wave 4, đang chờ người chơi dọn dẹp...");
        }        // Hàm công khai để bắt đầu đếm ngược từ một script khác
    }
}

