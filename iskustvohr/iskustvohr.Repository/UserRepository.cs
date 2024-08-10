using GravatarHelper.Common;
using iskustvohr.Model;
using iskustvohr.Model.Common;
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
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<User> CreateNewUserAsync(User user)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO \"User\" (\"Id\", \"FirstName\", \"LastName\", \"Email\", \"ProfileImageUrl\", \"Password\", \"RoleId\", \"BusinessId\", \"CreatedAt\", \"UpdatedAt\", \"CreatedBy\", \"UpdatedBy\", \"IsActive\", \"EmailVerificationId\") " +
                        "VALUES (@Id, @FirstName, @LastName, @Email, @ProfileImageUrl,  @Password, @RoleId, @BusinessId, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @IsActive, @EmailVerificationId);";
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@RoleId", user.Role.Id);
                    command.Parameters.AddWithValue("@BusinessId", DBNull.Value);
                    command.Parameters.AddWithValue("@ProfileImageUrl", GravatarHelper.Common.GravatarHelper.CreateGravatarUrl(user.Email, 515, "https://cdn.pixabay.com/photo/2016/04/01/10/11/avatar-1299805_1280.png", null, null, null, true));
                    command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", user.UpdatedAt);
                    command.Parameters.AddWithValue("@CreatedBy", user.Id);
                    command.Parameters.AddWithValue("@UpdatedBy", user.Id);
                    command.Parameters.AddWithValue("@IsActive", user.IsActive);
                    command.Parameters.AddWithValue("@EmailVerificationId", user.EmailVerificationId);
                    await connection.OpenAsync();

                    if (await command.ExecuteNonQueryAsync() > 0)
                    {
                        return user;
                    }

                    return null;
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

        public async Task<User> GetUserByEmailAsync(User user)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM \"User\" WHERE \"Email\" = @Email";
                    command.Parameters.AddWithValue("@Email", user.Email);
                    await connection.OpenAsync();
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        command.Dispose();
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        while(await reader.ReadAsync()) 
                        { 
                            return new User
                            {
                                Id = (Guid)reader["Id"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                Email = (string)reader["Email"],
                                Password = (string)reader["Password"],
                                ProfileImageUrl = (string)reader["ProfileImageUrl"],
                                Role = new Role { Id = (Guid)reader["RoleId"] },
                                Business = new Business { Id = reader["BusinessId"] as Guid? ?? Guid.Empty },
                                CreatedAt = (DateTime)reader["CreatedAt"],
                                UpdatedAt = (DateTime)reader["UpdatedAt"],
                                EmailConfirmed = (bool)reader["EmailConfirmed"],
                                EmailVerificationId = (Guid)reader["EmailVerificationId"],
                                PasswordResetToken = reader["PasswordResetToken"] as string ?? null,
                                PasswordResetTokenExpires = reader["PasswordResetTokenExpires"] as DateTime? ?? null,
                                IsActive = (bool)reader["IsActive"]
                            };
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<int> UpdateUserProfileAsync(User user)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"User\" SET \"FirstName\" = @FirstName, \"LastName\" = @LastName WHERE \"Id\" = @Id";
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdateUserProfilePhotoAsync(User user)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"User\" SET \"ProfileImageUrl\" = @ProfileImageUrl WHERE \"Id\" = @Id";
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@ProfileImageUrl", user.ProfileImageUrl);
                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdateUserPasswordResetAsync(User user)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"User\" SET \"PasswordResetToken\" = @PasswordResetToken, \"PasswordResetTokenExpires\" = @PasswordResetTokenExpires WHERE \"Id\" = @Id";
                    command.Parameters.AddWithValue("@PasswordResetToken", user.PasswordResetToken);
                    command.Parameters.AddWithValue("@PasswordResetTokenExpires", user.PasswordResetTokenExpires);
                    command.Parameters.AddWithValue("@Id", user.Id);
                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdateUserPasswordAsync(User user)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"User\" SET \"PasswordResetToken\" = @PasswordResetToken, \"PasswordResetTokenExpires\" = @PasswordResetTokenExpires, \"Password\" = @Password WHERE \"Id\" = @Id";
                    command.Parameters.AddWithValue("@PasswordResetToken", DBNull.Value);
                    command.Parameters.AddWithValue("@PasswordResetTokenExpires", DBNull.Value);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Id", user.Id);
                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> ConfirmEmailAsync(User user)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"User\" SET \"EmailConfirmed\" = True WHERE \"EmailVerificationId\" = @EmailVerificationId";
                    command.Parameters.AddWithValue("@EmailVerificationId", user.EmailVerificationId);
                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<User> GetUserByIdAsync(User user)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM \"User\" WHERE \"Id\" = @id";
                    command.Parameters.AddWithValue("@id", user.Id);
                    await connection.OpenAsync();
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        command.Dispose();
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        while(await  reader.ReadAsync())
                        {
                            return new User
                            {
                                Id = (Guid)reader["Id"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                Email = (string)reader["Email"],
                                Password = (string)reader["Password"],
                                ProfileImageUrl = (string)reader["ProfileImageUrl"],
                                Role = new Role { Id = (Guid)reader["RoleId"] },
                                Business = new Business { Id = reader["BusinessId"] as Guid? ?? Guid.Empty },
                                CreatedAt = (DateTime)reader["CreatedAt"],
                                UpdatedAt = (DateTime)reader["UpdatedAt"],
                                EmailConfirmed = (bool)reader["EmailConfirmed"],
                                EmailVerificationId = (Guid)reader["EmailVerificationId"],
                                PasswordResetToken = reader["PasswordResetToken"] as string ?? null,
                                PasswordResetTokenExpires = reader["PasswordResetTokenExpires"] as DateTime? ?? null,
                                IsActive = (bool)reader["IsActive"]
                            };
                        }
                        await connection.CloseAsync();
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
