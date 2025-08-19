using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Cần để dùng TextMeshPro

namespace MrX.EndlessSurvivor
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private Button upgradeCard1;
        [SerializeField] private Button upgradeCard2;
        [SerializeField] private Button upgradeCard3;
        [SerializeField] private UpgradeSO upgradeSO;
        [SerializeField] private Image iconUpgradeCard1;
        [SerializeField] private Image iconUpgradeCard2;
        [SerializeField] private Image iconUpgradeCard3;
        [SerializeField] private TextMeshProUGUI valueTxtUpgradeCard1;      // Text hiển thị %
        [SerializeField] private TextMeshProUGUI valueTxtUpgradeCard2;      // Text hiển thị %
        [SerializeField] private TextMeshProUGUI valueTxtUpgradeCard3;      // Text hiển thị %

        [SerializeField] private CanvasGroup canvasGroup;

        void OnEnable()
        {
            ShowUpgradeChoices();
        }
        // Hàm này được gọi khi panel được kích hoạt
        public void ShowUpgradeChoices()
        {
            // Xóa các listener cũ để tránh cộng dồn mỗi lần panel hiện lên
            upgradeCard1.onClick.RemoveAllListeners();
            upgradeCard2.onClick.RemoveAllListeners();
            upgradeCard3.onClick.RemoveAllListeners();
            List<UpgradeData> tempUpgradePool = new List<UpgradeData>(upgradeSO.allUpgrades);
            // --- Lấy và Debug Lựa Chọn 1 ---
            int index1 = UnityEngine.Random.Range(0, tempUpgradePool.Count);
            UpgradeData choice1 = tempUpgradePool[index1];
            iconUpgradeCard1.sprite = choice1.icon;
            valueTxtUpgradeCard1.text = $"{choice1.value}";
            tempUpgradePool.RemoveAt(index1); // Xóa khỏi kho tạm để không bị chọn lại
            // Debug.Log("Thẻ 1 random ra: " + choice1.upgradeName);

            // --- Lấy và Debug Lựa Chọn 2 ---
            int index2 = UnityEngine.Random.Range(0, tempUpgradePool.Count);
            UpgradeData choice2 = tempUpgradePool[index2];
            iconUpgradeCard2.sprite = choice2.icon;
            valueTxtUpgradeCard2.text = $"{choice2.value}";
            tempUpgradePool.RemoveAt(index2);
            // Debug.Log("Thẻ 2 random ra: " + choice2.upgradeName);

            // --- Lấy và Debug Lựa Chọn 3 ---
            int index3 = UnityEngine.Random.Range(0, tempUpgradePool.Count);
            UpgradeData choice3 = tempUpgradePool[index3];
            iconUpgradeCard3.sprite = choice3.icon;
            valueTxtUpgradeCard3.text = $"{choice3.value}";
            // Debug.Log("Thẻ 3 random ra: " + choice3.upgradeName);

            // 3. Gán sự kiện OnClick cho mỗi nút, truyền vào nâng cấp tương ứng
            upgradeCard1.onClick.AddListener(() => OnUpgradeSelected(choice1));
            upgradeCard2.onClick.AddListener(() => OnUpgradeSelected(choice2));
            upgradeCard3.onClick.AddListener(() => OnUpgradeSelected(choice3));
        }

        // // Hàm được gọi khi người chơi nhấn vào một nút
        private void OnUpgradeSelected(UpgradeData chosenUpgrade)
        {
            Debug.Log($"1.Chọn sự kiện click: {chosenUpgrade.upgradeName}");
            // 4. Phát sự kiện báo cho Player biết nâng cấp nào đã được chọn
            EventBus.Publish(new UpgradeChosenEvent { selectedUpgrade = chosenUpgrade });

            // 5. Báo cho GameManager quay lại trạng thái chơi game
            // GameManager.Ins.UpdateGameState(GameState.PLAYING);
            Time.timeScale = 1f;
            canvasGroup.alpha = 0;

        }
    }
}

