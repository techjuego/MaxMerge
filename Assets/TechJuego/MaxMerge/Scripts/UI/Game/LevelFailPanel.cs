using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TechJuego.MaxMerge.Utils;
using TechJuego.MaxMerge.Sound;
using TechJuego.MaxMerge.Monetization;

namespace TechJuego.MaxMerge
{
    public class LevelFailPanel : MonoBehaviour
    {
        [SerializeField] private Button m_ReplayButton;
        [SerializeField] private Button m_HomeButton;
        [SerializeField] private TextMeshProUGUI m_Score;
        [SerializeField] private TextMeshProUGUI m_BestScore;
        private void OnEnable()
        {
            UiUtility.SetButton(m_ReplayButton, OnClickRelayButton);
            UiUtility.SetButton(m_HomeButton, OnClickHomeButton);
            DataHandler.Instance.SetHighScore(GameManager.Instance.Score);
            m_Score.text = GameManager.Instance.Score.ToString();
            m_BestScore.text = "Best:-" + DataHandler.Instance.GetHighScore().ToString();
            AdsHandler.Instance.ShowBanner();
        }
        void OnClickRelayButton()
        {
            AdsHandler.Instance.HideBanner();
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        void OnClickHomeButton()
        {
            AdsHandler.Instance.HideBanner();
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
            SceneLoader.LoadScene("Menu", GameColors.Color1(), 1.5f, ChangeEffect.BottomFill);
        }
    }
}