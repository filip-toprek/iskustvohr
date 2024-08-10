using iskustvohr.Model;
using iskustvohr.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iskustvohr.Repository
{
    public class RoleRepository : IRoleRepository, IDisposable
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<Role> GetRoleByNameAsync(Role role)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM \"Role\" WHERE \"RoleName\" = @RoleName";
                    command.Parameters.AddWithValue("@RoleName", role.RoleName);
                    await connection.OpenAsync();
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        //command.Dispose();
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        while (await reader.ReadAsync())
                        {
                            return new Role
                            {
                                Id = (Guid)reader["Id"],
                                RoleName = (string)reader["RoleName"],
                                IsActive = (bool)reader["IsActive"]
                            };
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Role> GetRoleByIdAsync(Role role)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM \"Role\" WHERE \"Id\" = @Id";
                    command.Parameters.AddWithValue("@Id", role.Id);
                    await connection.OpenAsync();
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        //command.Dispose();
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        while (await reader.ReadAsync())
                        {
                            return new Role
                            {
                                Id = (Guid)reader["Id"],
                                RoleName = (string)reader["RoleName"],
                                IsActive = (bool)reader["IsActive"]
                            };
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Dispose()
        {
            return;
        }
    }
}
