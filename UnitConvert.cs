using Microsoft.Xna.Framework;

namespace TheBondOfStone
{
    class UnitConvert
    {
        public const float unitToPixel = 16f;
        public const float pixelToUnit = 1 / unitToPixel;

        public static Vector2 ToScreen(Vector2 worldCoordinates)
        {
            return worldCoordinates * unitToPixel;
        }

        public static Vector2 ToWorld(Vector2 screenCoordinates)
        {
            return screenCoordinates * pixelToUnit;
        }
    }
}
