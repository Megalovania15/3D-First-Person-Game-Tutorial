using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public float moveSpeed = 12f;
    public float turnSmoothTime = 0.1f;
    public Transform mainCamera;

    private float turnVelocity;

    private CharacterController cc;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        //first we need to set up movement along the x and z plane. These functions
        //will give you a value between 1 and -1 depending on whether W, A, S or D is pressed
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        //we then need to determine the direction that we move in, by parsing in the values received
        //from our input float and storing it in a Vector3 (because the character controller takes a 
        //Vector3 as a parameter. We add .normalized, to ensure that the value is always 1 or -1
        //so that if we press both A and W at the same time, we do not move faster
        Vector3 direction = new Vector3(moveX, 0, moveZ).normalized;

        //Here we are checking to see that the value of the magnitude of the direction Vector is 
        //more than 0f. The magnitude of a vector is measured through use of the Pythagoras theorum
        if (direction.magnitude >= 0.1f)
        {
            //to rotate the player, we need to find the the angle between the x and z axes,
            //which can be found through using the Mathf.Atan2 function. Convert this value to degrees
            //as it will be in radians
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

            //makes the rotation of the player smoother by interpolating between angles
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnSmoothTime);

            //sets the rotation of the player to the angle calculated above
            transform.rotation = Quaternion.Euler(0, angle, 0);

            //adjusts the movement direction to match the direction the camera is facing while still
            //allowing the player to move from side to side and backwards
            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            //calls the move function available with the character controller
            cc.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            anim.SetBool("isWalking", true);
        }
        else 
        {
            anim.SetBool("isWalking", false);
        }
        
    }
}
