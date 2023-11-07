using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayStationSchema.Data
{
    /// <summary>
    /// Представляет сущность "Парк"
    /// </summary>
    public class ParkData
    {
        readonly List<PathData> _paths;
        public IEnumerable<PathData> Paths { get => _paths; }

        public string Name { get; }

        public ParkData(IEnumerable<PathData> paths, string name) {
            _paths = new List<PathData>(paths);
            Name = name;
        }
    }
}
