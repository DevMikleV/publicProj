
namespace RailwayStationSchema.Data
{
    /// <summary>
    /// Представляет сущность "Схема станции"
    /// </summary>
    public class StationSchemaData
    {
        HashSet<PointData> _points;
        public IEnumerable<PointData> Points { get => _points; }

        HashSet<SegmentData> _segments;
        public IEnumerable<SegmentData> Segments { get => _segments; }

        HashSet<ParkData> _parks;
        public IEnumerable<ParkData> Parks { get => _parks; }

        public string Name { get; }

        public StationSchemaData(IEnumerable<PointData> points, string name) {
            _points = new HashSet<PointData>(points);
            _segments = new HashSet<SegmentData>();
            _parks = new HashSet<ParkData>();
            Name = name;
        }

        public bool AddSegment(SegmentData segment) {
            if (!_segments.Contains(segment) && _points.Contains(segment.Start) && _points.Contains(segment.End)) {
                _segments.Add(segment);
                return true;
            }
            return false;
        }
        public bool AddPark(ParkData park) {
            if (!_parks.Contains(park)) {
                foreach (var path in park.Paths) {
                    foreach (var segment in path.Segments) {
                        if (!_segments.Contains(segment))
                            return false;
                    }
                }
                _parks.Add(park);
                return true;
            }
            return false;
        }
    }
}
