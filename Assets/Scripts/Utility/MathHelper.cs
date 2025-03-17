using UnityEngine;

namespace Utility
{
    public static class MathHelper
    {
        public static (float min, float max) GetMinMax(params float[] values)
        {
            float min = float.MaxValue;
            float max = float.MinValue;
            float k;
            for (int i = 0; i < values.Length; i++)
            {
                k = values[i];
                if (k < min)
                    min = k;
                if (k > max)
                    max = k;
            }

            return (min, max);
        }

        public static Rect Encapsulate(this Rect rect, Rect other)
        {
            return Rect.MinMaxRect(
                Mathf.Min(rect.xMin, other.xMin),
                Mathf.Min(rect.yMin, other.yMin),
                Mathf.Max(rect.xMax, other.xMax),
                Mathf.Max(rect.yMax, other.yMax)
            );
        }

        public static Bounds ToBounds(this Rect rect)
        {
            var size = rect.size.FromGroundPlane();
            size.y = 1f;
            return new Bounds(rect.center.FromGroundPlane(), size);
        }
    }
}