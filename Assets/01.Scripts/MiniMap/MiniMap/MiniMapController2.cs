using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class MiniMapController2 : MonoBehaviour
{
	[Header("Grid Size")]
	public int width = 10;
	public int height = 10;

	[Header("UI")]
	public RectTransform gridRoot;
	public GridLayoutGroup gridLayout;
	public GameObject cellPrefab;

	[Header("Ship Icon")]
	public RectTransform goalPosIcon; //노란색 아이콘
	public RectTransform currentShipIcon; // 우주선 아이콘
	public RectTransform iconLayer;

	public MiniMapCell2[,] cells;
	public List<MiniMapCell2> cellList = new List<MiniMapCell2>();

	public UILineRenderer lineRenderer;

	public Section goalSection;

	public Dictionary<CellInfo, Sprite> cellSprites = new Dictionary<CellInfo, Sprite>();

	[SerializeField] private List<CellSpriteSet> cellSpriteSets;

	// Start is called once before the first execution of Update after the MonoBehaviour is

	private void Start()
	{
		FieldManager.Instance.inBlueSpot += ResetGrid;
		FieldManager.Instance.SectorChange += UpdateInfo;
	}


	// Update is called once per frame
	private void FixedUpdate()
	{
		if (FieldManager.Instance.isBlueSpot != true)
		{
			UpdateCurrentShipIconToLine();
			DrawLineRenderer();
		}	
	}

	public void Init()
	{
		InitDictionary();
		BuildGrid();

		ApplyBigCell();
		UpdateGoalPosIcon(5, 0);
		Canvas.ForceUpdateCanvases();
	}

	public void InitDictionary()
	{
		foreach (CellSpriteSet cellset in cellSpriteSets)
		{
			CellInfo cellInfo = new CellInfo(cellset.SectionType,
				cellset.SelectType, cellset.CheckType, cellset.IsPassed, cellset.IsBoss);

			cellSprites.Add(cellInfo, cellset.BaseSprite);
		}
	}

	void BuildGrid()
	{
		cells = new MiniMapCell2[width, height];
		for (int i = gridRoot.childCount - 1; i >= 0; i--)
			Destroy(gridRoot.GetChild(i).gameObject);

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				GameObject go = Instantiate(cellPrefab, gridRoot);
				MiniMapCell2 cell = go.GetComponent<MiniMapCell2>();
				cells[x, y] = cell;
				cell.MinimapController2 = this;
				cell.SetCoord(x, y);
				cellList.Add(cell);
				cell.cellInfo.cellType = FieldManager.Instance.sections[cell.x, cell.y].Type;
			}
		}
	}

	void ResetGrid()
	{
		foreach (MiniMapCell2 cell in cellList)
		{
			cell.cellInfo = new CellInfo(SectionType.None,CellSelectType.Unselected ,CellCheckType.UnChecked,false,false);
		}
		ResetLine();
		ChangeSprite();
		UpdateGoalPosIcon(5, 0);
		UpdateCurrentShipIconFromInt(5, 0);
	}

	void UpdateInfo()
	{
		foreach(MiniMapCell2 cell in cellList)
		{
			cell.cellInfo.cellType = FieldManager.Instance.sections[cell.x, cell.y].Type;
		}
		OnCellClicked(5,0);
	}

	public void OnCellClicked(int x, int y)
	{
		Debug.Log($"[MiniMapController2] CLICK RECEIVED : UI({x},{y})");
		if (ShipMover.Instance == null)
		{
			Debug.LogError("[MiniMap] ShipMover.Instance is NULL !!!");
			return;
		}
		ShipMover.Instance.MoveToGrid(x, y);
		goalSection = FieldManager.Instance.sections[x, y];
		UpdateSelectType(x, y);
		UpdateGoalPosIcon();
		ChangeSprite();
	}

	public void UpdateSelectType(int X, int Y) // 만약 UpdateInfo에서 Select가 됐으면 selected,unchecked,이벤트 타입,ispassed,isboss에 따라서 이미지가 바뀜
	{
		foreach (var cell in cellList)
		{
			cell.UpdateInfo(X, Y);
		}
	}


	public void ChangeSprite(int x, int y)
	{
		cells[x, y].CellSprite.sprite = cellSprites[cells[x, y].cellInfo];
		Canvas.ForceUpdateCanvases();
	}

	public void ChangeSprite()
	{
		foreach (var cell in cellList)
		{
			try
			{
				cell.CellSprite.sprite = cellSprites[cell.cellInfo];

			}
			catch (System.Exception e)
			{
				Debug.LogError($"ASSIGN FAIL: {e.GetType().Name} / {e.Message}\n{e.StackTrace}");

			}
		}
		Canvas.ForceUpdateCanvases();
	}

	public void ChangePassed(Vector3 position) //이 함수가 업데이트에서 우주선이동하는 스크립트에서 실행되면서 우주선의 현재위치를 계속 가져와서 닿은 셀(섹션)의 정보를 바꿔주고있음
	{
		float tileSize = 200f;
		int locationOffset = 5; //섹션 생성할때 x 는 -5에서부터 5까지 생성되게 만들었음 (10개)
		int x = Mathf.RoundToInt(position.x / tileSize) + locationOffset;
		int y = Mathf.RoundToInt(position.z / tileSize);
		cells[x, y].cellInfo.isPassed = true;
		ChangeSprite(x, y);
	}

	public void ChangeChecked(int x, int y)
	{
		cells[x, y].cellInfo.isPassed = true;
		cells[x, y].cellInfo.cellCheckType = CellCheckType.Checked;
		ChangeSprite(x, y);
	}

	void ApplyBigCell()
	{
		float cell = 58f;   // 🔥 여기 숫자 키우면 셀 커짐 (60 / 80 / 100 / 150 등 원하는 값)
		gridLayout.cellSize = new Vector2(cell, cell);

		gridLayout.spacing = new Vector2(4f, 4f); // 셀 사이 여백 원하면
	}

	void UpdateGoalPosIcon()
	{
		if (goalPosIcon == null || iconLayer == null || ShipMover.Instance == null)
			return;

		RectTransform targetCell = cells[goalSection.gridPos.x, goalSection.gridPos.y].GetComponent<RectTransform>();
		if (targetCell == null)
			return;

		Vector3 world = targetCell.TransformPoint(Vector3.zero);
		Vector3 local = iconLayer.InverseTransformPoint(world);

		goalPosIcon.localPosition = local;
		goalPosIcon.gameObject.SetActive(true);
	}

	public void UpdateGoalPosIcon(int x, int y)
	{
		if (goalPosIcon == null || iconLayer == null)
			return;

		RectTransform targetCell = cells[x, y].GetComponent<RectTransform>();
		if (targetCell == null)
			return;
		Vector3 world = targetCell.TransformPoint(Vector3.zero);
		Vector3 local = iconLayer.InverseTransformPoint(world);

		goalPosIcon.localPosition = local;
		goalPosIcon.gameObject.SetActive(true);
	}

	void UpdateCurrentShipIconFromInt(int x, int y)
	{
		RectTransform targetCell = cells[x,y].GetComponent<RectTransform>();
		if (targetCell == null)  Debug.Log("해당 셀이 없음"); return;
		Vector3 world = targetCell.TransformPoint(Vector3.zero);
		Vector3 local = iconLayer.InverseTransformPoint(world);
		currentShipIcon.localPosition = local;
		currentShipIcon.gameObject.SetActive(true);
	}

	void UpdateCurrentShipIconToLine() // 쉽무버의 현재위치를 가져와서 쉽아이콘을 위치로 변경
	{
		if (goalPosIcon == null || iconLayer == null || ShipMover.Instance == null)
			return;
		float sectionXMin = -1000f;
		float SectionWholeXLength = 1800f;

		float SectionXratio = (ShipMover.Instance.gameObject.transform.position.x - sectionXMin) / SectionWholeXLength;

		float MapsXMin = -279f;
		float MapsWholeLengthX = 558F;
		//--------------
		float CalculMapX = MapsXMin + (SectionXratio * MapsWholeLengthX);
		//--------------

		float sectionZMin = 0f;
		float SectionWholeZLength = 1800f;

		float SectionZratio = (ShipMover.Instance.gameObject.transform.position.z - sectionZMin) / SectionWholeZLength;

		float MapsZMin = -280.1409f;
		float MapsWholeLengthZ = 558F;
		//--------------
		float CalculMapZ = MapsZMin + (SectionZratio * MapsWholeLengthZ);
		//--------------

		Vector3 newIconsPos = new Vector3(CalculMapX, CalculMapZ, 0f);

		currentShipIcon.localPosition = newIconsPos;

	}
	public void DrawLineRenderer()
	{
		if (goalPosIcon == null || iconLayer == null || ShipMover.Instance == null)
			return;

		Vector3 currentShipIcon2 = currentShipIcon.localPosition;
		Vector2 curShipPos = new Vector2(currentShipIcon2.x, currentShipIcon2.y);
		MiniMapCell2 cell = cells[ShipMover.Instance.targetX, ShipMover.Instance.targetY]; // 목표지점의 위치를 가져옴
		Vector3 cellvector = cell.transform.localPosition;
		Vector2 cellsPos = new Vector2(cellvector.x, cellvector.y);
		lineRenderer.Points = new Vector2[2];
		lineRenderer.Points[0] = curShipPos;
		lineRenderer.Points[1] = cellsPos;
		lineRenderer.SetAllDirty();
	}
	public void DrawLineRendererToInt(int x, int y)
	{
		if (goalPosIcon == null || iconLayer == null || ShipMover.Instance == null)
			return;

		Vector3 currentShipIcon2 = currentShipIcon.localPosition;
		Vector2 curShipPos = new Vector2(currentShipIcon2.x, currentShipIcon2.y);
		MiniMapCell2 cell = cells[x, y]; // 목표지점의 위치를 가져옴
		Vector3 cellvector = cell.transform.localPosition;
		Vector2 cellsPos = new Vector2(cellvector.x, cellvector.y);
		lineRenderer.Points = new Vector2[2];
		lineRenderer.Points[0] = curShipPos;
		lineRenderer.Points[1] = cellsPos;
		lineRenderer.SetAllDirty();
	}

	public void ResetLine()
	{
		lineRenderer.Points = new Vector2[0];
		lineRenderer.SetAllDirty();
	}
	
}
