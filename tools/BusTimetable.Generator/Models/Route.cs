namespace BusTimetable.Generator.Models
{
    public class Route
    {
        public string Id { get; set; }
        public string Color { get; set; }
        public Point[] Path { get; set; }

        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string BusStopId { get; set; }
            public bool IsBusStop => !string.IsNullOrEmpty(BusStopId);
        }
    }
}
