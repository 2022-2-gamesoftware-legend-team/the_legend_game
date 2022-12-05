using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Handles the loading and unloading of chunks in the game.
/// </summary>
public class ChunkLoadManager : Singleton<ChunkLoadManager>
{
    [Header("Chunks")]
    public SliderData chunkData;
    public GameObject chunkPrefab;
    public GameObject chunkRoot;
    public LayerMask chunkLayer;

    private bool isUpdatingChunks = false;


    private void Start()
    {
        // Sets the camera in the horizontal center of the world, at the surface
        float cameraX = 0.5f * GenerationManager.Instance.worldWidth;
        Camera.main.transform.position = new Vector3(
            cameraX, GenerationManager.Instance.surfaceHeights[(int)cameraX],
            Camera.main.transform.position.z);

        StartCoroutine(LoadChunks());
        StartCoroutine(UnloadChunks());
    }


    /// <summary>
    /// Removes all chunks currently in game.
    /// </summary>
    public void ClearAllChunks()
    {
        StopAllCoroutines();

        // Get all chunks
        List<Chunk> chunksToUnload = new List<Chunk>();
        foreach (Transform child in chunkRoot.transform)
        {
            Chunk chunk = child.GetComponent<Chunk>();
            if (chunk != null)
                chunksToUnload.Add(chunk);
        }

        // Delete them
        foreach (Chunk chunk in chunksToUnload)
            if (chunk != null)
                chunk.UnloadChunk();

        StartCoroutine(LoadChunks());
        StartCoroutine(UnloadChunks());
    }


