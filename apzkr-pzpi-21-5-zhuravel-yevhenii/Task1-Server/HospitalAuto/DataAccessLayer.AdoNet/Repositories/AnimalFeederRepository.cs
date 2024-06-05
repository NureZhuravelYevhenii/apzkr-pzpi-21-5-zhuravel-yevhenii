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
    public class AnimalFeederRepository : IRepository<AnimalFeeder, Expression<Func<AnimalFeeder, bool>>>
    {
        private readonly string _connectionString;

        public AnimalFeederRepository(IOptions<AdoNetConfiguration> adoNetConfigurationOptions)
        {
            _connectionString = adoNetConfigurationOptions.Value?.ConnectionString ?? throw new ArgumentNullException(nameof(adoNetConfigurationOptions));
        }

        public async Task<AnimalFeeder> CreateAsync(AnimalFeeder entity, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("CreateAnimalFeeder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AnimalId", entity.AnimalId);
                    command.Parameters.AddWithValue("@FeederId", entity.FeederId);
                    command.Parameters.AddWithValue("@AmountOfFood", entity.AmountOfFood);
                    command.Parameters.AddWithValue("@FeedDate", entity.FeedDate);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public async Task<AnimalFeeder?> ReadByIdAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand($"SELECT * FROM AnimalFeeders WHERE Id = {id["Id"]}", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        if (await reader.ReadAsync(cancellationToken))
                        {
                            return MapAnimalFeederFromDataReader(reader);
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
                using (var command = new SqlCommand("DeleteAnimalFeeder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", GeneralIdExtractor<AnimalFeeder, KeyAttribute>.MapEntityFromEntityIds(id).Id);
                    var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    return result > 0;
                }
            }
        }

        public async Task<IEnumerable<AnimalFeeder>> ReadByPredicateAsync(Expression<Func<AnimalFeeder, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            var result = new List<AnimalFeeder>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("SELECT * FROM AnimalFeeders", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            result.Add(MapAnimalFeederFromDataReader(reader));
                        }
                        return result;
                    }
                }
            }
        }

        public Task<int> CountEntitiesByPredicateAsync(Expression<Func<AnimalFeeder, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            // Implement counting AnimalFeeders by predicate
            throw new NotImplementedException();
        }

        public Task<int> CountEntitiesByPredicateAsync(Expression<Func<AnimalFeeder, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            // Implement counting AnimalFeeders by predicate with localization
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AnimalFeeder>> ReadByPredicateAsync(Expression<Func<AnimalFeeder, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            // Implement reading AnimalFeeders by predicate with localization
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AnimalFeeder>> ReadByPredicateAsync(Expression<Func<AnimalFeeder, bool>> predicate, int take, int skip, IDictionary<Expression<Func<AnimalFeeder, object>>, bool> orderBy, CancellationToken cancellationToken = default)
        {
            // Implement reading AnimalFeeders by predicate with ordering
            throw new NotImplementedException();
        }

        public async Task<AnimalFeeder> UpdateAsync(AnimalFeeder entity, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("UpdateAnimalFeeder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", entity.Id);
                    command.Parameters.AddWithValue("@AnimalId", entity.AnimalId);
                    command.Parameters.AddWithValue("@FeederId", entity.FeederId);
                    command.Parameters.AddWithValue("@AmountOfFood", entity.AmountOfFood);
                    command.Parameters.AddWithValue("@FeedDate", entity.FeedDate);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public Task<AnimalFeeder> UpdateAsync(AnimalFeeder entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return UpdateAsync(entity, cancellationToken);
        }

        private AnimalFeeder MapAnimalFeederFromDataReader(SqlDataReader reader)
        {
            return new AnimalFeeder
            {
                Id = (Guid)reader["Id"],
                AnimalId = (Guid)reader["AnimalId"],
                FeederId = (Guid)reader["FeederId"],
                AmountOfFood = (double)reader["AmountOfFood"],
                FeedDate = (DateTime)reader["FeedDate"]
            };
        }

        public Task<AnimalFeeder> CreateAsync(AnimalFeeder entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return CreateAsync(entity, cancellationToken);
        }

        public Task<AnimalFeeder?> ReadByIdAsync(EntityIds id, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByIdAsync(id, cancellationToken);
        }

        public Task<bool> DeleteSeveralAsync(Expression<Func<AnimalFeeder, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
