using AutoMapper.Internal;
using BusinessLogicLayer.Entities.Attributes;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace BusinessLogicLayer.Helpers
{
    public static class UpdateHelper<TSource, TDestination>
    {
        private static Action<TSource, TDestination> _replaceOnlyNotNullProperties;

        static UpdateHelper()
        {
            var sourceEntityParameter = Expression.Parameter(typeof(TSource), "source");
            var destinationEntityParameter = Expression.Parameter(typeof(TDestination), "delivery");

            GetSameProperties(
                out IDictionary<string, PropertyInfo> sourcePropertyDictionary,
                out IDictionary<string, PropertyInfo> destinationPropertyDictionary);

            var allowNullProperties = typeof(TSource)
                .GetProperties()
                .Where(pi => pi.GetCustomAttribute<AllowUpdateNullAttribute>() is not null)
                .AsEnumerable();

            var propertyAssignmentBlocks = new Expression[destinationPropertyDictionary.Count()];

            var index = 0;
            foreach (var propertyInfoKeyValue in destinationPropertyDictionary)
            {
                propertyAssignmentBlocks[index++] = GetPropertyAssignment(
                    sourceEntityParameter,
                    sourcePropertyDictionary[propertyInfoKeyValue.Key],
                    destinationEntityParameter,
                    propertyInfoKeyValue.Value,
                    allowNullProperties);
            }

            var blockOfAssignments = Expression.Block(propertyAssignmentBlocks);

            _replaceOnlyNotNullProperties = Expression.Lambda<Action<TSource, TDestination>>(blockOfAssignments, sourceEntityParameter, destinationEntityParameter)
                .Compile();
        }

        public static void ReplaceUpdatedProperties(TSource source, TDestination delivery)
        {
            _replaceOnlyNotNullProperties(source, delivery);
        }

        private static void GetSameProperties(out IDictionary<string, PropertyInfo> sourceProperties, out IDictionary<string, PropertyInfo> destinationProperties)
        {
            var propertiesToReplaceInDestination = typeof(TDestination).GetProperties().AsEnumerable();
            var propertiesToReplaceInSource = typeof(TSource).GetProperties().AsEnumerable();

            destinationProperties = propertiesToReplaceInDestination
                .Where(propertyInfo => propertiesToReplaceInSource.Any(
                    pis => 
                    pis.Name == propertyInfo.Name 
                    && 
                    (pis.PropertyType.IsNullableType() 
                    ? pis.PropertyType.GenericTypeArguments.First()
                    : pis.PropertyType)
                    == 
                    (propertyInfo.PropertyType.IsNullableType()
                    ? propertyInfo.PropertyType.GenericTypeArguments.First()
                    : propertyInfo.PropertyType)))
                .ToDictionary(propertyInfo => propertyInfo.Name);
            sourceProperties = propertiesToReplaceInSource
                .Where(propertyInfo => propertiesToReplaceInDestination.Any(
                    pid =>
                    pid.Name == propertyInfo.Name
                    &&
                    (pid.PropertyType.IsNullableType()
                    ? pid.PropertyType.GenericTypeArguments.First()
                    : pid.PropertyType)
                    ==
                    (propertyInfo.PropertyType.IsNullableType()
                    ? propertyInfo.PropertyType.GenericTypeArguments.First()
                    : propertyInfo.PropertyType)
                    ))
                .ToDictionary(propertyInfo => propertyInfo.Name);
        }

        private static Expression GetPropertyAssignment(
            Expression sourceEntityExpression,
            PropertyInfo sourceProperty,
            Expression destinationEntityExpression,
            PropertyInfo destinationProperty,
            IEnumerable<PropertyInfo> allowUpdateNullProperties)
        {
            var deliveryPropertyExpression = Expression.Property(destinationEntityExpression, destinationProperty);
            var sourcePropertyExpression = Expression.Property(sourceEntityExpression, sourceProperty);
            var castSourceToDeliveryTypeExpression = Expression.Convert(sourcePropertyExpression, destinationProperty.PropertyType);
            var deliveryPropertyAssignment = (Expression) Expression.Assign(deliveryPropertyExpression, castSourceToDeliveryTypeExpression);
            if (
                !allowUpdateNullProperties.Contains(sourceProperty)
                &&
                (sourceProperty.PropertyType.IsNullableType() || !sourceProperty.PropertyType.IsValueType))
            {
                var testExpression = Expression.NotEqual(sourcePropertyExpression, Expression.Constant(null));
                deliveryPropertyAssignment = Expression.IfThen(testExpression, deliveryPropertyAssignment);
            }
            return deliveryPropertyAssignment;
        }
    }
}
