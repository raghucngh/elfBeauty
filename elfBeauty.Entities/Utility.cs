namespace elfBeauty.Core.Entities
{
    public class Utility
    {
        /// <summary>
        /// Determines the great-circle distance between two points on a sphere given their longitudes and latitudes
        /// </summary>
        public double Haversine(double lat1, double long1, double lat2, double long2)
        {
            const double earthRadius = 6371;

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(long2 - long1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return earthRadius * c;
        }

        private double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
