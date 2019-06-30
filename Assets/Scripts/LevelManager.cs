namespace WGJ.PuppetShadow
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LevelManager : Singleton<LevelManager>
    {

        [SerializeField]
        private PlayerCharacter player;

        [SerializeField]
        private Transform bulletsParent;

        [SerializeField]
        private Camera mainCamera;

        public PlayerCharacter Player { get => player; set => player = value; }
        public Transform BulletsParent { get => bulletsParent;  }

        // Start is called before the first frame update
        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
