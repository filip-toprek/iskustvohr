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
    public class WebsiteRepository : IWebsiteRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<Website> GetWebsiteByUrlAsync(Website website)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM \"Website\" WHERE \"URL\" = @Url";
                    command.Parameters.AddWithValue("@Url", website.URL);
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
                            return new Website
                            {
                                Id = (Guid)reader["Id"],
                                Name = (string)reader["Name"],
                                PhotoUrl = (string)reader["PhotoUrl"],
                                URL = (string)reader["URL"],
                                CreatedAt = (DateTime)reader["CreatedAt"],
                                UpdatedAt = (DateTime)reader["UpdatedAt"],
                                IsAssigned = (bool)reader["IsAssigned"],
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
                return null;
            }
        }

        public async Task<Website> CreateWebsite(Website website)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO \"Website\" (\"Id\", \"Name\", \"PhotoUrl\", \"URL\", \"CreatedAt\", \"UpdatedAt\", \"IsAssigned\", \"IsActive\") VALUES " +
                        "(@Id, @Name, @PhotoUrl, @URL, @CreatedAt, @UpdatedAt, @IsAssigned, @IsActive);";
                    command.Parameters.AddWithValue("@Id", website.Id);
                    command.Parameters.AddWithValue("@Name", website.Name);
                    command.Parameters.AddWithValue("@PhotoUrl", website.PhotoUrl);
                    command.Parameters.AddWithValue("@URL", website.URL);
                    command.Parameters.AddWithValue("@CreatedAt", website.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", website.UpdatedAt);
                    command.Parameters.AddWithValue("@IsAssigned", website.IsAssigned);
                    command.Parameters.AddWithValue("@IsActive", website.IsActive);

                    await connection.OpenAsync();
                    if(command.ExecuteNonQuery() > 0)
                    {
                        await connection.CloseAsync();
                        return website;
                    }else
                    {
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
