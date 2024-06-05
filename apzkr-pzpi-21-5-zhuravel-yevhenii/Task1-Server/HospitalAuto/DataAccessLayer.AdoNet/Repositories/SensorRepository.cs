using Core.Configurations;
using Core.EntityHelpers;
using DataAccessLayer.Abstractions;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.AdoNet.Repositories
{
    public class SensorRepository : IRepository<Sensor, Expression<Func<Sensor, bool>>>
    {
        private readonly string _connectionString;

        public SensorRepository(IOptions<AdoNetConfiguration> adoNetConfigurationOptions)
        {
            _connectionString = adoNetConfigurationOptions.Value?.ConnectionString ?? throw new ArgumentNullException(nameof(adoNetConfigurationOptions));
        }

        public async Task<Sensor> CreateAsync(Sensor entity, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("CreateSensor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AnimalId", entity.AnimalId);
                    command.Parameters.AddWithValue("@TypeId", entity.TypeId);
                    command.Parameters.AddWithValue("@AnimalCenterId", entity.AnimalCenterId);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public async Task<Sensor?> ReadByIdAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand($"SELECT * FROM Sensors WHERE Id = {id["Id"]}", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        if (await reader.ReadAsync(cancellationToken))
                        {
                            return MapSensorFromDataReader(reader);
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
                using (var command = new SqlCommand("DeleteSensor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", GeneralIdExtractor<Sensor, KeyAttribute>.MapEntityFromEntityIds(id));
                    var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    return result > 0;
                }
            }
        }

        public async Task<IEnumerable<Sensor>> ReadByPredicateAsync(Expression<Func<Sensor, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            var result = new List<Sensor>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("SELECT * FROM Sensors", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            result.Add(MapSensorFromDataReader(reader));
                        }
                        return result;
                    }
                }
            }
        }

        public Task<int> CountEntitiesByPredicateAsync(Expression<Func<Sensor, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountEntitiesByPredicateAsync(Expression<Func<Sensor, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Sensor>> ReadByPredicateAsync(Expression<Func<Sensor, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public  Task<IEnumerable<Sensor>> ReadByPredicateAsync(Expression<Func<Sensor, bool>> predicate, int take, int skip, IDictionary<Expression<Func<Sensor, object>>, bool> orderBy, CancellationToken cancellationToken = default)
        {
            return ReadByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public async Task<Sensor> UpdateAsync(Sensor entity, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("UpdateSensor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", entity.Id);
                    command.Parameters.AddWithValue("@AnimalId", entity.AnimalId);
                    command.Parameters.AddWithValue("@TypeId", entity.TypeId);
                    command.Parameters.AddWithValue("@AnimalCenterId", entity.AnimalCenterId);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public async Task<Sensor> UpdateAsync(Sensor entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return await UpdateAsync(entity, cancellationToken);
        }

        private Sensor MapSensorFromDataReader(SqlDataReader reader)
        {
            return new Sensor
            {
                Id = (Guid)reader["Id"],
                AnimalId = (Guid)reader["AnimalId"],
                TypeId = (Guid)reader["TypeId"],
                AnimalCenterId = (Guid)reader["AnimalCenterId"]
            };
        }

        public Task<Sensor> CreateAsync(Sensor entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return CreateAsync(entity, cancellationToken);
        }

        public Task<Sensor?> ReadByIdAsync(EntityIds id, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByIdAsync(id, cancellationToken);
        }

        public Task<bool> DeleteSeveralAsync(Expression<Func<Sensor, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
