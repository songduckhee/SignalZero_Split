using System.Collections;
using UnityEngine;

public class EventGenerator
{

	private Transform _blueSpotPos;


	public EventGenerator(Transform blueSpotPos)
	{
		_blueSpotPos = blueSpotPos;
	}

	
    public void HandleBlueSpot(Transform player)
    {
        



    }


	IEnumerator MovePosition(Transform transform)
	{
		for (int i = 0; i < 180f; i++)
		{

			yield return null;
		}
		yield return new WaitForSeconds(180f);
		transform.position = _blueSpotPos.position + new Vector3(0f, 2f, 0f);
	}


}
public class SectionEventSelector
{

}
