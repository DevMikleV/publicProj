using System.Diagnostics.CodeAnalysis;

namespace RailwayStationSchema.Utils
{
    public struct PointD
    {
        public double X { get; }
        public double Y { get; }

        public PointD(double x, double y) {
            X = x;
            Y = y;
        }

        public static bool operator ==(PointD p1, PointD p2) => p1.X == p2.X && p1.Y == p2.Y;
        public static bool operator !=(PointD p1, PointD p2) => p1.X != p2.X || p1.Y != p2.Y;

        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is PointD && (PointD) obj == this;
        public override readonly int GetHashCode() => HashCode.Combine(X, Y);

        public static PointD operator -(PointD p1, PointD p2) => new PointD(p1.X - p2.X, p1.Y - p2.Y);
    }
}
