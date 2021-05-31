using System;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using GlassStoreCore.Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;

namespace GlassStoreCore.Services.PaginationUowService
{
    public class PaginationUow : UnitOfWork, IPaginationUow
    {
        private readonly string _baseUri;

        public PaginationUow(GlassStoreContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager,
            IServiceProvider serviceProvider)
            : base(context, userManager, signInManager, roleManager)
        {
            var accessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var request = accessor.HttpContext.Request;
            _baseUri = string.Concat(request.Scheme, "://",
                                     request.Host.ToUriComponent());
        }

        public Uri GetPageUri(PaginationFilter paginationFilter, string route)
        {
            var endPointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(endPointUri.ToString(), "pageNumber", paginationFilter.PageNumber.ToString());
            modifiedUri =
                QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationFilter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}
