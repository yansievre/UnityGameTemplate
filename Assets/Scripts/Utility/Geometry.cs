using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class Geometry
    {
        private static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
        }

        public static bool PointInTriangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 point)
        {
            float d1, d2, d3;
            bool has_neg, has_pos;

            d1 = Sign(point, v1, v2);
            d2 = Sign(point, v2, v3);
            d3 = Sign(point, v3, v1);

            has_neg = d1 < 0 || d2 < 0 || d3 < 0;
            has_pos = d1 > 0 || d2 > 0 || d3 > 0;

            return !(has_neg && has_pos);
        }

        public static bool LineRayIntersection(Vector2 lineStart,
            Vector2 lineEnd,
            Vector2 rayStart,
            Vector2 rayDirection,
            out Vector2 ptIntersection,
            out float distance,
            out int side)
        {
            ptIntersection = lineStart;
            distance = 0f;
            rayDirection = rayDirection.normalized;
            side = 0;
            var line = lineEnd - lineStart;
            var toLine = lineStart - rayStart;
            side = Vector2.Dot(-toLine, line.TangentToNormal()) > 0 ? 1 : -1;
            // Denominator for ua and ub are the same, so store this calculation
            var d = rayDirection.y * line.x - rayDirection.x * line.y;

            //n_a and n_b are calculated as seperate values for readability
            var n_a = rayDirection.x * toLine.y - rayDirection.y * toLine.x;

            var n_b = line.x * toLine.y - line.y * toLine.x;

            // Make sure there is not a division by zero - this also indicates that
            // the lines are parallel.  
            // If n_a and n_b were both equal to zero the lines would be on top of each 
            // other (coincidental).  This check is not done because it is not 
            // necessary for this implementation (the parallel check accounts for this).
            if (Mathf.Abs(d) < float.Epsilon)
                return false;

            // Calculate the intermediate fractional point that the lines potentially intersect.
            var ua = n_a / d;

            if (ua < 0 || ua > 1)
                return false;

            var ub = n_b / d;
            distance = ub;

            if (ub >= 0d)
            {
                ptIntersection.x = lineStart.x + ua * (lineEnd.x - lineStart.x);
                ptIntersection.y = lineStart.y + ua * (lineEnd.y - lineStart.y);

                return true;
            }

            // The fractional point will be between 0 and 1 inclusive if the lines
            // intersect.  If the fractional calculation is larger than 1 or smaller
            // than 0 the lines would need to be longer to intersect.
            return false;
        }

        public static bool LineIntersection(Vector2 line0Start,
            Vector2 l0End,
            Vector2 l1Start,
            Vector2 l1End,
            out Vector2 ptIntersection,
            out float ua,
            out float ub)
        {
            ptIntersection = line0Start;
            ua = 0;
            ub = 0;
            // Denominator for ua and ub are the same, so store this calculation
            var d =
                (l1End.y - l1Start.y) * (l0End.x - line0Start.x)
                -
                (l1End.x - l1Start.x) * (l0End.y - line0Start.y);

            //n_a and n_b are calculated as seperate values for readability
            var n_a =
                (l1End.x - l1Start.x) * (line0Start.y - l1Start.y)
                -
                (l1End.y - l1Start.y) * (line0Start.x - l1Start.x);

            var n_b =
                (l0End.x - line0Start.x) * (line0Start.y - l1Start.y)
                -
                (l0End.y - line0Start.y) * (line0Start.x - l1Start.x);

            // Make sure there is not a division by zero - this also indicates that
            // the lines are parallel.  
            // If n_a and n_b were both equal to zero the lines would be on top of each 
            // other (coincidental).  This check is not done because it is not 
            // necessary for this implementation (the parallel check accounts for this).
            if (Mathf.Abs(d) < float.Epsilon)
                return false;

            // Calculate the intermediate fractional point that the lines potentially intersect.
            ua = n_a / d;
            ub = n_b / d;

            if (ua >= 0d && ua <= 1d && ub >= 0d && ub <= 1d)
            {
                ptIntersection.x = line0Start.x + ua * (l0End.x - line0Start.x);
                ptIntersection.y = line0Start.y + ua * (l0End.y - line0Start.y);

                return true;
            }

            // The fractional point will be between 0 and 1 inclusive if the lines
            // intersect.  If the fractional calculation is larger than 1 or smaller
            // than 0 the lines would need to be longer to intersect.
            return false;
        }

        private static readonly Dictionary<int, (bool success, Vector2 ptIntersection, float ua, float ub)> Cached =
            new();

        public static bool LineIntersectionCached(Vector2 line0Start,
            Vector2 line0ToEnd,
            Vector2 line1Start,
            Vector2 line1End,
            out Vector2 ptIntersection,
            out float ua,
            out float ub)
        {
            var hash = line0Start.GetHashCode() ^ line0ToEnd.GetHashCode() ^ line1Start.GetHashCode() ^
                       line1End.GetHashCode();

            if (Cached.TryGetValue(hash, out var cached))
            {
                ptIntersection = cached.ptIntersection;
                ua = cached.ua;
                ub = cached.ub;

                return cached.success;
            }

            ptIntersection = line0Start;
            ua = 0;
            ub = 0;
            // Denominator for ua and ub are the same, so store this calculation
            var d = (line1End.y - line1Start.y) * line0ToEnd.x - (line1End.x - line1Start.x) * line0ToEnd.y;

            // Make sure there is not a division by zero - this also indicates that
            // the lines are parallel.  
            // If n_a and n_b were both equal to zero the lines would be on top of each 
            // other (coincidental).  This check is not done because it is not 
            // necessary for this implementation (the parallel check accounts for this).
            if (Mathf.Abs(d) < float.Epsilon)
            {
                Cached.Add(hash, (false, ptIntersection, ua, ub));

                return false;
            }

            //n_a and n_b are calculated as seperate values for readability
            var n_a =
                (line1End - line1Start).x * (line0Start.y - line1Start.y)
                -
                (line1End - line1Start).y * (line0Start.x - line1Start.x);

            var n_b =
                line0ToEnd.x * (line0Start.y - line1Start.y)
                -
                line0ToEnd.y * (line0Start.x - line1Start.x);
            // Calculate the intermediate fractional point that the lines potentially intersect.
            ua = n_a / d;
            ub = n_b / d;

            if (ua >= 0d && ua <= 1d && ub >= 0d && ub <= 1d)
            {
                ptIntersection = line0Start + ua * line0ToEnd;
                Cached.Add(hash, (true, ptIntersection, ua, ub));

                return true;
            }

            // The fractional point will be between 0 and 1 inclusive if the lines
            // intersect.  If the fractional calculation is larger than 1 or smaller
            // than 0 the lines would need to be longer to intersect.
            Cached.Add(hash, (false, ptIntersection, ua, ub));

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineStart"></param>
        /// <param name="line"></param>
        /// <param name="point"></param>
        /// <param name="lineNormal"></param>
        /// <param name="closestPoint"></param>
        /// <param name="lineProject">Normalized value between lineStart and lineStart + line (lineEnd)</param>
        /// <param name="normalProject"></param>
        /// <returns>True if point can be project on the line, false if not</returns>
        public static bool ClosestPointOnLine(Vector2 lineStart, Vector2 line, Vector2 lineNormal, Vector2 point, out Vector2 closestPoint, out float lineProject, out float normalProject)
        {
            lineProject = SimpleDot(line, (point - lineStart)) / (line.magnitude*line.magnitude);
            normalProject = SimpleDot(lineNormal, (point - lineStart));
            closestPoint = lineStart + line.normalized * lineProject;
            return lineProject >= 0 && lineProject <= 1;
        }

        public static float SimpleDot(Vector2 lhs, Vector2 rhs)
        {
            return (lhs.x * rhs.x + lhs.y * rhs.y);
        }
    }
}