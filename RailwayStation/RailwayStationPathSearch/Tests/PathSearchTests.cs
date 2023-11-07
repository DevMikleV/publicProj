using Microsoft.VisualStudio.TestTools.UnitTesting;
using RailwayStationPathSearch.PathSearch;
using RailwayStationSchema.Data;
using RailwayStationSchema.Tests.Data;
using Unity;

namespace RailwayStationSchema.Tests
{
    [TestClass]
    public class PathSearchTests
    {
        TestDataSource _ds;
        IUnityContainer _uc;

        [TestInitialize]
        public void InitTest() {
            _ds = new TestDataSource();
            _uc = new UnityContainer();
            _uc.RegisterInstance<IRailwayStationDataSource>(_ds);
            _uc.RegisterType<IParkDataProvider, ParkDataProvider>();
            _uc.RegisterType<IStationPathFinder, ShortestPathFinder>();
        }

        [TestMethod]
        public async Task PathExistTest() {
            _ds.AddPoint(10, 20, "Точка 1");
            _ds.AddPoint(20, 20, "Точка 2");
            _ds.AddPoint(20, 30, "Точка 3");
            _ds.AddPoint(20, 40, "Точка 4");
            _ds.AddPoint(30, 40, "Точка 5");
            _ds.AddPoint(30, 30, "Точка 6");
            _ds.AddPoint(30, 20, "Точка 7");
            _ds.AddPoint(40, 20, "Точка 8");
            _ds.AddPoint(20, 10, "Точка 9");
            _ds.AddPoint(30, 10, "Точка 10");

            _ds.AddSegment(0, 1, "Участок 1");
            _ds.AddSegment(1, 2, "Участок 2");
            _ds.AddSegment(2, 3, "Участок 3");
            _ds.AddSegment(3, 4, "Участок 4");
            _ds.AddSegment(4, 5, "Участок 5");
            _ds.AddSegment(5, 6, "Участок 6");
            _ds.AddSegment(6, 7, "Участок 7");
            _ds.AddSegment(1, 8, "Участок 8");
            _ds.AddSegment(8, 9, "Участок 9");
            _ds.AddSegment(9, 6, "Участок 10");

            _ds.AddPath(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, "Путь 1");
            _ds.AddPark(new[] { 0 }, "Парк 1");
            _ds.AddStation(
                new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                new[] { 0 },
                "Станция 1");

            var dp = _uc.Resolve<IParkDataProvider>();
            Assert.IsNotNull(dp);

            var stations = await dp.LoadStations();
            Assert.IsNotNull(stations);
            Assert.AreEqual(1, stations.Count());
            var station = stations.First();
            Assert.IsNotNull(station);

            var segments = station.Segments.ToList();
            IStationPathFinder pathFinder = _uc.Resolve<IStationPathFinder>();
            var path = pathFinder.FingShortestPath(segments[0], segments[6], station).ToList();

            Assert.AreEqual(3, path.Count);
            Assert.AreEqual("Участок 8", path[0].Name);
            Assert.AreEqual("Участок 9", path[1].Name);
            Assert.AreEqual("Участок 10", path[2].Name);
        }

        [TestMethod]
        public async Task PathNotExistTest() {
            _ds.AddPoint(10, 20, "Точка 1");
            _ds.AddPoint(20, 20, "Точка 2");
            _ds.AddPoint(20, 30, "Точка 3");
            _ds.AddPoint(20, 40, "Точка 4");
            _ds.AddPoint(30, 40, "Точка 5");
            _ds.AddPoint(30, 30, "Точка 6");
            _ds.AddPoint(30, 20, "Точка 7");
            _ds.AddPoint(40, 20, "Точка 8");
            _ds.AddPoint(20, 10, "Точка 9");
            _ds.AddPoint(30, 10, "Точка 10");

            _ds.AddSegment(0, 1, "Участок 1");
            _ds.AddSegment(1, 2, "Участок 2");
            _ds.AddSegment(2, 3, "Участок 3");
            _ds.AddSegment(4, 5, "Участок 5");
            _ds.AddSegment(5, 6, "Участок 6");
            _ds.AddSegment(6, 7, "Участок 7");
            _ds.AddSegment(1, 8, "Участок 8");
            _ds.AddSegment(9, 6, "Участок 10");

            _ds.AddPath(new[] { 0, 1, 2, 3, 4, 5, 6, 7 }, "Путь 1");
            _ds.AddPark(new[] { 0 }, "Парк 1");
            _ds.AddStation(
                new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                new[] { 0, 1, 2, 3, 4, 5, 6, 7 },
                new[] { 0 },
                "Станция 1");

            var dp = _uc.Resolve<IParkDataProvider>();
            Assert.IsNotNull(dp);

            var stations = await dp.LoadStations();
            Assert.IsNotNull(stations);
            Assert.AreEqual(1, stations.Count());
            var station = stations.First();
            Assert.IsNotNull(station);

            var segments = station.Segments.ToList();
            IStationPathFinder pathFinder = _uc.Resolve<IStationPathFinder>();
            var path = pathFinder.FingShortestPath(segments[0], segments[5], station);
            Assert.IsNull(path);
        }
    }
}
