using RailwayStationFilling.Utils;
using RailwayStationSchema.Data;

namespace RailwayStationFilling.StationProcessing
{
    internal interface IStationProcessor
    {
        IEnumerable<PointData> GetParkFillPoints(ParkData park);
    }

    internal class StationParkFilling : IStationProcessor
    {
        class RotatePointsComparer : IComparer<PointData>
        {
            readonly PointData _a;

            public RotatePointsComparer(PointData start) {
                _a = start;
            }

            public int Compare(PointData x, PointData y) {
                return Math.Sign(MathUtils.VMultiplyZ(_a.Coordinates, x.Coordinates, y.Coordinates));
            }
        }

        public IEnumerable<PointData> FindMinConvexShell(IEnumerable<PointData> points) {
            var orderedByCoordsPoints = points.OrderBy(p => p.Coordinates.Y).ThenBy(p => p.Coordinates.X);
            var firstPoint = orderedByCoordsPoints.First();

            var orderedByRotatePoints = orderedByCoordsPoints.Skip(1).Order(new RotatePointsComparer(firstPoint)).ToList();

            Stack<PointData> sidePoints = new Stack<PointData>();
            sidePoints.Push(firstPoint);
            sidePoints.Push(orderedByRotatePoints[0]);

            for (int i = 1; i < orderedByRotatePoints.Count; i++) {
                while (sidePoints.Count > 1 &&
                    MathUtils.VMultiplyZ(sidePoints.ElementAt(1).Coordinates,
                        sidePoints.ElementAt(0).Coordinates,
                        orderedByRotatePoints[i].Coordinates) > 0)
                    sidePoints.Pop();
                sidePoints.Push(orderedByRotatePoints[i]);
            }
            return sidePoints;
        }

        public IEnumerable<PointData> GetParkFillPoints(ParkData park) {
            var parkPoints = park.Paths.SelectMany(p => p.Segments.SelectMany(s => new[] { s.Start, s.End })).Distinct();
            return FindMinConvexShell(parkPoints);
        }
    }
}
