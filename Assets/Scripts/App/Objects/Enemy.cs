using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;
using GrandDevs.AppName.Common;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace GrandDevs.AppName
{


    public class Enemy
    {
        private Common.Enumerators.SpawnPosition enemySpawnPosition;
        private List<SpriteRenderer> _elementToDieObjects;
        public List<Common.Enumerators.ElementType> buttonElementsToDie;
        public Common.Enumerators.EnemyType enemyType;
        public GameObject enemyObject;
        private float _enemySpeed;
        public int damage;
        public GameObject playerObject;
        private Vector3 _startPosition;
        private SpriteRenderer _dieElement;
        public bool isSpawn;
        public event Action<Enemy> EnemyEndMoveEvent;
        public GameObject _enemySprite;
        public int scoreGive;
        public GameObject fireGun;
        private Camera _camera;
        private Vector3 _leftBorder;
        private Vector3 _rightBorder;
        private bool _canShoot;
        public int lastElementIndex;

        public Enemy(EnemyData enemy, Transform parent, float enemySpeed)
        {
            isSpawn = false;
            enemyType = enemy.enemyType;
            enemySpawnPosition = enemy.enemySpawnPosition;
            _elementToDieObjects = new List<SpriteRenderer>();
            buttonElementsToDie = new List<Common.Enumerators.ElementType>();
            damage = 20;
            _enemySpeed = enemySpeed;
            enemyObject =
                MonoBehaviour.Instantiate(Resources.Load<GameObject>($"Prefabs/Game/{enemyType.ToString()}"),
                    parent, false);

            enemyObject.transform.parent.SetParent(parent);
            int count = 0;
            _enemySprite = enemyObject.transform.Find("EnemySprite").gameObject;
            switch (enemyType)
            {
                case Common.Enumerators.EnemyType.SmallSlime:
                    count = 2;
                    scoreGive = 10;
                    break;
                case Common.Enumerators.EnemyType.MediumSlime:
                    count = 3;
                    scoreGive = 20;
                    break;
                case Common.Enumerators.EnemyType.Witch:
                    count = 5;
                    scoreGive = 30;
                    fireGun = _enemySprite.transform.Find("FireGun").gameObject;
                    _camera = GameObject.Find("Camera").GetComponent<Camera>();
                    _leftBorder = _camera.ViewportToWorldPoint(new Vector2 (0,0));
                    _rightBorder = _camera.ViewportToWorldPoint(new Vector2 (1,0));
                    _canShoot = true;
                    break;
                case Common.Enumerators.EnemyType.Boss:
                    count = 11;
                    scoreGive = 40;
                    damage = 100;
                    _enemySpeed = enemySpeed / 2;
                    break;
            }

            for (int i = 0; i < count; i++)
            {
                buttonElementsToDie.Add(SetRandomElement());
            }

            playerObject = GameClient.Instance.GetService<IGameManager>().PlayerController.mainPlayer.playerObject;
            for (int i = 0; i <= buttonElementsToDie.Count - 1; i++)
            {
                _elementToDieObjects.Add(SetDieObject(i, buttonElementsToDie[i]));
            }
        }

        public void Dispose()
        {
            MonoBehaviour.Destroy(enemyObject);
        }

        public void StartMove()
        {
            enemyObject.transform.position = GameClient.Instance.GetService<IGameManager>().Battleground.transform
                .Find(enemySpawnPosition.ToString()).position;
            isSpawn = true;
            if (_enemySprite.transform.position.x > playerObject.transform.position.x)
            {
                _enemySprite.transform.rotation = new Quaternion(0, -180, 0, 0);
            }
            if (_enemySprite.transform.position.x < playerObject.transform.position.x)
            {
                _enemySprite.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }

        private SpriteRenderer SetDieObject(int index, Common.Enumerators.ElementType elementType)
        {
            _dieElement = enemyObject.transform.Find($"Element_{index}").GetComponent<SpriteRenderer>();
            SetElement(elementType, _dieElement);
            return _dieElement;
        }

        public void SetElement(Common.Enumerators.ElementType elementType, SpriteRenderer element)
        {
            element.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>($"Textures/char/Ic_{elementType.ToString().ToLower()}");
        }

        public void ResetColor()
        {
            for (int i = 0; i <= _elementToDieObjects.Count - 1; i++)
            {
                _elementToDieObjects[i].color = Color.white;
            }
            lastElementIndex = 0;
        }

        public void DestroyColor(int index)
        {
            _elementToDieObjects[index].color = Color.black;
            lastElementIndex = index + 1;
            
        }

        private Common.Enumerators.ElementType SetRandomElement()
        {
            var buttonType = (Common.Enumerators.ElementType) UnityEngine.Random.Range(0,
                Enum.GetNames(typeof(Common.Enumerators.ElementType)).Length);
            return buttonType;
        }

        public void Update()
        {
            if (isSpawn)
            {
                if (enemyObject != null)
                {
                    if (enemyType == Enumerators.EnemyType.SmallSlime || enemyType == Enumerators.EnemyType.MediumSlime)
                    {
                        enemyObject.transform.position = Vector2.MoveTowards(enemyObject.transform.position,
                            playerObject.transform.position, _enemySpeed * Time.deltaTime);
                        if (Vector3.Distance(enemyObject.transform.position, playerObject.transform.position) <= 1f)
                        {
                            Debug.Log("Enemy End Move");
                            EnemyEndMoveEvent?.Invoke(this);
                        }
                    }
                    if (enemyType == Enumerators.EnemyType.Witch)
                    {
                        if (_enemySprite.transform.rotation == new Quaternion(0, -1, 0, 0))
                        {
                            enemyObject.transform.Translate(Vector2.left * _enemySpeed * Time.deltaTime);
                            if (enemyObject.transform.position.x < _leftBorder.x-3f)
                            {
                                _enemySprite.transform.rotation = new Quaternion(0, 0, 0, 1);
                                _canShoot = true;
                            }
                        }
                        if (_enemySprite.transform.rotation == new Quaternion(0, 0, 0, 1))
                        {
                            enemyObject.transform.Translate(Vector2.right * _enemySpeed * Time.deltaTime);
                            if (enemyObject.transform.position.x > _rightBorder.x + 3f)
                            {
                                _enemySprite.transform.rotation = new Quaternion(0, -1, 0, 0);
                                _canShoot = true;
                            }
                        }
                        if (Vector3.Distance(new Vector2(enemyObject.transform.position.x, 0), new Vector2(playerObject.transform.position.x, 0)) <= 2f)
                        {
                            if (_canShoot)
                            {
                                EnemyEndMoveEvent?.Invoke(this);
                                _canShoot = false;
                            }
                        }
                    }
                }
            }
        }
    }
}