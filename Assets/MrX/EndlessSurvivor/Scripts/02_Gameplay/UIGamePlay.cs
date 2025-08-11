using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class UIGamePlay : MonoBehaviour
    {
        // test tốc độc asmdef;
        // [SerializeField] public GameObject MainMenuPanel;
        public CanvasGroup canvasGroupLevelUP;
        void Awake()
        {
            canvasGroupLevelUP.alpha = 0;
        }
        private void OnEnable()
        {

            // Đăng ký lắng nghe sự thay đổi trạng thái từ GameManager
            EventBus.Subscribe<StateUpdatedEvent>(HandleGameStateChange);//Lắng nghe trạng thái game do gamemanager quản lý
            // EventBus.Subscribe<InitialUIDataReadyEvent>(OnInitialUIDataReadyEvent);//Lắng nghe sự kiện load dữ liệu ban đầu
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<StateUpdatedEvent>(HandleGameStateChange);
            // EventBus.Unsubscribe<InitialUIDataReadyEvent>(OnInitialUIDataReadyEvent);
        }
        private void OnInitialUIDataReadyEvent(InitialUIDataReadyEvent value)//Cập nhật dữ liệu ban đầu lên ui
        {
            // HP_Count_Txt.SetText("{0}", value.defHealth);
            // Total_HP_Txt.SetText("{/}", value.maxHealth);
            // scoreGameGUITxt.text = $"{value.defScore}";
            // scoreHomeGUITxt.text = $"{value.defScore}";
        }// Hàm này được EventBus tự động gọi mỗi khi GameManager thay đổi trạng thái
        private void HandleGameStateChange(StateUpdatedEvent gameState)//1. Nhận thông báo và quản lý các ui
        {


            // Tắt hết các panel trước để đảm bảo sạch sẽ
            // pauseMenuPanel.SetActive(false);
            // PopUpPanel.SetActive(false);
            canvasGroupLevelUP.alpha = 0;
            // gameOverPanel.SetActive(false);
            // HomeGui.SetActive(false);

            // Bật panel tương ứng với trạng thái mới
            switch (gameState.CurState)
            {
                // case GameManager.GameState.PREPAIR:
                //     // MainMenuPanel.SetActive(true);
                //     break;
                case GameState.PLAYING:
                    canvasGroupLevelUP.alpha = 0;
                    // gameplayHUD.SetActive(true);
                    break;
                case GameState.PAUSE:
                    // pauseMenuPanel.SetActive(true);
                    break;
                case GameState.UPGRADEPHASE:
                    // gameplayHUD.SetActive(true);
                    Debug.Log("4.vào UPGRADEPHASE");
                    canvasGroupLevelUP.alpha = 1;
                    break;
                case GameState.GAMEOVER:
                    canvasGroupLevelUP.alpha = 0;
                    // Debug.Log("GAMEOVERUI");
                    // gameplayHUD.SetActive(true);
                    // gameOverPanel.SetActive(true);
                    break;
            }
        }
    }
}


