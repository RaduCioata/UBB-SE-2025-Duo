using System.Data;
using Microsoft.Data.SqlClient;
using Duo.Models;
using Duo.Data;
using System.Collections.Generic;
using System;
using Duo.Helpers;
using DuolingoNou.Models;
using Duo.Interfaces;

namespace Duo.Repositories
{
    /// <summary>
    /// Repository for user-related data operations.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IDataLink dataLink;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dataLink">The data access service.</param>
        public UserRepository(IDataLink dataLink)
        {
            this.dataLink = dataLink ?? throw new ArgumentNullException(nameof(dataLink));
        }

        /// <summary>
        /// Gets a user by username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Invalid username.", nameof(username));
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", username)
            };

            DataTable? dataTable = null;
            try
            {
                dataTable = dataLink.ExecuteReader("GetUserByUsername", parameters);
                if (dataTable.Rows.Count == 0)
                {
                    return null;
                }

                return Mappers.MapUser(dataTable.Rows[0]);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error when getting user by username: {ex.Message}", ex);
            }
            finally
            {
                dataTable?.Dispose();
            }
        }

        /// <summary>
        /// Gets a user by email.
        /// </summary>
        /// <param name="email">The email to search for.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        public User GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Invalid email.", nameof(email));
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Email", email)
            };

            DataTable? dataTable = null;
            try
            {
                dataTable = dataLink.ExecuteReader("GetUserByEmail", parameters);
                if (dataTable.Rows.Count == 0)
                {
                    return null;
                }

                return Mappers.MapUser(dataTable.Rows[0]);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error when getting user by email: {ex.Message}", ex);
            }
            finally
            {
                dataTable?.Dispose();
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>The ID of the newly created user.</returns>
        public int CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@PrivacyStatus", user.PrivacyStatus),
                new SqlParameter("@OnlineStatus", user.OnlineStatus),
                new SqlParameter("@DateJoined", user.DateJoined),
                new SqlParameter("@ProfileImage", user.ProfileImage ?? string.Empty),
                new SqlParameter("@TotalPoints", user.TotalPoints),
                new SqlParameter("@CoursesCompleted", user.CoursesCompleted),
                new SqlParameter("@QuizzesCompleted", user.QuizzesCompleted),
                new SqlParameter("@Streak", user.Streak),
                new SqlParameter("@LastActivityDate", user.LastActivityDate ?? (object)DBNull.Value),
                new SqlParameter("@Accuracy", user.Accuracy)
            };

            // Use ExecuteScalar to return the newly inserted UserId
            object result = dataLink.ExecuteScalar<int>("CreateUser", parameters);

            // Convert result to int (handle null safety)
            return result != null ? Convert.ToInt32(result) : -1;
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="user">The user with updated information.</param>
        public void UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", user.UserId),
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@PrivacyStatus", user.PrivacyStatus),
                new SqlParameter("@OnlineStatus", user.OnlineStatus),
                new SqlParameter("@DateJoined", user.DateJoined),
                new SqlParameter("@ProfileImage", user.ProfileImage ?? string.Empty),
                new SqlParameter("@TotalPoints", user.TotalPoints),
                new SqlParameter("@CoursesCompleted", user.CoursesCompleted),
                new SqlParameter("@QuizzesCompleted", user.QuizzesCompleted),
                new SqlParameter("@Streak", user.Streak),
                new SqlParameter("@LastActivityDate", user.LastActivityDate ?? (object)DBNull.Value),
                new SqlParameter("@Accuracy", user.Accuracy)
            };

            dataLink.ExecuteNonQuery("UpdateUser", parameters);
        }

        /// <summary>
        /// Validates if the provided credentials are correct.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if credentials are valid; otherwise, false.</returns>
        public bool ValidateCredentials(string username, string password)
        {
            User? user = GetUserByUsername(username);
            return user != null && user.Password == password;
        }

        /// <summary>
        /// Gets a user by their credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The user if credentials are valid; otherwise, null.</returns>
        public User GetUserByCredentials(string username, string password)
        {
            var user = GetUserByUsername(username);
            if (user != null && user.Password == password)
            {
                return user;
            }

            return null; // Either user not found or password doesn't match
        }

        /// <summary>
        /// Gets the top users by number of completed quizzes.
        /// </summary>
        /// <returns>A list of leaderboard entries sorted by quiz completion.</returns>
        public List<LeaderboardEntry> GetTopUsersByCompletedQuizzes()
        {
            var dataTable = dataLink.ExecuteReader("GetTopUsersByCompletedQuizzes");
            List<LeaderboardEntry> users = new List<LeaderboardEntry>();
            int rank = 1;

            foreach (DataRow row in dataTable.Rows)
            {
                users.Add(new LeaderboardEntry()
                {
                    Rank = rank++,
                    UserId = Convert.ToInt32(row["UserId"]),
                    Username = row["UserName"].ToString()!,
                    CompletedQuizzes = Convert.ToInt32(row["QuizzesCompleted"]),
                    Accuracy = Convert.ToDecimal(row["Accuracy"]),
                    ProfilePicture = ".. / .. / Assets /" + row["ProfileImage"].ToString()!
                });
            }

            return users;
        }

        /// <summary>
        /// Gets the top users by accuracy percentage.
        /// </summary>
        /// <returns>A list of leaderboard entries sorted by accuracy.</returns>
        public List<LeaderboardEntry> GetTopUsersByAccuracy()
        {
            var dataTable = dataLink.ExecuteReader("GetTopUsersByAccuracy");
            List<LeaderboardEntry> users = new List<LeaderboardEntry>();
            int rank = 1;

            foreach (DataRow row in dataTable.Rows)
            {
                users.Add(new LeaderboardEntry()
                {
                    Rank = rank++,
                    UserId = Convert.ToInt32(row["UserId"]),
                    Username = row["UserName"].ToString()!,
                    CompletedQuizzes = Convert.ToInt32(row["QuizzesCompleted"]),
                    Accuracy = Convert.ToDecimal(row["Accuracy"]),
                    ProfilePicture = ".. / .. / Assets /" + row["ProfileImage"].ToString()!
                });
            }

            return users;
        }

        /// <summary>
        /// Gets user statistics.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A user object with statistics.</returns>
        public User GetUserStats(int userId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId)
            };

            DataTable dataTable = dataLink.ExecuteReader("GetUserStats", parameters);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new User
                {
                    TotalPoints = Convert.ToInt32(row["TotalPoints"]),
                    Streak = Convert.ToInt32(row["Streak"]),
                    QuizzesCompleted = Convert.ToInt32(row["QuizzesCompleted"]),
                    CoursesCompleted = Convert.ToInt32(row["CoursesCompleted"])
                };
            }

            return null;
        }

        /// <summary>
        /// Gets all available achievements.
        /// </summary>
        /// <returns>A list of all achievements.</returns>
        public List<Achievement> GetAllAchievements()
        {
            DataTable dataTable = dataLink.ExecuteReader("GetAllAchievements");

            List<Achievement> achievements = new List<Achievement>();
            foreach (DataRow row in dataTable.Rows)
            {
                achievements.Add(new Achievement
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString()!,
                    Description = row["Description"].ToString()!,
                    Rarity = row["Rarity"].ToString()!
                });
            }

            return achievements;
        }

        /// <summary>
        /// Gets achievements earned by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of achievements earned by the user.</returns>
        public List<Achievement> GetUserAchievements(int userId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId)
            };

            DataTable dataTable = dataLink.ExecuteReader("GetUserAchievements", parameters);

            List<Achievement> achievements = new List<Achievement>();
            foreach (DataRow row in dataTable.Rows)
            {
                achievements.Add(new Achievement
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString()!,
                    Description = row["Description"].ToString()!,
                    Rarity = row["Rarity"].ToString()!,
                    AwardedDate = Convert.ToDateTime(row["DateEarned"])
                });
            }

            return achievements;
        }

        /// <summary>
        /// Awards an achievement to a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="achievementId">The ID of the achievement to award.</param>
        public void AwardAchievement(int userId, int achievementId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@AchievementId", achievementId),
                new SqlParameter("@DateEarned", DateTime.Now)
            };

            dataLink.ExecuteNonQuery("AwardAchievement", parameters);
        }

        /// <summary>
        /// Gets the friends of a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of the user's friends.</returns>
        public List<User> GetFriends(int userId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId)
            };

            DataTable dataTable = dataLink.ExecuteReader("GetFriends", parameters);

            List<User> friends = new List<User>();
            foreach (DataRow row in dataTable.Rows)
            {
                friends.Add(Mappers.MapUser(row));
            }

            return friends;
        }
    }
}