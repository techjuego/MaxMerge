using UnityEngine;
using System;

namespace TechJuego.MaxMerge
{
    // Singleton class for handling game data
    public class DataHandler : Singleton<DataHandler>
    {
        // Protected constructor to prevent instantiation from outside
        protected DataHandler()
        {
        }
     
        public int GetHighScore()
        {
          return PlayerPrefs.GetInt("HighScore");
        }
        public  void SetHighScore(int score)
        {
                    PlayerPrefs.SetInt("HighScore", score);  // Save the new high score
        }
    }
}
