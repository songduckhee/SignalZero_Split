
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public abstract class EventBase
{
	public abstract string AcceptBtnName { get; }
	public abstract string DetectBtnName { get; }
	public virtual string Option1BtnName { get; }
	public virtual string Option2BtnName { get; }
	public abstract EventType EventType { get; }

	public abstract int SectorNumber { get; }

	public List<string> AcceptLog = new List<string>();
	public List<string> DetectLog = new List<string>();
	public List<string> Option1Log = new List<string>();
	public List<string> Option2Log = new List<string>();
	public virtual void Accepted()
	{
		AcceptLog = new List<string> { "" };
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;// 이거 고치기
		EventManager.Instance.DeleteShipRemoveListener();
	}
	public virtual void Detected()
	{
		DetectLog = new List<string> { "" };
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
	public virtual void OtherOption()
	{
		Option1Log = new List<string> { "" };
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
	public virtual void OtherOption2()
	{
		Option2Log = new List<string> { "" };
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
	public void CraditReward(int count)
	{
		ItemData cradit = EventManager.Instance.Cradit;
		for (int i = 0; i < count; i++)
		{
			InventoryManager.Instance.FighterGetItem(cradit);
		}
	}
	public void WeaponReward(string ItemName)
	{
		ItemData weapon = EventManager.Instance.FindItemData(ItemName);
		InventoryManager.Instance.FighterGetItem(weapon);
	}
}
#region 섹터 1
public class RaidersWarning : EventBase // 약탈자들의 경고
{
	public override EventType EventType
	{
		get { return EventType.raidersWarning; }
	}

	public override string AcceptBtnName { get { return "승낙한다."; } }

	public override string DetectBtnName { get { return "거부한다."; } }

	public override int SectorNumber { get { return 1; } }

	public override string Option1BtnName { get { return "발칸 티켓을 보여준다"; } }
	public override void Accepted()
	{
		if (InventoryManager.Instance.CanSpendCredit(5) == true)
		{
			AcceptLog = new List<string> { "똑똑하군." };
			EventManager.Instance.clickDialogue.SetLine(AcceptLog);
			EventManager.Instance.clickDialogue.ApplyDialogue();
			EventManager.Instance.clickDialogue.ButtonClose();
			EventManager.Instance.currentEventSection.clearEvent = true;
			EventManager.Instance.DeleteShipRemoveListener();
		}
		else
		{
			AcceptLog = new List<string> { "크레딧이 없습니다." };
			EventManager.Instance.clickDialogue.SetLine(AcceptLog);
			EventManager.Instance.clickDialogue.ApplyDialogue();
			EventManager.Instance.clickDialogue.ButtonClose();
			EventManager.Instance.currentEventSection.clearEvent = true;
			EventManager.Instance.DeleteShipRemoveListener();
		}

	}

	public override void Detected()
	{
		//크레딧 30%를 잃는다. ( 소수점 올림 적용)
		DetectLog = new List<string> { "그럼 죽어라." };
		EventManager.Instance.RequestSpawn("raidersWarning", "Raider");
		EventManager.Instance.clickDialogue.SetLine(DetectLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		int random = UnityEngine.Random.Range(0, 50);
		if (InventoryManager.Instance.HasTicket())
		{
			if (random < 25)
			{
				Option1Log = new List<string> { "젠장, 당장 죽여!" };
				EventManager.Instance.RequestSpawn("raidersWarning", "Raider");
				// 적을 격파할 경우, 10047 Ticket을 얻는다.
			}
			else
			{
				Option1Log = new List<string> { "또 티켓이야? 오늘도 공쳤군.", "블루 스팟을 찾고 있나?\n조금 도와주지." };
				// 플레이어 우주선 위치 섹션 중심 주변 3x3를 밝혀준다.
				// 이 때, 해당하는 위치에 블루 스팟이 있을 경우 파란 핀을 찍어준다.
				// 게임을 재개한다.
			}
		}
		else
		{
			Option1Log = new List<string> { "티켓이 없습니다." };
		}
		EventManager.Instance.clickDialogue.SetLine(Option1Log);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

}
public class SurpriseInspection : EventBase // 불시검문
{
	public override EventType EventType
	{
		get { return EventType.surpriseInspection; }
	}

	public override string AcceptBtnName { get { return "제출 한다."; } }

	public override string DetectBtnName { get { return "공격 한다"; } }

	public override int SectorNumber { get { return 1; } }

	public override void Accepted()
	{
		AcceptLog = new List<string> { "협조에 감사드립니다.\n불만 사항은\n림보 제 8 외무성으로 접수해 주십시오." };
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		EventManager.Instance.RequestSpawn("surpriseInspection", "");
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

}
public class cleanupZone : EventBase // 정리 구역
{
	public override EventType EventType
	{
		get { return EventType.cleanupZone; }
	}

	public override string AcceptBtnName { get { return "무시하고 접근한다."; } }

	public override string DetectBtnName { get { return "떠난다."; } }

	public override int SectorNumber { get { return 1; } }

	public override void Accepted()
	{
		AcceptLog = new List<string> { "경고! 당신의 우주선은\n오류난 역장 생성기로\n가려져 있던 소행성과 충돌했다." };

		// 우주선 체력 10% 감소

		ShipMover.Instance.shipDamageReceiver.ApplyDamagePercent(10);
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

}
public class mysteriousSignal : EventBase // 의문의 신호
{
	public override EventType EventType
	{
		get { return EventType.mysteriousSignal; }
	}

	public override string AcceptBtnName { get { return ""; } }

	public override string DetectBtnName { get { return ""; } }

	public override int SectorNumber { get { return 1; } }


	public override void Accepted()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

}
public class decoy : EventBase // 유인책
{
	public override EventType EventType
	{
		get { return EventType.decoy; }
	}

	public override string AcceptBtnName { get { return "수락한다."; } }

	public override string DetectBtnName { get { return "거절한다."; } }

	public override int SectorNumber { get { return 1; } }

	public override void Accepted()
	{
		AcceptLog = new List<string> { "말이 좀 통하는군. 기다리지." };
		EventManager.Instance.isAccept = true;
		// 제국 상단 이벤트에서 시그널 재밍 선택지 개방
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();

	}

	public override void Detected()
	{
		DetectLog = new List<string> { "그래? 아쉽군.", "그럼 당신이 대신\n대가를 치루는 걸로." };
		EventManager.Instance.RequestSpawn("decoy", "Decoy");
		// 몬스터 스폰. 전부 처치 시 (30) 스크랩 획득
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class SuspiciousWeaponTrade : EventBase // 의심스러운 무기 판매
{
	public override EventType EventType
	{
		get { return EventType.suspiciousWeaponTrade; }
	}
	public override string AcceptBtnName { get { return "(70)크레딧을 준다."; } }

	public override string DetectBtnName { get { return "(120)크레딧을 준다."; } }

	public override int SectorNumber { get { return 1; } }

	public override string Option1BtnName { get { return "공격한다."; } }

	public override string Option2BtnName { get { return "떠난다."; } }

	public override void Accepted()
	{

		if (InventoryManager.Instance.CanSpendCredit(70) == true)
		{
			AcceptLog = new List<string> { "좋은 거래였다.", "[ Redline-16를 구매했다. ]" };
			WeaponReward("RedLine-16");
		}
		else
		{
			AcceptLog = new List<string> { "크레딧이 없습니다. ]" };
		}
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
		// W0001 인벤토리에 수납.
	}

	public override void Detected()
	{
		if (InventoryManager.Instance.CanSpendCredit(120) == true)
		{
			DetectLog = new List<string> { "내가 소개해줄 무기는…\n내가 장착하고 있는 거야.\n잘 보라고." };
			EventManager.Instance.RequestSpawn("suspiciousWeaponTrade", "Suspicious");
		}
		else
		{
			DetectLog = new List<string> { "크레딧이 없습니다." };
		}

		EventManager.Instance.clickDialogue.SetLine(DetectLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		// 적 스폰
		// 스폰 관찰자 달아놓기
		EventManager.Instance.RequestSpawn("suspiciousWeaponTrade", "Suspicious");
		// 전부 처치 시 (60) 스크랩을 획득한다.
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();

	}

	public override void OtherOption2()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class repairScam : EventBase // 수리 사기
{
	public override EventType EventType
	{
		get { return EventType.repairScam; }
	}

	public override string AcceptBtnName { get { return "크레딧을 (50) 주고\n우주선을 조금 수리한다."; } }

	public override string DetectBtnName { get { return "떠난다."; } }

	public override int SectorNumber { get { return 1; } }

	int buttonint = 0;

	public override void Accepted()
	{
		//우주선의 체력이 (10%) 감소한다. 크레딧을 (50) 소모한다. 적이 스폰한다. 전부 처치 시 (70) 스크랩을 획득한다.
		if (InventoryManager.Instance.CanSpendCredit(50) == true)
		{
			AcceptLog = new List<string> { "이 멍청이! 속았구나!" };
			ShipMover.Instance.shipDamageReceiver.ApplyDamagePercent(10);
			EventManager.Instance.RequestSpawn("repairScam", "RepairScam");
		}
		else
		{
			AcceptLog = new List<string> { "크레딧이 없습니다." };
		}

		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		//적이 스폰한다. 전부 처치 시 (50) 스크랩을 획득한다.
		EventManager.Instance.RequestSpawn("repairScam", "RepairScam");
		EventManager.Instance.clickDialogue.SetLine(DetectLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

}
public class ruleAnnouncement : EventBase // 규칙 공지
{
	public override EventType EventType
	{
		get { return EventType.ruleAnnouncement; }
	}

	public override string AcceptBtnName { get { return "티켓을 가지고 있다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }

	public override int SectorNumber { get { return 1; } }

	public override string Option1BtnName { get { return "없다고 한다."; } }


	public override void Accepted()
	{
		if (InventoryManager.Instance.HasTicket())
		{
			AcceptLog = new List<string> { "쯧, 꺼져." };
		}
		else
		{
			AcceptLog = new List<string> { "티켓이 없습니다." };
		}

		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		DetectLog = new List<string> { "일방적으로 교신을 끊었다.\n예상대로 공격이다." };
		EventManager.Instance.RequestSpawn("ruleAnnouncement", "");
		EventManager.Instance.clickDialogue.SetLine(DetectLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		Option1Log = new List<string> { " 그럼 하나 챙겨 가라고. " };
		EventManager.Instance.clickDialogue.SetLine(Option1Log);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		string acceptBtnName = "구매한다.(40 크레딧)";
		string DetectedBtnName = "무시한다";
		EventManager.Instance.clickDialogue.SetBtnName(acceptBtnName, DetectedBtnName);
		EventManager.Instance.clickDialogue.SetBtn(Accepted1, Detected1);
		EventManager.Instance.clickDialogue.ButtonOpen(true, true);
	}

	public void Accepted1()
	{
		if (InventoryManager.Instance.CanSpendCredit(40))
		{
			EventManager.Instance.CloseEventUI();
			WeaponReward("Ticket");
			EventManager.Instance.currentEventSection.clearEvent = true;
			EventManager.Instance.DeleteShipRemoveListener();
		}
		else
		{
			AcceptLog = new List<string> { "크레딧이 없습니다." };
			EventManager.Instance.clickDialogue.SetLine(AcceptLog);
			EventManager.Instance.clickDialogue.ApplyDialogue();
			EventManager.Instance.clickDialogue.ButtonClose();
			EventManager.Instance.currentEventSection.clearEvent = true;
			EventManager.Instance.DeleteShipRemoveListener();
		}

	}

	public void Detected1()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
//새로 만든것

public class covertSupplyShip : EventBase
{
	public override string AcceptBtnName { get { return "보급을 탈취한다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }
	public override EventType EventType { get { return EventType.covertSupplyShip; } }

	public override int SectorNumber { get { return 1; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "경고음이 울리며\n보급선과 함께 은폐 중이던\n무인기들이 접근해온다." };
		//몬스터 스폰
		EventManager.Instance.RequestSpawn("covertSupplyShip", "");
		WeaponReward("");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = false;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class covertSupplyShip2 : EventBase
{
	public override string AcceptBtnName { get { return "보급을 탈취한다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }
	public override EventType EventType { get { return EventType.covertSupplyShip2; } }

	public override int SectorNumber { get { return 1; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "경고음이 울리며\n보급선과 함께 은폐 중이던\n무인기들이 접근해온다." };
		//몬스터 스폰
		EventManager.Instance.RequestSpawn("covertSupplyShip2", "");
		WeaponReward("");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = false;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class covertSupplyShip3 : EventBase
{
	public override string AcceptBtnName { get { return "보급을 탈취한다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }
	public override EventType EventType { get { return EventType.covertSupplyShip3; } }

	public override int SectorNumber { get { return 1; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "경고음이 울리며\n보급선과 함께 은폐 중이던\n무인기들이 접근해온다." };
		//몬스터 스폰
		EventManager.Instance.RequestSpawn("covertSupplyShip3", "");
		WeaponReward("");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = false;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}

public class BlackMarket : EventBase
{
	public override string AcceptBtnName { get { return "상점"; } }

	public override string DetectBtnName { get { return "떠난다."; } }
	public override EventType EventType { get { return EventType.blackMarket; } }

	public override int SectorNumber { get { return 1; } }


	public override void Accepted()
	{
		if (InventoryManager.Instance.HasTicket())
		{
			if (FieldManager.Instance.SectorCount == 1)
			{
				EventManager.Instance.ShopDialogue.OpenShop(Npc.cape);
			}
			else if (FieldManager.Instance.SectorCount == 2)
			{
				EventManager.Instance.ShopDialogue.OpenShop(Npc.cape2);
			}
			else if (FieldManager.Instance.SectorCount >= 3)
			{
				EventManager.Instance.ShopDialogue.OpenShop(Npc.cape3);
			}

			EventManager.Instance.CloseEventUI();
			EventManager.Instance.currentEventSection.clearEvent = false;
			EventManager.Instance.DeleteShipRemoveListener();
		}
		else
		{
			AcceptLog = new List<string> { "티켓이 없잖아.\n티켓을 가져오도록 해." };
			EventManager.Instance.clickDialogue.SetLine(AcceptLog);
			EventManager.Instance.clickDialogue.ApplyDialogue();
			EventManager.Instance.clickDialogue.ButtonClose();
			EventManager.Instance.currentEventSection.clearEvent = false;
			EventManager.Instance.clickDialogue.ShopClose = true;
			EventManager.Instance.DeleteShipRemoveListener();
		}

	}
	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = false;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class ImperialTradeLine : EventBase
{
	public override string AcceptBtnName { get { return "공격한다"; } }

	public override string DetectBtnName { get { return "시그널 재밍"; } }
	public override string Option1BtnName { get { return "떠난다."; } }

	public override EventType EventType { get { return EventType.imperialTradeLine; } }

	public override int SectorNumber { get { return 1; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "흥미로운 자살 시도군." };
		EventManager.Instance.RequestSpawn("imperialTradeLine", "");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		DetectLog = new List<string> { "이게 무슨 짓이지?", "뭔가 계획이 있었나 본 데,\n네 놈 동료들은 널 버렸나 보군.", "걱정 마라.\n네 놈의 배를 부수고 기록을 확인해서\n그 놈들도 같은 곳에 보내주마." };
		EventManager.Instance.RequestSpawn("imperialTradeLine", "imperialTradeLine");
		EventManager.Instance.clickDialogue.SetLine(DetectLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class DockedRescueShip : EventBase
{
	public override string AcceptBtnName { get { return "크레딧을 (15) 주고\n우주선을 (50%) 수리한다."; } }

	public override string DetectBtnName { get { return "떠난다."; } }

	public override EventType EventType { get { return EventType.dockedRescueShip; } }

	public override int SectorNumber { get { return 1; } }


	public override void Accepted()
	{
		if (InventoryManager.Instance.CanSpendCredit(15))
		{
			ShipMover.Instance.shipDamageReceiver.ApplyRecoveryPercent(50);
			EventManager.Instance.CloseEventUI();
			EventManager.Instance.currentEventSection.clearEvent = true;
			EventManager.Instance.DeleteShipRemoveListener();
		}
		else
		{
			AcceptLog = new List<string> { "크레딧이 없습니다." };
			EventManager.Instance.clickDialogue.SetLine(AcceptLog);
			EventManager.Instance.clickDialogue.ApplyDialogue();
			EventManager.Instance.clickDialogue.ButtonClose();
			EventManager.Instance.currentEventSection.clearEvent = true;
			EventManager.Instance.DeleteShipRemoveListener();
		}

	}

	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

}
public class SignalRecorder : EventBase
{
	public override string AcceptBtnName { get { return "파괴한다."; } }

	public override string DetectBtnName { get { return "기록을 추출한다."; } }

	public override string Option1BtnName { get { return "떠난다."; } }

	public override EventType EventType { get { return EventType.signalRecorder; } }

	public override int SectorNumber { get { return 1; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "기록기를 파괴하자\n시그널이 요동친다.\n적이다." };
		EventManager.Instance.RequestSpawn("signalRecorder", "");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		DetectLog = new List<string> { "기록을 훔치는 것은 쉽지만,\n적들의 눈까지 속일 수는 없었다.", "적이 온다." };
		EventManager.Instance.RequestSpawn("signalRecorder", "");
		EventManager.Instance.clickDialogue.SetLine(DetectLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		Option1Log = new List<string> { "" };
		EventManager.Instance.clickDialogue.SetLine(Option1Log);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class RescueSignal : EventBase
{
	public override string AcceptBtnName { get { return "도와준다.\n스크랩 20 감소."; } }

	public override string DetectBtnName { get { return "떠난다."; } }

	public override EventType EventType { get { return EventType.rescueSignal; } }

	public override int SectorNumber { get { return 1; } }


	public override void Accepted()
	{
		int random = Random.Range(0, 2);
		if (random == 0)
		{
			AcceptLog = new List<string> { "이걸 속는 멍청이가 있네." };
			EventManager.Instance.RequestSpawn("rescueSignal", "");
		}
		else
		{
			AcceptLog = new List<string> { "감사합니다.\n이 은혜는 잊지 않겠습니다." };
			CraditReward(100);
		}
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		AcceptLog = new List<string> { "" };
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

}
#endregion
#region 섹터 2
public class ghostRescueSignal : EventBase
{
	public override EventType EventType
	{
		get { return EventType.ghostRescueSignal; }
	}

	public override string AcceptBtnName { get { return "구조 신호에\n접근한다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }

	public override int SectorNumber { get { return 2; } }



	public override void Accepted()
	{
		int random = Random.Range(0, 2);
		if (random == 0)
		{
			AcceptLog = new List<string> { "이미 무언가에 의해\n반파된 고철들만 떠다닌다.\n잔향 신호였던 듯 하다." };
			FighterManager.Instance.upgradeManager.GetScrap(30);
		}
		else
		{
			AcceptLog = new List<string> { "정체 불명의\n적들이 공격해온다." };
			EventManager.Instance.RequestSpawn("ghostRescueSignal", "");
		}

		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class ImperialControl : EventBase
{
	public override EventType EventType
	{
		get { return EventType.ImperialControl; }
	}
	public override string AcceptBtnName { get { return "뇌물을 준다.\n(30 크레딧 소모)"; } }

	public override string DetectBtnName { get { return "순순히 따라준다."; } }

	public override int SectorNumber { get { return 2; } }

	public override string Option1BtnName { get { return "공격한다."; } }


	public override void Accepted()
	{
		//크레딧 30 소모
		if (InventoryManager.Instance.CanSpendCredit(30) == true)
		{
			AcceptLog = new List<string> { "유기체 주제에\n두뇌 회전이 빠르군.\n떠나라." };
		}
		else
		{
			AcceptLog = new List<string> { "크레딧이 없습니다." };
		}
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		DetectLog = new List<string> { "다음은 없다.\n이 곳에서 나가라." };
		ShipMover.Instance.MoveShipToStartPos();
		EventManager.Instance.clickDialogue.SetLine(DetectLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		Option1Log = new List<string> { "흥미로운 자살 방법이군." };
		EventManager.Instance.RequestSpawn("ImperialControl", "Im_Control");
		EventManager.Instance.clickDialogue.SetLine(DetectLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
		// 전부 처치 시 (20) 스크랩을 획득한다.
	}
}
public class trackingSignal : EventBase
{
	public override EventType EventType
	{
		get { return EventType.trackingSignal; }
	}
	public override string AcceptBtnName { get { return "교신한다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }

	public override int SectorNumber { get { return 2; } }

	public override void Accepted()
	{
		AcceptLog = new List<string> { "당신은 추적기에 입력된\n암호를 해석해냈다.", "아마 이들은 누군가를 쫓고 있었으나,\n운 없게도 당신의 우주선이\n그 경로에 있었던 모양이다." };
		EventManager.Instance.RequestSpawn("trackingSignal", "");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
		// W0001 인벤토리에 수납.
	}

	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
		// 전부 처치 시 W0012 인벤토리에 수납.
	}

}
public class imperialScientist : EventBase
{
	public override EventType EventType
	{
		get { return EventType.imperialScientist; }
	}
	public override string AcceptBtnName { get { return "수락한다."; } }

	public override string DetectBtnName { get { return "거절한다."; } }

	public override int SectorNumber { get { return 2; } }

	public override string Option1BtnName { get { return "무시한다."; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "협조에 감사드립니다!\n무기는 사용 후 버려주시면 됩니다!" };
		WeaponReward("W0009 10031 Halo-18");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
		// W0001 인벤토리에 수납.
	}

	public override void Detected()
	{
		DetectLog = new List<string> { "아쉽군요.\n뭐 괜찮습니다.", "어차피 곧\n후회하시게 될 테니.", "제국의 천사가 완성 되었으니\n당신도 곧 따르게 될 겁니다." };
		EventManager.Instance.clickDialogue.SetLine(DetectLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
		// 적 스폰
		// 전부 처치 시 W0012 인벤토리에 수납.
	}

	public override void OtherOption()
	{
		DetectLog = new List<string> { "지금 제국 병기성의 요청을\n무시하시는 겁니까?", "유기체들은 예의가 없다는 게\n사실이었군요.", "흥미롭네요.\n한번 뇌를 열어봐야겠습니다." };
		EventManager.Instance.RequestSpawn("imperialScientist", "");
		EventManager.Instance.clickDialogue.SetLine(DetectLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class imperialHelpRequest : EventBase
{
	public override EventType EventType
	{
		get { return EventType.imperialHelpRequest; }
	}
	public override string AcceptBtnName { get { return "림보를 막아선다."; } }

	public override string DetectBtnName { get { return "제국을 막아선다."; } }

	public override int SectorNumber { get { return 2; } }

	public override string Option1BtnName { get { return "무시한다."; } }

	public override void Accepted()
	{
		AcceptLog = new List<string> { "제국의 용병인가?\n제국 광신도와 붙어먹는\n자들과는 대화하지 않겠다." };
		EventManager.Instance.RequestSpawn("imperialHelpRequest", "");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		DetectLog = new List<string> { "칼카탁스!\n좋아. 보상을 줄테니까\n먹고 떨어져라." };
		string accept = "수락한다.";
		string detect = "거절한다.";
		EventManager.Instance.clickDialogue.SetLine(DetectLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.SetBtnName(accept, detect);
		EventManager.Instance.clickDialogue.SetBtn(Accept2, Detect2);
	}

	private void Accept2()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
		WeaponReward("S0032 10061 로드_추진기");
		//S0032 10061 로드_추진기 를 지급한다.
	}
	private void Detect2()
	{
		EventManager.Instance.RequestSpawn("imperialHelpRequest", "");
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
		//몬스터 스폰
	}

	public override void OtherOption()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class imperialSentry : EventBase
{
	public override EventType EventType
	{
		get { return EventType.imperialSentry; }
	}
	public override string AcceptBtnName { get { return "접근한다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }

	public override int SectorNumber { get { return 2; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "센트리들이 카메라를\n빛내며 공격해온다!" };
		EventManager.Instance.RequestSpawn("imperialSentry", "");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class ghostSignal : EventBase
{
	public override EventType EventType
	{
		get { return EventType.ghostSignal; }
	}
	public override string AcceptBtnName { get { return "교신한다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }

	public override int SectorNumber { get { return 2; } }

	public override string Option1BtnName { get { return "끊는다."; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "시뮬러.\n이 이상 접근하지 마라." };
		CraditReward(30);
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		DetectLog = new List<string> { "시뮬러.\n나아가면 대가를 치를 것이다." };
		EventManager.Instance.RequestSpawn("ghostSignal", "");
		EventManager.Instance.clickDialogue.SetLine(DetectLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class dockedRescueShip2 : EventBase
{
	public override EventType EventType
	{
		get { return EventType.dockedRescueShip2; }
	}
	public override string AcceptBtnName { get { return "크레딧을 (10) 주고 우주선을 (50%) 수리한다."; } }

	public override string DetectBtnName { get { return "떠난다."; } }

	public override int SectorNumber { get { return 2; } }

	public override void Accepted()
	{
		if (InventoryManager.Instance.CanSpendCredit(10))
		{
			ShipMover.Instance.shipDamageReceiver.ApplyRecoveryPercent(50);
			EventManager.Instance.CloseEventUI();
			EventManager.Instance.currentEventSection.clearEvent = true;
			EventManager.Instance.DeleteShipRemoveListener();
		}
		else
		{
			AcceptLog = new List<string> { "크레딧이 없습니다." };
			EventManager.Instance.clickDialogue.SetLine(AcceptLog);
			EventManager.Instance.clickDialogue.ApplyDialogue();
			EventManager.Instance.clickDialogue.ButtonClose();
			EventManager.Instance.currentEventSection.clearEvent = true;
			EventManager.Instance.DeleteShipRemoveListener();
		}
			
	}

	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class dockedCargoShip : EventBase
{
	public override EventType EventType
	{
		get { return EventType.dockedCargoShip; }
	}
	public override string AcceptBtnName { get { return "알려준다."; } }

	public override string DetectBtnName { get { return "모른다."; } }

	public override int SectorNumber { get { return 2; } }

	public override string Option1BtnName { get { return "끊는다."; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "오, 고맙군. 이건 답례다." };
		CraditReward(100);
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		DetectLog = new List<string> { "아쉽군." };
		EventManager.Instance.clickDialogue.SetLine(DetectLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class ghostShip : EventBase
{
	public override EventType EventType
	{
		get { return EventType.ghostShip; }
	}
	public override string AcceptBtnName { get { return "접근한다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }

	public override int SectorNumber { get { return 2; } }

	public override string Option1BtnName { get { return "끊는다."; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "배는 상처 하나 없이 멀쩡했으나, 인기척이 없다.", "배에는 재물이 있었다." };
		CraditReward(50);
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class covertSupplyShip4 : EventBase
{
	public override EventType EventType
	{
		get { return EventType.covertSupplyShip4; }
	}
	public override string AcceptBtnName { get { return "보급을 탈취한다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }

	public override int SectorNumber { get { return 2; } }

	public override string Option1BtnName { get { return "끊는다."; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "경고음이 울리며 보급선과 함께 은폐 중이던 무인기들이 접근해온다." };
		EventManager.Instance.RequestSpawn("covertSupplyShip4", "");
		WeaponReward("");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class covertSupplyShip5 : EventBase
{
	public override EventType EventType
	{
		get { return EventType.covertSupplyShip5; }
	}
	public override string AcceptBtnName { get { return "보급을 탈취한다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }

	public override int SectorNumber { get { return 2; } }

	public override string Option1BtnName { get { return "끊는다."; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "경고음이 울리며 보급선과 함께 은폐 중이던 무인기들이 접근해온다." };
		EventManager.Instance.RequestSpawn("covertSupplyShip5", "");
		WeaponReward("");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class covertSupplyShip6 : EventBase
{
	public override EventType EventType
	{
		get { return EventType.covertSupplyShip6; }
	}
	public override string AcceptBtnName { get { return "보급을 탈취한다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }

	public override int SectorNumber { get { return 2; } }

	public override string Option1BtnName { get { return "끊는다."; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "경고음이 울리며 보급선과 함께 은폐 중이던 무인기들이 접근해온다." };
		EventManager.Instance.RequestSpawn("covertSupplyShip6h", "");
		WeaponReward("");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}

#endregion
#region 섹터 3
public class ghostRescueSignal2 : EventBase
{
	public override string AcceptBtnName { get { return "구조 신호에\n접근한다."; } }
	public override string DetectBtnName { get { return "떠난다."; } }
	public override EventType EventType { get { return EventType.ghostRescueSignal2; } }

	public override int SectorNumber { get { return 3; } }
	public override void Accepted()
	{
		int random = Random.Range(0, 2);
		if (random == 0)
		{
			AcceptLog = new List<string> { "이미 무언가에 의해\n반파된 고철들만 떠다닌다.\n잔향 신호였던 듯 하다." };
			FighterManager.Instance.upgradeManager.GetScrap(100);
		}
		else
		{
			AcceptLog = new List<string> { "정체 불명의 적들이\n공격해온다." };
			EventManager.Instance.RequestSpawn("ghostRescueSignal2", "");
		}

		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;// 이거 고치기
		EventManager.Instance.DeleteShipRemoveListener();
	}
	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

}
public class limboWarning : EventBase
{
	public override string AcceptBtnName { get { return "왜?"; } }
	public override string DetectBtnName { get { return "가야 한다."; } }
	public override EventType EventType { get { return EventType.limboWarning; } }

	public override int SectorNumber { get { return 3; } }
	public override void Accepted()
	{
		AcceptLog = new List<string> { "레티콘을 알고 있나?\n오는 길에 봤을 수도 있는데." , "보랏빛으로 빛나는\n우주 수정 생명체들.",
			"레테의 빙하로\n모여들고 있다.", "처음에는 무시 가능한\n수준이었는데…","뭔가 잘못됐어.\n아무튼 떠나라.","대신 이걸 주지." };

		FighterManager.Instance.upgradeManager.GetScrap(100);
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
	public override void Detected()
	{
		DetectLog = new List<string> { "그렇다면 이거라도 받아가라.", "우리한테는 딱히 쓸모 없지만\n시뮬러라면 다르니까." };
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		FighterManager.Instance.upgradeManager.GetScrap(100);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

}
public class imperialAdvice : EventBase
{
	public override string AcceptBtnName { get { return "나아간다."; } }
	public override string DetectBtnName { get { return "무시한다."; } }
	public override EventType EventType { get { return EventType.imperialAdvice; } }

	public override int SectorNumber { get; }
	public override void Accepted()
	{
		AcceptLog = new List<string> { "죽음을 두려워하지 않는군요.", "그것은 자의든 타의든\n칭송 받아 마땅한 의지지요.", "도움을 좀 드리죠." };
		WeaponReward("W0009 10031 Halo-18");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;// 이거 고치기
		EventManager.Instance.DeleteShipRemoveListener();
	}
	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

}
public class blasphemousEntity : EventBase
{
	public override string AcceptBtnName { get { return ""; } }
	public override string DetectBtnName { get { return ""; } }
	public override EventType EventType { get; }

	public override int SectorNumber { get { return 3; } }

	public List<string> AcceptLog = new List<string>();
	public List<string> DetectLog = new List<string>();
	public List<string> Option1Log = new List<string>();
	public List<string> Option2Log = new List<string>();
	public override void Accepted()
	{
		FighterManager.Instance.upgradeManager.GetScrap(80);
		EventManager.Instance.RequestSpawn("blasphemousEntity", "");
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class ghostShip2 : EventBase
{
	public override EventType EventType
	{
		get { return EventType.ghostShip2; }
	}
	public override string AcceptBtnName { get { return "접근한다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }

	public override int SectorNumber { get { return 3; } }

	public override string Option1BtnName { get { return "끊는다."; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "쉽은 상처 하나 없이 멀쩡했으나, 인기척이 없다.", "멀쩡한 화물이 하나 있었다." };
		WeaponReward("");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
public class ghostShip3 : EventBase
{
	public override EventType EventType
	{
		get { return EventType.ghostShip3; }
	}
	public override string AcceptBtnName { get { return "접근한다."; } }

	public override string DetectBtnName { get { return "무시한다."; } }

	public override int SectorNumber { get { return 3; } }

	public override string Option1BtnName { get { return "끊는다."; } }


	public override void Accepted()
	{
		AcceptLog = new List<string> { "골고다의적골고다의벽골고다의길을붙잡는가증스러움"};
		EventManager.Instance.RequestSpawn("ghostShip3","");
		WeaponReward("");
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void Detected()
	{
		DetectLog = new List<string> { "파괴된 잔해 속에서 스크랩을 찾아냈다." }; // 오브젝트 파괴 구현하기 // 섹션으로 생성된 오브젝트를 분류해서 해당 섹션의 오브젝트를 파괴하기
		EventManager.Instance.RequestSpawn("ghostShip3", "");
		FighterManager.Instance.upgradeManager.GetScrap(100);
		EventManager.Instance.clickDialogue.SetLine(AcceptLog);
		EventManager.Instance.clickDialogue.ApplyDialogue();
		EventManager.Instance.clickDialogue.ButtonClose();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}

	public override void OtherOption()
	{
		EventManager.Instance.CloseEventUI();
		EventManager.Instance.currentEventSection.clearEvent = true;
		EventManager.Instance.DeleteShipRemoveListener();
	}
}
#endregion