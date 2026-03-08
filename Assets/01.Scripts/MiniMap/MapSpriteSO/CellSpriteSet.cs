using UnityEngine;

[CreateAssetMenu(fileName = "CellSpriteSet", menuName = "Scriptable Objects/CellSpriteSet2")]
public class CellSpriteSet : ScriptableObject
{
	public CellSelectType SelectType;
	public CellCheckType CheckType;
	public SectionType SectionType;
	public bool IsPassed;
	public bool IsBoss;
	public Sprite BaseSprite;
}
