using RailwayStationSchema.Utils;

namespace RailwayStationFilling.Utils
{
    internal static class MathUtils
    {
        public static double VMultiplyZ(PointD a, PointD b, PointD c) {
            return (b.X - a.X) * (c.Y - b.Y) - (b.Y - a.Y) * (c.X - b.X);
        }
    }
}
