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


        public PlayerCharacter Player { get => player; set => player = value; }
        public Transform BulletsParent { get => bulletsParent;  }
        public Map Map { get => map; }
        public Pathfinder Pathfinder { get => pathfinder; }
        public Transform TestMarker { get => testMarker; }

        private float timer = 0f;
        private Vector3 possiblePoint;
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
        }

        // Update is called once per frame
        void Update()
        {
            if (timer > 3f)
            {
                //SpawnPuppetAt(PickValidSpawnPoint());
                timer = 0f;
            }
            timer += Time.deltaTime;
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
            return Vector3.Distance(spawnSpoint, player.transform.position) > 20f;
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
