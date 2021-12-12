using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDevs.AppName
{
    public interface IGameManager
    {
        PlayerController PlayerController { get; set; }
        public List<string> levelDataJson { get; set; }
        EnemyController EnemyController { get; set; }
        public GameObject Battleground { get; set;}
        
        public event Action<int> LevelCompete;
        public void LoadCompletedLevelData();
        public void ClearData();
        void StartGame(int levelIndex);
        void RestartGame();
        void PauseGame();
        void ContinueGame();
        void LoadData();
        void NextLevel();
        void EndGame(bool isWin);
        void Update();

    }
}

