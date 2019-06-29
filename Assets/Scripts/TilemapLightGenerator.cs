using Light2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapLightGenerator : MonoBehaviour
{

    [SerializeField] GameObject tilePrefab;
    private Tilemap tilemap;
    private LightObstacleGenerator lightGen;

    public struct TilePair {
        public Vector3 CenterWorld;
        public Sprite Sprite;
    }

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        lightGen = GetComponent<LightObstacleGenerator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        IEnumerable<TilePair> tiles = GetListUsedTiles();
        foreach (TilePair tile in tiles) {
            GenerateTileShadow(tile);
        }
    }

    private IEnumerable<TilePair> GetListUsedTiles()
    {

        List<TilePair> tiles = new List<TilePair>();
        //List<Vector3> tileWorldLocations = new List<Vector3>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = tilemap.CellToWorld(localPlace);
            if (tilemap.HasTile(localPlace))
            {
                TilePair tile = new TilePair();
                tile.CenterWorld = tilemap.GetCellCenterWorld(localPlace);
                tile.Sprite = tilemap.GetSprite(localPlace);

                tiles.Add(tile);



            }
        }

        return tiles;
    }

    private void GenerateTileShadow(TilePair tile)
    {
        //Generate a new GameObject
        GameObject shadowObject = Instantiate<GameObject>(tilePrefab, transform);
        shadowObject.transform.position = tile.CenterWorld;
        shadowObject.transform.localRotation = Quaternion.identity;

        //Use sprite
        SpriteRenderer sr = shadowObject.GetComponent<SpriteRenderer>();
        sr.sprite = tile.Sprite;

        //Copy Component
        CopyComponent<LightObstacleGenerator>(lightGen, shadowObject);

        shadowObject.SetActive(true);
    }

    private static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst as T;
    }
}
