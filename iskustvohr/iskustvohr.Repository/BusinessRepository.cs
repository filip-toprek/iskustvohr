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
    public class BusinessRepository : IBusinessRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<int> CreateBusinessAsync(Business newBusiness)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO \"Business\" (\"Id\", \"WebsiteId\", \"IsConfirmed\", \"BusinessEmail\") VALUES" +
                        "(@Id, @WebsiteId, @IsConfirmed, @BusinessEMail)";
                    command.Parameters.AddWithValue("@Id", newBusiness.Id);
                    command.Parameters.AddWithValue("@WebsiteId", newBusiness.Website.Id);
                    command.Parameters.AddWithValue("@IsConfirmed", false);
                    command.Parameters.AddWithValue("@BusinessEmail", newBusiness.BusinessEmail);
                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> VerifyBusinessAsync(Business newBusiness, Role role, Guid userId)
        {
            NpgsqlTransaction transaction = null;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"Business\" SET \"IsConfirmed\" = True WHERE \"EmailVerificationId\" = @EmailVerificationId;";
                    command.Parameters.AddWithValue("@EmailVerificationId", newBusiness.EmailVerificationId);
                    await connection.OpenAsync();
                    transaction = connection.BeginTransaction();
                    await command.ExecuteNonQueryAsync();

                    command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"User\" SET \"BusinessId\" = @BusinessId, \"RoleId\" = @RoleId WHERE \"Id\" = @Id;";
                    command.Parameters.AddWithValue("@Id", userId);
                    command.Parameters.AddWithValue("@BusinessId", newBusiness.Id);
                    command.Parameters.AddWithValue("@RoleId", role.Id);
                    await command.ExecuteNonQueryAsync();

                    command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"Website\" SET \"IsAssigned\" = True WHERE \"URL\" = @URL;";
                    command.Parameters.AddWithValue("@URL", newBusiness.Website.URL);
                    await command.ExecuteNonQueryAsync();
                    transaction.Commit();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return 0;
            }
        }

        public async Task<Business> GetBusinessByWebsiteIdAsync(Website website)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM \"Business\" WHERE \"WebsiteId\" = @WebsiteId";
                    command.Parameters.AddWithValue("@WebsiteId", website.Id);
                    await connection.OpenAsync();
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        command.Dispose();
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        while (await reader.ReadAsync())
                        {
                            return new Business
                            {
                                Id = (Guid)reader[0],
                                Website = new Website { Id = (Guid)reader[1] },
                                IsConfirmed = (bool)reader[2],
                                EmailVerificationId = (Guid)reader[3],
                                BusinessEmail = (string)reader[4]
                            };
                        }
                        await connection.CloseAsync();
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
