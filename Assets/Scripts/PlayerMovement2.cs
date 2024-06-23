using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public float speed = 12f;
    public float mouseSensitivity = 5f;
    public float gravity = -9.81f;
    public Camera mainCamera;

    private float xRotation = 0f;
    private Vector3 velocity;
    private CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        //character is a component that we add to our player, so we can find it in start
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Look();

        //find the movement on the "horizontal" and "vertical" axis
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //adding these vectors will give us the direction we're moving
        Vector3 move = transform.right * x * speed + transform.forward * z * speed;

        //can just call the Move function that the character controller comes with. Input the
        //vector calculated above and multiply it by Time.deltaTime to ensure that it updates
        //correctly
        cc.Move(move * Time.deltaTime);

        //because the character controller is not affected by physics, we have to add our own
        velocity.y += gravity * Time.deltaTime;

        //apply the velocity by calling Move again. To add a jump we would need to update the
        //velocity vector declared above
        cc.Move(velocity * Time.deltaTime);
    }

    void Look()
    {
        //for the camera rotation, we first need to get the position of the cursor,
        //which is on a 2D plane
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //we can rotate THE PLAYER around the y-axis by using the below method.
        //this will allow the player to turn around and view what might be around them
        transform.Rotate(new Vector3(0f, mouseX, 0f));

        //need to ensure that the value is within a certain range, if we add, then the value
        //would be too high we want to calculate the difference between the amount that we have
        //rotated and our cursor's position on the screen
        xRotation -= mouseY;

        //to rotate the camera horizontally, or up and down, we need to ensure that the value
        //is within a certain range generally, the player cannot wrap their whole head around,
        //so we clamp it to -90 and 90
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        //ensure you have a reference to the camera, and change it's rotation about the x-axis,
        //while maintaining the camera's rotation on the y-axis
        mainCamera.transform.eulerAngles = new Vector3(xRotation, transform.eulerAngles.y, 0f);
    }
}
