using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Extends SliderData to allow more detailed data for Customizer sliders.
/// </summary>
[CreateAssetMenu(menuName = "Blocks/SliderDataCustomizer")]
public class SliderDataCustomizer : SliderData
{
    public string itemName;
    public Tile itemTile;
    public Chunk.TileType itemType;
    public Sprite gridSprite;
}
