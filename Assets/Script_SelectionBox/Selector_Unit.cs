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
    public RectangleF rectSelectionRect;
    public Vector3 rectCenter;
    public Vector3 rectSize;
    public Vector3 rectHalfExtents;

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

    public Vector3 rowStartPosition;
    public Vector3 rowEndPosition;

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
            selectorBox.transform.position = rectCenter;
            selectorBox.transform.localScale = rectSize + new Vector3(0f,1f,0f);
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
                //Unit Info
                int unitCount = UnitFormationIndicators.Count;

                //RAY
                endPosition = DoRay();
                float rayDistance = (startPosition - endPosition).magnitude;
                float separation = 0.1f;
                float unitSize = Mathf.Abs(UnitFormationIndicators[0].transform.localScale.x);

                //RAYCAST
                Vector3 dir = (endPosition - startPosition);
                Ray mousDrag = new Ray(startPosition, dir);
                rowStartPosition = startPosition;
                rowEndPosition = endPosition;

                int nbRow = 0;

                if (rayDistance < unitSize)
                {
                    //DEFINIR UN RANGEMENT
                   
                }
                else if (rayDistance >= (unitSize + separation) * (maxRowFormation) || ((unitCount <= maxRowFormation) && (rayDistance == unitCount)))
                {
                    for (int i = 0; i < unitCount; i++)
                    {
                        if (i != 0 && i % maxRowFormation == 0)
                        {
                            nbRow += 1;
                            rowStartPosition = startPosition - (Vector3.Cross(dir, Vector3.up).normalized * nbRow);
                            rowEndPosition = endPosition - (Vector3.Cross(dir, Vector3.up).normalized * nbRow);
                            dir = (rowEndPosition - rowStartPosition);
                            mousDrag = new Ray(rowStartPosition, dir);
                            UnitFormationIndicators[i].transform.position = mousDrag.GetPoint(Mathf.Abs(((i - maxRowFormation * nbRow) * (unitSize + separation))));
                        }
                        else
                        {
                            UnitFormationIndicators[i].transform.position = mousDrag.GetPoint(Mathf.Abs(((i - maxRowFormation * nbRow) * (unitSize + separation))));
                        }
                    }
                }
                
                else if(rayDistance > (minRowFormation*(unitSize+separation)) && rayDistance < (maxRowFormation * (unitSize + separation)))
                {
                    Debug.Log("RAY DDISTANCE 3: " + rayDistance);
                    float currentRowMax = Mathf.Ceil(rayDistance / (unitSize + separation));
                    float lastRowPos = 0f;
                    for (int i = 0; i < unitCount; i++)
                    {
                        float positionStart = rowStartPosition.magnitude;
                        //Debug.Log("positionStart: " + positionStart);
                        if (i != 0 && i % currentRowMax == 0)
                        {
                            nbRow += 1;
                            //TRANSLATION D'UN VECTOR SUR UNE DISTANCE X Vector3 newSpot = oldSpotVector3 + (directionVector3.normalized * distanceFloat)
                            // PERPENDICULAIRE/PRODUIT SCALAIRE : Vecteur  A(ax,ay) perpendiculaire à B(bx;by) si (ax*bx + ay*by) = 0 DANS UNITY: Vector3.Cross()
                            rowStartPosition = startPosition - (Vector3.Cross(dir, Vector3.up).normalized * nbRow);
                            rowEndPosition =  endPosition -  (Vector3.Cross(dir, Vector3.up).normalized * nbRow);
                            //Define new ray for the next row
                            mousDrag = new Ray(rowStartPosition, dir);
                            if (Mathf.Floor(unitCount / currentRowMax) == nbRow)
                            {
                                //determin if the last row position unit is align or between 2 unit from the front row
                                if (currentRowMax % 2 == 0)
                                {
                                    //PAIR DONC moitié du nombre d'unité restant sur la dernière rangé
                                    lastRowPos = Mathf.Ceil((currentRowMax - (unitCount - nbRow * currentRowMax)) /2) * (unitSize + separation);
                                }
                                else
                                {
                                    //IMPAIR DONC (moitié du nombre d'unité restant arrondi inférieur + moitié d'une unité) sur la dernière rangé
                                    //On doit mettre le "-" pour le CEIL car 0.5 CEIL = 1 ET NON 0 PAR CONTRE CEIL(-0.5) = 0
                                    lastRowPos = ((unitSize + separation)/2) + Mathf.Abs( (Mathf.Ceil(-( (currentRowMax - (unitCount - nbRow * currentRowMax)) / 2) * (unitSize + separation) )) );
                                }
                            }

                            UnitFormationIndicators[i].transform.position = mousDrag.GetPoint(Mathf.Abs(lastRowPos + ((i - currentRowMax * nbRow) * (unitSize + separation))));
                        }
                        //lastRow
                        else
                        {
                            UnitFormationIndicators[i].transform.position = mousDrag.GetPoint(Mathf.Abs(lastRowPos + ((i - currentRowMax * nbRow) * (unitSize + separation))));
                        }
                    }
                }
                //LINE TOO BIG
                else
                {
                    for (int i = 0; i < unitCount; i++)
                    {
                        if (i != 0 && i % minRowFormation == 0)
                        {
                            nbRow += 1;
                            rowStartPosition = startPosition - (Vector3.Cross(dir, Vector3.up).normalized * nbRow);
                            rowEndPosition = endPosition - (Vector3.Cross(dir, Vector3.up).normalized * nbRow);
                            mousDrag = new Ray(rowStartPosition, dir);

                            UnitFormationIndicators[i].transform.position = mousDrag.GetPoint(Mathf.Abs(((i - minRowFormation * nbRow) * (unitSize + separation))));
                        }

                        else
                        {
                            UnitFormationIndicators[i].transform.position = mousDrag.GetPoint(Mathf.Abs(((i - minRowFormation * nbRow) * (unitSize + separation))));
                        }
                    }
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
        rectSize = startPosition - endPosition;
        rectSize.x = Mathf.Abs(rectSize.x);
        rectSize.y = Mathf.Abs(rectSize.y);
        rectSize.z = Mathf.Abs(rectSize.z);

        rectCenter = (startPosition + endPosition) * 0.5f;

        rectHalfExtents = rectSize * 0.5f;
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

        RaycastHit[] check = Physics.BoxCastAll(rectCenter, rectHalfExtents, Vector3.up);
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
        Gizmos.DrawWireCube(rectCenter, rectSize);
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
