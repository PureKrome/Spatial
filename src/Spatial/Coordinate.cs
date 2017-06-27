namespace WorldDomination.Spatial
{
    public class Coordinate
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public override string ToString()
        {
            return $"Lat:{Latitude} Long:{Longitude}";
        }
    }
}