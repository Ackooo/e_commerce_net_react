namespace Domain.Extensions;

using Domain.RequestHelpers;

using Microsoft.AspNetCore.Http;
using System.Text.Json;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, MetaData metaData)
    {
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        response.Headers.Add("Pagination", JsonSerializer.Serialize(metaData, options));
        //enable header for client (cors)
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
}