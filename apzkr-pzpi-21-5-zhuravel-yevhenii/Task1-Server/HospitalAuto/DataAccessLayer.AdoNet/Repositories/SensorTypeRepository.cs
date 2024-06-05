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
    public class SensorTypeRepository : IRepository<SensorType, Expression<Func<SensorType, bool>>>
    {
        private readonly string _connectionString;

        public SensorTypeRepository(IOptions<AdoNetConfiguration> adoNetConfigurationOptions)
        {
            _connectionString = adoNetConfigurationOptions.Value?.ConnectionString ?? throw new ArgumentNullException(nameof(adoNetConfigurationOptions));
        }

        public async Task<SensorType> CreateAsync(SensorType entity, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("CreateSensorType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", entity.Name);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public async Task<SensorType?> ReadByIdAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand($"SELECT * FROM SensorTypes WHERE Id = {id["Id"]}", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        if (await reader.ReadAsync(cancellationToken))
                        {
                            return MapSensorTypeFromDataReader(reader);
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
                using (var command = new SqlCommand("DeleteSensorType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", GeneralIdExtractor<SensorType, KeyAttribute>.MapEntityFromEntityIds(id));
                    var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    return result > 0;
                }
            }
        }

        public async Task<IEnumerable<SensorType>> ReadByPredicateAsync(Expression<Func<SensorType, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            var result = new List<SensorType>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("SELECT * FROM SensorTypes", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            result.Add(MapSensorTypeFromDataReader(reader));
                        }
                        return result;
                    }
                }
            }
        }

        public Task<int> CountEntitiesByPredicateAsync(Expression<Func<SensorType, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountEntitiesByPredicateAsync(Expression<Func<SensorType, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SensorType>> ReadByPredicateAsync(Expression<Func<SensorType, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public Task<IEnumerable<SensorType>> ReadByPredicateAsync(Expression<Func<SensorType, bool>> predicate, int take, int skip, IDictionary<Expression<Func<SensorType, object>>, bool> orderBy, CancellationToken cancellationToken = default)
        {
            return ReadByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public async Task<SensorType> UpdateAsync(SensorType entity, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("UpdateSensorType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", entity.Id);
                    command.Parameters.AddWithValue("@Name", entity.Name);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public async Task<SensorType> UpdateAsync(SensorType entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return await UpdateAsync(entity, cancellationToken);
        }

        private SensorType MapSensorTypeFromDataReader(SqlDataReader reader)
        {
            return new SensorType
            {
                Id = (Guid)reader["Id"],
                Name = (string)reader["Name"]
            };
        }

        public Task<SensorType> CreateAsync(SensorType entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return CreateAsync(entity, cancellationToken);
        }

        public Task<SensorType?> ReadByIdAsync(EntityIds id, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByIdAsync(id, cancellationToken);
        }

        public Task<bool> DeleteSeveralAsync(Expression<Func<SensorType, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
