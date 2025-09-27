using TechJuego.MaxMerge.Monetization;
using TechJuego.MaxMerge.Sound;
using UnityEngine;

namespace TechJuego.MaxMerge
{
    public class AutoInitializer
    {
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            AdsHandler.Instance.Load();
            SoundManager.Instance.Load();
        }
    }
}