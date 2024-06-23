using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveForce = 10f;
    public float maxSpeed = 10f;
    public float jumpForce = 10f;
    public float mouseSensitivity = 100f;
    public float raycastLength = 1f;
    public Camera mainCamera;

    private Rigidbody rb;
    private float xRotation = 0;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded();
        Look();
        Jump();

    }

    void FixedUpdate()
    {
        Move();
        
    }

    void Move()
    {
        //check for direction of movement and add a force in that direction once the specific input has been pressed
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-transform.right * moveForce, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * moveForce, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * moveForce, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * moveForce, ForceMode.Impulse);
        }
        /*if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            rb.velocity = Vector3.zero;
        }*/

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            if (rb.velocity.x < 0)
            {
                rb.velocity = new Vector3 (-maxSpeed, rb.velocity.y, rb.velocity.z);
            }
            else if(rb.velocity.x > 0)
            {
                rb.velocity = new Vector3(maxSpeed, rb.velocity.y, rb.velocity.z);
            }
        }
        if (Mathf.Abs(rb.velocity.z) > maxSpeed)
        {
            if (rb.velocity.z < 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -maxSpeed);
            }
            else if (rb.velocity.z > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxSpeed);
            }
        }
    }

    void Look()
    {
        //for the camera rotation, we first need to get the position of the cursor, which is on a 2D plane
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //we can rotate THE PLAYER around the y-axis by using the below method.
        //this will allow the player to turn around and view what might be around them
        transform.Rotate(new Vector3(0f, mouseX, 0f));

        //need to ensure that the value is within a certain range, if we add mouseY, then the value would be too high
        //we want to calculate the difference between the amount that we have rotated and our cursor's position
        //on the screen
        xRotation -= mouseY;

        //to rotate the camera up and down, we need to ensure that the value is within a certain range
        //generally, the player cannot wrap their whole head around, so we clamp it to -90 and 90
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        //ensure you have a reference to the camera, and change it's rotation about the x-axis, while maintaining
        //the camera's rotation on the y-axis
        mainCamera.transform.eulerAngles = new Vector3(xRotation, transform.eulerAngles.y, 0f);
    }

    void IsGrounded()
    {
        //for a raycast, you need to use RaycastHit to give you information about what the raycast is hitting
        RaycastHit hitInfo;

        //Physics.Raycast will give you a value that is either true or false, which you can use to determine whether
        //it has collided with something or not. It requires an origin, direction, length and can include information
        //about the physics layers it should detect a hit with. And with the out parameter, you can store the information
        //about the collision in the variable created earlier, hitInfo
        Physics.Raycast(transform.position, Vector3.down, out hitInfo, raycastLength);
        
        //to visualise it, you can draw a line
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, -raycastLength, transform.position.z), Color.green);

        //if the raycast has collided with something, then we can set isGrounded to true or false
        if (hitInfo.collider != false)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    void Jump()
    {
        //works like in 2D, check for input and to see if the boolean isGrounded is true
        //and add an impulse force in the "up" direction at a specific speed
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
