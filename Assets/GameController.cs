using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
	public GameObject boardObj, gridObj;
	public float spacing = 1f;
	private int activeIndex = -1;
	private List<GridSpace> gridArray = new List<GridSpace>();
	void Start()
	{
		var boardTransform = boardObj.transform;
		int index = 0;
		for (int y = -4; y <= 4; y++)
		{
			for (int x = -4; x <= 4; x++)
			{
				var go = Instantiate(gridObj, Vector3.zero, Quaternion.identity);
				go.transform.localScale = Vector3.one;
				go.transform.SetParent(boardTransform, false);

				var rect = go.GetComponent<RectTransform>();
				rect.localPosition = new Vector3(x * spacing, y * spacing, 0);
				
				/*
				string log = "x: " + x;
				log += ", y: " + y;
				log += ", s: " + spacing;
				Debug.Log(log);
				Debug.Log(rect.localPosition);
				*/

				var gs = go.GetComponentInChildren<GridSpace>();
				if (gs)
				{
					/*
					string log = "index: " + index;
					log += ", row: " + GetRowFromIndex(index);
					log += ", col: " + GetColFromIndex(index);
					log += ", box: " + GetBoxFromIndex(index);
					Debug.Log(log);
					 */

					gs.SetController(this);
					gs.SetIndex(index++);
					gs.SetText("");
					gridArray.Add(gs);
				}
			}
		}
		if (gridArray.Count != 81)
		{
			Debug.Log("ERROR: gridArray.Count == " + gridArray.Count);
		}
	}
	void OnGUI()
	{
		Event e = Event.current;
		if (e.isKey)
		{
			var keyCode = e.keyCode;
			//Debug.Log(keyCode);
			string keychar = e.character.ToString();
			int digit;
			if (int.TryParse(keychar, out digit))
			{
				SetActiveDigit(digit);
			}
			else if (keyCode == KeyCode.Delete)
			{
				SetActiveDigit();
			}
		}
	}
	void SetActiveDigit(int digit = 0)
	{
		if (activeIndex >= 0 && activeIndex < gridArray.Count)
		{
			string text = "";
			if (digit >= 1 && digit <= 9)
				text = digit.ToString();
			gridArray[activeIndex].SetText(text);
		}
	}
	public void SetActiveIndex(int index)
	{
		activeIndex = index;
		//Debug.Log("active index is " + activeIndex);
		HashSet<int> peers = GetPeers(index);

		for (int i = 0; i < gridArray.Count; i++)
			SetGridHighlight(i, false);

		foreach (int i in peers)
			SetGridHighlight(i);
	}
	public void ClearActiveIndex()
	{
		activeIndex = -1;
		//Debug.Log("active index is " + activeIndex);
	}
	public void SetGridHighlight(int index, bool isEnabled = true)
	{
		if (index >= 0 && index < gridArray.Count)
			gridArray[index].SetHighlight(isEnabled);
	}
	public HashSet<int> GetPeers(int index)
	{
		HashSet<int> peers = new HashSet<int>();
		if (index < 0 || index >= gridArray.Count)
			return peers;

		peers.UnionWith(GetRowPeers(GetRowFromIndex(index)));
		peers.UnionWith(GetColPeers(GetColFromIndex(index)));
		peers.UnionWith(GetBoxPeers(GetBoxFromIndex(index)));

		return peers;
	}
	public HashSet<int> GetRowPeers(int row)
	{
		HashSet<int> peers = new HashSet<int>();

		for (int x = 0; x < 9; x++)
			peers.Add(row * 9 + x);

		return peers;
	}
	public HashSet<int> GetColPeers(int col)
	{
		HashSet<int> peers = new HashSet<int>();

		for (int y = 0; y < 9; y++)
			peers.Add(y * 9 + col);

		return peers;
	}
	public HashSet<int> GetBoxPeers(int box)
	{
		HashSet<int> peers = new HashSet<int>();

		int row = box / 3 * 3;
		int col = (box % 3) * 3;

		for (int y = 0; y < 3; y++)
			for (int x = 0; x < 3; x++)
				peers.Add((row + y) * 9 + (col + x));

		return peers;
	}
	public int GetBoxFromIndex(int index)
	{
		return index / 9 / 3 * 3 + (index % 9) / 3;
	}
	public int GetRowFromIndex(int index)
	{
		return index / 9;
	}
	public int GetColFromIndex(int index)
	{
		return index % 9;
	}
}
