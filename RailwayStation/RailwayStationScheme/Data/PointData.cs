using RailwayStationSchema.Utils;

namespace RailwayStationSchema.Data
{
    /// <summary>
    /// Представляет сущность "Точка"
    /// </summary>
    public class PointData
    {
        public PointD Coordinates { get; }
        public string Name { get; }

        public PointData(PointD coordinates, string name) {
            Coordinates = coordinates;
            Name = name;
        }
    }
}
