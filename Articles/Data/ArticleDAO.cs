using Articles.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Articles.Data
{
    public class ArticleDAO
    {

        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=aspnet-Articles-20240223013831;Integrated Security=True;Connect Timeout=30;";

        //Getting rating by UserId
        public int GetRatingByUserId(string userId, int articleId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Rating FROM dbo.UserArticleRating WHERE UserId = @userId AND ArticleId = @articleId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@userId", System.Data.SqlDbType.NVarChar).Value = userId;
                command.Parameters.Add("@articleId", System.Data.SqlDbType.Int).Value = articleId;

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            return 0;
        }

        //Getting userId by username
        public string GetIdByUsername(string username)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id FROM dbo.AspNetUsers WHERE username = @username";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = username;

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetString(0);
                    }
                }

            }
            return null;
        }

        // Fetching all Articles from database
        public List<Article> GetAll()
        {

            List<Article> articles = new List<Article>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM dbo.Articles";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Article article = new Article();
                        article.ID = reader.GetInt32(0);
                        article.Name = reader.GetString(1);
                        article.Category = reader.GetString(2);
                        article.Price = reader.GetDecimal(3);

                        articles.Add(article);
                    }
                }
            }

            return articles;
        }

        // Fetching one Article from database
        public Article GetById(int id)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM dbo.Articles WHERE id = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                Article article = new Article();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        article.ID = reader.GetInt32(0);
                        article.Name = reader.GetString(1);
                        article.Category = reader.GetString(2);
                        article.Price = reader.GetDecimal(3);
                    }
                }
                return article;
            }

        }

        // Calculates the average rating for a specific article.
        public double? GetAverageRatingForArticle(int articleId)
        {
            double? averageRating = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT CAST(AVG(CAST(Rating AS DECIMAL(10, 2))) AS DECIMAL(10, 2)) FROM dbo.UserArticleRating WHERE ArticleId = @articleId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@articleId", SqlDbType.Int).Value = articleId;

                connection.Open();

                var result = command.ExecuteScalar();

                if (result != DBNull.Value && result != null)
                {
                    averageRating = Convert.ToDouble(result);
                }
            }

            return averageRating;
        }

        // Store Article in database
        public int Create(Article article)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO dbo.Articles VALUES(@Name, @Category, @Price)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 100).Value = article.Name;
                command.Parameters.Add("@Category", System.Data.SqlDbType.VarChar, 100).Value = article.Category;
                command.Parameters.Add("@Price", System.Data.SqlDbType.Decimal, 100).Value = article.Price;

                connection.Open();

                int newId = command.ExecuteNonQuery();

                return newId;
            }

        }

        // Updates the rating of an article by a specific user.
        public int Update(Article article, string userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queryCheck = "SELECT * FROM dbo.UserArticleRating WHERE articleId = @articleId AND userId = @userId";

                SqlCommand commandCheck = new SqlCommand(queryCheck, connection);
                commandCheck.Parameters.Add("@userId", System.Data.SqlDbType.NVarChar, 100).Value = userId;
                commandCheck.Parameters.Add("@articleId", System.Data.SqlDbType.Int).Value = article.ID;

                connection.Open();

                SqlDataReader reader = commandCheck.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Close();

                    return -1;
                }
                else
                {
                    reader.Close();

                    string query = "INSERT INTO dbo.UserArticleRating (userId, articleId, Rating) VALUES (@userId, @articleId, @Rating)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@userId", System.Data.SqlDbType.NVarChar, 100).Value = userId;
                    command.Parameters.Add("@articleId", System.Data.SqlDbType.Int).Value = article.ID;
                    command.Parameters.Add("@Rating", System.Data.SqlDbType.Int).Value = article.Rating;

                    return command.ExecuteNonQuery();
                }
            }
        }

        //Updating the article
        public int UpdateArticle(Article article)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE dbo.Articles SET Name = @Name, Category = @Category, Price = @Price WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@Id", System.Data.SqlDbType.VarChar, 100).Value = article.ID;
                command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 100).Value = article.Name;
                command.Parameters.Add("@Category", System.Data.SqlDbType.VarChar, 100).Value = article.Category;
                command.Parameters.Add("@Price", System.Data.SqlDbType.Decimal, 100).Value = article.Price;

                connection.Open();

                int newId = command.ExecuteNonQuery();

                return newId;
            }

        }

        //Deleting the aricle
        internal int Delete(int id)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM dbo.Articles WHERE id = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@id", System.Data.SqlDbType.VarChar, 100).Value = id;
                connection.Open();

                int deletedId = command.ExecuteNonQuery();

                return deletedId;
            }
        }
    }
}
