using UnityEngine;

namespace GrandDevs.AppName
{
    [System.Serializable]
    public class PlayerInfo
    {
        public int maxHealth { get; set; }
        public int maxMana { get; set; } 
        public int scoreNumber { get; set; }
        
        public float playerManaRecoverTime { get; set; }
    }
    public class Player
    {
        public int maxHealth;
        public int maxMana;
        public int health;
        public int mana;
        public float playerManaRecoverTime;
        private Vector3 _playerPosition;
        public GameObject playerObject;
        private float speed;
        private Vector3 _leftBorder;
        private Vector3 _rightBorder;
        private Camera _camera;
        public bool canLeftMove;
        public bool canRightMove;
        public GameObject playerSprite;
        public int scoreNumber;
        public Player(PlayerData playerData)
        {
            var parent = GameClient.Instance.GetService<IGameManager>().Battleground.transform
                .Find(Common.Enumerators.SpawnPosition.PlayerSpawnPosition.ToString());
            maxMana = playerData.maxMana;
            mana = maxMana;
            playerManaRecoverTime = playerData.playerManaRecoverTime;
            _playerPosition = parent.position;
            playerObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>($"Prefabs/Game/MainPlayer"),
                _playerPosition, Quaternion.identity);
            playerSprite = playerObject.transform.Find("PlayerSprite").gameObject;
            maxHealth = playerData.maxHealth;
            health = maxHealth;
            speed = 2;
            _camera = GameObject.Find("Camera").GetComponent<Camera>();
            scoreNumber = playerData.scoreNumber;
            _leftBorder = _camera.ViewportToWorldPoint(new Vector2 (0,0));
            _rightBorder = _camera.ViewportToWorldPoint(new Vector2 (1,0));
            canLeftMove = true;
            canRightMove = true;
        }

        public Player()
        {
            
        }
        public void LeftMove()
        {
            canLeftMove = true;
            canRightMove = true;
            if (canLeftMove)
            {
                
                if (playerObject.transform.position.x <= _leftBorder.x)
                {
                    canLeftMove = false;
                    return;
                }
                playerObject.transform.Translate(Vector2.left * speed * Time.deltaTime);
            }
        }
        public void RightMove()
        {
            canRightMove = true;
            canLeftMove = true;
            if (canRightMove)
            {
                if (playerObject.transform.position.x >= _rightBorder.x)
                {
                    canRightMove = false;
                    return;
                }
                playerObject.transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
        }
    }
}