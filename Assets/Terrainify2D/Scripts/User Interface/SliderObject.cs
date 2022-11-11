using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Describes a slider object in the UI.
/// </summary>
public class SliderObject : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler
{
    public Slider inputSlider;    
    public Text inputSliderValueDisplay;
    public SliderData.SliderField field;
    public SliderData dataObject;

    private readonly string decimalFormatting = "F3";


    private IEnumerator Start()
    {
        yield return null;
        InitializeField();
    }


    /// <summary>
    /// Initializes by grabbing data from the assigned data object.
    /// </summary>
    public void InitializeField()
    {
        if (dataObject == null)
            return;

        inputSlider.value = dataObject.GetSliderData(field);
        inputSliderValueDisplay.text = !inputSlider.wholeNumbers ? inputSlider.value.ToString(decimalFormatting) 
            : inputSlider.value.ToString();
    }


    /// <summary>
    /// Updates this slider and its related data object with a new slider value.
    /// Called automatically by the related slider's OnValueChanged event.
    /// If this slider activated the Perlin preview map, it updates the map as well.
    /// </summary>
    public void EditField()
    {
        float inputValue = inputSlider.value;
        dataObject.SetSliderData(field, inputValue);
        inputSliderValueDisplay.text = !inputSlider.wholeNumbers ? 
            inputValue.ToString(decimalFormatting) : inputValue.ToString();

        if (UserInterface.Instance.perlinPreviewImage.enabled)
            UpdatePerlinPreviewMap();

        // Saves the data on its Scriptable Object to persist after exiting Play mode
#if UNITY_EDITOR
        EditorUtility.SetDirty(dataObject);
#endif
    }


    /// <summary>
    /// Updates the perlin map previewer with the right data.
    /// </summary>
    /// <returns></returns>
    private bool UpdatePerlinPreviewMap()
    {      
        // Multi-layer view
        if (UserInterface.Instance.perlinPreviewAllToggle.isOn)
        {
            switch (field)
            {
                case SliderData.SliderField.PERLIN_SPEED:
                case SliderData.SliderField.PERLIN_LEVEL:
                case SliderData.SliderField.ZONE_PERLIN_SPEED:
                case SliderData.SliderField.ZONE_PERLIN_LEVEL:
                case SliderData.SliderField.MAP_PERLIN_SPEED:
                case SliderData.SliderField.MAP_PERLIN_LEVEL:
                    UserInterface.Instance.CalculatePerlinPreviewMap(
                        dataObject.sliderData[(int)SliderData.SliderField.PERLIN_SPEED],
                        dataObject.sliderData[(int)SliderData.SliderField.PERLIN_LEVEL],
                        dataObject.sliderData[(int)SliderData.SliderField.ZONE_PERLIN_SPEED],
                        dataObject.sliderData[(int)SliderData.SliderField.ZONE_PERLIN_LEVEL],
                        dataObject.sliderData[(int)SliderData.SliderField.MAP_PERLIN_SPEED],
                        dataObject.sliderData[(int)SliderData.SliderField.MAP_PERLIN_LEVEL]);
                    return true;
                default:
                    return false;
            }
        }
        // Per-layer view
        else
        {
            switch (field)
            {
                case SliderData.SliderField.PERLIN_SPEED:
                case SliderData.SliderField.PERLIN_LEVEL:
                    UserInterface.Instance.CalculatePerlinPreviewMap(
                        dataObject.sliderData[(int)SliderData.SliderField.PERLIN_SPEED],
                        dataObject.sliderData[(int)SliderData.SliderField.PERLIN_LEVEL]);
                    return true;
                case SliderData.SliderField.ZONE_PERLIN_SPEED:
                case SliderData.SliderField.ZONE_PERLIN_LEVEL:
                    UserInterface.Instance.CalculatePerlinPreviewMap(
                        dataObject.sliderData[(int)SliderData.SliderField.ZONE_PERLIN_SPEED],
                        dataObject.sliderData[(int)SliderData.SliderField.ZONE_PERLIN_LEVEL]);
                    return true;
                case SliderData.SliderField.MAP_PERLIN_SPEED:
                case SliderData.SliderField.MAP_PERLIN_LEVEL:
                    UserInterface.Instance.CalculatePerlinPreviewMap(
                        dataObject.sliderData[(int)SliderData.SliderField.MAP_PERLIN_SPEED],
                        dataObject.sliderData[(int)SliderData.SliderField.MAP_PERLIN_LEVEL]);
                    return true;
                default:
                    return false;
            }
        }
    }


    /// <summary>
    /// Shows the Perlin preview map on mouse over if updating it was a success.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UpdatePerlinPreviewMap())
            UserInterface.Instance.perlinPreviewImage.enabled = true;
    }


    /// <summary>
    /// Hides the Perlin preview map when the mouse leaves this object.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        UserInterface.Instance.perlinPreviewImage.enabled = false;
    }
}
 