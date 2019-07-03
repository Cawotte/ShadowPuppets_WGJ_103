namespace WGJ.PuppetShadow
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class LevelManager : Singleton<LevelManager>
    {

        [SerializeField]
        private PlayerCharacter player;

        [SerializeField]
        private Transform bulletsParent;
        [SerializeField]
        private Transform puppetsParent;
        [SerializeField]
        private Transform forbiddenSpawnParent;

        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private Tilemap tilemap;

        [SerializeField]
        private Map map;

        [SerializeField]
        private GameObject originalPuppetPrefab;

        [SerializeField]
        private Transform testMarker;

        private Pathfinder pathfinder;

        [SerializeField]
        private bool moveAroundMarker = false;

        [SerializeField]
        [ReadOnly]
        private int score = 0;
        [SerializeField]
        [ReadOnly]
        private int currentPuppets = 0;

        public PlayerCharacter Player { get => player; set => player = value; }
        public Transform BulletsParent { get => bulletsParent;  }
        public Map Map { get => map; }
        public Pathfinder Pathfinder { get => pathfinder; }
        public Transform TestMarker { get => testMarker; }
        public int Score { get => score; set => score = value; }
        public int CurrentPuppets { get => currentPuppets; set => currentPuppets = value; }

        private float timer = 0f;
        private Vector3 possiblePoint;
        private List<Bounds> forbiddenArea;
        // Start is called before the first frame update
        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            map = new Map(tilemap.layoutGrid, tilemap);
            pathfinder = new Pathfinder(map);
            possiblePoint = map.GetRandomSpawnPoint();
            forbiddenArea = GetForbiddenArea();
        }

        // Update is called once per frame
        void Update()
        {
            if (moveAroundMarker)
            {
                if (timer > 2f)
                {
                    MoveTestMarkerAround();
                    timer = 0f;
                }
                timer += Time.deltaTime;
            }
        }

        public Transform GetTarget()
        {
            if (player == null)
            {
                return testMarker.transform;
            }
            else
            {
                return player.transform;
            }
        }

        public void MoveTestMarkerAround()
        {
            testMarker.transform.position = map.GetRandomEmptyPosition();

        }
        public void SpawnNewPuppet()
        {
            SpawnPuppetAt(PickValidSpawnPoint());
        }
        private Vector3 PickValidSpawnPoint()
        {
            Vector3 spawnPoint;
            do
            {
                spawnPoint = map.GetRandomSpawnPoint();
            } while (!SpawnPointIsValid(spawnPoint));

            return spawnPoint;
        }

        private void SpawnPuppetAt(Vector3 spawnPos)
        {
            GameObject newPuppet = Instantiate(originalPuppetPrefab, puppetsParent);
            newPuppet.transform.position = spawnPos;

            newPuppet.SetActive(true);
        }
        private bool SpawnPointIsValid(Vector3 spawnSpoint)
        {
            Vector2 spawn2 = spawnSpoint;
            foreach (Bounds bound in forbiddenArea)
            {
                if (bound.Contains(spawn2))
                {
                    return false;
                }
            }
            return Vector3.Distance(spawnSpoint, player.transform.position) > 20f;
        }

        private List<Bounds> GetForbiddenArea()
        {
            List < Bounds > bounds = new List<Bounds>();
            SpriteRenderer[] sprites = forbiddenSpawnParent.gameObject.GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sprites.Length; i++)
            {
                bounds.Add(sprites[i].bounds);
            }

            return bounds;
        }

        private void OnDrawGizmos()
        {
            if (possiblePoint != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, possiblePoint);
            }
        }
    }
}
