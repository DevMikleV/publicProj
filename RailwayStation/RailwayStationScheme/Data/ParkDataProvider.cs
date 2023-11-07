using RailwayStationSchema.Utils;
using System.Data;

namespace RailwayStationSchema.Data
{
    public interface IParkDataProvider
    {
        Task<IEnumerable<StationSchemaData>> LoadStations();
    }

    public class ParkDataProvider : IParkDataProvider
    {
        readonly IRailwayStationDataSource _dataSource;

        public ParkDataProvider(IRailwayStationDataSource dataSource) {
            _dataSource = dataSource;
        }

        IList<PointData> LoadPoints() {
            return _dataSource.PointsStorage.Select(p => new PointData(new PointD(p.X, p.Y), p.Name)).ToList();
        }
        IList<SegmentData> LoadSegments(IList<PointData> points) {
            return _dataSource.SegmentsStorage.Select(s => new SegmentData(points[s.Start], points[s.End], s.Name)).ToList();
        }
        IList<PathData> LoadPaths(IList<SegmentData> segments) {
            return _dataSource.PathsStorage.Select(p => {
                var path = new PathData(p.Name);
                p.Segments.ToList().ForEach(s => path.AddSegment(segments[s]));
                return path;
            }).ToList();
        }
        IList<ParkData> LoadParks(IList<PathData> paths) {
            return _dataSource.ParksStorage.Select(p => new ParkData(p.Paths.Select(p => paths[p]), p.Name)).ToList();
        }
        IEnumerable<StationSchemaData> LoadSchema(IList<PointData> points, IList<SegmentData> segments, IList<ParkData> parks) {
            return _dataSource.SchemaStorage.Select(s => {
                var schema = new StationSchemaData(s.Points.Select(p => points[p]), s.Name);
                s.Segments.ToList().ForEach(s => schema.AddSegment(segments[s]));
                s.Parks.ToList().ForEach(p => schema.AddPark(parks[p]));
                return schema;
            });
        }

        public async Task<IEnumerable<StationSchemaData>> LoadStations() {
            return await Task.Run(() => {
                var points = LoadPoints();
                var segments = LoadSegments(points);
                var paths = LoadPaths(segments);
                var parks = LoadParks(paths);
                return LoadSchema(points, segments, parks);
            });
        }
    }
}