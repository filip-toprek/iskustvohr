using iskustvohr.Common;
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
    public class ReviewRepository : IReviewRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<PagedList<Review>> GetReviewsByUserIdAsync(User user)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT \"Review\".*, \"User\".*, \"Website\".* " +
                        "FROM \"Review\" " +
                        "JOIN \"User\" ON \"User\".\"Id\" = \"Review\".\"UserId\"" +
                        "JOIN \"Website\" ON \"Website\".\"Id\" = \"Review\".\"WebsiteId\"" +
                        "WHERE \"Review\".\"IsActive\" = True AND \"Review\".\"IsReview\" = True AND \"User\".\"Id\" = @Id;";
                    command.Parameters.AddWithValue("@Id", user.Id);
                    await connection.OpenAsync();
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        //command.Dispose();
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        List<Review> reviewList = new List<Review>();
                        while (await reader.ReadAsync())
                        {
                            reviewList.Add(new Review
                            {
                                Id = (Guid)reader["Id"],
                                User = new User
                                {
                                    Id = (Guid)reader["UserId"],
                                    Email = (string)reader["Email"],
                                    FirstName = (string)reader["FirstName"],
                                    LastName = (string)reader["LastName"],
                                    ProfileImageUrl = (string)reader["ProfileImageUrl"],
                                    Role = new Role { Id = (Guid)reader["RoleId"] },
                                    Business = new Business { Id = reader["BusinessId"] as Guid? ?? Guid.Empty },
                                },
                                Website = new Website
                                {
                                    Id = (Guid)reader["WebsiteId"],
                                    Name = (string)reader["Name"],
                                    PhotoUrl = (string)reader["PhotoUrl"],
                                    URL = (string)reader["URL"]
                                },
                                ReviewText = (string)reader["ReviewText"],
                                ReviewScore = (int)reader["ReviewScore"],
                                CreatedAt = (DateTime)reader["CreatedAt"],
                                UpdatedAt = (DateTime)reader["UpdatedAt"],
                                IsActive = (bool)reader["IsActive"],
                                Reply = new Reply { Id = reader["ReplyId"] as Guid? ?? Guid.Empty }
                            });
                        }
                        await connection.CloseAsync();
                        return new PagedList<Review>(reviewList, 5, await GetReviewsTotalCount());
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PagedList<Review>> GetReviewsAsync(Review review, Paging paging)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT \"Review\".*, \"User\".*, \"Website\".* " +
                        "FROM \"Review\" " +
                        "JOIN \"User\" ON \"User\".\"Id\" = \"Review\".\"UserId\"" +
                        "JOIN \"Website\" ON \"Website\".\"Id\" = \"Review\".\"WebsiteId\"" +
                        "WHERE \"Review\".\"WebsiteId\" = @WebsiteId AND \"Review\".\"IsActive\" = True AND \"Review\".\"IsReview\" = True" +
                        " ORDER BY \"Review\".\"CreatedAt\" ASC OFFSET @Offset ROWS FETCH NEXT 5 ROWS ONLY;";
                    command.Parameters.AddWithValue("@WebsiteId", review.Website.Id);
                    command.Parameters.AddWithValue("@Offset", (paging.PageNumber - 1) * 5);
                    await connection.OpenAsync();
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        //command.Dispose();
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        List<Review> reviewList = new List<Review>();
                        while (await reader.ReadAsync())
                        {
                            reviewList.Add(new Review
                            {
                                Id = (Guid)reader["Id"],
                                User = new User
                                {
                                    Id = (Guid)reader["UserId"],
                                    Email = (string)reader["Email"],
                                    FirstName = (string)reader["FirstName"],
                                    LastName = (string)reader["LastName"],
                                    ProfileImageUrl = (string)reader["ProfileImageUrl"],
                                    Role = new Role { Id = (Guid)reader["RoleId"] },
                                    Business = new Business { Id = reader["BusinessId"] as Guid? ?? Guid.Empty },
                                },
                                Website = new Website
                                {
                                    Id = (Guid)reader["WebsiteId"],
                                    Name = (string)reader["Name"],
                                    PhotoUrl = (string)reader["PhotoUrl"],
                                    URL = (string)reader["URL"]
                                },
                                ReviewText = (string)reader["ReviewText"],
                                ReviewScore = (int)reader["ReviewScore"],
                                CreatedAt = (DateTime)reader["CreatedAt"],
                                UpdatedAt = (DateTime)reader["UpdatedAt"],
                                IsActive = (bool)reader["IsActive"],
                                Reply = new Reply { Id = reader["ReplyId"] as Guid? ?? Guid.Empty }
                            });
                        }
                        await connection.CloseAsync();
                        return new PagedList<Review>(reviewList, 5, await GetReviewsTotalCount());
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO \"Review\" (\"Id\", \"UserId\", \"WebsiteId\", \"ReviewText\", \"ReviewScore\", \"CreatedAt\", \"UpdatedAt\", \"CreatedBy\", \"UpdatedBy\", \"IsActive\", \"ReplyId\", \"IsReview\")" +
                        "VALUES (@Id, @UserId, @WebsiteId, @ReviewText, @ReviewScore, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @IsActive, @ReplyId, @IsReview);";
                    command.Parameters.AddWithValue("@Id", review.Id);
                    command.Parameters.AddWithValue("@userId", review.User.Id);
                    command.Parameters.AddWithValue("@WebsiteId", review.Website.Id);
                    command.Parameters.AddWithValue("@ReviewText", review.ReviewText);
                    command.Parameters.AddWithValue("@ReviewScore", review.ReviewScore);
                    command.Parameters.AddWithValue("@CreatedAt", review.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", review.UpdatedAt);
                    command.Parameters.AddWithValue("@CreatedBy", review.CreatedBy);
                    command.Parameters.AddWithValue("@UpdatedBy", review.UpdatedBy);
                    command.Parameters.AddWithValue("@IsActive", review.IsActive);
                    command.Parameters.AddWithValue("@IsReview", review.IsReview);
                    object replyIdValue = review.Reply.Id == Guid.Empty ? DBNull.Value : (object)review.Reply.Id;
                    command.Parameters.AddWithValue("@ReplyId", replyIdValue);
                    await connection.OpenAsync();
                    if (await command.ExecuteNonQueryAsync() > 0)
                    {
                        await connection.CloseAsync();
                        return review;
                    }
                    await connection.CloseAsync();
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> CreateReplyAsync(Guid ReviewId, Guid ReplyId)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"Review\" SET \"ReplyId\" = @ReplyId WHERE \"Id\" = @ReviewId";
                    command.Parameters.AddWithValue("@ReplyId", ReplyId);
                    command.Parameters.AddWithValue("@ReviewId", ReviewId);
                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<Review> GetReviewByIdAsync(Review review)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT \"Review\".*, \"User\".*, \"Website\".* " +
                        "FROM \"Review\" " +
                        "JOIN \"User\" ON \"User\".\"Id\" = \"Review\".\"UserId\"" +
                        "JOIN \"Website\" ON \"Website\".\"Id\" = \"Review\".\"WebsiteId\"" +
                        "WHERE \"Review\".\"IsActive\" = True AND \"Review\".\"Id\" = @Id";
                    command.Parameters.AddWithValue("@Id", review.Id);
                    await connection.OpenAsync();
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        //command.Dispose();
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        Review reviewToReturn = null;
                        while (await reader.ReadAsync())
                        {
                            reviewToReturn = (new Review
                            {
                                Id = (Guid)reader["Id"],
                                User = new User
                                {
                                    Id = (Guid)reader["UserId"],
                                    Email = (string)reader["Email"],
                                    FirstName = (string)reader["FirstName"],
                                    LastName = (string)reader["LastName"],
                                    ProfileImageUrl = (string)reader["ProfileImageUrl"],
                                    Role = new Role { Id = (Guid)reader["RoleId"] },
                                    Business = new Business { Id = reader["BusinessId"] as Guid? ?? Guid.Empty },
                                },
                                Website = new Website
                                {
                                    Id = (Guid)reader["WebsiteId"],
                                    Name = (string)reader["Name"],
                                    PhotoUrl = (string)reader["PhotoUrl"],
                                    URL = (string)reader["URL"]
                                },
                                ReviewText = (string)reader["ReviewText"],
                                ReviewScore = (int)reader["ReviewScore"],
                                CreatedAt = (DateTime)reader["CreatedAt"],
                                UpdatedAt = (DateTime)reader["UpdatedAt"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                IsActive = (bool)reader["IsActive"],
                                Reply = new Reply { Id = reader["ReplyId"] as Guid? ?? Guid.Empty }
                            });
                        }
                        await connection.CloseAsync();
                        return reviewToReturn;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> UpdateReviewAsync(Review review)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"Review\" SET \"ReviewText\" = @ReviewText, \"ReviewScore\" = @ReviewScore, \"UpdatedAt\" = @UpdatedAt," +
                        "\"UpdatedBy\" = @UpdatedBy " +
                        "WHERE \"Id\" = @ReviewId";
                    command.Parameters.AddWithValue("@UpdatedAt", review.UpdatedAt);
                    command.Parameters.AddWithValue("@UpdatedBy", review.UpdatedBy);
                    command.Parameters.AddWithValue("@ReviewText", review.ReviewText);
                    command.Parameters.AddWithValue("@ReviewScore", review.ReviewScore);
                    command.Parameters.AddWithValue("@ReviewId", review.Id);
                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteReviewAsync(Guid id)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"Review\" SET \"IsActive\" = False WHERE \"Id\" = @ReviewId";
                    command.Parameters.AddWithValue("@ReviewId", id);
                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<Reply> GetReplyByIdAsync(Reply review)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT \"Review\".*, \"User\".*, \"Website\".* " +
                        "FROM \"Review\" " +
                        "JOIN \"User\" ON \"User\".\"Id\" = \"Review\".\"UserId\"" +
                        "JOIN \"Website\" ON \"Website\".\"Id\" = \"Review\".\"WebsiteId\"" +
                        "WHERE \"Review\".\"IsActive\" = True AND \"Review\".\"Id\" = @Id AND \"Review\".\"IsReview\" = False";
                    command.Parameters.AddWithValue("@Id", review.Id);
                    await connection.OpenAsync();
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        //command.Dispose();
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        Reply replyToReturn = null;
                        while (await reader.ReadAsync())
                        {
                            replyToReturn = (new Reply
                            {
                                Id = (Guid)reader["Id"],
                                User = new User
                                {
                                    Id = (Guid)reader["UserId"],
                                    Email = (string)reader["Email"],
                                    FirstName = (string)reader["FirstName"],
                                    LastName = (string)reader["LastName"],
                                    ProfileImageUrl = (string)reader["ProfileImageUrl"],
                                    Role = new Role { Id = (Guid)reader["RoleId"] },
                                    Business = new Business { Id = reader["BusinessId"] as Guid? ?? Guid.Empty },
                                },
                                ReplyText = (string)reader["ReviewText"],
                                CreatedAt = (DateTime)reader["CreatedAt"],
                                UpdatedAt = (DateTime)reader["UpdatedAt"],
                                IsActive = (bool)reader["IsActive"],
                            });
                        }
                        await connection.CloseAsync();
                        return replyToReturn;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<int> GetReviewsTotalCount()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                StringBuilder querryBuilder = new StringBuilder("SELECT COUNT(*) FROM \"Review\" WHERE \"IsActive\" = True AND \"Review\".\"IsReview\" = True;");
                command.CommandText = querryBuilder.ToString();
                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int count))
                {
                    await connection.CloseAsync();
                    return count;
                }
                return 0;
            }
        }
    }
}
