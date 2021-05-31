using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using OrderViewer.Models;

namespace OrderViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Data()
        {
            ViewBag.Message = "Data fetched from SQL Server";

            List<OrderEntry> orderList = new List<OrderEntry>();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection("Integrated Security = SSPI; " +
            "Initial Catalog=mydatabase;" +
            "Data Source=localhost;"))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM OrderEntries", con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        OrderEntry orderEntry = new OrderEntry();
                        orderEntry.Status = (bool)ds.Tables[0].Rows[i]["status"];
                        orderEntry.FileName = (string)ds.Tables[0].Rows[i]["filename"];
                        orderEntry.FileDate = (DateTime)ds.Tables[0].Rows[i]["filedate"];
                        orderList.Add(orderEntry);
                    }
                }
                con.Close();
            }
            return View(orderList);
        }

    }
}