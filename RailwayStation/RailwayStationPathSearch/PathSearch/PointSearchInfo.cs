using RailwayStationSchema.Data;

namespace RailwayStationPathSearch.PathSearch
{
    internal class PointSearchInfo
    {
        public PointData Point { get; }
        public double Weight { get; set; } = int.MaxValue;
        public bool WasProcessed { get; set; } = false;
        public PointSearchInfo Previous { get; set; } = null;

        public PointSearchInfo(PointData point) {
            Point = point;
        }
    }
}
