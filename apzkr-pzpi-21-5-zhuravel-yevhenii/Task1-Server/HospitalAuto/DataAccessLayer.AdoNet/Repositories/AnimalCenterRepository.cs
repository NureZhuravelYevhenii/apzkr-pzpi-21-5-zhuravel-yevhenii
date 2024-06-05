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
    public class AnimalCenterRepository : IRepository<AnimalCenter, Expression<Func<AnimalCenter, bool>>>
    {
        private readonly string _connectionString;

        public AnimalCenterRepository(IOptions<AdoNetConfiguration> adoNetConfigurationOptions)
        {
            _connectionString = adoNetConfigurationOptions.Value?.ConnectionString ?? throw new ArgumentNullException(nameof(adoNetConfigurationOptions));
        }

        public async Task<AnimalCenter> CreateAsync(AnimalCenter entity, CancellationToken cancellationToken = default)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("CreateAnimalCenter", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", entity.Name);
                    command.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);
                    command.Parameters.AddWithValue("@RefreshToken", entity.RefreshToken);
                    command.Parameters.AddWithValue("@Address", entity.Address);
                    command.Parameters.AddWithValue("@Info", entity.Info);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public async Task<AnimalCenter?> ReadByIdAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand($"SELECT * FROM AnimalCenters WHERE Id = {id["Id"]}", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        if (await reader.ReadAsync(cancellationToken))
                        {
                            return MapAnimalCenterFromDataReader(reader);
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
                using (var command = new SqlCommand("DeleteAnimalCenter", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", GeneralIdExtractor<AnimalCenter, KeyAttribute>.MapEntityFromEntityIds(id).Id);
                    var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    return result > 0;
                }
            }
        }

        public async Task<IEnumerable<AnimalCenter>> ReadByPredicateAsync(Expression<Func<AnimalCenter, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            var result = new List<AnimalCenter>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("SELECT * FROM AnimalCenters", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            result.Add(MapAnimalCenterFromDataReader(reader));
                        }
                    }
                }
                foreach (var animalCenter in result)
                {
                    using (var commandAnimal = new SqlCommand($"SELECT * FROM Animals WHERE Animals.AnimalCenterId = '{animalCenter.Id}'", connection))
                    {
                        commandAnimal.CommandType = CommandType.Text;
                        using var reader = await commandAnimal.ExecuteReaderAsync();
                        animalCenter.Animals = (await MapAnimalsFromDataReader(reader)).ToList();
                    }
                }
                return result;
            }
        }

        private AnimalCenter MapAnimalCenterFromDataReader(SqlDataReader reader)
        {
            return new AnimalCenter
            {
                Id = (Guid)reader["Id"],
                Name = (string)reader["Name"],
                PasswordHash = (string)reader["PasswordHash"],
                RefreshToken = (string)reader["RefreshToken"],
                Address = (string)reader["Address"],
                Info = (string)reader["Info"]
            };
        }

        private async Task<IEnumerable<Animal>> MapAnimalsFromDataReader(SqlDataReader reader)
        {
            var result = new List<Animal>();

            while (await reader.ReadAsync())
            {
                result.Add(new Animal
                {
                    Name = (string)reader["Name"],
                });
            }

            return result;
        }

        public Task<AnimalCenter> CreateAsync(AnimalCenter entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return CreateAsync(entity, cancellationToken);
        }

        public Task<AnimalCenter?> ReadByIdAsync(EntityIds id, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByIdAsync(id, cancellationToken);
        }

        public Task<IEnumerable<AnimalCenter>> ReadByPredicateAsync(Expression<Func<AnimalCenter, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public Task<IEnumerable<AnimalCenter>> ReadByPredicateAsync(Expression<Func<AnimalCenter, bool>> predicate, int take, int skip, IDictionary<Expression<Func<AnimalCenter, object>>, bool> orderBy, CancellationToken cancellationToken = default)
        {
            return ReadByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public Task<int> CountEntitiesByPredicateAsync(Expression<Func<AnimalCenter, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountEntitiesByPredicateAsync(Expression<Func<AnimalCenter, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<AnimalCenter> UpdateAsync(AnimalCenter entity, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("UpdateAnimalCenter", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", entity.Id);
                    command.Parameters.AddWithValue("@Name", entity.Name);
                    command.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);
                    command.Parameters.AddWithValue("@RefreshToken", entity.RefreshToken);
                    command.Parameters.AddWithValue("@Address", entity.Address);
                    command.Parameters.AddWithValue("@Info", entity.Info);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public Task<AnimalCenter> UpdateAsync(AnimalCenter entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return UpdateAsync(entity, cancellationToken);
        }

        public Task<bool> DeleteSeveralAsync(Expression<Func<AnimalCenter, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
