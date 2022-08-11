using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float speed =0.06f, zoomSpeed=10f, rotateSpeed=0.1f, maxHeight=40f, minHeight=1f, normalSpeed = 1f,shiftSpeed=2f;
    [SerializeField] Vector3 p1, p2;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // used to increase zoom speed
            speed = shiftSpeed;
            zoomSpeed = 20f;
        }
        else
        {
            speed = normalSpeed;
            zoomSpeed = 10f;
        }
        float hsp = transform.position.y * 
            speed * Input.GetAxis("Horizontal");
        float vsp = transform.position.y * 
            speed * Input.GetAxis("Vertical");
        float scrollsp = //Mathf.Log(transform.position.y) * 
            -zoomSpeed * Input.GetAxis("Mouse ScrollWheel");

        // setting scroll speed to 0 when maximum or minimum height is reached
        if (transform.position.y >= maxHeight && scrollsp > 0)
        {
            scrollsp = 0;
        }
        else if (transform.position.y <= minHeight && scrollsp < 0)
        {
            scrollsp = 0;
        }

        if ((transform.position.y + scrollsp) > maxHeight)
        {
            scrollsp = maxHeight- transform.position.y;
        }
        else if ((transform.position.y + scrollsp) < minHeight)
        {
            scrollsp = minHeight - transform.position.y;
        }
        Vector3 verticalMove = new Vector3(0, scrollsp, 0);
        Vector3 lateralMove = hsp * transform.right;
        Vector3 forwardMove = transform.forward;

        forwardMove.y = 0;
        forwardMove.Normalize();
        forwardMove *= vsp;

        Vector3 move = verticalMove + lateralMove + forwardMove;

        transform.position += move;
        GetCameraRotation();
    }
    void GetCameraRotation()
    {
        if (Input.GetMouseButtonDown(2))
        {
            p1 = Input.mousePosition;
        }
        if(Input.GetMouseButton(2))
        {
            p2 = Input.mousePosition;

            float dx = (p2 - p1).x * rotateSpeed;
            float dy = (p2 - p1).y * rotateSpeed;

            transform.rotation *= Quaternion.Euler(new Vector3(0, dx, 0));
            transform.GetChild(0).transform.rotation *= Quaternion.Euler(new Vector3(-dy, 0, 0));
            p1 = p2;
        }
    }
}
