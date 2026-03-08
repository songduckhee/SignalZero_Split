using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShopSO", menuName = "Scriptable Objects/ShopSO")]
public class ShopSO : ScriptableObject
{
	public string NPCName;
	public Npc Npc;
	public List<string> dialogue;
	public List <string> gambling; // 갬블링
	public List<string> repair; // 수리
	public List <string> firstshops;
	public List <string> buyScrap;
}
