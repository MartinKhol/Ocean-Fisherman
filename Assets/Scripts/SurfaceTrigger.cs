using UnityEngine;

// Surface trigger that allows player to ascend to the boat

public class SurfaceTrigger : MonoBehaviour
{
    [SerializeField] private GameObject surfaceText; //ui text 
    [SerializeField] private PlayerMovement player; //reference to player character
    private bool inTrigger; //true when player is inside trigger collider

    //Called when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           // surfaceText.SetActive(true);
            inTrigger = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
          //  surfaceText.SetActive(false);
            inTrigger = false;
        }
    }

    private void Update()
    {
        if (inTrigger && (Input.GetAxis("Vertical") > 0.1f)) 
        {
            player?.Surface();
            inTrigger = false;
        }
    }
}
