using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RecrutariBlazorWasm.Shared.Helpers;
using System;

namespace RecrutariBlazorWasm.Server.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int pageNumber, int pageSize, int totalItems)
        {
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var paginationHeader = new MetaData() { PageNumber = pageNumber, TotalPages = totalPages, PageSize = pageSize, TotalItems = totalItems, };

            //var options = new JsonSerializerOptions
            //{
            //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            //};

            //response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
            //response.Headers.Add("Access-Control-Expose-Headers", "Pagination");

            response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

        }
    }
}
