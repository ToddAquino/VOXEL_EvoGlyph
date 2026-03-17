    using System.Collections.Generic;
    using UnityEngine;
    public class ExplorationHandler : MonoBehaviour
    {
        public static ExplorationHandler Instance;
        [SerializeField] List<Area> areaList;

        [Header("Current Environment Data")]
        [SerializeField] Area currentArea;
        [SerializeField] GameObject Player;
    
        void Awake()
        {
            Instance = this;
            Initialize(areaList[0]);
        }

        private void Start()
        {
            

        }
        void Initialize(Area area)
        {
            currentArea = area;
            ExplorationData data = GameManager.Instance.ExplorationData;
            foreach (Checkpoint checkpoint in currentArea.Checkpoints)
            {
                checkpoint.CheckpointSet += SetCurrentCheckpoint;
            }
            foreach (EnemyEncounter encounter in currentArea.EnemyEncounters)
            {
                // Only spawn if not defeated
                if (data.IsEnemyDefeated(encounter.GetEnemyID()))
                {
                    encounter.gameObject.SetActive(false);
                }
            }
            foreach (TomePickup pickup in currentArea.TomePickups)
            {
                if (data.IsTomeLooted(pickup.GetTomeID()))
                {
                    pickup.gameObject.SetActive(false);
                }
            }
            Player.transform.position = GameManager.Instance.ExplorationData.GetPlayerPosition();
        }
        // Update is called once per frame
        void Update()
        {
        
        }

        void SetCurrentCheckpoint(Checkpoint checkpoint)
        {
            GameManager.Instance.ExplorationData.LastCheckpointPosition = checkpoint.transform.position;
        }

        public void SetLastPosition(Vector3 transform)
        {
            GameManager.Instance.ExplorationData.LastPlayerPosition = transform;
        }
    }

    [System.Serializable]
    public class Area
    {
        public List<Checkpoint> Checkpoints;
        public List<EnemyEncounter> EnemyEncounters;
        public List<TomePickup> TomePickups;
    }