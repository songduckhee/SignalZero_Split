using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.UI.Extensions;

public class MiniMapController : MonoBehaviour
{
    [Header("Grid Size")]
    public int width = 10;
    public int height = 10;

    [Header("UI")]
    public RectTransform gridRoot;
    public GridLayoutGroup gridLayout;
    public GameObject cellPrefab;

    [Header("Ship Icon")]
    public RectTransform shipIcon;
    public RectTransform currentShipIcon;
    public RectTransform iconLayer;

    public MiniMapCell[,] cells;

    public UILineRenderer lineRenderer;



    void Start()
    {
		StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        
        BuildGrid();
        yield return null;

        ApplyBigCell();

        Canvas.ForceUpdateCanvases();
        RefreshAllCells();
        //UpdateShipIcon();
    }

    void LateUpdate()
    {
        UpdateShipIcon();
		UpdateCurrentShipIconToLine();
		DrawLineRenderer();

	}

    // ---------------- GRID 생성 ----------------
    void BuildGrid()
    {
        cells = new MiniMapCell[width, height];
        for (int i = gridRoot.childCount - 1; i >= 0; i--)
            Destroy(gridRoot.GetChild(i).gameObject);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject go = Instantiate(cellPrefab, gridRoot);
                MiniMapCell cell = go.GetComponent<MiniMapCell>();
				cells[x, y] = cell;
				cell.map = this;
                cell.SetCoord(x, y);
                
            }
        }
    }

    // ---------------- 전체 셀 갱신 ----------------
    public void RefreshAllCells()
    {
        foreach (Transform t in gridRoot)
        {
            MiniMapCell c = t.GetComponent<MiniMapCell>();
            if (c != null)
                c.RefreshVisited();
        }
    }

    // ---------------- 🟡 쉽 아이콘 위치 갱신 ----------------
    void UpdateShipIcon()
    {
        if (shipIcon == null || iconLayer == null || ShipMover.Instance == null)
            return;

        RectTransform targetCell = null;

        foreach (Transform t in gridRoot) // 그리드루트의 자식들을 다 순회
        {
            MiniMapCell cell = t.GetComponent<MiniMapCell>();
            if (cell == null) continue;

            if (cell.x == ShipMover.Instance.targetX && cell.y == ShipMover.Instance.targetY)
            {
                targetCell = cell.GetComponent<RectTransform>();
                break;
            }
        }

        if (targetCell == null)
            return;

        Vector3 world = targetCell.TransformPoint(Vector3.zero);
        Vector3 local = iconLayer.InverseTransformPoint(world);

        shipIcon.localPosition = local;
        shipIcon.gameObject.SetActive(true);
    }

    void UpdateCurrentShipIcon()
    {
        Vector2Int shipPos = ShipMover.Instance.GetCurrentPosToVector2Int();
        RectTransform currentSell = cells[shipPos.x,shipPos.y].gameObject.GetComponent<RectTransform>();

        Vector3 world = currentSell.TransformPoint(Vector3.zero);
        Vector3 local = iconLayer.InverseTransformPoint(world);

        currentShipIcon.localPosition = local;
        currentShipIcon.gameObject.SetActive(true);
    }
    void UpdateCurrentShipIconToLine()
    {
		if (shipIcon == null || iconLayer == null || ShipMover.Instance == null)
			return;
		float sectionXMin = -1000f;
        float SectionWholeXLength = 1800f;

		float SectionXratio =  (ShipMover.Instance.gameObject.transform.position.x - sectionXMin)/SectionWholeXLength;

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

        Vector3 newIconsPos = new Vector3(CalculMapX,CalculMapZ,0f);

        currentShipIcon.localPosition = newIconsPos;

	} 

    public void DrawLineRenderer()
    {
		if (shipIcon == null || iconLayer == null || ShipMover.Instance == null)
			return;

		Vector3 currentShipIcon2 = currentShipIcon.localPosition;
        Vector2 curShipPos = new Vector2(currentShipIcon2.x,currentShipIcon2.y);
        MiniMapCell cell = cells[ShipMover.Instance.targetX,ShipMover.Instance.targetY];
        RectTransform cellPos = cell.GetComponent<RectTransform>();
        Vector3 cellvector = cell.transform.localPosition;
        Vector2 cellsPos = new Vector2(cellvector.x,cellvector.y);
        lineRenderer.Points = new Vector2[2];
        lineRenderer.Points[0] = curShipPos;
        lineRenderer.Points[1] = cellsPos;
		lineRenderer.SetAllDirty();
	}

    // ---------------- 클릭 → 실제 이동 ----------------
    public void OnCellClicked(int x, int y)
    {
        Debug.Log($"[MiniMapController] CLICK RECEIVED : UI({x},{y})");
        if (ShipMover.Instance == null)
        {
            Debug.LogError("[MiniMap] ShipMover.Instance is NULL !!!");
            return;
        }

        Debug.Log($"[MiniMapController] CALL MoveToGrid({x},{y})");
        ShipMover.Instance.MoveToGrid(x, y);
    }

    public void OnSpriteChangeUnSelected(int X, int Y)
    {
        foreach(MiniMapCell cell in cells)
        {
            if(cell.x == X && cell.y == Y)
            {
                continue;
            }
            cell.ChangeStateToUnselected();
        }
    }

    void ApplyBigCell()
    {
        float cell = 58f;   // 🔥 여기 숫자 키우면 셀 커짐 (60 / 80 / 100 / 150 등 원하는 값)
        gridLayout.cellSize = new Vector2(cell, cell);

        gridLayout.spacing = new Vector2(4f, 4f); // 셀 사이 여백 원하면
    }


}
