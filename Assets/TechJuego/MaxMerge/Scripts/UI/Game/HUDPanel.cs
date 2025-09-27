using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TechJuego.MaxMerge.Utils;
using TechJuego.MaxMerge.Sound;

namespace TechJuego.MaxMerge
{
    public class HUDPanel : MonoBehaviour
    {
        [SerializeField] private Button m_SettingButton;
        [SerializeField] private TextMeshProUGUI m_Score;
        private void OnEnable()
        {
            UiUtility.SetButton(m_SettingButton, OnClickSettingButton);
            GameEvents.OnUpdateScore += GameEvents_OnUpdateScore;
        }
        private void OnDisable()
        {
            GameEvents.OnUpdateScore -= GameEvents_OnUpdateScore;
        }
        private void Start()
        {
            m_Score.text = GameManager.Instance.Score.ToString();
        }
        private void GameEvents_OnUpdateScore()
        {
            var lastValue = float.Parse(m_Score.text);
            TechTween.ScaleTo(m_Score.gameObject, Vector3.one, 0.3f).SetEaseType(EaseTween.EaseInOutBounce);
            StartCoroutine(UpdateScore(lastValue, GameManager.Instance.Score, 1, (val) =>
            {
                m_Score.text = ((int)val).ToString();
            }));
           
        }
       
        public IEnumerator UpdateScore(float start, float end, float seconds, Action<float> onupdate)
        {
            float elapsedTime = 0;
            float startingPos = start;
            while (elapsedTime < seconds)
            {
                var val = Mathf.Lerp(startingPos, end, (elapsedTime / seconds));
                onupdate?.Invoke((int)val);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            onupdate?.Invoke(end);
        }
        private void OnClickSettingButton()
        {
            InGameUI.Instance.ShowSettingPanel();
        }
    }
}