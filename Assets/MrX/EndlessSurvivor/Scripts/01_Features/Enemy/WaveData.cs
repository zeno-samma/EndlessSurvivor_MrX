using System.Collections.Generic;
using UnityEngine;

namespace MrX.EndlessSurvivor
{
    // System.Serializable để nó có thể được hiển thị và chỉnh sửa trong Inspector của Unity
    [System.Serializable]
    public class WaveData
    {
        [Tooltip("Tên của wave để dễ nhận biết trong Inspector")]
        public string waveName;
        public List<EnemySpawnGroup> enemyGroups;
        // Hàm tiện ích để tự động tính tổng số enemy trong wave này
        public int GetTotalEnemiesInWave()
        {
            int total = 0;
            // Duyệt qua từng nhóm và cộng dồn số lượng
            foreach (var group in enemyGroups)
            {
                total += group.count;
            }
            return total;
        }

    }

}
