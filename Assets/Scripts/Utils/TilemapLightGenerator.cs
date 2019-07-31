using Light2D;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Make the 2DLights assets works with tilemap. 
/// Basically generate a LightObstacle Tile for each non-empty tile of the tilemap.
/// </summary>
public class TilemapLightGenerator : MonoBehaviour
{
    /**
     * Make the 2D Lights assets works with a tilemap.
     * For each Tile of the tilemap, use a tilePrefab to build a gameobject Tile that
     * will be on top of the actual tile, and attach it a copy of the Tilemap's LightGeneratorObstacle component,
     * which will make the tile create a 2D Lights shadow object.
     * */

    //A prefab tile that will be used as a base for all shadows.
    [SerializeField] GameObject tilePrefab;
    private Tilemap tilemap;
    private LightObstacleGenerator lightGen;

    /// <summary>
    /// Struct to pair a tile Sprite with a world pos.
    /// </summary>
    public struct TilePair {
        public Vector3 CenterWorld;
        public Sprite Sprite;
    }

    private void Awake()
    {
        //Get importantt components
        tilemap = GetComponent<Tilemap>();
        lightGen = GetComponent<LightObstacleGenerator>();
    }
    
    void Start()
    {
        IEnumerable<TilePair> tiles = GetListUsedTiles();
        foreach (TilePair tile in tiles) {
            GenerateTileShadow(tile);
        }
    }

    /// <summary>
    /// Get all the tiles in the tilemap and save them as a TilePair list.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// For a given TilePair, will generate a TileShadow using the prefab 
    /// and the tilemap LightObstacleGenerator setting
    /// </summary>
    /// <param name="tile"></param>
    private void GenerateTileShadow(TilePair tile)
    {
        //Generate a new GameObject
        GameObject shadowObject = Instantiate<GameObject>(tilePrefab, transform);
        shadowObject.transform.position = tile.CenterWorld;
        shadowObject.transform.localRotation = Quaternion.identity;

        //Use sprite
        SpriteRenderer sr = shadowObject.GetComponent<SpriteRenderer>();
        sr.sprite = tile.Sprite;

        //Copy LightObstacle component to the tiles that will act as a shadow.
        CopyComponent<LightObstacleGenerator>(lightGen, shadowObject).enabled = true;

        shadowObject.SetActive(true);
    }

    /// <summary>
    /// Thanks Stack Overflow. Copy a component to another gameobject.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="original"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
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
