﻿using Abner.Domain.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Abner.Infrastructure.Core.Extensions
{
    public static class EFCoreContextExtension
    {
        public static void EnableGloableSoftDeleteQueryFilter(this ModelBuilder builder)
        {
            var softDeletes = builder.Model.GetEntityTypes()
                .Where(t => t.ClrType.IsAssignableTo(typeof(ISoftDelete)));

            foreach (var entityType in softDeletes)
            {
                // 1. Add the IsDeleted property
                //entityType.GetOrAddProperty("IsDeleted", typeof(bool));

                // 2. Create the query filter
                var parameter = Expression.Parameter(entityType.ClrType);

                // EF.Property<bool>(post, "IsDeleted")
                var propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
                var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant(nameof(ISoftDelete.IsDeleted)));

                // EF.Property<bool>(post, "IsDeleted") == false
                BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));

                // post => EF.Property<bool>(post, "IsDeleted") == false
                var lambda = Expression.Lambda(compareExpression, parameter);

                builder.Entity(entityType.ClrType).HasQueryFilter(lambda);

                //var isDeletedProperty = entityType.FindProperty(nameof(ISoftDelete.IsDeleted));
                //var parameter = Expression.Parameter(entityType.ClrType, "p");
                //var filter = Expression.Lambda(Expression.Not(Expression.Property(parameter, isDeletedProperty.PropertyInfo)), parameter);
                //entityType.SetQueryFilter(filter);
            }
        }
    }
}
