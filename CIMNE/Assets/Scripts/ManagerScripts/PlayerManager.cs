using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;

    
    public float groundDistance = 0.2f;
    public float jumpHeight = 3f;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float gravity = -9.81f;
    public float MaxHeigh = 4f;
    public LayerMask groundMask; 
    public Transform groundCheck;
    public CharacterController controller;

    private bool isGrounded, restart, type, finish, instructions;
    private bool controlling = true;
    private bool falling = false;
    private float speed;
    private float startYPos;
    private float oldYspeed = 0f;
    private Vector3 velocity;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        Debug.Log("PlayerManager Start");
        controlling = true;
        speed = walkSpeed;
        restart = GlobalController.current.restart; 
        type = GlobalController.current.type; 
        finish = GlobalController.current.finish;
        instructions = GlobalController.current.instructions;
    }

    void Update()
    {
        if (controlling)
        {

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }

            if (instructions)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    EventController.current.OpenInstructuions();
                }
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                speed = runSpeed;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = walkSpeed;
            }

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            if (!isGrounded )
            {
                if (!falling)
                {
                    falling = true;
                    startYPos = gameObject.transform.position.y;
                } 
                
                if (velocity.y < 0 && oldYspeed > 0)
                {
                    startYPos = gameObject.transform.position.y;
                }
                oldYspeed = velocity.y;
            }

            if (isGrounded)
            {

                if (falling)
                {
                    float endYPos = gameObject.transform.position.y;
                    //Debug.Log("Tall: " + (startYPos - endYPos) + " Max: " + damageThershold);
                    falling = false;
                    if (startYPos - endYPos > MaxHeigh)
                    {
                        EventController.current.ChangeMode("finish", "You fall from " + (startYPos - endYPos) + "m");
                    }
                }

                if (velocity.y < 0)
                {
                    velocity.y = -2f;
                }
            }

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            if (Input.GetKeyDown("space"))
            {
                if (isGrounded)
                {
                    EventController.current.Write("Jump");
                    //GameObject.Find("Controller").GetComponent<DataWritter>().Write("Jump");
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
            }

            if (restart)
            {

                if (Input.GetKeyDown(KeyCode.R))
                {
                    Debug.Log("Key R");
                    Application.LoadLevel(0);
                }
            }

            if (type)
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    Debug.Log("Key T");
                    EventController.current.ChangeMode("type", null);
                }
            }

            if (finish)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("Key E");
                    EventController.current.ChangeMode("finish", "You ended manually the simulation");
                }
            }
        }       
    }

    public void Activate()
    {
        controlling = true;
    }

    public void Deactivate()
    {
        controlling = false;
    }

}
