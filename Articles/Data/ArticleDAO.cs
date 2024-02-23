using Articles.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Articles.Data
{
    public class ArticleDAO
    {


        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=aspnet-Articles-20240223013831;Integrated Security=True;Connect Timeout=30;";

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
                        article.Rating = reader.GetInt32(4);

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
                        article.Rating = reader.GetInt32(4);
                    }
                }
                return article;
            }

        }

        // Store Article in database
        public int Create(Article article)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO dbo.Articles VALUES(@Name, @Category, @Price, @Rating)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 100).Value = article.Name;
                command.Parameters.Add("@Category", System.Data.SqlDbType.VarChar, 100).Value = article.Category;
                command.Parameters.Add("@Price", System.Data.SqlDbType.Decimal, 100).Value = article.Price;
                command.Parameters.Add("@Rating", System.Data.SqlDbType.Int, 100).Value = article.Rating;

                connection.Open();

                int newId = command.ExecuteNonQuery();

                return newId;
            }

        }

        public int Update(Article article)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string query = "UPDATE dbo.Articles SET Rating =@Rating WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@Id", System.Data.SqlDbType.VarChar, 100).Value = article.ID;
                command.Parameters.Add("@Rating", System.Data.SqlDbType.VarChar, 100).Value = article.Rating;

                connection.Open();

                int newId = command.ExecuteNonQuery();

                return newId;
            }

        }

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

        internal List<Article> SearchForName(string searchPhrase)
        {

            List<Article> articles = new List<Article>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM dbo.Articles WHERE NAME LIKE @searchForMe";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@searchForMe", System.Data.SqlDbType.NVarChar).Value = "%" + searchPhrase + "%";

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
                        article.Rating = reader.GetInt32(4);

                        articles.Add(article);
                    }
                }
            }

            return articles;
        }
    }
}
