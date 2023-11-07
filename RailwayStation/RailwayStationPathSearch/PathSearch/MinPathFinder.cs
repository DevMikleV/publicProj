using RailwayStationSchema.Data;
using RailwayStationSchema.Utils;

namespace RailwayStationPathSearch.PathSearch
{
    internal interface IStationPathFinder
    {
        IEnumerable<SegmentData> FingShortestPath(SegmentData start, SegmentData end, StationSchemaData station);
    }

    internal class ShortestPathFinder : IStationPathFinder
    {
        SegmentData _startSegment;
        SegmentData _endSegment;
        StationSchemaData _station;
        List<PointSearchInfo> _searchInfo = new List<PointSearchInfo>();

        void CalculateStartEndPoints(out PointData start, out PointData end) {
            start = new PointData(new PointD((_startSegment.Start.Coordinates.X + _startSegment.End.Coordinates.X) / 2.0,
                      (_startSegment.Start.Coordinates.Y + _startSegment.End.Coordinates.Y) / 2.0),
                      "Начало");
            end = new PointData(new PointD((_endSegment.Start.Coordinates.X + _endSegment.End.Coordinates.X) / 2.0,
                                  (_endSegment.Start.Coordinates.Y + _endSegment.End.Coordinates.Y) / 2.0),
                                  "Конец");
        }
        IEnumerable<SegmentData> CalculateSegments(PointData startPoint, PointData endPoint) {
            SegmentData startSegment1 = new SegmentData(_startSegment.Start, startPoint, _startSegment.Name + "_1");
            SegmentData startSegment2 = new SegmentData(startPoint, _startSegment.End, _startSegment.Name + "_2");
            SegmentData endSegment1 = new SegmentData(_endSegment.Start, endPoint, _endSegment.Name + "_1");
            SegmentData endSegment2 = new SegmentData(endPoint, _endSegment.End, _endSegment.Name + "_2");

            var segments = _station.Segments.Where(s => s != _startSegment && s != _endSegment);
            return segments.Concat(new[] {
                startSegment1, startSegment2, endSegment1, endSegment2
            });
        }
        void InitSearchInfo(PointData start) {
            foreach (var info in _searchInfo) {
                info.Weight = info.Point == start ? 0 : int.MaxValue;
                info.WasProcessed = false;
                info.Previous = null;
            }
        }
        PointSearchInfo GetInfo(PointData p) {
            return _searchInfo.Find(x => x.Point == p);
        }
        PointSearchInfo GetNextInfo() {
            return _searchInfo.Where(x => !x.WasProcessed).OrderBy(x => x.Weight).FirstOrDefault();
        }
        IEnumerable<SegmentData> GetSegmentsFromPoint(PointData point, IEnumerable<SegmentData> segments) {
            return segments.Where(s => s.Start == point || s.End == point);
        }
        void CalculateWeights(PointData start, IEnumerable<SegmentData> segments) {
            InitSearchInfo(start);
            PointSearchInfo currentInfo = GetInfo(start);
            while (currentInfo != null && currentInfo.Weight != int.MaxValue) {
                currentInfo.WasProcessed = true;

                var segmentsFrom = GetSegmentsFromPoint(currentInfo.Point, segments);

                foreach (var segment in segmentsFrom) {
                    var pointTo = segment.Start == currentInfo.Point ? segment.End : segment.Start;

                    var info = GetInfo(pointTo);
                    if (info != null) {
                        double newWeight = currentInfo.Weight + segment.Length;
                        if (newWeight < info.Weight) {
                            info.Weight = newWeight;
                            info.Previous = currentInfo;
                        }
                    }
                }
                currentInfo = GetNextInfo();
            }
        }
        List<PointData> GetMinPath(PointData start, PointData end) {
            var info = GetInfo(end);
            if (!info.WasProcessed)
                return null;
            var startInfo = GetInfo(start);
            var minPath = new List<PointData>();
            while (info != null && info != startInfo) {
                minPath.Insert(0, info.Point);
                info = info.Previous;
            }
            if (info == null)
                return null;
            minPath.Insert(0, info.Point);
            return minPath;
        }
        List<PointData> GetShortestPath(PointData start, PointData end, IEnumerable<SegmentData> segments) {
            CalculateWeights(start, segments);
            return GetMinPath(start, end);
        }
        IEnumerable<SegmentData> GetPathBySegments(List<PointData> pathByPoints) {
            List<SegmentData> pathSegments = new List<SegmentData>();
            for (int i = 0; i < pathByPoints.Count - 1; i++) {
                var point1 = pathByPoints[i];
                var point2 = pathByPoints[i + 1];
                var neighbourSegments = _station.Segments.Where(s => s.Start == point1 || s.End == point1);
                var segment = neighbourSegments.Single(s => s.Start == point1 && s.End == point2 ||
                                                            s.Start == point2 && s.End == point1);
                pathSegments.Add(segment);
            }
            return pathSegments;
        }

        public IEnumerable<SegmentData> FingShortestPath(SegmentData start, SegmentData end, StationSchemaData station) {
            _startSegment = start;
            _endSegment = end;
            _station = station;

            CalculateStartEndPoints(out PointData startPoint, out PointData endPoint);
            var segments = CalculateSegments(startPoint, endPoint);

            _searchInfo.AddRange(_station.Points.Select(x => new PointSearchInfo(x)).Concat(new[] {
                new PointSearchInfo(startPoint),
                new PointSearchInfo(endPoint)
            }));

            var pathByPoints = GetShortestPath(startPoint, endPoint, segments);
            if (pathByPoints == null)
                return null;
            var pathBySegments = GetPathBySegments(pathByPoints.Where(p => p != startPoint && p != endPoint).ToList());

            return pathBySegments;
        }
    }
}
