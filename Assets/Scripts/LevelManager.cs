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
        private Camera mainCamera;

        [SerializeField]
        private Tilemap tilemap;

        [SerializeField]
        private Map map;

        [SerializeField]
        private Transform testMarker;

        private Pathfinder pathfinder;


        public PlayerCharacter Player { get => player; set => player = value; }
        public Transform BulletsParent { get => bulletsParent;  }
        public Map Map { get => map; }
        public Pathfinder Pathfinder { get => pathfinder; }
        public Transform TestMarker { get => testMarker; }

        // Start is called before the first frame update
        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            map = new Map(tilemap.layoutGrid, tilemap);
            pathfinder = new Pathfinder(map);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
