namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using UnityEngine.Tilemaps;

    [Serializable]
    public class TileNode
    {
        [SerializeField][ReadOnly]
        private Vector3Int cellPos;

        [SerializeField][ReadOnly]
        private Vector3 centerWorld;

        [SerializeField]
        [ReadOnly]
        private bool isObstacle;

        #region Properties
        public Vector3Int CellPos { get => cellPos; }
        public Vector3 CenterWorld { get => centerWorld; }
        public bool IsObstacle { get => isObstacle;  }
        #endregion

        public TileNode(Vector3Int cellPos, Vector3 centerWorld, bool isObstacle)
        {
            this.cellPos = cellPos;
            this.centerWorld = centerWorld;
            this.isObstacle = isObstacle;
        }
    }
}
