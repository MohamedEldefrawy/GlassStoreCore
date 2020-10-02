using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.APIs.Filters;
using Microsoft.AspNetCore.WebUtilities;

namespace GlassStoreCore.Services.UriService
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }


        public Uri GetPageUri(PaginationFilter paginationFilter, string route)
        {
            var _endPointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(_endPointUri.ToString(), "pageNumber", paginationFilter.PageNumber.ToString());
            modifiedUri =
                QueryHelpers.AddQueryString(modifiedUri.ToString(), "pageSize", paginationFilter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}
