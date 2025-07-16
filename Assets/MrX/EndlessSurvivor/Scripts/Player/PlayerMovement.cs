using Unity.Netcode;
using UnityEngine;

namespace MrX.EndlessSurvivor
{
    [RequireComponent(typeof(Rigidbody2D))] // Đảm bảo đối tượng luôn có Rigidbody2D
    public class PlayerMovement : MonoBehaviour
    {
        public PlayerConfigSO playerConfig; // Biến để chứa file config của người chơi

        private Rigidbody2D rb; // Để xử lý vật lý
        Vector2 moveInput;
        // private Animator m_anim;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            // Lấy component Rigidbody2D gắn trên cùng đối tượng
            rb = GetComponent<Rigidbody2D>();
            // m_anim = GetComponent<Animator>();
        }
        void Start()
        {
            // mainCam = Camera.main; // Lấy camera chính của game
        }

        // Update is called once per frame
        void Update()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            moveInput = new Vector2(moveX, moveY).normalized;
        }
        void FixedUpdate()
        {
            rb.linearVelocity = moveInput * playerConfig.initialMoveSpeed;
        }
    }
}
