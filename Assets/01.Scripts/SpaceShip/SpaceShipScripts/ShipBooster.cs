using UnityEngine;

public class ShipBooster : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject leftBooster;
    [SerializeField] GameObject rightBooster;

    float StopSize = 0;
    float MovingSize = 10.12f;
    float currentSize ;
    float lastSize;
    public float Speed = 5;
    public bool isMoving = false;

	private void Update()
	{
        if ( isMoving == false)
        {
            lastSize = currentSize;
            if( currentSize > StopSize)
            {
                currentSize -= Time.deltaTime * Speed;
            }
            else if( currentSize < 0)
            {
                currentSize = 0;
            }
        }
        else if ( isMoving == true)
        {
            lastSize = currentSize;
            if ( currentSize < MovingSize)
            {
                currentSize += Time.deltaTime * Speed;
            }
            else if(currentSize > MovingSize)
            {
                currentSize = MovingSize;
            }
        }
		leftBooster.transform.localScale = Vector3.one * currentSize;
        rightBooster.transform.localScale = Vector3.one * currentSize;
	}

	public void IsMoving(bool value)
    {
        if(value == false)
        {
            leftBooster.transform.localScale = Vector3.one * StopSize;
            rightBooster.transform.localScale = Vector3.one * StopSize;
        }
        else if(value == true)
        {
            leftBooster.transform.localScale = Vector3.one * MovingSize;
            rightBooster.transform.localScale = Vector3.one * MovingSize;
        }
    }


}
