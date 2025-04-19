using LinqKit;
using System.Linq.Expressions;

namespace Infra.Implementations.Query
{
    /// <summary>
    /// Class to facilitate the manipulation of filter/queries expressions
    /// Really useful when joining filters/expressions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Filter<T> where T : class, new()
    {
        private ExpressionStarter<T> Predicate { get; set; }

        public Filter(Expression<Func<T, bool>> filter = null)
        {
            Predicate = filter != null 
                ? PredicateBuilder.New(filter) 
                : null;
        }

        public void And(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
                return;

            if (Predicate == null)
                Predicate = PredicateBuilder.New(filter);
            else 
                Predicate = Predicate.And(filter);
        }

        public void Or(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
                return;

            if (Predicate == null)
                Predicate = PredicateBuilder.New(filter);
            else
                Predicate = Predicate.Or(filter);
        }

        public void And(Filter<T> filter)
        {
            if (filter == null)
                return;

            if (Predicate == null)
                Predicate = PredicateBuilder.New(filter.GetExpression());
            else
                Predicate = Predicate.And(filter.GetExpression());
        }

        public void Or(Filter<T> filter)
        {
            if (filter == null)
                return;

            if (Predicate == null)
                Predicate = PredicateBuilder.New(filter.GetExpression());
            else
                Predicate = Predicate.Or(filter.GetExpression());
        }

        public Expression<Func<T, bool>> GetExpression()
        {   
            if(Predicate == null)
            {
                var defaultPredicate = PredicateBuilder.New<T>(x => true);
                return (Expression<Func<T, bool>>)defaultPredicate?.Expand();
            }

            return (Expression<Func<T, bool>>)Predicate?.Expand();
        }

        public override string ToString()
        {
            return GetExpression()?.ToString();
        }
    }
}
