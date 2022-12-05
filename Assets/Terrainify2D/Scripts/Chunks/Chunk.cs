using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Describes a chunk in the world that can load and unload itself.
/// 
/// Added additional functions to provide quick integration into
/// world systems, such as getting and setting tiles, the block
/// type, changing tile colors, setting tile rotation etcetera.
/// </summary>
public class Chunk : MonoBehaviour
{
    public Vector3Int Position { get; private set; }
    public Vector3Int ChunkPosition { get; private set; }
    public Tilemap tileMapBlocksFront, tileMapBlocksBack;
    public TileType[,] tileTypeBlocksFront, tileTypeBlocksBack;
    public enum TileType
    {
        AIR,
        COAL,
        COPPER,
        DIRT,
        DIRT_GRASS,
        IRON,
        STONE
    }
    public enum TilemapType
    {
        BLOCKS_FRONT,
        BLOCKS_BACK
    }

    private BoxCollider2D chunkCollider;
    private bool isUnloading = false;
       

    private void Start()
    {
        Position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        ChunkPosition = new Vector3Int(
            Position.x / GenerationManager.Instance.chunkSize,
            Position.y / GenerationManager.Instance.chunkSize, 0);

        // Used to store the block types for each Tilemap
        tileTypeBlocksFront = new TileType[
            GenerationManager.Instance.chunkSize, 
            GenerationManager.Instance.chunkSize];
        tileTypeBlocksBack = new TileType[
            GenerationManager.Instance.chunkSize,
            GenerationManager.Instance.chunkSize];

        // Set the offset and size of the chunk's box collider based on the chunkSize.
        chunkCollider = GetComponent<BoxCollider2D>();
        chunkCollider.size = new Vector2(
            GenerationManager.Instance.chunkSize,
            GenerationManager.Instance.chunkSize);
        chunkCollider.offset = new Vector2(
            GenerationManager.Instance.chunkSize / 2,
            GenerationManager.Instance.chunkSize / 2);
       
        LoadChunk();
    }


    /// <summary>
    /// Generates this chunk.
    /// </summary>
    private void LoadChunk()
    {
        StartCoroutine(GenerationManager.Instance.GenerateChunk(this));
    }

    /// <summary>
    /// Deletes this chunk from the world.
    /// </summary>
    public void UnloadChunk()
    {
        /* Destroying the chunk doesn't happen instantly. DestroyImmediate works, but it is
         * safer to just track a bool until it is loaded out of memory. */
        Destroy(gameObject);
        isUnloading = true;
    }


    /// <summary>
    /// Returns the right Tilemap for the given TilemapType constant for this chunk.
    /// </summary>
    /// <param name="mapType"></param>
    /// <returns></returns>
    private Tilemap GetMap(TilemapType mapType)
    {
        if (isUnloading)
            return null;

        switch (mapType)
        {
            case TilemapType.BLOCKS_FRONT:
                return tileMapBlocksFront;
            case TilemapType.BLOCKS_BACK:
                return tileMapBlocksBack;
            default:
                return null;
        }
    }


    /// <summary>
    /// Returns the right TileType array for the given TilemapType constant for this chunk.
    /// These TileType arrays hold the block types for all chunk blocks.
    /// </summary>
    /// <param name="mapType"></param>
    /// <returns></returns>
    private TileType[,] GetTypeMap(TilemapType mapType)
    {
        if (isUnloading)
            return null;

        switch (mapType)
        {
            case TilemapType.BLOCKS_FRONT:
                return tileTypeBlocksFront;
            case TilemapType.BLOCKS_BACK:
                return tileTypeBlocksBack;
            default:
                return null;
        }
    }


    /// <summary>
    /// Returns the tile stored in the given Tilemap, on the given position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="mapType"></param>
    /// <returns></returns>
    public TileBase GetChunkTile(Vector3Int position, TilemapType mapType)
    {
        if (isUnloading)
            return null;

        Tilemap targetMap = GetMap(mapType);
        if (targetMap == null)
            return null;

        Vector3Int relativePosition = position - Position;
        return targetMap.GetTile(relativePosition);
    }


    /// <summary>
    /// Fills the given position in the given Tilemap with the given Tile.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="mapType"></param>
    /// <param name="tile"></param>
    public void SetChunkTile(Vector3Int position, TilemapType mapType, Tile tile)
    {
        if (isUnloading)
            return;

        Tilemap targetMap = GetMap(mapType);
        if (targetMap == null)
            return;

        Vector3Int relativePosition = position - Position;
        targetMap.SetTile(relativePosition, tile);
    }


    /// <summary>
    /// Sets the color of the tile in the given Tilemap at the given position.
    /// 
    /// NOTE: Tiles by default have TileFlags that prevent color changes, rotation
    /// lock etc. Make sure you unlock these tiles by changing its TileFlags or go 
    /// into the inspector and visit your tiles, turn on debug mode and change it there,
    /// before using this function.    
    /// </summary>
    /// <param name="position"></param>
    /// <param name="color"></param>
    /// <param name="mapType"></param>
    public void SetChunkTileColor(Vector3Int position, Color color,
        TilemapType mapType = TilemapType.BLOCKS_FRONT)
    {
        if (isUnloading)
            return;

        Tilemap targetMap = GetMap(mapType);
        if (targetMap == null)
            return;

        Vector3Int relativePosition = position - Position;
        targetMap.SetColor(relativePosition, color);
    }


    /// <summary>
    /// Returns the tile type of a block in this chunk, in the given Tilemap.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="mapType"></param>
    /// <returns></returns>
    public TileType GetChunkTileType(Vector3Int position, TilemapType mapType)
    {
        if (isUnloading)
            return TileType.AIR;

        TileType[,] data = GetTypeMap(mapType);
        if (data == null)
            return TileType.AIR;

        Vector3Int relativePosition = position - Position;
        return data[relativePosition.x, relativePosition.y];
    }


    /// <summary>
    /// Sets the tile type of a block in this chunk, in the given Tilemap.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="mapType"></param>
    /// <param name="type"></param>
    public void SetChunkTileType(Vector3Int position, TilemapType mapType, TileType type)
    {
        if (isUnloading)
            return;

        TileType[,] data = GetTypeMap(mapType);
        if (data == null)
            return;

        Vector3Int relativePosition = position - Position;
        data[relativePosition.x, relativePosition.y] = type;
    }


    /// <summary>
    /// Sets the rotation of a given tile at the given position in the given Tilemap.
    /// 
    /// NOTE: Tiles by default have TileFlags that prevent color changes, rotation
    /// lock etc. Make sure you unlock these tiles by changing its TileFlags or go 
    /// into the inspector and visit your tiles, turn on debug mode and change it there,
    /// before using this function.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="mapType"></param>
    /// <param name="rotation"></param>
    public void SetChunkTileRotation(Vector3Int position, TilemapType mapType, Vector3 rotation)
    {
        if (isUnloading)
            return;

        Tilemap targetMap = GetMap(mapType);
        if (targetMap == null)
            return;

        Vector3Int relativePosition = position - Position;
        Quaternion matrixRotation = Quaternion.Euler(rotation);
        Matrix4x4 matrix = Matrix4x4.Rotate(matrixRotation);
        targetMap.SetTransformMatrix(relativePosition, matrix);
    }
}