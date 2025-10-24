using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UILineRenderer : MaskableGraphic
{
    [SerializeField]
    private Vector2[] _points;

    public Vector2[] points
    {
        get => _points;
        set
        {
            _points = value;
            SetVerticesDirty(); // Forces a redraw
        }
    }

    public float thickness = 10f;
    public bool center = true;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points == null || points.Length < 2)
            return;

        for (int i = 0; i < points.Length - 1; i++)
        {
            // Create a line segment between the next two points
            CreateLineSegment(points[i], points[i + 1], vh, i);

            int index = i * 5;

            // Add the line segment to the triangles array
            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index);

            // Add beveled edges between segments
            if (i != 0)
            {
                vh.AddTriangle(index, index - 1, index - 3);
                vh.AddTriangle(index + 1, index - 1, index - 2);
            }
        }
    }

    private void CreateLineSegment(Vector3 point1, Vector3 point2, VertexHelper vh, int i)
{
    Vector3 offset = center ? (rectTransform.sizeDelta / 2) : Vector2.zero;

    // Create vertex template
    UIVertex vertex = UIVertex.simpleVert;
    Color vertexColor = color;

    vertex.color = vertexColor;

    // Create the start of the segment
    Quaternion point1Rotation = Quaternion.Euler(0, 0, RotatePointTowards(point1, point2) + 90);
    vertex.position = point1Rotation * new Vector3(-thickness / 2, 0);
    vertex.position += point1 - offset;
    vh.AddVert(vertex);
    vertex.position = point1Rotation * new Vector3(thickness / 2, 0);
    vertex.position += point1 - offset;
    vh.AddVert(vertex);

    // Create the end of the segment
    Quaternion point2Rotation = Quaternion.Euler(0, 0, RotatePointTowards(point2, point1) - 90);
    vertex.position = point2Rotation * new Vector3(-thickness / 2, 0);
    vertex.position += point2 - offset;
    vh.AddVert(vertex);
    vertex.position = point2Rotation * new Vector3(thickness / 2, 0);
    vertex.position += point2 - offset;
    vh.AddVert(vertex);

    // Also add the end point
    vertexColor = new Color(0, 0, 0, 0); // Fully transparent
    vertex.position = point2 - offset;
    vh.AddVert(vertex);
}

    private float RotatePointTowards(Vector2 vertex, Vector2 target)
    {
        return Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * Mathf.Rad2Deg;
    }
}

