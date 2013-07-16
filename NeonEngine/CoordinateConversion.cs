using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace NeonEngine
{
    public static class CoordinateConversion
    {
        public const float unitToPixel = 100.0f;
        public const float pixelToUnit = 1 / unitToPixel;

        public static Vector2 worldToScreen(Vector2 worldCoordinates)
        {
            return worldCoordinates * unitToPixel;
        }

        public static Vector2 screenToWorld(Vector2 screenCoordinates)
        {
            return screenCoordinates * pixelToUnit;
        }

        public static float worldToScreen(float worldCoordinate)
        {
            return worldCoordinate * unitToPixel;
        }

        public static float screenToWorld(float screenCoordinates)
        {
            return screenCoordinates * pixelToUnit;
        }
    }
}
