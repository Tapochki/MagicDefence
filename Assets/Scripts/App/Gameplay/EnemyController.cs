using System.Collections.Generic;
using GrandDevs.AppName.Common;
using UnityEngine;

namespace GrandDevs.AppName
{
   public class EnemyController
   {
      private List<Enemy> _enemies;
      private GameData _gameData;
      private GameObject _enemyPool;
      private float _timer;
      private float _levelTimer;
      private int _enemyIndex;
      private GameManager _gameManager;
      private List<FireBall> _fireBalls;
      public EnemyController(GameManager gameManager)
      {
         _enemies = new List<Enemy>();
         _fireBalls = new List<FireBall>();
         _enemyPool = GameObject.Find("EnemyPool");
         _gameManager = gameManager;
      }

      public void UpdateData(GameData gameData)
      {
         _gameData = gameData;
         _levelTimer = _gameData.levelTimeData.enemySpawnTimer;
         _timer = _levelTimer;
         _enemyIndex = _gameData.allEnemy.Count - 1;
         SpawnEnemy();
      }

      private Enemy SetEnemy(int idEnemy)
      {
         Debug.Log(_gameData.allEnemy[idEnemy]);
         Enemy newEnemy = new Enemy(_gameData.allEnemy[idEnemy], _enemyPool.transform,
            _gameData.levelTimeData.enemySpeed);
         return newEnemy;
      }

      public void ClearData()
      {
         foreach (var item in _enemies)
         {
            item.Dispose();
         }

         foreach (var item in _fireBalls)
         {
            item.Dispose();
         }
         _fireBalls.Clear();
         _enemies.Clear();
      }
      private void SpawnEnemy()
      {
         for (int i = 0; i <= _gameData.allEnemy.Count - 1; i++)
         {
            Enemy enemy = SetEnemy(i);
            enemy.EnemyEndMoveEvent += EnemyMoveEventEnd;
            _enemies.Add(enemy);
         }
      }
      public void PauseEnemy()
      {
         foreach (var item in _enemies)
         {
            item.enemyObject.SetActive(false);
         }
      }
      public void ContinueEnemy()
      {
         foreach (var item in _enemies)
         {
            item.enemyObject.SetActive(true);
         }
      }

      public Enemy FindClosetEnemy(GameObject playerObject)
      {
         Enemy closetEnemy = _enemies[0];
        
         for (int i = 0; i <= _enemies.Count - 1; i++)
         {
            float closetEnemyDistance =
               Vector2.Distance(closetEnemy.enemyObject.transform.position, playerObject.transform.position);
            var enemyPlayerDistance = Vector2.Distance(_enemies[i].enemyObject.transform.position,
               playerObject.transform.position);
            Debug.Log($"Closet:{closetEnemy.enemyObject.name} {closetEnemyDistance} enemyDis {_enemies[i].enemyObject.name} {enemyPlayerDistance}");
            if (enemyPlayerDistance < closetEnemyDistance)
            {
               closetEnemy = _enemies[i];
            }
         }
         if (closetEnemy.isSpawn == false)
         {
            Debug.Log("Spawn false");
            return null;
         }
         Debug.Log(playerObject.transform.position);
         Debug.Log($"{closetEnemy.enemyObject.name} + {closetEnemy.enemyObject.transform.position}");
         return closetEnemy;
      }

      private void EnemyMoveEventEnd(Enemy enemyEndMove)
      {
         if (enemyEndMove.enemyType == Enumerators.EnemyType.Witch)
         {
            var fireBall = new FireBall(enemyEndMove.playerObject, enemyEndMove.fireGun.transform);
            _fireBalls.Add(fireBall);
            fireBall.FireBallEndMoveEvent += FireBallEndMove;
            return;
         }
         _gameManager.PlayerController.PlayerGetHit(enemyEndMove.damage);
         _gameManager.PlayerController.Deselect(enemyEndMove);
         EnemyDestroy(enemyEndMove);
      }

      public void EnemyDestroy(Enemy enemy)
      {
         enemy.Dispose();
         _enemies.Remove(enemy);
         if (_enemies.Count == 0)
         {
            _gameManager.EndGame(true);
         }
      }

      private float RandomTime()
      {
         return Random.Range(_levelTimer - 2.5f, _levelTimer + 1);
      }

      private void FireBallEndMove(FireBall fireBall, bool isHit)
      {
         if (isHit == true)
         {
            _gameManager.PlayerController.PlayerGetHit(fireBall.fireBallDamage);
         }
         _fireBalls.Remove(fireBall);
         fireBall.Dispose();
      }
      public void Update()
      {
         Debug.Log(_fireBalls.Count);
         if (_fireBalls.Count != 0)
         {
            for (int i = 0; i <= _fireBalls.Count-1; i++)
            {
               _fireBalls[i].Update();
            }
         }
         if (_enemies.Count != 0)
         {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
               _timer = RandomTime();
               if (_enemyIndex >= 0)
               {
                  _enemies[_enemyIndex].StartMove();
               }

               _enemyIndex--;
            }

            for (int i = 0; i <= _enemies.Count - 1; i++)
            {
               _enemies[i].Update();
            }
         }
      }

   }
}
