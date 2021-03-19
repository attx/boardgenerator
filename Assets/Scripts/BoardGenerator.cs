using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boardgame;

public class BoardGenerator : MonoBehaviour
{
    private readonly static float fieldDistance = 2.0f;

    private readonly static float fieldHeight = 0.1f;

    private readonly static float groundHeight = 0.2f;

    private bool generated = false;

    private GameObject go;

    public void CreateRandom()
    {
        int size = Utils.IntRange(3, 16);
        int depth = Utils.IntRange(1, 5);
        bool connector = Utils.IntRange(0, 2) == 1;

        while(!Utils.IsValidBoard(size, depth, connector))
        {
            size = Utils.IntRange(3, 16);
            depth = Utils.IntRange(1, 5);
            connector = Utils.IntRange(0, 1) == 1;
        }

        Create(size, depth, connector);
    }
    
    public void Create(int size, int depth, bool connector) {
        if (generated) {
            DestroyImmediate(go);
        }
        go = new GameObject("Generated");
        generated = true;

        int divisions = connector ? size * 2 : size;

        List<Color> colors = Utils.GetRandomColorList(size);
        float radius = Utils.GetPolygonRadiusBySideLength(fieldDistance * 2, divisions);
        List<Vector3> corners = Utils.GetPolygonCorners(radius, divisions);

        for (int s = 0; s < corners.Count; s++)
        {
            int sectorIndex = connector ? s / 2 : s;

            if (connector && s % 2 != 0)
            {
                // Connector
                Vector3 pos = Utils.GetPointDistanceFromObject(corners[s], Vector3.zero, 0);

                CreateField(pos, Color.white);
            }
            else
            {
                float angle = (360.0f / divisions) * s;

                // Start base
                for (int b = 1; b <= 2; b++)
                {
                    Vector3 pos = Utils.GetPointDistanceFromObject(corners[s], Vector3.zero, fieldDistance * (depth + b));

                    for (int i = -1; i <= 1; i += 2)
                    {
                        Vector3 basePos = Utils.GetPointFromPointByAngle(pos, angle, (fieldDistance / 2) * i);

                        CreateField(basePos, colors[sectorIndex]);
                    }
                }
                
                // Left path
                for (int d = connector ? (int)0 : (int)1; d < depth; d++)
                {
                    Vector3 pos = Utils.GetPointDistanceFromObject(corners[s], Vector3.zero, d * fieldDistance);
                    Vector3 left = Utils.GetPointFromPointByAngle(pos, angle, fieldDistance);

                    CreateField(left, Color.white);

                    // Path exit
                    if (d == depth - 1)
                    {
                        CreateField(pos, Color.white);
                    }
                }
    
                // Right path
                if (connector || depth > 1)
                {
                    for (int d = depth - 1; d >= 0; d--)
                    {
                        Vector3 pos = Utils.GetPointDistanceFromObject(corners[s], Vector3.zero, d * fieldDistance);
                        Vector3 right = Utils.GetPointFromPointByAngle(pos, angle, -fieldDistance);

                        CreateField(right, Color.white);
                    }
                }

                // End base
                for (int b = 2; b < 6; b++)
                {
                    Vector3 pos = Utils.GetPointDistanceFromObject(corners[s], Vector3.zero, (depth * fieldDistance) - (b * fieldDistance));

                    CreateField(pos, colors[sectorIndex]);
                }
            }
        }
        CreateGround();
    }

    private void CreateField(Vector3 position, Color color) {
        GameObject field = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        field.transform.localScale = new Vector3(1, fieldHeight, 1);
        field.transform.position = position;
        field.transform.parent = go.transform;  
        field.GetComponent<Renderer>().material.color = color;
    }

    private void CreateGround()
    {
        int spacing = 4;

        Bounds bounds = Utils.GetMaxBounds(go);
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.transform.localScale = new Vector3(bounds.extents.x * 2 + spacing, groundHeight, bounds.extents.z * 2 + spacing);
        ground.transform.position = new Vector3(bounds.center.x, -(groundHeight / 2) - (fieldHeight / 2), bounds.center.z);
        ground.transform.parent = go.transform;
        ground.GetComponent<Renderer>().material.color = Color.yellow;
    }
}
