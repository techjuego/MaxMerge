using UnityEngine;
using TechJuego.MaxMerge.Sound;
using System.Collections.Generic;
//using TechJuego.PlanetMerge.HapticFeedback;
using TechJuego.MaxMerge.Monetization;
using TechJuego.MaxMerge.Utils;
using TechJuego.MaxMerge.HapticFeedback;

namespace TechJuego.MaxMerge
{
    // The GameManager class handles game mechanics such as spawning items, combining them, and tracking game state.
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }
        private static GameManager _instance;
        public GameManager()
        {
            _instance = this;
        }
        public MergeItem m_MergeCoin;
        public GameObject BornPos;
        public GameObject m_MergeEffect;
        public bool isGameOver;
        public ParticleSystem m_MergeParticle;
        public MergeItem m_MergItem;
        [SerializeField] private GameObject m_BombEffect;
        public MergeItem currentMergeItem;
        public int m_AdsScore;
        private int m_Score = 0;
        private int lastTriggerScore = 0;
        public int rangeSize;
        public int MaxRangeSize;
        public int spawnedItemIndex;
        private bool isDraging;
        public int Score
        {
            get { return m_Score; }
            set
            {
                m_Score = value;
                GameEvents.OnUpdateScore?.Invoke();

                if (m_Score - lastTriggerScore >= m_AdsScore)
                {
                    lastTriggerScore = m_Score - (m_Score % m_AdsScore); // store the last multiple of 3000
                    AdsHandler.Instance.ShowInterstitial();
                }
            }
        }
        void Start()
        {
            rangeSize = 1;
            SoundEvents.OnPlayLoopSound?.Invoke("BGMUSIC");
            GameStateHandler.Instance.m_GameState = GameState.InProgress;
            CreateNewItem();
        }
        private int GetIndex()
        {
            spawnedItemIndex++;
            return spawnedItemIndex;
        }
        int GetWeightedRandomIndex(int size)
        {
            if (size <= 1) return 1;

            float totalWeight = 0f;
            float[] weights = new float[size];

            // Assign weights: largest weight for 1, smallest for size
            for (int i = 0; i < size; i++)
            {
                weights[i] = size - i;  // size=5 -> weights = [5,4,3,2,1]
                totalWeight += weights[i];
            }

            float rand = Random.value * totalWeight;
            float cumulative = 0f;

            for (int i = 0; i < size; i++)
            {
                cumulative += weights[i];
                if (rand < cumulative)
                    return i + 1; // Add 1 to ensure minimum number is 1
            }

            return size; // fallback
        }
        public void CreateItem()
        {
            Invoke(nameof(CreateNewItem), 1f);  
        }
        public void CreateNewItem()
        {
            if (rangeSize >= MaxRangeSize)
            {
                rangeSize = MaxRangeSize;
            }
            int num = GetWeightedRandomIndex(rangeSize);
            if (m_MergeCoin != null)
            {
                MergeItem item = m_MergeCoin;
                if (item != null)
                {
                    var curItem = Instantiate(item, BornPos.transform.position, item.transform.rotation);
                    curItem.gameplayIndex = GetIndex();
                    curItem.SetDetail(num);
                    currentMergeItem = curItem;
                    curItem.itemState = ItemState.Ready;
                }
            }
        }
        public void CombineItem(Vector3 currentPos, Vector3 collisionPos, int itemIndex)
        {
            int newrange = itemIndex +1;
            if(newrange > rangeSize)
            {
                rangeSize = newrange;
            }
            Vector3 newPos = (currentPos + collisionPos) / 2;
            var combineItem = Instantiate(m_MergeCoin, newPos, Quaternion.identity);
            combineItem.gameplayIndex = GetIndex();
            combineItem.SetDetail( itemIndex);
            combineItem.itemState = ItemState.Collision;
            combineItem.GetComponent<Rigidbody2D>().gravityScale = 1f;
            combineItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            SoundEvents.OnPlaySingleShotSound?.Invoke("Merge");
            var particl = m_MergeParticle.main;
            var effect = Instantiate(m_MergeEffect);
            effect.transform.position = newPos;
            if (HapticSetting.GetVibrate())
            {
                HapticCall.Instance.MediumHaptic();
            }
        }
    
        private void Update()
        {
            if(GameStateHandler.Instance.m_GameState == GameState.InProgress)
            {
                if (currentMergeItem != null)
                {
                        if (UiUtility.IsPointerOverUIObject())
                        {
                            return;
                        }
                        if (currentMergeItem.itemState == ItemState.Ready)
                        {
                            GameEvents.OnMosueDown?.Invoke(currentMergeItem.transform.position);
                        }
                        if (currentMergeItem.itemState == ItemState.Ready)
                        {
                            if (Input.GetMouseButtonDown(0)) // On mouse down, start dragging
                            {
                                isDraging = true;
                                currentMergeItem.offset = currentMergeItem.transform.position - GetMouseWorldPosition();  // Store the offset from the mouse
                            }
                            if (Input.GetMouseButtonUp(0) && isDraging) // On mouse up, stop dragging and drop the item
                            {
                                isDraging = false;
                                currentMergeItem.m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                                currentMergeItem.m_Rigidbody2D.gravityScale = 1f;  // Enable gravity
                                currentMergeItem.itemState = ItemState.Dropping;  // Change state to Dropping
                                currentMergeItem = null;
                                CreateItem();  // Create new fruit item
                                GameEvents.OnMosueUp?.Invoke();
                            }
                            if (isDraging)
                            {
                                if (currentMergeItem.itemState == ItemState.Ready)
                                {
                                    Vector3 mousePos = GetMouseWorldPosition() + currentMergeItem.offset;
                                    currentMergeItem.transform.position = new Vector3(Mathf.Clamp(mousePos.x, -currentMergeItem.limit_x, currentMergeItem.limit_x),
                                    currentMergeItem.transform.position.y, currentMergeItem.transform.position.z);
                                }
                            }
                        }
                }
            }
        }
        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }
    }
}