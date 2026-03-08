using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public enum CellSelectType
{
	Unselected,
	Selected
}
public enum CellCheckType
{
    UnChecked,
	Checked
}

public class MiniMapCell : MonoBehaviour, IPointerClickHandler
{
    public int x;
    public int y;

    public MiniMapController map;
    public Image image;

    [Header("지나갈 때 아이콘")]
    public Image OverlayIcon; // 보스몬스터

    public bool IsbossSpawned;
    public bool visited;

    public Color unvisitedColor = new Color(0.1f, 0.1f, 0.1f, 0.9f);
    public Color visitedColor = new Color(0.9f, 0.95f, 1f, 1f);

    private CellSelectType cellState = CellSelectType.Unselected;
    private CellCheckType checkState = CellCheckType.UnChecked;
    private SectionType sectionType;

    [SerializeField] private List<CellSpriteSet> cellSpriteSets;

    void Awake()
    {
        if (image == null)
            image = GetComponent<Image>();

        //아이콘초기화
        OverlayIcon.gameObject.SetActive(false);
        FieldManager.Instance.StartInit += ResetType;
    }

    public void SetCoord(int x, int y)
    {
        this.x = x;
        this.y = y;
        gameObject.name = $"Cell_{x}_{y}";
    }
    

    public void ResetType()
    {
        sectionType = FieldManager.Instance.sections[x,y].Type;
    }



    public void RefreshVisited()
    {
        var mover = ShipMover.Instance;
        if (mover == null || mover.visited == null || map == null)
            return;

        // 🔁 좌표계 맞추기 (Ship = 위0 / 미니맵 = 아래0)
   

        //bool visitedCell = mover.visited[x, y];

        // 🎯 현재 위치인지 확인
        bool isCurrent =
            mover.targetX == x &&
            mover.targetY == y;

        if (isCurrent)
        {
   //         checkState = CheckState.Checked;
   //         cellState = CellState.Selected;
			//CellImageChange();
			image.color = new Color(1f, 0.85f, 0f, 1f);  // 🟡 현재 위치(눈에 확 띄게)
        }
        else if (visited)
        {
            cellState = CellSelectType.Unselected;
			checkState = CellCheckType.Checked;
			CellImageChange();
            image.color = new Color(0.35f, 0.8f, 1f, 1f); // 🔵 지나온 곳(밝은 파랑)
        }
        else
        {
            checkState = CellCheckType.UnChecked;
            cellState = CellSelectType.Unselected;
			CellImageChange();
			image.color = unvisitedColor;                 // ⚫ 아직 안 간 곳
        }
    }

    public void CellImageChange()
    {
        foreach(CellSpriteSet cell in cellSpriteSets)
        {
            if(cell.SelectType == cellState && cell.CheckType == checkState && cell.SectionType == sectionType)
            {
                image.sprite = cell.BaseSprite;
            }
            if(cell.IsBoss == true && IsbossSpawned == true)
            {
                OverlayIcon.gameObject.SetActive(true);
                OverlayIcon.sprite = cell.BaseSprite;
            }
        }
    }

    public void ChangeStateToUnselected()
    {
        cellState = CellSelectType.Unselected;
    }


    // 🔥 여기서 좌 / 우클릭 모두 처리 가능
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"[MiniMap] RIGHT CLICK : ({x},{y})");
            map.OnCellClicked(x, y);
            cellState = CellSelectType.Selected;
            CellImageChange(); //셀렉트 이미지로 바뀜
        }

        // 만약 좌클릭도 쓰고 싶으면:
        // if (eventData.button == PointerEventData.InputButton.Left)
        // { ... }
    }
}
