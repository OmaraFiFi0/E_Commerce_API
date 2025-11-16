using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presistence
{
    internal static class SpecificationsEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> entryPoint, ISpecifications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var Query = entryPoint;
            if(specifications.Criteria is not null)
            {
                Query = Query.Where(specifications.Criteria);
            }
            if(specifications is not null)
            {
                if(specifications.IncludeExpression.Any() && specifications.IncludeExpression is not null)
                {
                    Query = specifications.IncludeExpression
                        .Aggregate(Query, (CurrentQuery, includeExp) => CurrentQuery.Include(includeExp));
                }
            
                if (specifications.OrderBy is not null)
                {
                    Query=Query.OrderBy(specifications.OrderBy);
                }
                if (specifications.OrderByDesc is not null)
                {
                    Query = Query.OrderByDescending(specifications.OrderByDesc);
                }
            
            }



            return Query;
        }
    }
}
