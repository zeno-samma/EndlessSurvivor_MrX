using System.Collections.Generic;
using UnityEngine;

namespace MrX.EndlessSurvivor
{
    // Dòng này cho phép bạn tạo file WaveData từ menu Assets -> Create
    [CreateAssetMenu(fileName = "StageDataSO", menuName = "Survivor/StageData")]
    public class StageDataSO : ScriptableObject
    {
        [Header("STAGE CONFIGURATION")]
        public string stageName;
        public string stageDescription;

        [Header("WAVE LIST")]
        // Đây là danh sách chứa toàn bộ thông tin các wave của màn chơi này
        public List<WaveData> waves;
    }
}