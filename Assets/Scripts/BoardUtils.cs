using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boardgame {
    public static class Utils
    {
        public static bool IsValidBoard(int players, int depth, bool connector)
        {
            if (!connector)
            {
                if ((depth == 4 && (players == 6 || players == 7)) ||
                    (depth > 4 && players < 8)) return true;
            }

            if (connector)
            {
                if ((depth == 1 && players > 7) || 
                    (depth == 2 && players > 5) || 
                    (depth == 3 && players > 4) || 
                    (depth > 3)) return true;
            }
            return false;
        }

        public static int IntRange(int min, int max) => UnityEngine.Random.Range(min, max + 1);

        public static float GetPolygonRadiusBySideLength(float sideLength, int sideCount)
        {
            return sideLength / (2 * Mathf.Tan((180.0f / sideCount) * Mathf.Deg2Rad));
        }

        public static Vector3 GetPointFromPointByAngle(Vector3 position, float angle, float distance)
        {
            return new Vector3(
                position.x + (Mathf.Sin(angle * Mathf.Deg2Rad) * distance),
                position.y,
                position.z + (Mathf.Cos(angle * Mathf.Deg2Rad) * distance)
            );
        }

        public static float GetAngleOnCircleByDivision(int divisions, int division)
        {
            return division * Mathf.PI * 2f / divisions;
        }

        public static List<Vector3> GetPolygonCorners(float radius, int count)
        {
            List<Vector3> corners = new List<Vector3>();

            for (int i = count; i > 0; i--)
            {
                float angle = GetAngleOnCircleByDivision(count, i);

                corners.Add(
                    GetPointOnCircleByAngle(radius, angle)
                );
            }

            return corners;
        }
        private static Vector3 GetPointOnCircleByAngle(float radius, float angle)
        {
            return new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        }

        public static Vector3 GetPointDistanceFromObject(Vector3 target, Vector3 final, float distanceFromSurface)
        {
            Vector3 directionOfTravel = target - final;
            Vector3 finalDirection = directionOfTravel + directionOfTravel.normalized * distanceFromSurface;
            return final + finalDirection;
        }

        public static List<Color> GetRandomColorList(int length)
        {
            List<Color> colors = new List<Color>();
            for(int i = 0; i < length; i++)
            {
                colors.Add(
                    UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f)
                );
            }
            return colors;
        }

        public static Bounds GetMaxBounds(GameObject go) {
            Bounds bounds = new Bounds(go.transform.position, Vector3.zero);
            foreach (Renderer r in go.GetComponentsInChildren<Renderer>()) {
                bounds.Encapsulate(r.bounds);
            }
            return bounds;
        }
    }
}