using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing; // POUR RECTANGLEF

public class Selector_Unit : MonoBehaviour
{
    [Header("For Raycast")]
    public Camera cam;
    public LayerMask mask;

    [Header("Selection Box start/end points")]
    public Vector3 startPosition; //mouse click position start
    public Vector3 endPosition;

    [Header("Selection Box Info")]
    public RectangleF rect_selectionRect;
    public Vector3 rect_Center;
    public Vector3 rect_Size;
    public Vector3 rect_halfExtents;

    [Header("All Currently Selected Units")]
    public List<GameObject> selectedUnits;

    [Header("GameObject for selection box")]
    public GameObject selectorBox;

    private Vector3 movetoPos = Vector3.zero; //pour mouvement donc PAS MAINTENANT

    [Header("Formation Unit")]

    //public Transform spawnPosition;
    public GameObject Unit_FormationIndicatorSize;
    public List<GameObject> UnitFormationIndicators;

    //limit Row
    public int maxRowFormation = 10; // dépendra du type d'unité(class_unit.maxRow)
    public int minRowFormation = 5;

    // Start is called before the first frame update
    void Start()
    {
        UnitFormationIndicators = new List<GameObject>();
        selectedUnits = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPosition = DoRay();
            selectorBox.SetActive(true);
        }
        //Draw the selection rectangle
        if(Input.GetMouseButton(0))
        {
            endPosition = DoRay();
            HandleRectangle();
            selectorBox.transform.position = rect_Center;
            selectorBox.transform.localScale = rect_Size + new Vector3(0f,1f,0f);
        }
        //Deselect all unit and destroy the selection cube
        if (Input.GetMouseButtonUp(0))
        {
            selectorBox.SetActive(false);
            endPosition = DoRay();
            HandleRectangle();
            SelectAllUnits();
        }
        //Mouvement ICI
        if(selectedUnits.Count > 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                startPosition = DoRay();
                //Give the correct form to the formation tool depending of the scale of the unit selected
                Unit_FormationIndicatorSize = UnitFormationCylinder(selectedUnits[0].transform);

                Vector3 newPlacement = startPosition;
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    if (i == 0)
                    {
                        UnitFormationIndicators.Add(Instantiate(Unit_FormationIndicatorSize, newPlacement, Quaternion.identity).gameObject);
                        continue;
                    }
                    //Detect when a new colum must begin (depending of the size of the units list)
                    if (i % minRowFormation == 0)
                    {
                        newPlacement.x = startPosition.x;
                        newPlacement.z -= selectedUnits[0].transform.localScale.z;
                    }
                    //align units one after another, next to each other
                    else
                    {
                        newPlacement.x += (selectedUnits[0].transform.localScale.x) + 0.1f;
                    }
                    UnitFormationIndicators.Add(Instantiate(Unit_FormationIndicatorSize, newPlacement, Quaternion.identity).gameObject);
                }

            }
            // align units depending of the direction of the mouse
            if (Input.GetMouseButton(1))
            {
                endPosition = DoRay();
                Vector3 dir = (endPosition - startPosition).normalized;
                Ray mousDrag = new Ray(startPosition, dir);
                Vector3 newPlacement = startPosition;
                //Debug.DrawLine(startPosition, startPosition + dir * 10, UnityEngine.Color.red, Mathf.Infinity);
                int NumRow = 0;
                for (int i = 0; i < UnitFormationIndicators.Count; i++)
                {
                    if(i == 0)
                    {
                        UnitFormationIndicators[i].transform.position = newPlacement;
                        continue;
                    }
                    if (i % minRowFormation == 0)
                    {

                        NumRow += 1;
                        newPlacement = mousDrag.GetPoint((startPosition+UnitFormationIndicators[0].transform.localPosition).magnitude) ;
                        /*
                        newPlacement.x = UnitFormationIndicators[0].transform.position.x - (1f * NumRow);
                        newPlacement.z = UnitFormationIndicators[0].transform.position.z + (1f * NumRow);
                        */

                    }
                    else
                    {
                        newPlacement = mousDrag.GetPoint(Mathf.Abs((UnitFormationIndicators[i - 1].transform.localPosition).magnitude + 0.1f));
                        /*
                        newPlacement = mousDrag.GetPoint(UnitFormationIndicators[i-1].transform.position.magnitude);
                        
                        newPlacement.x += UnitFormationIndicators[i-1].transform.position.x + (0.1f * i);
                        newPlacement.z += UnitFormationIndicators[i-1].transform.position.z + (0.1f * i);
                        */

                    }
                    UnitFormationIndicators[i].transform.position = newPlacement;
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                foreach(GameObject clone in UnitFormationIndicators)
                {
                    Destroy(clone.gameObject);
                }
                UnitFormationIndicators.Clear();
            }
        }
        Debug.DrawLine(startPosition, endPosition);
    }
    private GameObject UnitFormationCylinder(Transform Unitscale)
    {
        //a terme DEVRA repéré cavalerie = triangle; troupe à pied = cylindre
        Unit_FormationIndicatorSize = GameObject.Find("Unit_FormationIndicator");
        Unit_FormationIndicatorSize.transform.localScale = Unitscale.localScale;

        return Unit_FormationIndicatorSize;
    }

    private void HandleRectangle()
    {
        rect_Size = startPosition - endPosition;
        rect_Size.x = Mathf.Abs(rect_Size.x);
        rect_Size.y = Mathf.Abs(rect_Size.y);
        rect_Size.z = Mathf.Abs(rect_Size.z);

        rect_Center = (startPosition + endPosition) * 0.5f;

        rect_halfExtents = rect_Size * 0.5f;
    }

    private void ClearAllUnits()
    {
        for(int i = 0; i < selectedUnits.Count; i++)
        {
            Units unit = selectedUnits[i].GetComponent<Units>();
            // changement de couleur ici normalement
            Renderer rSelect = selectedUnits[i].GetComponent<Renderer>();
            Material mSelect = rSelect.material;
            mSelect.color = UnityEngine.Color.blue;
        }
        //Remove every component in the list
        selectedUnits.Clear();
    }

    private void SelectAllUnits()
    {
        ClearAllUnits();

        RaycastHit[] check = Physics.BoxCastAll(rect_Center, rect_halfExtents, Vector3.up);
        //verify that colliders are units
        for(int i = 0; i < check.Length; i++)
        {
            if(check[i].collider.CompareTag("Unit"))
            {
                Renderer rSelect = check[i].collider.GetComponent<Renderer>();
                Units unit = check[i].collider.GetComponent<Units>();
                // changement de couleur ici normalement
                Material mSelect = rSelect.material;
                mSelect.color = UnityEngine.Color.red;

                selectedUnits.Add(unit.gameObject);
            }
        }
    }

    private Vector3 DoRay()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit,1000f,mask))
        {
            if(hit.collider.CompareTag("Ground"))
            {
                return hit.point;
            }
        }

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        //cube we draw
        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawWireCube(rect_Center, rect_Size);
        //start point of the cube
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(startPosition, 0.5f);
        //end point of the cube
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(endPosition, 0.5f);
        // POUR MOUVEMENT
        /*
        Gizmos.color = UnityEngine.Color.cyan;
        Gizmos.DrawSphere(movetoPos, 0.5f);
        */
    }
}
