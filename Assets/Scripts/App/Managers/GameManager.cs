using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace GrandDevs.AppName
{
    public class CompletedLevelInfo
    {
        public List<int> completedLevelId { get ; set;}
    }
    public class GameManager : IService, IGameManager
    {
        private IUIManager _uiManager;
        private GameData _levelGameData;
        private PlayerData _playerData;
        private CompletedLevelInfo _completedLevelInfo;
        private bool _isGameStart;
        private int _levelIndex;
        public List<string> levelDataJson { get;  set ; }
        private string _playerDataJson;
        private string _completedLevelDataJson;
        public EnemyController EnemyController { get; set; }

        public PlayerController PlayerController { get; set; }

        public event Action<int> LevelCompete;
        public GameObject Battleground { get; set;}
        
        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
            //_completedLevelInfo.completedLevelId = new List<int>();
            LoadData();
            _isGameStart = false;
            _levelIndex = 0;
            PlayerController = new PlayerController(this);
            EnemyController = new EnemyController(this);
            
        }

        public void LoadCompletedLevelData()
        {
            _completedLevelDataJson = Resources.Load<TextAsset>($"GameData/CompletedLevelInfo").text;
            _completedLevelInfo = JsonConvert.DeserializeObject<CompletedLevelInfo>(_completedLevelDataJson);
            foreach (var levelId in _completedLevelInfo.completedLevelId)
            {
                LevelCompete?.Invoke(levelId);
            }
        }
        

        public void LoadData()
        {
            levelDataJson = new List<string>();
            for (int i = 0; i <= 5; i++)
            {
                levelDataJson.Add(Resources.Load<TextAsset>($"GameData/DataLevel_{i}").text);
            }
            _playerDataJson = Resources.Load<TextAsset>($"GameData/PlayerData").text;
        }
        public void Dispose()
        {

        }

        public void ClearData()
        {
            
            _isGameStart = false;
            EnemyController.ClearData();
            PlayerController.ClearData();
        }

        public void StartGame(int levelIndex)
        {
            _levelIndex = levelIndex;
            SetData(_levelIndex);
            _isGameStart = true;
            UpdateData();
        }

        public void RestartGame()
        {
            ClearData();
            SetData(_levelIndex);
            _isGameStart = true;
            UpdateData();
        }

        public void PauseGame()
        {
            _isGameStart = false;
            PlayerController.PausePlayer();
            EnemyController.PauseEnemy();
        }
        public void SavePlayerData()
        {
            PlayerInfo playerInfo = new PlayerInfo()
            {
                maxHealth = PlayerController.mainPlayer.maxHealth,
                maxMana = PlayerController.mainPlayer.maxMana,
                scoreNumber = PlayerController.mainPlayer.scoreNumber,
                playerManaRecoverTime = PlayerController.mainPlayer.playerManaRecoverTime
            };
            _playerDataJson = JsonConvert.SerializeObject(playerInfo, Formatting.Indented);
            System.IO.File.WriteAllText("Assets/Resources/GameData/PlayerData.json", _playerDataJson);
            Debug.Log(_playerDataJson);
        }
        public void ContinueGame()
        {
            _isGameStart = true;
            PlayerController.ContinuePlayer();
            EnemyController.ContinueEnemy();
        }

        private void UpdateData()
        {
            PlayerController.UpdateData(_levelGameData, _playerData);
            EnemyController.UpdateData(_levelGameData);
        }

        public void NextLevel()
        {
            
            ClearData();
            _levelIndex++;
            if (_levelIndex > levelDataJson.Count - 1)
            {
                _levelIndex = 0;
            }
            Debug.Log($"Load {_levelIndex} level");
            _isGameStart = true;
            UpdateData();
        }
        
        public void EndGame(bool isWin)
        {
            if (_isGameStart)
            {
                if (isWin)
                {
                    _completedLevelInfo.completedLevelId.Add(_levelIndex);
                    _completedLevelInfo.completedLevelId = _completedLevelInfo.completedLevelId.Distinct().ToList();
                    _completedLevelDataJson = JsonConvert.SerializeObject(_completedLevelInfo, Formatting.Indented);
                    System.IO.File.WriteAllText("Assets/Resources/GameData/CompletedLevelInfo.json", _completedLevelDataJson);
                    SavePlayerData();
                    Debug.Log("Win");
                    LevelCompete?.Invoke(_levelIndex);
                    _uiManager.SetPage<WinMenuPage>();
                    ClearData();
                }
                else
                {
                    Debug.Log("Lose");
                    _uiManager.SetPage<LoseMenuPage>();
                    ClearData();
                }
            }
        }

        private void SetData(int index)
        {
            
            _levelGameData = JsonConvert.DeserializeObject<GameData>(levelDataJson[index]);
            _playerData = JsonConvert.DeserializeObject<PlayerData>(_playerDataJson);
            Debug.Log(_playerData.maxHealth);
            
        }
        public void Update()
        {
            if (_isGameStart)
            {
                PlayerController.Update();
                EnemyController.Update();
            }
        }
    }
}
