using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDevs.AppName
{
    public class FireBall
    {
        private GameObject _playerObject;
        private float _fireBallSpeed;
        private GameObject _fireBallobject;
        private Vector3 _fireBallMove;
        public int fireBallDamage;
        public event Action<FireBall, bool> FireBallEndMoveEvent;
        public bool isHit;

        public FireBall(GameObject playerObject, Transform fireGunPosition)
        {
            _playerObject = playerObject;
            _fireBallobject =
                MonoBehaviour.Instantiate(Resources.Load<GameObject>($"Prefabs/Game/FireBall"));
            _fireBallobject.transform.position = fireGunPosition.position;
            _fireBallSpeed = 3f;
            fireBallDamage = 40;
            _fireBallMove = new Vector3(_playerObject.transform.position.x, _playerObject.transform.position.y - 10f) - new Vector3(_fireBallobject.transform.position.x, 0);
            isHit = false;
        }

        public void Dispose()
        {
            MonoBehaviour.Destroy(_fireBallobject);
        }
        public void Update()
        {
            Debug.Log(_playerObject);
            if (_playerObject != null)
            {
                _fireBallobject.transform.position = Vector3.MoveTowards(_fireBallobject.transform.position,
                    _fireBallMove, _fireBallSpeed * Time.deltaTime);
                if (Vector3.Distance(_fireBallobject.transform.position, _playerObject.transform.position) >= 20f)
                {
                    Debug.Log("Miss");
                    FireBallEndMoveEvent?.Invoke(this, false);
                }
                if (Vector3.Distance(_fireBallobject.transform.position,_playerObject.transform.position) <= 2f)
                {
                    isHit = true;
                    Debug.Log("Hit");
                    FireBallEndMoveEvent?.Invoke(this,true);
                }
            }
        }
    }
}
