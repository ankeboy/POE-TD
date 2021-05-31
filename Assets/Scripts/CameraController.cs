using UnityEngine;

public class CameraController : MonoBehaviour
{
    float panSpeed = 30f;
    float panBorderThickness = 10f;  // the distance to the edge of the border used as buffer for mouse movement

    float scrollSpeed = 2f;
    public float minY = 20f;
    public float maxY = 110f;

    public float minX = 20f;
    public float maxX = 60f;
    public float minZ = -80f;
    public float maxZ = 0;

    void Update()
    {
        if (GameManager.GameIsOver)
        {
            this.enabled = false;
            return;
        }

        //camera movement
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness) // or is represented by 2 vertical lines
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);   //Vector3.forward is the same as Vector3(0f, 0f, 1f). Multiply by Panspeed. 
            //Multiply by deltaTime, so that the speed is relative to real time and not PC processor time. Move relative to World Space, rather than camera, as camera is rotated.
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        //camera zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;

        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);     //Clamp limits the first argument within the range of the other two arguments
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.position = pos;
    }
}
