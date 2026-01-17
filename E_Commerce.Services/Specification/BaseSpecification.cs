using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specification
{
    public abstract class BaseSpecification<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public BaseSpecification(Expression<Func<TEntity,bool>> criteriaExp)
        {
            Criteria = criteriaExp;
        }


        #region Filteration
        public Expression<Func<TEntity, bool>> Criteria { get; }

        #endregion


        #region InClude
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];
        protected void AddInclude(Expression<Func<TEntity, object>> Include)
        {
            IncludeExpression.Add(Include);
        }

        #endregion

        #region Ordering
        public Expression<Func<TEntity, object>> OrderBy { private set; get; }
        public Expression<Func<TEntity, object>> OrderByDesc { private set; get; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> OrderByExp)
        {
            OrderBy = OrderByExp;
        }
        protected void AddOrderDesc(Expression<Func<TEntity, object>> OrderByDescExp)
        {
            OrderByDesc = OrderByDescExp;
        }
        #endregion

        #region Pagination

        public int Skip { private set; get; }

        public int Take { private set; get; }

        public bool IsPaginated { private set; get; }


        protected void ApplyPaginated(int PageSize,int PageIndex)
        {
           IsPaginated = true;

            Skip = (PageIndex-1)* PageSize;

            Take = PageSize; 
        }
        #endregion

    }
}
