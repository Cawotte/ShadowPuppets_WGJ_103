namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;


    public class TilePath
    {
        private TileNode[] path;
        private TilePath simplifiedPath;
        private Vector3Int[] directionPath;

        public TileNode[] Path { get => path;  }

        public TileNode Start
        {
            get => path[0];
        }

        public TileNode Goal
        {
            get => path[path.Length - 1];
        }
        public bool IsEmpty
        {
            get => Size == 0;
        }

        public int Size
        {
            get => (path == null) ? 0 : path.Length;
        }
        public Vector3Int[] DirectionPath { get => directionPath;  }
        public TilePath SimplifiedPath { get => simplifiedPath;  }

        public TileNode this[int i]
        {
            get => path[i];
        }

        /// <summary>
        /// Empty path
        /// </summary>
        public TilePath()
        {

        }

        public TilePath(IEnumerable<TileNode> tiles, bool isSimplified = false)
        {
            this.path = tiles.ToArray();

            LoadDirectionPath();

            if (!isSimplified)
            {
                LoadSimplifiedPath();
            }
        }



        public Stack<TileNode> GetReversePath()
        {
            return new Stack<TileNode>(path);
        }

        public Vector3[] GetWorldPath()
        {
            Vector3[] cellPath = new Vector3[Size];

            for (int i = 0; i < Size; i++)
            {
                cellPath[i] = path[i].CenterWorld;
            }

            return cellPath;
        }

        public Vector3Int[] GetCellPath()
        {
            Vector3Int[] cellPath = new Vector3Int[Size];

            for (int i = 0; i < Size; i++)
            {
                cellPath[i] = path[i].CellPos;
            }

            return cellPath;
        }

        private void LoadDirectionPath()
        {
            if (IsEmpty) return;

            directionPath = new Vector3Int[Size - 1];

            for (int i = 0; i < Size - 1; i++)
            {
                directionPath[i] = path[i + 1].CellPos - path[i].CellPos;
            }
        }

        private void LoadSimplifiedPath()
        {
            if (IsEmpty) return;

            List<TileNode> simplePath = new List<TileNode>();

            Vector3Int lastDirection = directionPath[0];

            simplePath.Add(Start);

            //We add a node to the path only when the direction change
            for (int i = 1; i < Size; i++)
            {
                if (i == Size - 1)
                {
                    simplePath.Add(path[i]); //goal
                    break;
                }
                if (directionPath[i - 1] != directionPath[i])
                {
                    simplePath.Add(path[i]);
                }
            }

            simplifiedPath = new TilePath(simplePath, true);
        }

    }
}
