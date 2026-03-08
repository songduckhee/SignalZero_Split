using UnityEngine;
using System;

public class ShipMover : MonoBehaviour
{
	public static ShipMover Instance;

	[Header("Grid Size")]
	public int maxWidth = 10;
	public int maxHeight = 10;

	[Header("Movement")]
	public float sectionSize = 200f; // 400f
	public float moveSpeed = 8f;
	public float rotateSpeed = 7f;   // 🔥 선회 속도 추가

	public int targetX;
	public int targetY;

	public bool[,] visited;

	private bool _shipBodyDestoyed = false;
	public bool shipBodyDestoyed {  get { return _shipBodyDestoyed; } set { _shipBodyDestoyed = value; } }

	Vector3 targetPos;
	public bool moving { get;  private set;  }

	[Header ("연관된 스크립트들 + 싱글톤사용을 위해 넣어둔 스크립트")]
	[SerializeField] private ShipEventHandler shipEventHandler = new ShipEventHandler();
	public MiniMapController2 miniMapController2;
	public ShipDamageReceiver shipDamageReceiver;
	public ShipBooster shipBooster;

	void Awake()
	{
		Instance = this;
		FieldManager.Instance.StartInit += miniMapController2.Init;
		FieldManager.Instance.EndInit += Init;
		
	}

	void Update()
	{
		if (moving)
			MoveStep();
	}

	private void Init()
	{
		targetX = 5;
		targetY = 0;
		miniMapController2.UpdateGoalPosIcon(targetX, targetY);
		shipDamageReceiver = GetComponent<ShipDamageReceiver>();
		shipBooster = GetComponent<ShipBooster>();
		shipBooster.isMoving = false;
		//MarkVisited(currentX, currentY);
		MoveToGrid(targetX, targetY);
	}

	public void MoveToGrid(int x, int y)
	{
		targetX = x;
		targetY = y;

		Section[,]sections = FieldManager.Instance.sections;
		if (sections[targetX,targetY] != null)
		{
			targetPos = sections[targetX, targetY].transform.position;
			targetPos.y = 0;
			moving = true;
		}
		else
		{
			Debug.Log($"섹션{sections[targetX,targetY]}가 비어있음!");
		}
		
		shipEventHandler.StartEventHandler(x,y,this.transform);
		shipBooster.isMoving = true;
		//사운드 스타트
	}

	void MoveStep()
	{
		Vector3 dir = (targetPos - transform.position);
		dir.y = 0f;

		// 🔥 회전 (부드럽게 선회)
		if (dir.sqrMagnitude > 0.001f)
		{
			Quaternion targetRot = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Slerp(
				transform.rotation,
				targetRot,
				rotateSpeed * Time.deltaTime
			);
		}

		// 🔥 이동
		transform.position = Vector3.MoveTowards(
			transform.position,
			targetPos,
			moveSpeed * Time.deltaTime);
		shipEventHandler.MoveEventHandler(transform);
		miniMapController2.ChangePassed(transform.position);

		// 도착 판정
		if (Vector3.Distance(transform.position, targetPos) < 0.1f)
		{
			miniMapController2.ChangeChecked(targetX,targetY);
			shipEventHandler.arriveEvent();
			moving = false;
			shipBooster.isMoving = false;

			//사운드 종료
			
		}
	}

	public Vector2Int GetCurrentPosToVector2Int()
	{
		float tileSize = 200f;
		int locationOffset = 5; //섹션 생성할때 x 는 -5에서부터 5까지 생성되게 만들었음 (10개)
		int x = Mathf.RoundToInt(transform.position.x/tileSize) + locationOffset ;
		int y = Mathf.RoundToInt(transform.position.z/tileSize);

		return new Vector2Int(x, y); //섹션의 위치를 가져오기위해 필요한 섹션 인덱스
	}
	public void MoveShipToStartPos()
	{
		Vector3 startPos = FieldManager.Instance.sections[5,0].transform.position;
	  this.gameObject.transform.position = new Vector3 (startPos.x,transform.position.y,startPos.z);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = new Vector3 (startPos.x, transform.position.y, startPos.z);
		//여기서 이동할때 생기는 파란색 파티클이 있었음 좋겠음.
	}
	public void ShipDestroy()
	{
		this.gameObject.SetActive(false);
		shipBooster.isMoving = false;
	}

}
