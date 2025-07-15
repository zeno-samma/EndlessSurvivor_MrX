using System.Collections.Generic;
using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class MyPool
    {
        private Stack<GameObject> stack = new Stack<GameObject>();
        private GameObject baseObj;
        private ReturnToMyPool returnPool;
        // private Transform container; // Biến để lưu lại "thùng chứa"
        public MyPool(GameObject baseObj)
        {
            this.baseObj = baseObj;
        }

        public GameObject Get(bool activeValue, Vector3 postion)
        {

            Debug.Log(postion);
            if (baseObj == null)
            {
                Debug.Log("Null");
                return null;
            }
            // KHAI BÁO 'tmp' NHƯ MỘT BIẾN CỤC BỘ
            GameObject tmp = null;
            // Debug.Log(stack.Count);
            while (stack.Count > 0)
            {
                tmp = stack.Pop();//
                if (tmp != null)
                {
                    tmp.transform.position = postion;
                    tmp.SetActive(activeValue);
                    return tmp;
                }
                else
                {
                    // Debug.LogWarning($"game object with key {baseObj.name} has been destroyed!");
                }
            }
            // Debug.Log("Thêm mới");
            tmp = GameObject.Instantiate(baseObj, postion, Quaternion.identity);
            // === DÒNG QUAN TRỌNG ===
            // Đặt object mới vào làm con của "thùng chứa"
            // tmp.transform.SetParent(container);
            returnPool = tmp.AddComponent<ReturnToMyPool>();
            returnPool.pool = this;
            tmp.SetActive(activeValue);
            return tmp;
        }
        public void AddToPool(GameObject obj)
        {
            stack.Push(obj);
            // Debug.Log($"[POOL] {baseObj.name} -> Đã thêm vào stack. Tổng stack: {stack.Count}");
        }
    }

}
