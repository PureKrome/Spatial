namespace Spatial.Services
{
    public class Coordinate
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public override string ToString()
        {
            return string.Format("Lat:{0} Long:{1}", Latitude, Longitude);
        }
    }
}