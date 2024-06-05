using BusinessLogicLayer.Entities.Attributes;
using Core.Configurations;
using Core.EntityHelpers;
using Core.ExpressionHelpers;
using Core.Localizations;
using DataAccessLayer.Abstractions;
using DataAccessLayer.Entities.Attributes;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace DataAccessLayer.Repositories.BaseRepositories
{
    public class BaseRepository<T> : IRepository<T, Expression<Func<T, bool>>>
        where T : class
    {
        protected readonly IMongoCollection<T> _collection;
        protected readonly IMongoDatabase _database;

        public BaseRepository(IOptions<MongoDbConfiguration> mongoDbConfigurationOptions, IStringLocalizer<Resource> stringLocalizer)
        {
            var mongoDbConfiguration = mongoDbConfigurationOptions.Value
                ?? throw new ArgumentNullException(nameof(mongoDbConfigurationOptions));

            var client = new MongoClient(mongoDbConfiguration.ConnectionString);
            _database = client.GetDatabase(mongoDbConfiguration.DbName);

            var collectionName = typeof(T).GetCustomAttribute<CollectionNameAttribute>()?.CollectionName ??
                throw new ArgumentException(string.Format(stringLocalizer["{0} does not specify collection name. Add CollectionNameAttribute."].Value, typeof(T).Name));
            _collection = _database.GetCollection<T>(collectionName);
        }

        public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
            return entity;
        }

        public virtual Task<T?> ReadByIdAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            return InternalReadByIdAsync(id, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> ReadByPredicateAsync(Expression<Func<T, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            var filter = EditExpression<T>.RemoveAttributedProperties<BsonIgnoreAttribute>(predicate) ?? (entity => true);

            return await _collection
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<int> CountEntitiesByPredicateAsync(Expression<Func<T, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            var filter = EditExpression<T>.RemoveAttributedProperties<BsonIgnoreAttribute>(predicate) ?? (entity => true);
            return (int)await _collection.Find(filter).Skip(skip).Limit(take).CountDocumentsAsync(cancellationToken);
        }

        public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var id = GeneralIdExtractor<T, KeyAttribute>.GetId(entity);
            var filter = GetFilterDefinition(id);
            await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            var filter = GetFilterDefinition(id);
            var result = await _collection.DeleteOneAsync(filter, cancellationToken);
            return result.DeletedCount > 0;
        }

        public virtual async Task<bool> DeleteSeveralAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var result = await _collection.DeleteManyAsync(predicate, cancellationToken);
            return result.DeletedCount > 0;
        }

        protected FilterDefinition<T>? GetFilterDefinition(EntityIds id)
        {
            var filterBuilder = Builders<T>.Filter;
            FilterDefinition<T>? filter = null;

            foreach (var keyValuePair in id)
            {
                if (filter is null)
                {
                    filter = filterBuilder.Eq(keyValuePair.Key, keyValuePair.Value);
                }
                else
                {
                    filter &= filterBuilder.Eq(keyValuePair.Key, keyValuePair.Value);
                }
            }

            return filter;
        }

        private async Task<T?> InternalReadByIdAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            var filter = GetFilterDefinition(id);

            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public virtual Task<T?> ReadByIdAsync(EntityIds id, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByIdAsync(id, cancellationToken);
        }

        public virtual Task<IEnumerable<T>> ReadByPredicateAsync(Expression<Func<T, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public virtual Task<int> CountEntitiesByPredicateAsync(Expression<Func<T, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            return CountEntitiesByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> ReadByPredicateAsync(Expression<Func<T, bool>> predicate, int take, int skip, IDictionary<Expression<Func<T, object>>, bool> orderBy, CancellationToken cancellationToken = default)
        {
            var filter = EditExpression<T>.RemoveAttributedProperties<BsonIgnoreAttribute>(predicate) ?? (entity => true);

            var filteredCollection = _collection
                    .Find(filter)
                    .Skip(skip)
                    .Limit(take);

            foreach (var keyValue in orderBy)
            {
                filteredCollection = keyValue.Value ? 
                    filteredCollection.SortByDescending(keyValue.Key)
                    :
                    filteredCollection.SortBy(keyValue.Key);
            }

            return await filteredCollection.ToListAsync();
        }

        public virtual Task<T> CreateAsync(T entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return CreateAsync(entity, cancellationToken);
        }

        public virtual Task<T> UpdateAsync(T entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return UpdateAsync(entity, cancellationToken);
        }
    }
}
