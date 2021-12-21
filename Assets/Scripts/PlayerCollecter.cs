using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollecter : MonoBehaviour
{
	public float radius = 0.2f;
	public LayerMask collectionMask;

	public GameObject floatingTextPrefab;

	private Vector3 collectionOffset = new Vector3(0, 0.2f, 0);
	private PlayerState playerState;

    private void Start()
    {
        playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {        
		Collider2D col = Physics2D.OverlapCircle (transform.position + collectionOffset, radius, collectionMask);
		if (col != null)
		{
			var fish = col.GetComponent<CollectibleFish>();

			var obj = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
			obj.GetComponentInChildren<Text>().text = "+" + fish.value;
			playerState.AddLoot(fish.PickUp());
			SoundManger.Instance.PlayCatchFish();
		}	
    }

	public void DebtReducedText(int value)
    {
		if (value > 0)
		{
			var obj = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
			obj.GetComponentInChildren<Text>().text = "Sold for " + value;
		}
	}
}
