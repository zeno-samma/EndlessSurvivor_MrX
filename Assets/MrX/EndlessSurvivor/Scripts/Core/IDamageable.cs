namespace MrX.EndlessSurvivor
{
    // Interface định nghĩa một "hợp đồng"
    // Bất kỳ class nào implement interface này BẮT BUỘC phải có hàm TakeDamage
    public interface IDamageable
    {
        void TakeDamage(float damageAmount);
    }
}