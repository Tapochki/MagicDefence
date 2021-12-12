using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


namespace GrandDevs.AppName
{
    [Serializable]
    public class PlayerData
    {
        public int maxHealth;
        public int maxMana;
        public int scoreNumber;
        public float playerManaRecoverTime;
    }
    [Serializable]
    public class CompletedLevelData
    {
        public int[] completedLevelId;
    }
    [Serializable]
    public class EnemyData
    {
        public Common.Enumerators.EnemyType enemyType;
        public Common.Enumerators.SpawnPosition enemySpawnPosition;
    }

    public class LevelTimeData
    {
        public float enemySpawnTimer;
        public float enemySpeed;
    }

    [Serializable]
    public class GameData
    {
       // public PlayerData player;
        public List<EnemyData> allEnemy;
        public LevelTimeData levelTimeData;
    }
}


