using Core.EntityHelpers;
using DataAccessLayer.Entities;
using MongoDB.Driver.Core.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq.Expressions;
using Core.Configurations;
using Microsoft.Extensions.Options;
using DataAccessLayer.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.AdoNet.Repositories
{
    public class AnimalRepository : IRepository<Animal, Expression<Func<Animal, bool>>>
    {
        private readonly string _connectionString;

        public AnimalRepository(IOptions<AdoNetConfiguration> adoNetConfigurationOptions)
        {
            _connectionString = adoNetConfigurationOptions.Value?.ConnectionString ?? throw new ArgumentNullException(nameof(adoNetConfigurationOptions));
        }

        public Task<int> CountEntitiesByPredicateAsync(Expression<Func<Animal, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountEntitiesByPredicateAsync(Expression<Func<Animal, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Animal> CreateAsync(Animal entity, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("CreateAnimal", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", entity.Name);
                    command.Parameters.AddWithValue("@TypeId", entity.TypeId);
                    command.Parameters.AddWithValue("@AnimalCenterId", entity.AnimalCenterId);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public Task<Animal> CreateAsync(Animal entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return CreateAsync(entity, cancellationToken);
        }

        public async Task<bool> DeleteAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("DeleteAnimal", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", GeneralIdExtractor<Animal, KeyAttribute>.MapEntityFromEntityIds(id).Id);
                    var result = await command.ExecuteScalarAsync(cancellationToken);
                    return result != null && (int)result > 0;
                }
            }
        }

        public Task<bool> DeleteSeveralAsync(Expression<Func<Animal, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }

        public async Task<Animal?> ReadByIdAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand($"SELECT * FROM Animals WHERE Id = {id["Id"]}", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        if (await reader.ReadAsync(cancellationToken))
                        {
                            return MapAnimalFromDataReader(reader);
                        }
                        return null;
                    }
                }
            }
        }

        public Task<Animal?> ReadByIdAsync(EntityIds id, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByIdAsync(id, localizationCode, cancellationToken);
        }

        public async Task<IEnumerable<Animal>> ReadByPredicateAsync(Expression<Func<Animal, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            var result = new List<Animal>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("SELECT * FROM Animals", connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            result.Add(MapAnimalFromDataReader(reader));
                        }
                        return result;
                    }
                }
            }
        }

        public Task<IEnumerable<Animal>> ReadByPredicateAsync(Expression<Func<Animal, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public Task<IEnumerable<Animal>> ReadByPredicateAsync(Expression<Func<Animal, bool>> predicate, int take, int skip, IDictionary<Expression<Func<Animal, object>>, bool> orderBy, CancellationToken cancellationToken = default)
        {
            return ReadByPredicateAsync(predicate, take, skip, cancellationToken);
        }

        public async Task<Animal> UpdateAsync(Animal entity, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new SqlCommand("UpdateAnimal", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", entity.Id);
                    command.Parameters.AddWithValue("@Name", entity.Name);
                    command.Parameters.AddWithValue("@TypeId", entity.TypeId);
                    command.Parameters.AddWithValue("@AnimalCenterId", entity.AnimalCenterId);
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            return entity;
        }

        public Task<Animal> UpdateAsync(Animal entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return UpdateAsync(entity, cancellationToken);
        }

        private Animal MapAnimalFromDataReader(SqlDataReader reader)
        {
            return new Animal
            {
                Id = (Guid)reader["Id"],
                Name = (string)reader["Name"],
                TypeId = (Guid)reader["TypeId"],
                AnimalCenterId = (Guid)reader["AnimalCenterId"]
            };
        }
    }
}
