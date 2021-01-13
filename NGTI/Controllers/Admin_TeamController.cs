using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NGTI.Models;

namespace NGTI.Controllers
{
    
    public class Admin_TeamController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;
        public Admin_TeamController(UserManager<ApplicationUser> userManager)
        {
            _userManger = userManager;
        }
        //sql connectionstring var
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";
        public IActionResult Overview()
        {
            SqlConnection conn = new SqlConnection(connectionString); 
            var id = _userManger.GetUserId(HttpContext.User);
            string sql = $"SELECT t.TeamName, COUNT(tm.UserId) AS count FROM Teams t LEFT JOIN TeamMembers tm ON t.TeamName = tm.TeamName WHERE t.teamname IN(SELECT DISTINCT teamname from teammembers WHERE userid = N'{id}') GROUP BY t.TeamName;";
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
        //view teams
        public IActionResult Index()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT t.TeamName, COUNT(tm.UserId) AS count FROM Teams t LEFT JOIN TeamMembers tm ON t.TeamName = tm.TeamName GROUP BY t.TeamName;";
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
            List<Employee> model = SqlMethods.GetUsers();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddTeamMembers(IEnumerable<string> members,string teamName)
        {
            foreach (string a in members)
            {
                if (a != "false" && a != "False")
                {
                    SqlMethods.QueryVoid("INSERT INTO teamMembers VALUES('"+teamName+"','"+a+"');");
                }
            }
            return RedirectToAction("Overview");
        }

        public IActionResult EditTeam(string name)
        {
            TempData["name"] = name;
            List<Employee> users = SqlMethods.GetUsersForEdit(name);
            List<Employee> members = GetMembers(name);
            EditTeam model = new EditTeam { TeamMembers = members, Users = users };
            return View(model);
        }
        [HttpPost]
        public IActionResult EditTeam(IEnumerable<string> AddMembers, IEnumerable<string> DelMembers, string TeamName,string newTeamName)
        {
            System.Diagnostics.Debug.WriteLine(TeamName+" "+newTeamName);
            if(newTeamName != TeamName)
            {
                try
                {
                    SqlMethods.QueryVoid("UPDATE Teams SET TeamName = '"+ newTeamName +"' WHERE TeamName = '"+ TeamName +"'");
                    TeamName = newTeamName;
                }
                catch (Exception ex)
                {
                    Team check = GetTeam(newTeamName);
                    if (check.TeamName == newTeamName)
                    {
                        TempData["msg"] = "Name already taken";
                    }
                    else
                    {
                        TempData["msg"] = ex;
                    }
                }
            }
            foreach (string a in AddMembers)
            {
                if (a != "false" && a != "False")
                {
                    SqlMethods.QueryVoid("INSERT INTO teamMembers VALUES('" + TeamName + "','" + a + "');");
                }
            }
            foreach (string a in DelMembers)
            {
                if (a != "false" && a != "False")
                {
                    SqlMethods.QueryVoid("DELETE FROM teamMembers WHERE TeamName = '" + TeamName +"' AND UserId = '" + a +"'");
                }
            }
            return RedirectToAction("Overview");
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
            Deleterow("DELETE Teams WHERE TeamName = '" + name + "'");
            System.Diagnostics.Debug.WriteLine("DELETED " + name);
            return RedirectToAction("Overview");
        }
        public Team GetTeam(string name)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT t.TeamName, COUNT(tm.UserId) AS count FROM Teams t LEFT JOIN TeamMembers tm ON t.TeamName = tm.TeamName WHERE t.TeamName = '" + name + "' GROUP BY t.TeamName;";
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
            string sql = "SELECT a.Id, a.Email, a.BHV, a.Admin FROM AspNetUsers a JOIN TeamMembers tm ON a.Id = tm.UserId WHERE tm.TeamName = '" + name + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new List<Employee>();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var obj = new Employee();
                    obj.Id = (string)rdr["Id"];
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
