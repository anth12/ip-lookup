
namespace IpLookup.Domain
{
    public class Location
    {
        public Location(string city, string country, string continent, GeoCoordinate coordinate, decimal timezoneOffset)
        {
            City = city;
            Country = country;
            Continent = continent;
            Coordinate = coordinate;
            TimezoneOffset = timezoneOffset;
        }

        public string City { get; }
        public string Country { get; }
        public string Continent { get; }

        public GeoCoordinate Coordinate { get; }

        public decimal TimezoneOffset { get; }
    }
}
