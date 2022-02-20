using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerClimb : MonoBehaviour
{
    public enum PlayerState
    {
        WALKING,
        FALLING,
        CLIMBING
    }

    [SerializeField] public PlayerState state = PlayerState.CLIMBING;
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float climbSpeed = 2f;

    Rigidbody rb;

    float h = 0f;
    float v = 0f;
    bool jumpDown = false;


    public float rayLength = 1.02f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame

    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        if (!jumpDown)
            jumpDown = Input.GetButtonDown("Jump");
    }
    void FixedUpdate()
    {

        Vector2 input = SquareToCircle(new Vector2(h, v));
        Transform cam = Camera.main.transform;
        Vector3 moveDirection = Quaternion.FromToRotation(cam.up, Vector3.up)
            * cam.TransformDirection(new Vector3(input.x, 0f, input.y));

        switch (state)
        {
            case PlayerState.WALKING: { HandleWalking(moveDirection); } break;
            case PlayerState.FALLING: { HandleFalling(); } break;
            case PlayerState.CLIMBING: { HandleClimbing(input); } break;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position,
                                Vector3.down,
                                out hit,
                                rayLength))
            state = PlayerState.WALKING;
        else if (state == PlayerState.WALKING)
            state = PlayerState.FALLING;

        rb.useGravity = state != PlayerState.CLIMBING;

        //reset input
        jumpDown = false;
    }

    void HandleWalking(Vector3 moveDirection)
    {
        Vector3 oldVelo = rb.velocity;
        Vector3 newVelo = moveDirection * walkSpeed;
        newVelo.y = oldVelo.y;
        if (jumpDown)
            newVelo.y = 5f;
            state = PlayerState.FALLING;
        rb.velocity = newVelo;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            transform.forward = moveDirection;
        }
    }

    void HandleFalling()
    {
        if (jumpDown && Physics.Raycast(transform.position, transform.forward * 0.4f))
            state = PlayerState.CLIMBING;
    }

    void HandleClimbing(Vector2 input)
    {
        //check wall cross pattern
        Vector3 offset = transform.TransformDirection(Vector2.one * 0.5f);
        Vector3 checkDirection = Vector3.zero;
        int k = 0;
        for (int i = 0; i < 4; i++)
        {
            RaycastHit checkHit;
            if (Physics.Raycast(transform.position + offset,
                                transform.forward,
                                out checkHit))
            {
                checkDirection += checkHit.normal;
                k++;
            }
            //Rotate offset by 90 deg
            offset = Quaternion.AngleAxis(90f, transform.forward) * offset;
        }
        checkDirection /= k;

        //check wall directly infront
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -checkDirection, out hit))
        {
            float dot = Vector3.Dot(transform.forward, -hit.normal);

            rb.position = Vector3.Lerp(rb.position,
                                        hit.point + hit.normal * 0.51f,
                                        10f * Time.fixedDeltaTime);
            transform.forward = -hit.normal;
            rb.position = Vector3.Lerp(rb.position,
                                        hit.point + hit.normal * 0.51f,
                                        10f * Time.fixedDeltaTime);
            rb.useGravity = false;
            rb.velocity = transform.TransformDirection(input) * climbSpeed;

            if (jumpDown)
            {
                rb.velocity = Vector3.up * 5f + hit.normal * 3f;
                state = PlayerState.FALLING;
            }
        }
        else
        {
            state = PlayerState.FALLING;
        }
    }

        Vector2 SquareToCircle(Vector2 input)
        {
            return (input.sqrMagnitude >= 1f) ? input.normalized : input;
        }
    
}
