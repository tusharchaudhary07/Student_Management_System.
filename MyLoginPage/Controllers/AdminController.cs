using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyLoginPage.Models;
using Oracle.ManagedDataAccess.Client;

namespace MyLoginPage.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        UserDbContext usdb = new UserDbContext();
        StudentDb sdb = new StudentDb();



        [Obsolete]
        public ActionResult AdminDashboard()
        {
            if (Session["USERID"] == null || Session["ROLE"]?.ToString() != "admin")
            {
                return RedirectToAction("Login", "Account");
            }

            List<UserClass> us = usdb.GetAllUsers();
            return View(us);
        }

        //public ActionResult ViewSubmissions()
        //{
        //    List<StudentDetails> students = sdb.GetAllStudentDetails();
        //    return View(students);

        //}

        public ActionResult ViewSubmissions(string searchTerm)
        {
            List<StudentDetails> students = new List<StudentDetails>();

            string cs = ConfigurationManager.ConnectionStrings["Smlhr_DbConnection"].ConnectionString;

            using (OracleConnection con = new OracleConnection(cs))
            {
                string query = "SELECT ID, USERID, NAME, BRANCH, MOBILENO, ADDRESS FROM STUDENT_DETAILS";


                // If search term is provided, add WHERE clause
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query += " WHERE LOWER(NAME) LIKE :term OR LOWER(BRANCH) LIKE :term OR LOWER(MOBILENO) LIKE :term OR LOWER(ADDRESS) LIKE :term OR LOWER(USERID) LIKE :term";
                }


                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        cmd.Parameters.Add(new OracleParameter("term", "%" + searchTerm.ToLower() + "%"));
                    }

                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        StudentDetails s = new StudentDetails
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            USERID = reader["USERID"].ToString(),
                            NAME = reader["NAME"].ToString(),
                            BRANCH = reader["BRANCH"].ToString(),
                            MOBILENO = reader["MOBILENO"].ToString(),
                            ADDRESS = reader["ADDRESS"].ToString()
                        };
                        students.Add(s);
                    }
                }
                return View(students);
            }



        }

    }
    }
