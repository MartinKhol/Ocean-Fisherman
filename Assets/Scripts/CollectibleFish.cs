using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleFish : MonoBehaviour
{	
	public int value = 100;
	
	public int PickUp ()
	{
		Destroy (gameObject);
		return value;
	}
	
}
