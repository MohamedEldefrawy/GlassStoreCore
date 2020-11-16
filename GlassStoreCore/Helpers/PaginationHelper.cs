using System;
using System.Collections.Generic;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.Data.Response;
using GlassStoreCore.Services.PaginationUowService;

namespace GlassStoreCore.Helpers
{
    public static class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedResponse<T>(List<T> pagedData,
            PaginationFilter paginationFilter, int totalRecords, IPaginationUow uriService, string route)
        {
            var response = new PagedResponse<List<T>>(pagedData, paginationFilter.PageNumber, paginationFilter.PageSize);
            var totalPages = totalRecords / (double)paginationFilter.PageSize;
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            response.NextPage = paginationFilter.PageNumber >= 1 && paginationFilter.PageNumber < roundedTotalPages
                ? uriService
                    .GetPageUri(new PaginationFilter(paginationFilter.PageNumber + 1, paginationFilter.PageSize),
                                route) : null;
            response.PreviousPage =
                paginationFilter.PageNumber - 1 >= 1 && paginationFilter.PageNumber <= roundedTotalPages
                    ? uriService
                        .GetPageUri(new PaginationFilter(paginationFilter.PageNumber - 1, paginationFilter.PageSize),
                                    route) : null;
            response.FirstPage = uriService.GetPageUri(new PaginationFilter(1, paginationFilter.PageSize), route);
            response.LastPage =
                uriService.GetPageUri(new PaginationFilter(roundedTotalPages, paginationFilter.PageSize), route);
            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;
            return response;
        }
    }
}
