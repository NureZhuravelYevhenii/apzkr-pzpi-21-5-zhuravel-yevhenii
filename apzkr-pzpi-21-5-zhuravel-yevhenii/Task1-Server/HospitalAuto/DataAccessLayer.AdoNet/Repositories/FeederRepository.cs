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
    public class FeederRepository : IRepository<Feeder, Expression<Func<Feeder, bool>>>
    {
        private readonly string _connectionString;

        public FeederRepository(IOptions<AdoNetConfiguration> adoNetConfigurationOptions)
        {
            _connectionString = adoNetConfigurationOptions.Value?.ConnectionString ?? throw new ArgumentNullException(nameof(adoNetConfigurationOptions));
        }

        public async Task<Feeder> CreateAsync(Feeder entity, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("CreateFeeder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Location", entity.Location);
                    command.Parameters.AddWithValue("@AnimalCenterId", entity.AnimalCenterId);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public async Task<Feeder?> ReadByIdAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand($"SELECT * FROM Feeders WHERE Id = {id["Id"]}", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        if (await reader.ReadAsync(cancellationToken))
                        {
                            return MapFeederFromDataReader(reader);
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
                using (var command = new SqlCommand("DeleteFeeder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", GeneralIdExtractor<Feeder, KeyAttribute>.MapEntityFromEntityIds(id));
                    var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    return result > 0;
                }
            }
        }

        public async Task<IEnumerable<Feeder>> ReadByPredicateAsync(Expression<Func<Feeder, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            var result = new List<Feeder>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("SELECT * FROM Feeders", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            result.Add(MapFeederFromDataReader(reader));
                        }
                        return result;
                    }
                }
            }
        }

        public async Task<int> CountEntitiesByPredicateAsync(Expression<Func<Feeder, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            return await CountEntitiesByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public async Task<int> CountEntitiesByPredicateAsync(Expression<Func<Feeder, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            return await CountEntitiesByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public async Task<IEnumerable<Feeder>> ReadByPredicateAsync(Expression<Func<Feeder, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            return await ReadByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public async Task<IEnumerable<Feeder>> ReadByPredicateAsync(Expression<Func<Feeder, bool>> predicate, int take, int skip, IDictionary<Expression<Func<Feeder, object>>, bool> orderBy, CancellationToken cancellationToken = default)
        {
            return await ReadByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public async Task<Feeder> UpdateAsync(Feeder entity, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("UpdateFeeder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", entity.Id);
                    command.Parameters.AddWithValue("@Location", entity.Location);
                    command.Parameters.AddWithValue("@AnimalCenterId", entity.AnimalCenterId);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public async Task<Feeder> UpdateAsync(Feeder entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return await UpdateAsync(entity, cancellationToken);
        }

        private Feeder MapFeederFromDataReader(SqlDataReader reader)
        {
            return new Feeder
            {
                Id = (Guid)reader["Id"],
                Location = (GeoPoint)reader["Location"],
                AnimalCenterId = (Guid)reader["AnimalCenterId"]
            };
        }

        public Task<Feeder> CreateAsync(Feeder entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return CreateAsync(entity, cancellationToken);
        }

        public Task<Feeder?> ReadByIdAsync(EntityIds id, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByIdAsync(id, cancellationToken);
        }

        public Task<bool> DeleteSeveralAsync(Expression<Func<Feeder, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

}
