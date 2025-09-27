using UnityEngine;
using TechJuego.MaxMerge.Rateus;

namespace TechJuego.MaxMerge
{
    public class InGameUI : MonoBehaviour
    {
        public static InGameUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InGameUI();
                }
                return _instance;
            }
        }
        private static InGameUI _instance;
        public InGameUI()
        {
            _instance = this;
        }
        [SerializeField] private HUDPanel m_HUDUI;
        [SerializeField] private LevelFailPanel m_LevelFailUI;
        [SerializeField] private RateUsPopup m_RateUsPopup;
        [SerializeField] private InGameSettingsPanel m_InGameSettingsPanel;
        private void OnEnable()
        {
            m_HUDUI.gameObject.SetActive(true);
            m_LevelFailUI.gameObject.SetActive(false);
            m_InGameSettingsPanel.gameObject.SetActive(false);
            m_RateUsPopup.gameObject.SetActive(false);
            GameEvents.OnGameEnd += GameEvents_OnGameEnd;
            GameEvents.OnShowRateUs += GameEvents_OnShowRateUs;
            AudioListener.volume = GameSetting.GetSound() ? 1 : 0;
        }
        private void OnDisable()
        {
            GameEvents.OnGameEnd -= GameEvents_OnGameEnd;
            GameEvents.OnShowRateUs -= GameEvents_OnShowRateUs;
        }
        private void GameEvents_OnShowRateUs()
        {
            m_RateUsPopup.gameObject.SetActive(true);
        }
        private void GameEvents_OnGameEnd()
        {
            m_LevelFailUI.gameObject.SetActive(true);
        }
        public void ShowSettingPanel()
        {
            m_InGameSettingsPanel.gameObject.SetActive(true);
        }
    }
}