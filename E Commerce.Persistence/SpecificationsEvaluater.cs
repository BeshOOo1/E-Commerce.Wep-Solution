using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence
{
    public static class SpecificationsEvaluater
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> EntryPoint,
           ISpecifications<TEntity,TKey> specifications )where TEntity : BaseEntity<TKey>
        {
            var Query = EntryPoint;
            if ( specifications is not null )
            {
                if(specifications.Criteria is not null)
                {
                    Query = Query.Where(specifications.Criteria);
                }

                if(specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Any())
                {
                    Query = specifications.IncludeExpressions.Aggregate(Query
                        ,(CurrentQuery, includeExp) => CurrentQuery.Include(includeExp));
                }
                
                if(specifications.OrderBy is not null)
                {
                    Query = Query.OrderBy(specifications.OrderBy);
                }
                
                if(specifications.OrderByDescending is not null)
                {
                    Query = Query.OrderByDescending(specifications.OrderByDescending);
                }
            
                if(specifications.IsPaginated)
                {
                    Query = Query.Skip(specifications.Skip).Take(specifications.Take);
                }
            }
            return Query;
        }
    }
}
