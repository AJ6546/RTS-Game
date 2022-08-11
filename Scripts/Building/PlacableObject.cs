using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    public bool placed { get; private set; }
    public Vector3Int size { get; private set; }
    Vector3[] vertices;

    void GetColliderVertexPositionsLocal()
    {
        BoxCollider b = GetComponentInChildren<BoxCollider>();
        vertices = new Vector3[4];
        vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;
    }
    void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[4];
        for(int i =0;i<vertices.Length;i++)
        {
            Vector3 worldPos = transform.TransformPoint(vertices[i]);
            vertices[i] = BuildingSystem.instance.gridLayout.WorldToCell(worldPos);
        }
        size = new Vector3Int(Mathf.Abs((vertices[0] - vertices[1]).x), Mathf.Abs((vertices[0] - vertices[3]).y), 1);
    }
    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(vertices[0]);
    }
    private void Start()
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();
    }
    public virtual void Place()
    {
        ObjectDrag drag = GetComponent<ObjectDrag>();

        Destroy(drag);
        placed = true;
    }
    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));
        size = new Vector3Int(size.y, size.x, 1);

        Vector3[] vert = new Vector3[vertices.Length];
        for(int i=0;i<vert.Length;i++)
        {
            vert[i] = vertices[(i + 1) % vertices.Length];
        }
        vertices = vert;
    }
}
