using Azure;
using Azure.Data.Tables;
using IpLookup.Domain;
using IpLookup.Infrastructure.TableStorage.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace IpLookup.Infrastructure.TableStorage
{
    internal class GeoLocationRepository : IGeoLocationRepository
    {
}
