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
            string sql = "SELECT t.TeamName, COUNT(tm.EmpEmail) AS count FROM Teams t LEFT JOIN TeamMembers tm ON t.TeamName = tm.TeamName GROUP BY t.TeamName;";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new List<Team>();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var obj = new Team();
                    obj.TeamName = (string)rdr["TeamName"];
                    obj.Members = (int)rdr["count"];
                    model.Add(obj);
                }
            }
            conn.Close();
            return View(model);
        }
        [HttpGet]
        public IActionResult CreateTeam()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTeam(string teamName)
        {
            try
            {
                SqlMethods.QueryVoid("INSERT INTO Teams(TeamName) VALUES('" + teamName + "')");
                /*SqlConnection conn = new SqlConnection(connectionString);
                string sql = "INSERT INTO Teams(TeamName) VALUES('"+teamName+"')";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (conn)
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                }
                conn.Close();*/
                TempData["name"] = teamName;
                return RedirectToAction("AddTeamMembers");
            }
            catch(Exception ex)
            {
                Team check = GetTeam(teamName);
                System.Diagnostics.Debug.WriteLine(check.TeamName);
                if (check.TeamName == teamName)
                {
                    TempData["msg"] = "Name already taken";
                }
                else
                {
                    TempData["msg"] = ex;
                }
            }
            return View();
        }
        public IActionResult AddTeamMembers()
        {
            string teamName = (string)TempData["name"];
            ViewData["name"] = teamName;
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM AspNetUsers";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new List<Employee>();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var obj = new Employee();
                    obj.Email = (string)rdr["Email"];
                    //obj.Name = (string)rdr["Id"];
                    //Hardcoded bhv en admin
                    obj.BHV = false;
                    obj.Admin = true;
                    model.Add(obj);
                }
            }
            conn.Close();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddTeamMembers(string teamName)
        {
            System.Diagnostics.Debug.WriteLine("teamname = "+ teamName);
            /*foreach (string a in members)
            //{
                System.Diagnostics.Debug.WriteLine(a);
                //SqlMethods.QueryVoid("INSERT INTO teamMembers VALUES(teamName,a);");
            }*/
            return RedirectToAction("Index");
        }

        public IActionResult EditTeam(string name)
        {
            TempData["name"] = name;
            List<Employee> model = GetMembers(name);
            return View(model);
        }
        [HttpPost]
        public IActionResult EditTeam(string[] arr)
        {
            System.Diagnostics.Debug.WriteLine("arrived at editTeam Post => "+arr);
            System.Diagnostics.Debug.WriteLine("=> "+arr);
            return View();
        }
        public IActionResult DetailsTeam(string name)
        {
            TempData["name"] = name;
            List<Employee> model = GetMembers(name);
            return View(model);
        }
        public IActionResult DeleteTeam(string name)
        {
            TempData["name"] = name;
            List<Employee> model = GetMembers(name);
            return View(model);
        }

        public IActionResult DeleteConfirmed(string name)
        {
            System.Diagnostics.Debug.WriteLine(name);
            Deleterow("DELETE Teams WHERE TeamName = '" + name + "'");
            System.Diagnostics.Debug.WriteLine("DELETED " + name);
            return RedirectToAction("Index");
        }
        public Team GetTeam(string name)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT t.TeamName, COUNT(tm.EmpEmail) AS count FROM Teams t LEFT JOIN TeamMembers tm ON t.TeamName = tm.TeamName WHERE t.TeamName = '" + name + "' GROUP BY t.TeamName;";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new Team();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var obj = new Team();
                    obj.TeamName = (string)rdr["TeamName"];
                    obj.Members = (int)rdr["count"];
                    model = obj;
                }
            }
            conn.Close();
            return model;
        }
        public List<Employee> GetMembers(string name)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM Employees JOIN TeamMembers tm ON Email = tm.EmpEmail WHERE tm.TeamName = '" + name + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new List<Employee>();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var obj = new Employee();
                    //obj.Name = (string)rdr["Name"];
                    obj.Email = (string)rdr["Email"];
                    obj.BHV = (bool)rdr["BHV"];
                    obj.Admin = (bool)rdr["Admin"];
                    model.Add(obj);
                }
            }
            conn.Close();
            return model;
        }
        public void Deleterow(string query)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = query;
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
            }
            conn.Close();
        }
    }
}
