using UnityEngine;
namespace MrX.EndlessSurvivor
{
    [System.Serializable]// System.Serializable để nó có thể được hiển thị và chỉnh sửa trong Inspector của Unity
    public class EnemySpawnGroup
    {
        [Tooltip("Tên prefab của enemy trong pool")]
        public string enemyName;

        [Tooltip("Số lượng enemy trong nhóm này")]
        public int count;

        [Tooltip("Thời gian chờ giữa mỗi lần spawn enemy trong nhóm")]
        public float spawnInterval;

        [Tooltip("Thời gian chờ trước khi bắt đầu spawn nhóm này")]
        public float delayBeforeSpawningGroup;
    }
}