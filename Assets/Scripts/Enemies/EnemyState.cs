using System.Collections;
using Pathfinding;
using Player;
using UnityEngine;

namespace Enemies
{
    public class EnemyState : MonoBehaviour
    {
        private static Transform _wallsPosition;
        
        [SerializeField] private EnemySettings enemySettings;
        [SerializeField] private SheepSettings sheepSettings;

        private AIDestinationSetter destinationSetter;
        private AIPath aiPath;
        private bool updateTarget = true;
        
        private Transform PlayerTransform => PlayerController.Instance.transform;

        private void Awake()
        {
            if (_wallsPosition == null)
            {
                _wallsPosition = GameObject.FindGameObjectWithTag("WallCenter").transform;
                DontDestroyOnLoad(_wallsPosition);
            }
        }

        void Start()
        {
            destinationSetter = GetComponent<AIDestinationSetter>();
            aiPath = GetComponent<AIPath>();
            // InitializeSettings(); // todo: find out why this line doesnt work
        }

        private void Update()
        {
            if (!updateTarget) return;
            updateTarget = false;
            ManageTarget();
        }
        
        private void InitializeSettings()
        {
            if (sheepSettings != null) return;
            var sheepSetts = AssetBundle.FindObjectsOfType<SheepSettings>();
            if (sheepSetts.Length > 1) 
                Debug.LogWarning("Enemy State ambiguity: None sheep settings was provided and there are more then one in project!");
            if (sheepSetts.Length == 0)
            {
                Debug.LogError("Enemy State Problem: None Sheep settings were found!");
                return;
            }
            sheepSettings = sheepSetts[0];
        }

        private IEnumerator DelayChange()
        {
            yield return new WaitForSeconds(enemySettings.targetRefreshTime);
            updateTarget = true;
        }

        private void ManageTarget()
        {
            if (Vector2.Distance(aiPath.destination, transform.position) <= 1f && destinationSetter.target == _wallsPosition)
            {
                var sheepIndex = Random.Range(0, sheepSettings.sheeps.Count);
                destinationSetter.target = sheepSettings.sheeps[sheepIndex].transform;
                return;
            }
            var enemyPos = transform.position;
            var distanceFromPlayer = Vector2.Distance(PlayerTransform.position, enemyPos);
            var distanceFromWalls = Vector2.Distance(_wallsPosition.position, enemyPos);
            destinationSetter.target = distanceFromPlayer < distanceFromWalls ? PlayerTransform : _wallsPosition;
            StartCoroutine(DelayChange());
        }
    }
}
