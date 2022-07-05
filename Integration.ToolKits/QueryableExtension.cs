using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.ToolKits
{
    public static class QueryableExtension
    {
        public static IQueryable<T> ToPage<T>(this IQueryable<T> queryable, PagedQuery pagedQuery)
        {
            var skip = Math.Max((pagedQuery.Page - 1) * pagedQuery.Size, 0);
            return queryable.Skip(skip).Take(Math.Max(pagedQuery.Size, 0));
        }
    }
}
