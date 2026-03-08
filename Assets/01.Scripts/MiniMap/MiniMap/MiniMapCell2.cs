using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;




public struct CellInfo
{

	public SectionType cellType;
	public CellSelectType cellSelectType;
	public CellCheckType cellCheckType;
	public bool isPassed;
	public bool isBossHere;

	public CellInfo(SectionType type, CellSelectType cellSelectType, CellCheckType cellCheckType, bool IsPassed, bool IsBossHere)
	{
		this.cellType = type;
		this.cellSelectType = cellSelectType;
		this.cellCheckType = cellCheckType;
		this.isPassed = IsPassed;
		this.isBossHere = IsBossHere;
	}
}

public class MiniMapCell2 : MonoBehaviour, IPointerClickHandler
{

	public int x, y;
	public MiniMapController2 MinimapController2;
	[SerializeField] public CellInfo cellInfo = new CellInfo(SectionType.None, CellSelectType.Unselected, CellCheckType.UnChecked, false, false);
	public Image CellSprite;
	public Image BossSprite;

	public void UpdateInfo(int X, int Y)
	{
		if (X == x && Y == y)
		{
			cellInfo.cellSelectType = CellSelectType.Selected;
		}
		else
		{
			cellInfo.cellSelectType = CellSelectType.Unselected;
		}
	}


	public void SetCoord(int X, int Y)
	{
		x = X;
		y = Y;
		gameObject.name = $"Cell_{x}_{y}";
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (ShipMover.Instance.shipBodyDestoyed != true && FieldManager.Instance.isBlueSpot == false)
		{
			MinimapController2.OnCellClicked(x, y);
			//클릭 사운드
		}
		if (ShipMover.Instance.shipBodyDestoyed == true)
		{
			Debug.Log("기체 파괴");
		}
	}
}
