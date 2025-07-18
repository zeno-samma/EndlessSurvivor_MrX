using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class ScrollingBackground : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed = 1f; // Tốc độ di chuyển của background
        private float backgroundWidth;
        void Start()
        {
            // Lấy chiều rộng của sprite
            backgroundWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        void Update()
        {
            // Di chuyển background sang trái
            transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

            // Nếu background đã di chuyển ra khỏi màn hình một khoảng bằng chiều rộng của nó
            if (transform.position.x < -backgroundWidth)
            {
                // Dịch chuyển nó về phía trước một khoảng bằng 2 lần chiều rộng
                // để nó nằm ngay sau background kia
                transform.position += new Vector3(backgroundWidth * 2f, 0, 0);
            }
        }
    }

}

