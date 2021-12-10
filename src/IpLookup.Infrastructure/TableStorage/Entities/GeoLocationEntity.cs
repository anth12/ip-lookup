using Azure;
using Azure.Data.Tables;
using System;

namespace IpLookup.Infrastructure.TableStorage.Entities
{
    internal class GeoLocationEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string Continent { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public double TimezoneOffset { get; set; }


        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
