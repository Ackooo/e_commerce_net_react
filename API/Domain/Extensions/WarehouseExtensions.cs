namespace Domain.Extensions;

using Domain.DTOs.Wms;
using Domain.Entities.Wms;

public static class WarehouseExtensions
{
    #region Queryable

    public static IQueryable<Warehouse> Sort(this IQueryable<Warehouse> query, string? orderBy)
    {
        if(string.IsNullOrWhiteSpace(orderBy)) return query.OrderBy(p => p.Name);

        query = orderBy switch
        {
            "latitude" => query.OrderBy(p => p.Latitude),
            "latitudeDesc" => query.OrderByDescending(p => p.Latitude),
            "longitude" => query.OrderBy(p => p.Longitude),
            "longitudeDesc" => query.OrderByDescending(p => p.Longitude),
            "location" => query.OrderBy(p => p.Location),
            "locationDesc" => query.OrderByDescending(p => p.Location),
            _ => query.OrderBy(p => p.Name)
        };

        return query;
    }

    public static IQueryable<Warehouse> Search(this IQueryable<Warehouse> query, string? searchTerm)
    {
        if(string.IsNullOrEmpty(searchTerm)) return query;

        var searchTermLowerCase = searchTerm.Trim().ToLower();
        return query.Where(p => p.Name.Contains(
            searchTermLowerCase, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IQueryable<Warehouse> Filter(this IQueryable<Warehouse> query, string? locations)
    {
        if(string.IsNullOrEmpty(locations)) return query;

        var locationList = locations.ToLower().Split(",").ToList();
        query = query.Where(p => locationList.Count == 0 || locationList.Contains(p.Location.ToLower()));

        return query;
    }

    #endregion

    #region Mapper

    public static Warehouse MapToWarehouse(this CreateWarehouseDto dto)
    {
        return new Warehouse
        {
            Name = dto.Name,
            Description = dto.Description,
            Location = dto.Location,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };
    }

    public static WarehouseDto MapToDto(this Warehouse warehouse)
    {
        return new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Description = warehouse.Description,
            Location = warehouse.Location,
            Latitude = warehouse.Latitude,
            Longitude = warehouse.Longitude
        };
    }

    #endregion

}
