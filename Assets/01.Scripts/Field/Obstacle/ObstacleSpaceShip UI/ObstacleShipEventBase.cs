using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleShipEventBase
{
	public abstract string AcceptBtnName { get; }
	public abstract string DetectBtnName { get; }
	public virtual string Option1BtnName { get { return ""; } }
	public virtual string Option2BtnName { get { return ""; } }
	public abstract ObstacleEventType EventType { get; }


	[Header("경로")]
	ShipDialogue shipDialogue = EventManager.Instance.ShipDialogue;
	public abstract int SectorNumber { get; }

	public List<string> AcceptLog = new List<string>();
	public List<string> DetectLog = new List<string>();
	public List<string> Option1Log = new List<string>();
	public List<string> Option2Log = new List<string>();
	public virtual void Accepted()
	{
		AcceptLog = new List<string> { "" };
		shipDialogue.SetLine(AcceptLog);
		shipDialogue.StartNextDialogue();
		shipDialogue.ActiveButton(false);
		shipDialogue.SetClear(true);
	}
	public virtual void Detected()
	{
		DetectLog = new List<string> { "" };
		shipDialogue.SetLine(DetectLog);
		shipDialogue.StartNextDialogue();
		shipDialogue.ActiveButton(false);
		shipDialogue.SetClear(true);
	}
	public virtual void Option1()
	{
		Option1Log = new List<string> { "" };
		shipDialogue.SetLine(Option1Log);
		shipDialogue.StartNextDialogue();
		shipDialogue.ActiveButton(false);
		shipDialogue.SetClear(true);
	}
	public virtual void Option2()
	{
		Option2Log = new List<string> { "" };
		shipDialogue.SetLine(Option2Log);
		shipDialogue.StartNextDialogue();
		shipDialogue.ActiveButton(false);
		shipDialogue.SetClear(true);
	}

	public class BlackMarketCape : ObstacleShipEventBase
	{
		public override string AcceptBtnName { get { return "상점"; } }

		public override string DetectBtnName { get { return "떠난다."; } }
		public override ObstacleEventType EventType { get { return ObstacleEventType.blackMarketCape; } }

		public override int SectorNumber { get { return 1; } }


		public override void Accepted()
		{
			AcceptLog = new List<string> { "" };
			shipDialogue.SetLine(AcceptLog);
			shipDialogue.StartNextDialogue();
			shipDialogue.ActiveButton(false);
			shipDialogue.SetClear(true);
		}
		public override void Detected()
		{
			AcceptLog = new List<string> { "" };
			shipDialogue.SetLine(AcceptLog);
			shipDialogue.StartNextDialogue();
			shipDialogue.ActiveButton(false);
			shipDialogue.SetClear(true);
		}
	}
	public class ImperialTradeLine : ObstacleShipEventBase
	{
		public override string AcceptBtnName { get { return "공격한다"; } }

		public override string DetectBtnName { get { return "시그널 재밍"; } }
		public override string Option1BtnName { get { return "떠난다."; } }

		public override ObstacleEventType EventType { get { return ObstacleEventType.imperialTradeLine; } }

		public override int SectorNumber { get { return 1; } }


		public override void Accepted()
		{
			AcceptLog = new List<string> { "흥미로운 자살 시도군." };
			shipDialogue.SetLine(AcceptLog);
			shipDialogue.StartNextDialogue();
			shipDialogue.ActiveButton(false);
			shipDialogue.SetClear(true);
		}

		public override void Detected()
		{
			AcceptLog = new List<string> { "이게 무슨 짓이지?", "뭔가 계획이 있었나 본 데, 네 놈 동료들은 널 버렸나 보군.", "걱정 마라. 네 놈의 배를 부수고 기록을 확인해서 그 놈들도 같은 곳에 보내주마." };
			shipDialogue.SetLine(AcceptLog);
			shipDialogue.StartNextDialogue();
			shipDialogue.ActiveButton(false);
			shipDialogue.SetClear(true);
		}

		public override void Option1()
		{
			AcceptLog = new List<string> { "" };
			shipDialogue.SetLine(AcceptLog);
			shipDialogue.StartNextDialogue();
			shipDialogue.ActiveButton(false);
			shipDialogue.SetClear(true);
		}
	}
	public class DockedRescueShip : ObstacleShipEventBase
	{
		public override string AcceptBtnName { get { return "크레딧을 (15) 주고 우주선을 (50%) 수리한다."; } }

		public override string DetectBtnName { get { return "떠난다."; } }

		public override ObstacleEventType EventType { get { return ObstacleEventType.dockedRescueShip; } }

		public override int SectorNumber { get { return 1; } }


		public override void Accepted()
		{
			AcceptLog = new List<string> { "" };
			shipDialogue.SetLine(AcceptLog);
			shipDialogue.StartNextDialogue();
			shipDialogue.ActiveButton(false);
			shipDialogue.SetClear(true);
		}

		public override void Detected()
		{
			AcceptLog = new List<string> { "" };
			shipDialogue.SetLine(AcceptLog);
			shipDialogue.StartNextDialogue();
			shipDialogue.ActiveButton(false);
			shipDialogue.SetClear(true);
		}

	}
	public class RescueSignal : ObstacleShipEventBase
	{
		public override string AcceptBtnName { get { return "도와준다. 스크랩 20 감소."; } }

		public override string DetectBtnName { get { return "떠난다."; } }

		public override ObstacleEventType EventType { get { return ObstacleEventType.rescueSignal; } }

		public override int SectorNumber { get { return 1; } }


		public override void Accepted()
		{
			int random = Random.Range(0, 2);
			if (random == 0)
			{
				AcceptLog = new List<string> { "이걸 속는 멍청이가 있네." };
			}
			else
			{
				AcceptLog = new List<string> { "감사합니다. 이 은혜는 잊지 않겠습니다." };
			}
			shipDialogue.SetLine(AcceptLog);
			shipDialogue.StartNextDialogue();
			shipDialogue.ActiveButton(false);
			shipDialogue.SetClear(true);
		}

		public override void Detected()
		{
			AcceptLog = new List<string> { "" };
			shipDialogue.SetLine(AcceptLog);
			shipDialogue.StartNextDialogue();
			shipDialogue.ActiveButton(false);
			shipDialogue.SetClear(true);
		}

	}
	public class SignalRecorder : ObstacleShipEventBase
	{
		public override string AcceptBtnName { get { return "파괴한다."; } }

		public override string DetectBtnName { get { return "기록을 추출한다."; } }

		public override string Option1BtnName { get { return "떠난다."; } }

		public override ObstacleEventType EventType { get { return ObstacleEventType.signalRecorder; } }

		public override int SectorNumber { get { return 1; } }


		public override void Accepted()
		{
			AcceptLog = new List<string> { "기록기를 파괴하자 시그널이 요동친다. 적이다." };
			shipDialogue.SetLine(AcceptLog);
			shipDialogue.StartNextDialogue();
			shipDialogue.ActiveButton(false);
			shipDialogue.SetClear(true);
		}

		public override void Detected()
		{
			AcceptLog = new List<string> { "기록을 훔치는 것은 쉽지만, 적들의 눈까지 속일 수는 없었다.", "적이 온다." };
			shipDialogue.SetLine(AcceptLog);
			shipDialogue.StartNextDialogue();
			shipDialogue.ActiveButton(false);
			shipDialogue.SetClear(true);
		}

		public override void Option1()
		{
			AcceptLog = new List<string> { "" };
			shipDialogue.SetLine(AcceptLog);
			shipDialogue.StartNextDialogue();
			shipDialogue.ActiveButton(false);
			shipDialogue.SetClear(true);
		}
	}
}
