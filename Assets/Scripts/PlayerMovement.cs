using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public float HorizontalForce = 1f;
    public float VerticalForce = 1f;
    public float DashForce = 5f;

    public UnityEvent OnSurface;
    public UnityEvent OnSubmerge;

    private new Rigidbody2D rigidbody2D;

    public static bool surfaced = true;

    public Transform shopParent;
    public Vector3 shopOffset;
    public Vector3 submergePosition;

    public bool dashUnlocked = true;
    public float dashCd = 1.5f;
    private float lastDash = -2f;
    private Vector3 movement = Vector3.zero;
    private Animator animator;
    [SerializeField]
    private ParticleSystem dashParticles;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        surfaced = true;
    }

    void Update()
    {
        if(justSurfaced)
        {
            justSurfaced = false;
            return;
        }
        
        if (surfaced && (Input.GetAxis("Vertical") < -0.1f)) 
            Submerge();
        else if (!surfaced)
        {
            if (dashUnlocked && Input.GetButtonDown("Jump") && (lastDash+dashCd) < Time.time)
            {
                rigidbody2D.AddForce(rigidbody2D.velocity.normalized * DashForce, ForceMode2D.Impulse);
                lastDash = Time.time;
                SoundManger.Instance.PlayDash();
                dashParticles.Play();
            }
            else
            {
                movement.x = Input.GetAxis("Horizontal") * HorizontalForce;
                movement.y = Input.GetAxis("Vertical") * VerticalForce;

                if (movement.x < 0)
                    FlipLeft();
                else
                    FlipRight();

                animator.SetBool("Moving", movement != Vector3.zero);
            }
        }
    }

    private void FixedUpdate()
    {
        rigidbody2D.AddForce(movement);
    }

    private bool justSurfaced = false;

    public void Surface()
    {
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        justSurfaced = true;
        surfaced = true;
        transform.parent = shopParent;
        transform.localPosition = shopOffset;
        OnSurface.Invoke();
        animator.SetBool("OnLand", true);
    }

    public void Submerge()
    {
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;

        surfaced = false;
        OnSubmerge.Invoke();
        transform.parent = null;
        transform.position = submergePosition;
        animator.SetBool("OnLand", false);
    }

    void FlipLeft()
    {
        Vector3 scale = transform.localScale;
        scale.x = -1f;
        transform.localScale = scale;

    }
    void FlipRight()
    {
        Vector3 scale = transform.localScale;
        scale.x = 1f;
        transform.localScale = scale;
    }
}
