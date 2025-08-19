using UnityEngine;

// Enum để định danh các loại nâng cấp
public enum UpgradeType
{
    AttackDamage,
    MaxHealth,
    Def,
    MoveSpeed,
    FireRate
    // Thêm các loại khác ở đây sau này, ví dụ: MoveSpeed, PickupRadius...
}
[System.Serializable] // << Rất quan trọng, để nó hiện ra trong Inspector
public class UpgradeData
{
    public string upgradeName; // Ví dụ: "Tăng Sát Thương"
    public string description;
    public Sprite icon;
    public UpgradeType type;       // << Quan trọng: Cho biết đây là loại nâng cấp nào
    public float value;        // << Quan trọng: Lượng tăng là bao nhiêu
}