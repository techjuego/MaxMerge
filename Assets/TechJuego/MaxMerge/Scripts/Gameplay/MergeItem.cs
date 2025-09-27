using UnityEngine;
using TechJuego.MaxMerge.Utils;
using TMPro;

namespace TechJuego.MaxMerge
{
    // This class controls the behavior of the mergeable items (fruits) in the game
    public class MergeItem : MonoBehaviour
    {
        public int gameplayIndex;
        public int itemIndex;
        public ItemState itemState;
        public float limit_x;
        public Vector3 offset;
        public Rigidbody2D m_Rigidbody2D;
        public TextMeshPro m_Number;
        private void OnEnable()
        {
        }
     
        private void OnDisable()
        {
        }
    
        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
        }
        public void SetDetail(int index)
        {
            itemIndex = index;
            m_Number.text = index.ToString();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains("Wall") || collision.gameObject.tag.Contains("MergeItem"))
            {
                itemState = ItemState.Collision;  
            }
            if ((int)itemState >= (int)ItemState.Dropping && collision.gameObject.tag.Contains("MergeItem"))
            {
                if (itemIndex == collision.gameObject.GetComponent<MergeItem>().itemIndex)
                {
                    int NextIndex = itemIndex + 1;
                        // Ensure only one item merges with another (prevents merging with self)
                        if (gameplayIndex > collision.gameObject.GetComponent<MergeItem>().gameplayIndex)
                        {
                            // Calculate score for merging items based on itemIndex
                            var score = (NextIndex + 1);
                            GameManager.Instance.Score += score;  // Update score
                            GameManager.Instance.CombineItem(gameObject.GetComponent<Transform>().position, collision.transform.position, NextIndex);  // Combine items into a new item
                            GameEvents.OnUpdateScore?.Invoke();  // Trigger score update event
                            // Destroy the merged items
                            Destroy(collision.gameObject);
                            Destroy(gameObject);
                        }
                }
            }
        }
    }
}
