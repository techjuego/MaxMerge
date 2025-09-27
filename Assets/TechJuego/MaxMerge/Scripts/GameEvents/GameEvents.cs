using UnityEngine;

namespace TechJuego.MaxMerge
{
    // Class to manage game-related events using delegates
    public class GameEvents
    {
        public delegate void OnAction();
        public static OnAction OnGameEnd;
        public static OnAction OnMosueUp;
        public static OnAction OnGameStart;
        public static OnAction OnShowRateUs;
        public static OnAction OnUpdateScore;
        public delegate void OnAction1(Vector3 pos);
        public static OnAction1 OnMosueDown;
    }
}

