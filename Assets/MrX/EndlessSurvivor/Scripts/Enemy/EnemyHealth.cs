using System.Collections;
using MRX.DefenseGameV1;
using UnityEngine;
using UnityEngine.UI;

namespace MrX.EndlessSurvivor
{
    public class EnemyHealth : MonoBehaviour
    {
        public Image healthBarFill; // Kéo Image thanh máu vào đây 
        [SerializeField] public int maxHealth;
        [SerializeField] private int currentHealth;
        public int minCoinBonus;
        public int maxCoinBonus;
        private Rigidbody2D m_rb;
        // private Animator m_anim;
        private bool isDead;
        private Coroutine damageCoroutine;

        public bool IsComponentNull()
        {
            return m_rb == null;
        }
        private void OnEnable()
        {
            currentHealth = maxHealth;
            // Cập nhật thanh máu đầy khi mới xuất hiện
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            if (healthBarFill != null)
            {
                healthBarFill.fillAmount = (float)currentHealth / maxHealth;
            }
        }

        private void Start()
        {
            // m_anim = GetComponent<Animator>();
            m_rb = GetComponent<Rigidbody2D>();
        }
        void OnTriggerEnter2D(Collider2D colTarget)
        {
            if (IsComponentNull() || isDead) return;
            if (colTarget.gameObject.CompareTag(Const.PLAYER_TAG) && !isDead)//So sánh va chạm tag player
            {
                // m_anim.SetBool(Const.ATTACK_ANIM, true);
                // Debug.Log("Va chạm");
                // 2. Lấy script "Enemy" từ chính đối tượng vừa va chạm
                PlayerHealth playerHealth = colTarget.GetComponentInChildren<PlayerHealth>();

                // 3. Kiểm tra để chắc chắn là đã lấy được script (tránh lỗi null)
                if (playerHealth != null)
                {
                    // Bắt đầu coroutine và truyền "player" vào,
                    // đồng thời lưu lại coroutine này vào biến damageCoroutine
                    // Debug.Log("playerHealth");
                    damageCoroutine = StartCoroutine(TakeDamage(playerHealth));
                }
            }
        }
        private IEnumerator TakeDamage(PlayerHealth playerToDamage) // Gây sát thương cho người chơi
        {
            while (true) // Dùng vòng lặp để gây sát thương liên tục
            {
                if (playerToDamage != null)
                {
                    playerToDamage.TakeDamagePlayer(10); // Gây sát thương cho player đã truyền vào
                    // Debug.Log("Tiếp tục gây sát thương lên người chơi!");
                }

                // Chờ 1 giây rồi lặp lại
                yield return new WaitForSeconds(1f);
            }
        }
        void OnTriggerExit2D(Collider2D colTarget)
        {
            if (IsComponentNull() || isDead) return;
            if (colTarget.gameObject.CompareTag(Const.PLAYER_TAG) && !isDead)//So sánh va chạm tag player
            {
                // m_anim.SetBool(Const.ATTACK_ANIM, true);
                // Debug.Log("Thoát va chạm");
                // Nếu có coroutine đang chạy, hãy dừng nó lại
                if (damageCoroutine != null)
                {
                    StopCoroutine(damageCoroutine);
                    damageCoroutine = null; // Đặt lại về null để an toàn
                    // Debug.Log("Đã dừng gây sát thương.");
                }
            }
        }
        /// Trả về tỷ lệ máu hiện tại (từ 0.0 đến 1.0).
        public float GetHealthPercentage()
        {
            // Tránh lỗi chia cho 0 nếu maxHealth chưa được thiết lập
            if (maxHealth <= 0) return 0;

            return (float)currentHealth / maxHealth;
        }
        // Phương thức nhận sát thương từ bên ngoài
        public void TakeDamageEnemy(int damage)//Nhận sát thương từ Bullet
        {
            // Debug.Log("TakeDamage: " + damage);
            if (currentHealth <= 0) return; // Nếu đã chết rồi thì không nhận thêm sát thương
            currentHealth -= damage;
            // Debug.Log("currentHealth " + currentHealth);
            // Kiểm tra nếu đã chết
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                int coinBonus = UnityEngine.Random.Range(minCoinBonus, maxCoinBonus);
                EventBus.Publish(new EnemyDiedEvent { diecoin = coinBonus });
                // Debug.Log("Chết");
                gameObject.SetActive(false);
            }
            // Cập nhật UI ngay sau khi máu thay đổi
            UpdateHealthBar();
        }
    }
}

