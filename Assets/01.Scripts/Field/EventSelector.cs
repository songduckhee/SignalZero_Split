using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public enum SectionType
{
	None,
	Monster,
	Event,
	BlueSpot
}
public class EventSelector 
{

	public SectionType GetRandomType()
	{
		
		int r = Random.Range(0,100);
		if(r > 40)
		{

			return SectionType.Monster;
		}
		else if ( r <= 40 && r > 0)
		{
			return SectionType.Event;
		}
		else
		{
			return SectionType.BlueSpot;
		}
		
	}

	public bool GetRandomBool()
	{
		int r = Random.Range(0,100);
		if(r > 70)
		{
			return false;
		}
		else
		{
			return true;
		}
	}


}
