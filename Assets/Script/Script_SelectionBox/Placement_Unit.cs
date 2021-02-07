using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement_Unit : MonoBehaviour
{
    #region Selector_Units
    private Selector_Unit _selector_Unit;
    private GameObject _unitFormationIndicatorSize;
    private List<GameObject> _selectedUnits;
    #endregion Selector_Units
    private Camera cam;
    private LayerMask mask;

    public int maxRowFormation = 10; // dépendra du type d'unité(class_unit.maxRow)
    public int minRowFormation = 5;

    public Vector3 startPosition; //mouse click position start
    public Vector3 endPosition;

    public Vector3 rowStartPosition;
    public Vector3 rowEndPosition;

    public List<GameObject> UnitFormationIndicators;
    // Start is called before the first frame update
    void Start()
    {
        //END-START POSITION
        //startPosition = _selector_Unit.startPosition;
        //endPosition = _selector_Unit.endPosition;

        UnitFormationIndicators = _selector_Unit.UnitFormationIndicators;

        _unitFormationIndicatorSize = _selector_Unit.Unit_FormationIndicatorSize;
        _selectedUnits = _selector_Unit.selectedUnits;
    }

    // Update is called once per frame
    void Update()
    {
        if (_selectedUnits.Count > 0)
        {
            startPosition = _selector_Unit.startPosition;
            endPosition = _selector_Unit.endPosition;
            if (Input.GetMouseButtonDown(1))
            {
                startPosition = Selection_Utils.DoRay(cam, mask);
                //Prefab generation and transform
                //1 Load and create prefab
                //2 Give to the prefab the correct size (size of unit selected)
                //3 Destroy the prefab

                _unitFormationIndicatorSize = Resources.Load<GameObject>("SelectionBox/Unit_FormationIndicator");
                _unitFormationIndicatorSize = Instantiate(_unitFormationIndicatorSize, startPosition, Quaternion.identity);
                //Give the correct form to the formation tool depending of the scale of the unit selected
                _selector_Unit.Unit_FormationIndicatorSize = _selector_Unit.UnitFormationCylinder(_selectedUnits[0].transform);
                Destroy(_unitFormationIndicatorSize.gameObject);

                Vector3 newPlacement = startPosition;
                for (int i = 0; i < _selectedUnits.Count; i++)
                {
                    if (i == 0)
                    {
                        UnitFormationIndicators.Add(Instantiate(_unitFormationIndicatorSize, newPlacement, Quaternion.identity).gameObject);
                        continue;
                    }
                    //Detect when a new colum must begin (depending of the size of the units list)
                    if (i % minRowFormation == 0)
                    {
                        newPlacement.x = startPosition.x;
                        newPlacement.z -= _selectedUnits[0].transform.localScale.z;
                    }
                    //align units one after another, next to each other
                    else
                    {
                        newPlacement.x += (_selectedUnits[0].transform.localScale.x) + 0.1f;
                    }
                    UnitFormationIndicators.Add(Instantiate(_unitFormationIndicatorSize, newPlacement, Quaternion.identity).gameObject);
                }

            }
            if (Input.GetMouseButton(1))
            {
                //Unit Info
                int unitCount = UnitFormationIndicators.Count;

                //RAY
                endPosition = Selection_Utils.DoRay(cam, mask);
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
                            rowStartPosition = startPosition - (Vector3.Cross(dir, Vector3.up).normalized * nbRow * (unitSize + separation));
                            rowEndPosition = endPosition - (Vector3.Cross(dir, Vector3.up).normalized * nbRow * (unitSize + separation));
                            mousDrag = new Ray(rowStartPosition, dir);
                        }
                        UnitFormationIndicators[i].transform.position = mousDrag.GetPoint(Mathf.Abs(((i - maxRowFormation * nbRow) * (unitSize + separation))));
                    }
                }

                else if (rayDistance > (minRowFormation * (unitSize + separation)) && rayDistance < (maxRowFormation * (unitSize + separation)))
                {
                    Debug.Log("RAY DDISTANCE 3: " + rayDistance);
                    float currentRowMax = Mathf.Ceil(rayDistance / (unitSize + separation));
                    float lastRowPos = 0f;
                    for (int i = 0; i < unitCount; i++)
                    {
                        //Debug.Log("positionStart: " + positionStart);
                        if (i != 0 && i % currentRowMax == 0)
                        {
                            nbRow += 1;
                            //TRANSLATION D'UN VECTOR SUR UNE DISTANCE X Vector3 newSpot = oldSpotVector3 + (directionVector3.normalized * distanceFloat)
                            // PERPENDICULAIRE/PRODUIT SCALAIRE : Vecteur  A(ax,ay) perpendiculaire à B(bx;by) si (ax*bx + ay*by) = 0 DANS UNITY: Vector3.Cross()
                            rowStartPosition = startPosition - (Vector3.Cross(dir, Vector3.up).normalized * nbRow * (unitSize + separation));
                            rowEndPosition = endPosition - (Vector3.Cross(dir, Vector3.up).normalized * nbRow * (unitSize + separation));
                            //Define new ray for the next row
                            mousDrag = new Ray(rowStartPosition, dir);
                            if (Mathf.Floor(unitCount / currentRowMax) == nbRow)
                            {
                                //determin if the last row position unit is align or between 2 unit from the front row
                                if (currentRowMax % 2 == 0)
                                {
                                    //PAIR DONC moitié du nombre d'unité restant sur la dernière rangé
                                    lastRowPos = Mathf.Ceil((currentRowMax - (unitCount - nbRow * currentRowMax)) / 2) * (unitSize + separation);
                                }
                                else
                                {
                                    //IMPAIR DONC (moitié du nombre d'unité restant arrondi inférieur + moitié d'une unité) sur la dernière rangé
                                    //On doit mettre le "-" pour le CEIL car 0.5 CEIL = 1 ET NON 0 PAR CONTRE CEIL(-0.5) = 0
                                    lastRowPos = ((unitSize + separation) / 2) + Mathf.Abs((Mathf.Ceil(-((currentRowMax - (unitCount - nbRow * currentRowMax)) / 2) * (unitSize + separation))));
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
                            rowStartPosition = startPosition - (Vector3.Cross(dir, Vector3.up).normalized * nbRow * (unitSize + separation));
                            rowEndPosition = endPosition - (Vector3.Cross(dir, Vector3.up).normalized * nbRow * (unitSize + separation));
                            mousDrag = new Ray(rowStartPosition, dir);
                        }
                        UnitFormationIndicators[i].transform.position = mousDrag.GetPoint(Mathf.Abs(((i - minRowFormation * nbRow) * (unitSize + separation))));
                    }
                }

            }
            //for now it destroy the placeholders
            //next, placeholders shall remain on the terrain until units move to their position.
            if (Input.GetMouseButtonUp(1))
            {
                foreach (GameObject clone in UnitFormationIndicators)
                {
                    Destroy(clone.gameObject);
                }
                UnitFormationIndicators.Clear();
            }
        }
    }
}
