using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Describes a simple item in the Customize grid section in the UI.
/// </summary>
public class GridItem : MonoBehaviour
{
    /* Every grid item points to a ScriptableObject of SliderDataCustomizer
     * to store its slider data. */
    public SliderDataCustomizer Data { get; set; }

    private Button button;
    private Image image;


    private void Awake()
    {
        // Runs a custom function whenever a GridItem is clicked/pressed
        button = GetComponent<Button>();
        button.onClick.AddListener(() => { SelectItem(); });

        image = GetComponent<Image>();
    }


    /// <summary>
    /// Assign a ScriptableObject to this GridItem to store its settings in.
    /// </summary>
    /// <param name="data"></param>
    public void InitializeItem(SliderDataCustomizer data)
    {
        Data = data;
        image.sprite = data.gridSprite;

        // Disable if this is the data of the default terrain block.
        if (data == GenerationManager.Instance.defaultBlock)
            gameObject.SetActive(false);
    }

    /// <summary>
    /// Clears the selection of this item.
    /// </summary>
    public void DeselectItem()
    {
        image.color = Color.white;
    }

    /// <summary>
    /// Selects this item.
    /// </summary>
    public void SelectItem()
    {
        if (UserInterface.Instance.SelectedItem != null)
            UserInterface.Instance.SelectedItem.DeselectItem();
        UserInterface.Instance.SelectedItem = this;
        image.color = Color.green;

        // Update all customizer UI sliders in the scene with the data of this GridItem
        UserInterface.Instance.customizerTitle.text = Data.itemName;
        foreach (SliderObject customizerEntry in UserInterface.Instance.CustomizerSliders)
        {
            customizerEntry.dataObject = Data;
            customizerEntry.InitializeField();
        }
    }
}
