using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection_Utils : MonoBehaviour
{
    static Camera cam;
    static LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {

    }
    /// <summary>
    /// Give you the Vector3 where the play click
    /// </summary>
    /// <param name="cam">Camera from wich de ray will point</param>
    /// <param name="mask">Layer concerned</param>
    /// <returns></returns>
    public static Vector3 DoRay(Camera cam, LayerMask mask)
    {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, mask))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return hit.point;
            }
        }

        return Vector3.zero;
    }
}