    /// <summary>
    /// Returns the chunk at the given position. Uses a Raycast to detect the 
    /// chunk's BoxCollider.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Chunk GetChunk(Vector3Int position)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            new Vector2(position.x + 0.5f, position.y + 0.5f),
            Vector2.zero, 0f, chunkLayer);
        return hit ? hit.collider.GetComponent<Chunk>() : null;
    }

    /// <summary>
    /// Returns the tile stored in the chunk at the given position, in the given map.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="mapType"></param>
    /// <returns></returns>
    public TileBase GetChunkTile(Vector3Int position, Chunk.TilemapType mapType)
    {
        Chunk chunk = GetChunk(position);
        if (chunk != null)
            return chunk.GetChunkTile(position, mapType);
        return null;
    }

    /// <summary>
    /// Sets the tile in the chunk at the given position, in the given map.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="mapType"></param>
    /// <param name="tile"></param>
    public void SetChunkTile(Vector3Int position, Chunk.TilemapType mapType, Tile tile)
    {
        Chunk chunk = GetChunk(position);
        if (chunk != null)
            chunk.SetChunkTile(position, mapType, tile);
    }

    /// <summary>
    /// Returns the type of tile stored in the chunk at the given position, in the given map.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="mapType"></param>
    /// <returns></returns>
    public Chunk.TileType GetChunkTileType(Vector3Int position, Chunk.TilemapType mapType)
    {
        Chunk chunk = GetChunk(position);
        if (chunk != null)
            return chunk.GetChunkTileType(position, mapType);
        return Chunk.TileType.AIR;
    }

    /// <summary>
    /// Sets the type of tile for the chunk at the given position, in the given map.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="mapType"></param>
    /// <param name="type"></param>
    public void SetChunkTileType(Vector3Int position, Chunk.TilemapType mapType, Chunk.TileType type)
    {
        Chunk chunk = GetChunk(position);
        if (chunk != null)
            chunk.SetChunkTileType(position, mapType, type);
    }


    /// <summary>
    /// Returns a Rect that defines the area where chunks can be loaded in.
    /// </summary>
    /// <returns></returns>
    private Rect GetChunkLoadBounds()
    {
        Vector3 regionStart = Camera.main.transform.position + 
            Vector3.left * chunkData.GetSliderData(SliderData.SliderField.CHUNK_RADIUS_HORIZONTAL) + 
            Vector3.down * chunkData.GetSliderData(SliderData.SliderField.CHUNK_RADIUS_VERTICAL);
        Vector3 regionEnd = Camera.main.transform.position +
            Vector3.right * chunkData.GetSliderData(SliderData.SliderField.CHUNK_RADIUS_HORIZONTAL) + 
            Vector3.up * chunkData.GetSliderData(SliderData.SliderField.CHUNK_RADIUS_VERTICAL);

        // Convert to int for automatic flooring of coordinates
        int regionStartX = (int)regionStart.x / GenerationManager.Instance.chunkSize;
        int regionStartY = (int)regionStart.y / GenerationManager.Instance.chunkSize;
        int regionEndX = ((int)regionEnd.x + GenerationManager.Instance.chunkSize) / GenerationManager.Instance.chunkSize;
        int regionEndY = ((int)regionEnd.y + GenerationManager.Instance.chunkSize) / GenerationManager.Instance.chunkSize;
        Rect loadBoundaries = new Rect(regionStartX, regionStartY, regionEndX - regionStartX, regionEndY - regionStartY);

        return loadBoundaries;
    }

    /// <summary>
    /// Starts and maintains the sequence for loading chunks.
    /// </summary>
    /// <param name="done"></param>
    /// <param name="loadAll"></param>
    private IEnumerator LoadChunks()
    {
        while (true)
        {
            isUpdatingChunks = true;
            yield return StartCoroutine(PerformLoadChunks());                
            isUpdatingChunks = false;
            yield return null;
        }
    }

    /// <summary>
    /// Starts and maintains the sequence for unloading chunks.
    /// </summary>
    /// <returns></returns>
    private IEnumerator UnloadChunks()
    {
        while (true)
        {
            if (!isUpdatingChunks)
                yield return StartCoroutine(PerformUnloadChunks());
            yield return null;
        }
    }


    /// <summary>
    /// Scans and loads chunks in view using a Rect as bounds.
    /// </summary>
    /// <param name="loadAll"></param>
    private IEnumerator PerformLoadChunks()
    {
        Rect loadBoundaries = GetChunkLoadBounds();
        List<Chunk> chunksToLoad = new List<Chunk>();
        for (int h = (int)loadBoundaries.xMax; h >= (int)loadBoundaries.xMin; h--)
        {
            for (int v = (int)loadBoundaries.yMax; v >= (int)loadBoundaries.yMin; v--)
            {
                if ((h < 0 || h >= GenerationManager.Instance.worldWidth / GenerationManager.Instance.chunkSize) ||
                    (v < 0 || v >= GenerationManager.Instance.worldHeight / GenerationManager.Instance.chunkSize))
                    continue;
                Vector3Int chunkPosition = new Vector3Int(h, v, 0);
                Vector3Int worldPosition = new Vector3Int(
                    h * GenerationManager.Instance.chunkSize,
                    v * GenerationManager.Instance.chunkSize, 0);

                if (loadBoundaries.Contains(chunkPosition) && !GetChunk(worldPosition))
                {   
                    // Chunk automatically loads itself upon creation
                    chunksToLoad.Add(Instantiate(chunkPrefab, worldPosition, Quaternion.identity, chunkRoot.transform)
                        .GetComponent<Chunk>());
                    yield return null;
                }
            }
        }
    }


    /// <summary>
    /// Unloads chunks that are outside the view, when we're not loading chunks.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformUnloadChunks()
    {
        Rect loadBoundaries = GetChunkLoadBounds();
        List<Chunk> chunksToUnload = new List<Chunk>();
        foreach (Transform child in chunkRoot.transform)
        {
            Chunk chunk = child.GetComponent<Chunk>();
            if (chunk != null)
            {
                if (!loadBoundaries.Contains(chunk.ChunkPosition))
                    chunksToUnload.Add(chunk);
            }
        }

        foreach (Chunk chunk in chunksToUnload)
        {
            while (isUpdatingChunks)
                yield return null;

            if (chunk != null)
                chunk.UnloadChunk();
            yield return null;
        }
    }
}