using Articles.Data;
using Articles.Models;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Articles.Controllers
{
    public class ArticleController : Controller
    {
     
        public ActionResult Index()
        {
            List<Article> articles = new List<Article>();

            ArticleDAO articleDAO = new ArticleDAO();

            articles = articleDAO.GetAll();

            return View("Index", articles);
        }

        public ActionResult Details(int id)
        {
            ArticleDAO articleDAO = new ArticleDAO(); 

            Article article = articleDAO.GetById(id);

            return View("Details", article);
        }

        public ActionResult Create()
        {
            return View("ArticleForm");
        }

        public ActionResult ProccesCreate(Article article)
        {
            ArticleDAO articleDAO = new ArticleDAO();

            articleDAO.Create(article);

            return View("Details", article);
        }

        public ActionResult Rate(int id)
        {
            ArticleDAO articleDAO = new ArticleDAO();

            Article article = articleDAO.GetById(id);

            return View("EditForm", article);
        }

        public ActionResult ProccesUpdate(Article article)
        {
            ArticleDAO articleDAO = new ArticleDAO();

            var userId = articleDAO.GetIdByUsername(User.Identity.Name);

            articleDAO.Update(article, userId);

            return View("Rating", article);
        }

        public ActionResult EditArticle(int id)
        {
            ArticleDAO articleDAO = new ArticleDAO();

            Article article = articleDAO.GetById(id);

            return View("ArticleUpdate", article);
        }

        public ActionResult ProccesUpdateArticle(Article article)
        {
            ArticleDAO articleDAO = new ArticleDAO();

            articleDAO.UpdateArticle(article);

            return View("Details", article);
        }

        public ActionResult Delete(int id)
        {
            ArticleDAO articleDAO = new ArticleDAO();
            articleDAO.Delete(id);

            List<Article> articles = articleDAO.GetAll();

            return View("Index", articles);
        }

        public ActionResult ExportToExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Articles");

                // Header
                ws.Cell(1, 1).Value = "ID";
                ws.Cell(1, 2).Value = "Name";
                ws.Cell(1, 3).Value = "Category";
                ws.Cell(1, 4).Value = "Price";
                ws.Range("A1:D1").Style.Fill.BackgroundColor = XLColor.Green;

                // Connect to database
                System.Data.DataTable dt = new System.Data.DataTable();
                SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-Articles-20240223013831;Integrated Security=True;Connect Timeout=30");
                SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM dbo.Articles", con);
                ad.Fill(dt);

                int i = 2; 

                foreach (System.Data.DataRow row in dt.Rows)
                {
                    ws.Cell(i, 1).Value = row[0].ToString();
                    ws.Cell(i, 2).Value = row[1].ToString();
                    ws.Cell(i, 3).Value = row[2].ToString();
                    ws.Cell(i, 4).Value = row[3].ToString();
                    i = i + 1;
                }

                i = i - 1;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument-spreadsheetml.sheet",
                        "ArticleExcel.xlsx"
                        );
                }

            }
        }
    }
}