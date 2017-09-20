using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridSpace : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	public bool isSelected = false, isHighlight = false;
	public Button button;
	public Image selectImage, background;
	public Text buttonText;
	public string digitText;
    private int gridIndex;
	private GameController gameController;
	public void SetController(GameController gc)
	{
		gameController = gc;
	}
    public void SetIndex(int index)
    {
        gridIndex = index;
        //Debug.Log("setting index: " + index);
    }
	public void SetText(string text)
	{
        //Debug.Log("setting text: " + text + " at index " + gridIndex);
		buttonText.text = digitText = text;
	}
	public void SetHighlight(bool isEnable = true)
	{
		//Debug.Log("highlight: " + gridIndex + ", " + isEnable);
		isHighlight = isEnable;
		background.gameObject.SetActive(isHighlight);
	}
	public void OnSelect(BaseEventData eventData)
	{
		isSelected = true;
		selectImage.gameObject.SetActive(isSelected);
        gameController.SetActiveIndex(gridIndex);
	}
	public void OnDeselect(BaseEventData eventData)
	{
		isSelected = false;
		selectImage.gameObject.SetActive(isSelected);
        gameController.ClearActiveIndex();
	}
}
