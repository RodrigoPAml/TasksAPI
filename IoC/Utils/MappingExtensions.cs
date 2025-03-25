using AutoMapper;
using Domain.Base;
using Infra.Base;
using System.Reflection;

namespace IoC.Utils
{
    /// <summary>
    /// Mapping extensions to construct entities with
    /// private default constructors and others
    /// </summary>
    public static class MappingExtensions
    {
        public static IMappingExpression<S, D> BypassConstructor<S, D>(this IMappingExpression<S, D> exp)
            where S : BaseDbEntity
            where D : BaseEntity
        {
            exp.ConstructUsing(src => InvokeConstructor<S, D>());

            return exp;
        }

        public static IMappingExpression<S, D> LinkReference<S, D>(this IMappingExpression<S, D> exp) 
            where S : BaseEntity 
            where D : BaseDbEntity
        {
            return exp.AfterMap((src, dest) =>
            {
                if (dest != null && src != null)
                {
                    dest.DomainRef = src;
                }
            });
        }

        private static D InvokeConstructor<S,D>() where D : class
        {
            var constructor = typeof(D).GetConstructor(
                  BindingFlags.NonPublic | BindingFlags.Instance,
                  null,
                  Type.EmptyTypes,
                  null
            );

            if (constructor != null)
            {
                var obj = constructor.Invoke(null);

                return obj as D;
            }

            throw new InvalidOperationException($"Default private constructor not found for {typeof(D).FullName}");
        }
    }
}