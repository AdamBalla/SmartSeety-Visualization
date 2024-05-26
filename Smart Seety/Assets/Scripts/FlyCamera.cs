using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlyCamera : MonoBehaviour
{
    const float speedMove = 0.25f;
    const float speedRotate = 0.25f;
    const float speedForward = 0.25f;
    const float stepMove = 5f;
    const float stepRotate = 5f;
    const float focalLength = 100.0f;
    Vector3 prevMousePosition;
    Vector3 currMousePosition;

    [HideInInspector]
    public GameObject selectedObject;

    // Use this for initialization
    private void Start()
    {
        currMousePosition = Input.mousePosition;
    }

    // Update is called once per frame
    private void Update()
    {
        prevMousePosition = currMousePosition;
        currMousePosition = Input.mousePosition;
        Vector3 distance = currMousePosition - prevMousePosition;
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetMouseButton(0))
            {
                RotateHorizontal(distance.x);
                RotateVertical(-distance.y);
            }

            if (Input.GetMouseButton(2))
            {
                MoveHorizontal(-distance.x);
                MoveVertical(-distance.y);
            }

            if (Input.GetMouseButton(1))
            {
                MoveForward(distance.x);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                int layerMask = 1 << 9;
                Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, ray.direction, out hit, Mathf.Infinity, layerMask))
                {
                    DiselectObject();
                    SelectObject(hit.collider.gameObject);
                }
                else
                {
                    DiselectObject();
                }
            }
        }
    }

    private void MoveHorizontal(float dist)
    {
        transform.position += transform.right * dist * speedMove;
    }

    private void MoveVertical(float dist)
    {
        transform.position += transform.up * dist * speedMove;
    }

    private void MoveForward(float dist)
    {
        transform.position += transform.forward * dist * speedForward;
    }

    private void RotateHorizontal(float dist)
    {
        transform.RotateAround(transform.position + transform.forward * focalLength, transform.up, dist * speedRotate);
    }

    private void RotateVertical(float dist)
    {
        transform.RotateAround(transform.position + transform.forward * focalLength, transform.right, dist * speedRotate);
    }

    private void SelectObject(GameObject gameObject0)
    {
        var graphs = transform.Find("Graphs");
        graphs.Find("Plane").gameObject.SetActive(true);
        transform.Find("Graphs").gameObject.SetActive(true);
        selectedObject = gameObject0;
        var sd = selectedObject.transform.parent.parent.GetComponent<Device>();
        var text = graphs.Find("ObjectNameText");
        text.gameObject.SetActive(true);
        text.GetComponent<TextMesh>().text = sd.name;
        var camera = sd.GetComponentInChildren<Camera>(true);
        if (camera != null)
            camera.enabled = true;

        sd.selected = true;
    }

    private void DiselectObject()
    {
        var graphs = transform.Find("Graphs");
        graphs.Find("ObjectNameText").gameObject.SetActive(false);
        graphs.Find("Plane").gameObject.SetActive(false);
        graphs.gameObject.SetActive(false);
        transform.Find("Canvas").Find("Sensors").Find("Preview").gameObject.SetActive(false);
        if (selectedObject != null)
        {
            var sd = selectedObject.transform.parent.parent.GetComponent<Device>();
            var camera = sd.GetComponentInChildren<Camera>(true);
            if (camera != null)
                camera.enabled = false;

            sd.selected = false;
        }

        selectedObject = null;
    }
}
