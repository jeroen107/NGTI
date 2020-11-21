using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NGTI.Models;

namespace NGTI.Controllers
{
    public class Admin_TeamController : Controller
    {
        //sql connectionstring var
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";
        public IActionResult Index()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM Teams";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new List<Team>();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var obj = new Team();
                    obj.Id = (int)rdr["Id"];
                    obj.TeamName = (string)rdr["TeamName"];
                    model.Add(obj);
                }
            }
            conn.Close();
            return View(model);
        }

        public IActionResult CreateTeam()
        {
            return View();
        }

        public IActionResult DetailsTeam(int id)
        {
            Team model = sqlcommnd(id);
            return View(model);
        }

        public IActionResult DeleteTeam(int id)
        {
            Team model = sqlcommnd(id);
            return View(model);
        }

        public Team sqlcommnd(int id)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM Teams WHERE Id = id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new Team();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var obj = new Team();
                    obj.Id = (int)rdr["Id"];
                    obj.TeamName = (string)rdr["TeamName"];
                    model = obj;
                }
            }
            conn.Close();
            return model;
        }
    }
}
