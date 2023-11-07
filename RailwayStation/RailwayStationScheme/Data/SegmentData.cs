
namespace RailwayStationSchema.Data
{
    /// <summary>
    /// Представляет сущность "Участок"
    /// </summary>
    public class SegmentData
    {
        public PointData Start { get; set; }
        public PointData End { get; set; }

        double _length = -1;
        public double Length {
            get {
                if (_length >= 0)
                    return _length;
                var delta = End.Coordinates - Start.Coordinates;
                double x = Math.Abs(delta.X);
                double y = Math.Abs(delta.Y);
                return _length = Math.Sqrt(x * x + y * y);
            }
        }
        public string Name { get; }

        public SegmentData(PointData start, PointData end, string name) {
            Start = start;
            End = end;
            Name = name;
        }

        public bool IsConnectedWith(SegmentData segment) {
            return Start.Coordinates == segment.Start.Coordinates
                || Start.Coordinates == segment.End.Coordinates
                || End.Coordinates == segment.Start.Coordinates
                || End.Coordinates == segment.End.Coordinates;
        }
    }
}
