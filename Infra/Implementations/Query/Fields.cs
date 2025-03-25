using System.Linq.Expressions;

namespace Infra.Implementations.Query
{
    /// <summary>
    /// Class to facilitate the selection of fields for an entity
    /// Used in partial updates by the repositories
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Fields<T> where T : class
    {
        private List<Expression<Func<T, object>>> _fields = new List<Expression<Func<T, object>>>();

        public Fields(params Expression<Func<T, object>>[] fields)
        {
            if (fields == null || fields.Count() == 0)
                throw new ArgumentException("Value is empty or null");

            foreach (var field in fields)
            {
                if (!_fields.Any(x => IsEquals(x, field)))
                    _fields.Add(field);
            }
        }

        public void AddField(Expression<Func<T, object>> field)
        {
            if (field == null)
                throw new ArgumentException("Value is null");

            if (!_fields.Any(x => IsEquals(x, field)))
                _fields.Add(field);
        }

        public void RemoveField(Expression<Func<T, object>> field)
        {
            if (field == null)
                throw new ArgumentException("Value is null");

            _fields.RemoveAll(x => IsEquals(x, field));
        }

        public void AddAllFields()
        {
            AddAllFieldsExcept();
        }

        public void AddAllFieldsExcept(params Expression<Func<T, object>>[] ignoreFields)
        {
            if (ignoreFields == null)
                throw new ArgumentException("Value is null");

            var allProperties = typeof(T).GetProperties();

            foreach (var property in allProperties)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var propertyExpression = Expression.Property(parameter, property);
                var lambdaExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(propertyExpression, typeof(object)), parameter);

                bool isEqual = ignoreFields.Any(x => IsEquals(x, lambdaExpression));

                 if (!_fields.Any(x => IsEquals(x, lambdaExpression)) && !isEqual)
                    _fields.Add(lambdaExpression);
            }
        }

        public void RemoveAllFields()
        {
            _fields.Clear();
        }

        public bool ContainsField(Expression<Func<T, object>> field)
        {
            if (field == null)
                return false;

            return _fields.Any(x => IsEquals(x, field));
        }

        public int Count()
        {
            return _fields.Count();
        }

        public List<Expression<Func<T, object>>> GetFields()
        {
            return _fields;
        }

        public IEnumerable<string> GetNames()
        {
            foreach (var field in _fields)
            {
                yield return GetName(field);
            }
        }

        static private bool IsEquals(Expression<Func<T, object>> left, Expression<Func<T, object>> right)
        {
            return GetName(left) == GetName(right);
        }

        static private string GetName(Expression<Func<T, object>> field)
        {
            if (field.Body.NodeType == ExpressionType.Convert)
            {
                var operand = ((UnaryExpression)field.Body).Operand;
                var member = (MemberExpression)operand;

                return member.Member.Name;
            }
            else
            {
                var expression = (MemberExpression)field.Body;

                return expression.Member.Name;
            }
        }
    }
}