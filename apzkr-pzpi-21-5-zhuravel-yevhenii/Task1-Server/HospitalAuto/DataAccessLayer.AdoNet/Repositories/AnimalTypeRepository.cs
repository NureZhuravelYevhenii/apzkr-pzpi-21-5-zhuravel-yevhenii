using Core.Configurations;
using Core.EntityHelpers;
using DataAccessLayer.Abstractions;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace DataAccessLayer.AdoNet.Repositories
{
    public class AnimalTypeRepository : IRepository<AnimalType, Expression<Func<AnimalType, bool>>>
    {
        private readonly string _connectionString;

        public AnimalTypeRepository(IOptions<AdoNetConfiguration> adoNetConfigurationOptions)
        {
            _connectionString = adoNetConfigurationOptions.Value?.ConnectionString ?? throw new ArgumentNullException(nameof(adoNetConfigurationOptions));
        }

        public async Task<AnimalType> CreateAsync(AnimalType entity, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("CreateAnimalType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", entity.Name);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public async Task<AnimalType?> ReadByIdAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand($"SELECT * FROM AnimalTypes WHERE Id = {id["Id"]}", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        if (await reader.ReadAsync(cancellationToken))
                        {
                            return MapAnimalTypeFromDataReader(reader);
                        }
                        return null;
                    }
                }
            }
        }

        public async Task<bool> DeleteAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("DeleteAnimalType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", GeneralIdExtractor<AnimalType, KeyAttribute>.MapEntityFromEntityIds(id));
                    var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    return result > 0;
                }
            }
        }

        public async Task<IEnumerable<AnimalType>> ReadByPredicateAsync(Expression<Func<AnimalType, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            var result = new List<AnimalType>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("SELECT * FROM AnimalTypes", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            result.Add(MapAnimalTypeFromDataReader(reader));
                        }
                        return result;
                    }
                }
            }
        }

        public Task<int> CountEntitiesByPredicateAsync(Expression<Func<AnimalType, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            // Implement counting AnimalTypes by predicate
            throw new NotImplementedException();
        }

        public Task<int> CountEntitiesByPredicateAsync(Expression<Func<AnimalType, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            // Implement counting AnimalTypes by predicate with localization
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AnimalType>> ReadByPredicateAsync(Expression<Func<AnimalType, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public Task<IEnumerable<AnimalType>> ReadByPredicateAsync(Expression<Func<AnimalType, bool>> predicate, int take, int skip, IDictionary<Expression<Func<AnimalType, object>>, bool> orderBy, CancellationToken cancellationToken = default)
        {
            return ReadByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public async Task<AnimalType> UpdateAsync(AnimalType entity, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("UpdateAnimalType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", entity.Id);
                    command.Parameters.AddWithValue("@Name", entity.Name);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public Task<AnimalType> UpdateAsync(AnimalType entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return UpdateAsync(entity, cancellationToken);
        }

        private AnimalType MapAnimalTypeFromDataReader(SqlDataReader reader)
        {
            return new AnimalType
            {
                Id = (Guid)reader["Id"],
                Name = (string)reader["Name"]
            };
        }

        public Task<AnimalType> CreateAsync(AnimalType entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return CreateAsync(entity, cancellationToken);
        }

        public Task<AnimalType?> ReadByIdAsync(EntityIds id, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByIdAsync(id, cancellationToken);
        }

        public Task<bool> DeleteSeveralAsync(Expression<Func<AnimalType, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

}
