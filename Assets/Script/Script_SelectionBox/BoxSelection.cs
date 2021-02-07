using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSelection : MonoBehaviour
{
    [SerializeField] Collider[] selections; // Indicator in the Unity UI: number of object selected
    [SerializeField] Box box; //indicator in the Unity UI so we can see the box value

    private Vector3 startPosition; //mouse click position start
    private Vector3 dragPosition;

    private Camera cam;
    private Ray ray; //distance  between 2 points

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main; //we select the main camera
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;

            ray = cam.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out hit , 100f);

            if(Input.GetMouseButtonDown(0))
            {
                //On Drag start
                startPosition = hit.point;
                box.baseMin = startPosition;


            }
            //When dragging the mouse
            dragPosition = hit.point;
            box.baseMax = dragPosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            //When mouse released
            selections = Physics.OverlapBox(box.Center, box.Extents, Quaternion.identity);
            box.baseMin = new Vector3(0, 0, 0);
            box.baseMax = new Vector3(0, 0, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(box.Center, box.Size);
    }
}

[System.Serializable]
public class Box
{
    public Vector3 baseMin; //startClick position
    public Vector3 baseMax; //encClick position

    public Vector3 Center
    {
        get
        {
            Vector3 center = baseMin + (baseMax - baseMin) * 0.5f;
            center.y = (baseMax - baseMin).magnitude * 0.5f;
            return center;
        }
    }

    public Vector3 Size
    {
        get
        {
            return new Vector3(Mathf.Abs(baseMax.x - baseMin.x), (baseMax - baseMin).magnitude, Mathf.Abs(baseMax.z - baseMin.z));
        }
    }

    public Vector3 Extents
    {
        //needed for Collider (go to OnDrawGizmo)
        get
        {
            return Size * 0.5f;
        }
    }
}
