namespace RailwayStationSchema.Data
{
    /// <summary>
    /// Представляет сущность "Путь"
    /// </summary>
    public class PathData
    {
        readonly HashSet<SegmentData> _segments;
        public IEnumerable<SegmentData> Segments { get => _segments; }
        public string Name { get; }

        public PathData(string name) {
            _segments = new HashSet<SegmentData>();
            Name = name;
        }

        public bool AddSegment(SegmentData segment) {
            if (!_segments.Contains(segment) && (_segments.Count == 0 || _segments.Any(s => s.IsConnectedWith(segment)))) {
                _segments.Add(segment);
                return true;
            }
            return false;
        }
    }
}
