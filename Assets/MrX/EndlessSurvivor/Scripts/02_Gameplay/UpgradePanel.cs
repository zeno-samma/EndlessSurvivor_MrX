using UnityEngine;
using UnityEngine.UI;

namespace MrX.EndlessSurvivor
{
    public class UpgradePanel : MonoBehaviour
    {
        public Button upgradeCard1;
        public Button upgradeCard2;
        public Button upgradeCard3;

        // Hàm này được gọi khi panel được kích hoạt
        // public void ShowUpgradeChoices()
        // {
        //     // 1. Lấy ngẫu nhiên 3 nâng cấp (từ danh sách các ScriptableObject nâng cấp)
        //     UpgradeSO randomUpgrade1 = GetRandomUpgrade();
        //     UpgradeSO randomUpgrade2 = GetRandomUpgrade();
        //     UpgradeSO randomUpgrade3 = GetRandomUpgrade();

        //     // 2. Hiển thị thông tin nâng cấp lên các thẻ UI

        //     // 3. Gán sự kiện OnClick cho mỗi nút, truyền vào nâng cấp tương ứng
        //     upgradeCard1.onClick.AddListener(() => OnUpgradeSelected(randomUpgrade1));
        //     upgradeCard2.onClick.AddListener(() => OnUpgradeSelected(randomUpgrade2));
        //     upgradeCard3.onClick.AddListener(() => OnUpgradeSelected(randomUpgrade3));
        // }

        // // Hàm được gọi khi người chơi nhấn vào một nút
        // private void OnUpgradeSelected(UpgradeSO chosenUpgrade)
        // {
        //     // 4. Phát sự kiện báo cho Player biết nâng cấp nào đã được chọn
        //     EventBus.Publish(new UpgradeChosenEvent { selectedUpgrade = chosenUpgrade });

        //     // 5. Báo cho GameManager quay lại trạng thái chơi game
        //     GameManager.Ins.UpdateGameState(GameState.PLAYING);
        // }
    }
}

