﻿using Articles.Data;
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
        // GET: Article
        public ActionResult Index()
        {
            List<Article> articles = new List<Article>();

            ArticleDAO articleDAO = new ArticleDAO();

            articles = articleDAO.GetAll();

            return View("Index", articles);
        }

        public ActionResult Details(int id)
        {
            ArticleDAO articleDAO = new ArticleDAO(); // wihtout DP to make it as simple as possible

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

        public ActionResult Edit(int id)
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

        public ActionResult Delete(int id)
        {
            ArticleDAO articleDAO = new ArticleDAO();
            articleDAO.Delete(id);

            List<Article> articles = articleDAO.GetAll();

            return View("Index", articles);
        }

        public ActionResult SearchForm()
        {
            return View("SearchForm");
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
                ws.Cell(1, 5).Value = "Rating";
                ws.Range("A1:E1").Style.Fill.BackgroundColor = XLColor.Green;

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
                    ws.Cell(i, 5).Value = row[4].ToString();
                    i = i + 1;
                }

                i = i - 1;

                //  ws.Cells("A4:E" + 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //  ws.Cells("A4:E" + 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //  ws.Cells("A4:E" + 1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //  ws.Cells("A4:E" + 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

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