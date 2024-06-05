using BusinessLogicLayer.Entities.Attributes;
using DataAccessLayer.Entities.Attributes;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.EntityHelpers
{
    public static class GeneralIdExtractor<T, TIdAttribute>
        where TIdAttribute : Attribute
    {
        private static Func<T, EntityIds> _getId = null!;
        private static Func<EntityIds, Expression<Func<T, bool>>> _getByIdPredicate = null!;
        private static Func<EntityIds, T> _mapEntity = null!;
        private static Func<IEnumerable<T>, Expression<Func<T, bool>>> _getFieldVariants = null!;
        private static Func<string, Expression<Func<T, object>>?> _getPropertyExpression = null!;

        static GeneralIdExtractor()
        {
            var keyProperties = typeof(T).GetProperties()
                .Where(propertyInfo => propertyInfo.GetCustomAttribute<TIdAttribute>() != null);

            if (!keyProperties.Any())
            {
                return;
            }

            _getId = GetGetIdDelegate(keyProperties);
            _getByIdPredicate = GetGetByIdPredicateDelegate(keyProperties);
            _mapEntity = GetMapEntityFromEntityIds(keyProperties);
            _getFieldVariants = GetGetFieldVariantsPredicate(keyProperties);
            _getPropertyExpression = GetGetPropertyExpression();
        }

        public static EntityIds GetId(T entity)
        {
            return _getId?.Invoke(entity) ?? new EntityIds();
        }

        public static Expression<Func<T, bool>> GetByIdPredicate(EntityIds ids)
        {
            return _getByIdPredicate?.Invoke(ids) ?? Expression.Lambda<Func<T, bool>>(Expression.Constant(true), Expression.Parameter(typeof(T)));
        }

        public static T MapEntityFromEntityIds(EntityIds ids)
        {
            return _mapEntity(ids);
        }

        public static Expression<Func<T, bool>> GetFieldVariants(IEnumerable<T> variants)
        {
            return _getFieldVariants(variants);
        }

        public static Expression<Func<T, object>>? GetPropertyExpression(string propertyName)
        {
            return _getPropertyExpression(propertyName);
        }

        private static Func<string, Expression<Func<T, object>>?> GetGetPropertyExpression()
        {
            var allProperties = typeof(T).GetProperties();
            var propertyNameParameter = Expression.Parameter(typeof(string), "propertyName");
            var expressionsForBlock = new List<Expression>();
            var lastLabel = Expression.Label(typeof(Expression<Func<T, object>>), "Last expression");
            foreach (var property in allProperties)
            {
                var internalEntityParameter = Expression.Parameter(typeof(T), "entity");
                Expression lambda;
                if (!property.PropertyType.IsClass)
                {
                    lambda = Expression.Lambda<Func<T, object>>(
                                Expression.Convert(Expression.Property(internalEntityParameter, property), typeof(object)),
                                internalEntityParameter);
                }
                else
                {
                    lambda = Expression.Lambda<Func<T, object>>(
                                Expression.Property(internalEntityParameter, property), internalEntityParameter);
                }

                expressionsForBlock.Add(
                    Expression.IfThen(
                        Expression.Equal(
                            Expression.Constant(property.Name),
                            propertyNameParameter),
                        Expression.Return(lastLabel, lambda)));
            }
            expressionsForBlock.Add(Expression.Label(lastLabel, Expression.Constant(null, typeof(Expression<Func<T, object>>))));

            var resultLambda = Expression.Lambda<Func<string, Expression<Func<T, object>>?>>(Expression.Block(expressionsForBlock), propertyNameParameter);

            return resultLambda.Compile();
        }

        private static Func<IEnumerable<T>, Expression<Func<T, bool>>> GetGetFieldVariantsPredicate(IEnumerable<PropertyInfo> keyProperties)
        {
            var method = typeof(Enumerable).GetMethods().Where(mi => mi.Name == "Any" && mi.GetParameters().Length == 2).FirstOrDefault()
                ?? throw new InvalidOperationException("Unable to get Any methods.");

            method = method.MakeGenericMethod(typeof(T));

            var variantsParameter = Expression.Parameter(typeof(IEnumerable<T>), "variants");
            var mainEntityParameter = Expression.Parameter(typeof(T), "mainEntity");
            var entityParameter = Expression.Parameter(typeof(T), "entity");
            var predicate = (Expression)null!;
            foreach (var keyProperty in keyProperties)
            {
                var mainEntityProperty = Expression.Property(mainEntityParameter, keyProperty);
                var entityProperty = Expression.Property(entityParameter, keyProperty);
                var equalityExpression = Expression.Equal(mainEntityProperty, entityProperty);

                var methodCallingExpression = Expression.Call(
                    null,
                    method,
                    variantsParameter,
                    Expression.Lambda<Func<T, bool>>(equalityExpression, entityParameter));

                predicate = predicate is null ? methodCallingExpression : Expression.AndAlso(predicate, methodCallingExpression);
            }
            var lambda = Expression.Lambda<Func<T, bool>>(predicate, mainEntityParameter);
            return Expression.Lambda<Func<IEnumerable<T>, Expression<Func<T, bool>>>>(
                lambda,
                variantsParameter)
                .Compile();
        }

        private static Func<T, EntityIds> GetGetIdDelegate(IEnumerable<PropertyInfo> keyProperties)
        {
            var entityParameter = Expression.Parameter(typeof(T), "entity");
            var resultVariable = Expression.Variable(typeof(EntityIds), "result");
            var resultAssign = Expression.Assign(resultVariable, Expression.New(typeof(EntityIds).GetConstructor(Array.Empty<Type>())!));
            var propertyValuesRetrievingExpressions = new Expression[keyProperties.Count() + 2];
            var count = 0;
            propertyValuesRetrievingExpressions[count++] = resultAssign;
            var addMethod = typeof(EntityIds).GetMethod("Add", [typeof(string), typeof(object)])
                ?? throw new InvalidOperationException("Unable to get Add method in ICollection.");
            foreach (PropertyInfo property in keyProperties)
            {
                var name = property.Name;

                propertyValuesRetrievingExpressions[count++] = Expression.Call(resultVariable, addMethod,
                    Expression.Constant(name, typeof(string)),
                    Expression.Convert(
                        Expression.Property(entityParameter, property), typeof(object)
                    ));
            }
            propertyValuesRetrievingExpressions[count++] = resultVariable;
            var getPropertyValues = Expression.Block(typeof(EntityIds), [resultVariable], propertyValuesRetrievingExpressions);

            return Expression.Lambda<Func<T, EntityIds>>(getPropertyValues, entityParameter).Compile();
        }

        private static Func<EntityIds, Expression<Func<T, bool>>> GetGetByIdPredicateDelegate(IEnumerable<PropertyInfo> keyProperties)
        {
            var idDictionaryParameter = Expression.Parameter(typeof(EntityIds), "ids");
            var entityParameter = Expression.Parameter(typeof(T), "entity");
            var getByKeyMethod = typeof(EntityIds).GetMethod("TryGetValue")
                ?? throw new InvalidOperationException("Unable to get TryGetValue method in IDictionary.");
            var idVariablesGetters = new Expression[keyProperties.Count() + 1];
            var idVariables = new ParameterExpression[keyProperties.Count()];
            Expression predicate = null!;
            var count = 0;
            foreach (PropertyInfo property in keyProperties)
            {
                var idValue = Expression.Variable(typeof(object), property.Name);
                idVariables[count] = idValue;
                idVariablesGetters[count++] = Expression.Call(idDictionaryParameter, getByKeyMethod, Expression.Constant(property.Name, typeof(string)), idValue);
                if (predicate is null)
                {
                    predicate = Expression.Equal(
                            Expression.Property(entityParameter, property),
                            Expression.Convert(idValue, property.PropertyType));
                    continue;
                }
                predicate = Expression.And(predicate, Expression.Equal(
                            Expression.Property(entityParameter, property),
                            Expression.Convert(idValue, property.PropertyType)));
            }
            var entityIdAndIdEquality = Expression.Invoke(
                Expression.Lambda<Func<Expression<Func<T, bool>>>>(
                    Expression.Lambda(predicate, entityParameter)
                ));
            idVariablesGetters[count++] = entityIdAndIdEquality;
            var gettingIdsBlock = Expression.Block(idVariables, idVariablesGetters);

            return Expression.Lambda<Func<EntityIds, Expression<Func<T, bool>>>>(gettingIdsBlock, idDictionaryParameter).Compile();
        }

        private static Func<EntityIds, T> GetMapEntityFromEntityIds(IEnumerable<PropertyInfo> keyProperties)
        {
            var tryGetValueMethod = typeof(EntityIds).GetMethod("TryGetValue")
                ?? throw new InvalidOperationException("Unable to get TryGetValueMethod from EntityIds.");
            var entityIdsParameter = Expression.Parameter(typeof(EntityIds), "entityIds");
            var resultVariable = Expression.Variable(typeof(T), "result");
            var newEntityAssign = Expression.Assign(resultVariable, Expression.New(typeof(T).GetConstructor(new Type[0])
                ?? throw new InvalidOperationException($"Unable to get constructor without parameters from {typeof(T).Name}.")
                ));
            var assignationExpressions = new List<Expression>()
            {
                newEntityAssign,
            };
            var variables = new List<ParameterExpression>()
            {
                resultVariable
            };

            foreach (var property in keyProperties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }
                var idVariable = Expression.Variable(typeof(object), property.Name);

                assignationExpressions.Add(Expression.Call(entityIdsParameter, tryGetValueMethod, Expression.Constant(property.Name), idVariable));

                assignationExpressions.Add(
                    Expression.Assign(Expression.Property(resultVariable, property),
                    Expression.Convert(idVariable, property.PropertyType)));
                variables.Add(idVariable);
            }

            var label = Expression.Label(typeof(T));
            var returnResultExpression = Expression.Return(label, resultVariable);
            var returnLabel = Expression.Label(label, Expression.New(typeof(T).GetConstructor(new Type[0])!));

            assignationExpressions.Add(returnResultExpression);
            assignationExpressions.Add(returnLabel);

            var resultBlock = Expression.Block(variables, assignationExpressions);

            return Expression.Lambda<Func<EntityIds, T>>(resultBlock, entityIdsParameter).Compile();
        }
    }
}
