using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {
    public bool blockMovement = false;
    public GridController grid;
    [SerializeField]float cameraSpeed = 0.2f;
    [SerializeField] float scrollSpeed = 20f;
    [SerializeField] float cameraY_PosMax = 10f;
    float xMin, zMin;
    float xMax, zMax;
    float yMin, yMax;

    float doubleTouchDist = 0;


    // Use this for initialization
    void Start () {
        xMin = grid.transform.position.x;
        zMin = grid.transform.position.z;
        xMax = grid.transform.position.x + grid.terrainWidth;
        zMax = grid.transform.position.z + grid.terrainHeight;
        yMin = grid.transform.position.y + 3f;
        yMax = grid.transform.position.y + 3f + cameraY_PosMax;
        
    }
	
	// Update is called once per frame
	void Update () {

        if (blockMovement) return;

        //TouchMove();
        //TouchZoom();

        MouseMove();
        MouseZoom();       
        
		
	}


    void TouchMove()
    {
        if (Input.touchCount == 1)
        {

            float mouseX = Input.GetTouch(0).deltaPosition.x;
            float mouseY = Input.GetTouch(0).deltaPosition.y;
            Vector3 moveDirection = new Vector3(mouseX, 0, mouseY);

            Quaternion buff = transform.rotation;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            transform.rotation = buff;
            transform.position += -moveDirection * Time.deltaTime * cameraSpeed;

            Vector3 currentPos = transform.position;
            transform.position = new Vector3(Mathf.Clamp(currentPos.x, xMin, xMax), currentPos.y, Mathf.Clamp(currentPos.z, zMin, zMax));

        }

    }

    void TouchZoom()
    {
        if (Input.touchCount == 2)
        {
            Vector2 f1 = Input.GetTouch(0).position;
            Vector2 f2 = Input.GetTouch(1).position;

            if (doubleTouchDist == 0) doubleTouchDist = Vector2.Distance(f1, f2);
            float delta = Vector2.Distance(f1, f2) - doubleTouchDist;
            if (delta != 0)
            {
                Vector3 offset = (transform.TransformDirection(Vector3.forward) * delta).normalized * Time.deltaTime * scrollSpeed;
                if (transform.position.y + offset.y > yMax || transform.position.y + offset.y < yMin) return;

                transform.position += offset;
                Vector3 currentPos = transform.position;
                transform.position = new Vector3(Mathf.Clamp(currentPos.x, xMin, xMax), Mathf.Clamp(currentPos.y, yMin, yMax), Mathf.Clamp(currentPos.z, zMin, zMax));
            }

            doubleTouchDist = Vector2.Distance(f1, f2);
        }
        else doubleTouchDist = 0;
    }

    void MouseMove()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            Vector3 moveDirection = new Vector3(mouseX, 0, mouseY);

            Quaternion buff = transform.rotation;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            transform.rotation = buff;
            transform.position += -moveDirection * Time.deltaTime * cameraSpeed;

            Vector3 currentPos = transform.position;
            transform.position = new Vector3(Mathf.Clamp(currentPos.x, xMin, xMax), currentPos.y, Mathf.Clamp(currentPos.z, zMin, zMax));

        }
        

    }

    void MouseZoom()
    {
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll!=0)
        {
            
            Vector3 offset = -(transform.TransformDirection(Vector3.forward) * mouseScroll).normalized * Time.deltaTime * scrollSpeed;
            if (transform.position.y + offset.y > yMax || transform.position.y + offset.y < yMin) return;

            transform.position += offset;
            Vector3 currentPos = transform.position;
            transform.position = new Vector3(Mathf.Clamp(currentPos.x, xMin, xMax), Mathf.Clamp(currentPos.y, yMin, yMax), Mathf.Clamp(currentPos.z, zMin, zMax));
            
        }
        
    }

}
