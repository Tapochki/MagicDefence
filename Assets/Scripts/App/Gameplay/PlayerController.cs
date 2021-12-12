using System;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDevs.AppName
{
    public class PlayerController
    {
        public Player mainPlayer;
        private List<Common.Enumerators.ElementType> _attackElements;
        private int _elementToDestroyIndex;
        private Enemy _closetEnemy;
        public event Action<int> HealthValueChangeEvent;
        public event Action<int, int> SetMaxPlayerHealthManaEvent;
        public event Action<int> ManaValueChangeEvent;
        private GameData _gameData;
        private GameManager _gameManager;
        private float _timer;
        
        
        public event Action<int> scoreChange;

        public PlayerController(GameManager gameManager)
        {
            _gameManager = gameManager;
            _attackElements = new List<Common.Enumerators.ElementType>();
            _elementToDestroyIndex = 0;
        }
        public void PausePlayer()
        {
            mainPlayer.playerObject.SetActive(false);
        }
        public void ContinuePlayer()
        {
            mainPlayer.playerObject.SetActive(true);
        }
        public void UpdateData(GameData gameData, PlayerData playerData)
        {
            _gameData = gameData;
            mainPlayer = new Player(playerData);
            _timer = playerData.playerManaRecoverTime;
            SetMaxPlayerHealthManaEvent?.Invoke(mainPlayer.maxHealth, mainPlayer.maxMana);
            HealthValueChangeEvent?.Invoke(mainPlayer.health);
            ManaValueChangeEvent?.Invoke(mainPlayer.mana);
            scoreChange?.Invoke(mainPlayer.scoreNumber);
        }
        public void ClearData()
        {
            MonoBehaviour.Destroy(mainPlayer.playerObject);
        }
        public void PlayerGetHit(int damage)
        {
            mainPlayer.health -= damage;
            if (mainPlayer.health <= 0)
            {
                _gameManager.EndGame(false);
            }
         //   Deselect();
            HealthValueChangeEvent?.Invoke(mainPlayer.health);
        }

        public void Update()
        {
            if (mainPlayer.mana < mainPlayer.maxMana)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    _timer = mainPlayer.playerManaRecoverTime;
                    mainPlayer.mana += 1;
                    ManaValueChangeEvent?.Invoke(mainPlayer.mana);
                }
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                AttackClosetEnemy(Common.Enumerators.ElementType.Water);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                AttackClosetEnemy(Common.Enumerators.ElementType.Wind);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                AttackClosetEnemy(Common.Enumerators.ElementType.Ground);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                AttackClosetEnemy(Common.Enumerators.ElementType.Fire);
            }
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                mainPlayer.RightMove();
                mainPlayer.playerSprite.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                mainPlayer.LeftMove();
                mainPlayer.playerSprite.transform.rotation = new Quaternion(0, -180, 0, 0);
            }
        }
        public void AttackClosetEnemy(Common.Enumerators.ElementType elementType)
        {
            Debug.Log("Find Closet Enemy");
            _closetEnemy = _gameManager.EnemyController.FindClosetEnemy(mainPlayer.playerObject);
            if (_closetEnemy != null)
            {
                _elementToDestroyIndex = _closetEnemy.lastElementIndex;
                if (mainPlayer.playerSprite.transform.position.x > _closetEnemy.enemyObject.transform.position.x)
                {
                    mainPlayer.playerSprite.transform.rotation = new Quaternion(0, -180, 0, 0);
                }
                if (mainPlayer.playerSprite.transform.position.x < _closetEnemy.enemyObject.transform.position.x)
                {
                    mainPlayer.playerSprite.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                if (mainPlayer.mana >= 5)
                {
                    mainPlayer.mana -= 5;
                    ManaValueChangeEvent?.Invoke(mainPlayer.mana);
                    _attackElements = _closetEnemy.buttonElementsToDie;
                    if (GameClient.IsDebugMode == true)
                    {
                        _closetEnemy.DestroyColor(_elementToDestroyIndex);
                        
                        if (_elementToDestroyIndex == _attackElements.Count - 1)
                        {
                            Debug.Log("StartDestroy");
                            _gameManager.EnemyController.EnemyDestroy(_closetEnemy);
                            mainPlayer.scoreNumber += _closetEnemy.scoreGive;
                            scoreChange?.Invoke(mainPlayer.scoreNumber);
                            Deselect();
                            return;
                        }
                        _elementToDestroyIndex++;
                    }
                    else if (elementType == _attackElements[_elementToDestroyIndex])
                    {
                        _closetEnemy.DestroyColor(_elementToDestroyIndex);
                        _elementToDestroyIndex = _closetEnemy.lastElementIndex;
                        if (_elementToDestroyIndex == _attackElements.Count - 1)
                        {
                            Debug.Log("StartDestroy");
                            mainPlayer.scoreNumber += _closetEnemy.scoreGive;
                            _gameManager.EnemyController.EnemyDestroy(_closetEnemy);
                            scoreChange?.Invoke(mainPlayer.scoreNumber);
                            Deselect();
                            return;
                        }

                        _elementToDestroyIndex++;
                    }
                    else
                    {
                        Debug.Log(
                            $"Mistake attackElements:{_attackElements[_elementToDestroyIndex]} != buttonType: {elementType}");
                        Deselect();
                    }
                }
                else
                {
                    Debug.Log("Low Mana");
                }
                
            }
            else
            {
                Deselect();
            }
        }
        public void Deselect()
        {
            if (_closetEnemy != null)
            {
                _closetEnemy.ResetColor();
            }

            _closetEnemy = null;
            _elementToDestroyIndex = 0;
        }
        public void Deselect(Enemy enemy)
        {
            if (_closetEnemy != null)
            {
                if (_closetEnemy == enemy)
                {
                    _closetEnemy.ResetColor();
                    _closetEnemy = null;
                    _elementToDestroyIndex = 0;
                }
            }
        }
    }
}
