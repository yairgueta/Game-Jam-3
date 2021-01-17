using System.Collections;
using Pathfinding;
using Player;
using UnityEngine;

namespace Enemies
{
    public class EnemyState : MonoBehaviour
    {
        private static Transform _playerTransform;
        private static Transform _wallsPosition;
    
        [SerializeField] private EnemySettings enemySettings;
    
        private AIDestinationSetter destinationSetter;
        private bool updateTarget = true;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            _playerTransform = FindObjectOfType<PlayerController>().gameObject.transform;
            _wallsPosition = GameObject.FindGameObjectWithTag("WallCenter").transform;
        }

        void Start()
        {
            destinationSetter = GetComponent<AIDestinationSetter>();
        }

        private void Update()
        {
            if (updateTarget)
            {
                updateTarget = false;
                ManageTarget();
            }
        }

        private IEnumerator DelayChange()
        {
            yield return new WaitForSeconds(enemySettings.targetRefreshTime);
            updateTarget = true;
        }

        private void ManageTarget()
        {
            var enemyPos = transform.position;
            var distanceFromPlayer = Vector2.Distance(_playerTransform.position, enemyPos);
            var distanceFromWalls = Vector2.Distance(_wallsPosition.position, enemyPos);
            destinationSetter.target = distanceFromPlayer < distanceFromWalls ? _playerTransform : _wallsPosition;
            StartCoroutine(DelayChange());
        }
    }
}
