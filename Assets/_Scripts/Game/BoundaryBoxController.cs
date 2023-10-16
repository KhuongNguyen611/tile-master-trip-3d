using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryBoxController : StaticInstance<BoundaryBoxController>
{
    private List<MeshRenderer> _listBoundaries = new();

    void Start()
    {
        foreach (Transform boundary in transform)
        {
            _listBoundaries.Add(boundary.GetComponent<MeshRenderer>());
        }
    }

    public void ShowBoundary()
    {
        _listBoundaries.ForEach((boundaryMesh) => boundaryMesh.enabled = true);
    }
}
