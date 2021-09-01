using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    public CharacterController2D controller;
    public Animator animator;
    public GameObject deadPanel;

    public float runSpeed = 40f;
    public float life = 3f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
            animator.SetBool("IsCrouching", true);
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
            animator.SetBool("IsCrouching", false);
        }

        if (Input.GetButtonDown("Dash"))
        {
            CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
            if (horizontalMove > 0)
                rb.velocity = Vector2.right * dashSpeed;
            if (horizontalMove < 0)
                rb.velocity = Vector2.left * dashSpeed;
        }

        if (life <= 0)
        {
            Dead();
        }

    }

    public void OnCrouching(bool onCrouching)
    {
        animator.SetBool("IsCrouching", onCrouching);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.name)
        {
            case "Frog":
                Hurt();
                break;
            case "Bullet_Frog(Clone)":
                Hurt();
                break;
            case "Deadzone":
                Hurt();
                break;
            case "Worm":
                Hurt();
                break;
            case "Roca(Clone)":
                Hurt();
                break;
            case "Opossum":
                Hurt();
                break;
            case "Beatle(Clone)":
                Hurt();
                break;
            default:
                animator.SetBool("IsHurting", false);
                break;
        }
    }

    void Hurt()
    {
        if (animator.GetBool("IsHurting") == false)
        {
            animator.SetBool("IsHurting", true);
            jump = true;
            life = life - 1;
        }
    }

    void Dead()
    {
        Destroy(this.gameObject);
        deadPanel.SetActive(true);
    }
}
