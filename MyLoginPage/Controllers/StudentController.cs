using System;
using System.Configuration;
using System.Web.Mvc;
using Oracle.ManagedDataAccess.Client;
using MyLoginPage.Models;

namespace MyLoginPage.Controllers
{
    public class StudentController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["Smlhr_DbConnection"].ConnectionString;

        // Check if student has already submitted details
        [HttpGet]
        public ActionResult SubmitDetails()
        {

            string userid = Session["USERID"]?.ToString();
            if (userid == null)
                return RedirectToAction("Login", "Account");

            StudentDetails student = GetStudentDetailsByUserId(userid);
            if (student != null)
            {
                return RedirectToAction("SubmitSuccess");
            }

            return View();
        }

        [HttpPost]
        public ActionResult SubmitDetails(StudentDetails student)
        {
            System.Diagnostics.Debug.WriteLine("SubmitDetails POST CALLED");

            if (ModelState.IsValid)
            {
                student.USERID = Session["USERID"]?.ToString();

                using (OracleConnection con = new OracleConnection(cs))
                {
                    string query = "INSERT INTO STUDENT_DETAILS (USERID, NAME, BRANCH, MOBILENO, ADDRESS) " +
                                   "VALUES (:userid, :name, :branch, :mobileno, :address)";
                    using (OracleCommand cmd = new OracleCommand(query, con))
                    {
                        cmd.Parameters.Add(new OracleParameter("userid", student.USERID));
                        cmd.Parameters.Add(new OracleParameter("name", student.NAME));
                        cmd.Parameters.Add(new OracleParameter("branch", student.BRANCH));
                        cmd.Parameters.Add(new OracleParameter("mobileno", student.MOBILENO));
                        cmd.Parameters.Add(new OracleParameter("address", student.ADDRESS));

                        con.Open();
                        int rows = cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine($"Inserted {rows} row(s)");
                    }
                }

                return RedirectToAction("SubmitSuccess");
            }

            ViewBag.Message = "Please correct the form.";
            return View(student);
        }

       
        public ActionResult SubmitSuccess()
        {
            if (Session["USERID"] == null)
                return RedirectToAction("Login", "Account");

            string userid = Session["USERID"]?.ToString();
            StudentDetails student = GetStudentDetailsByUserId(userid);
            return View(student);
        }

        [HttpGet]
        public ActionResult EditDetails()
        {
            string userid = Session["USERID"]?.ToString();
            StudentDetails student = GetStudentDetailsByUserId(userid);
            return View(student);
        }

        [HttpPost]
        public ActionResult EditDetails(StudentDetails student)
        {
            using (OracleConnection con = new OracleConnection(cs))
            {
                string query = "UPDATE STUDENT_DETAILS SET NAME = :name, BRANCH = :branch, MOBILENO = :mobileno, ADDRESS = :address WHERE ID = :id";
                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.Parameters.Add(new OracleParameter("name", student.NAME));
                    cmd.Parameters.Add(new OracleParameter("branch", student.BRANCH));
                    cmd.Parameters.Add(new OracleParameter("mobileno", student.MOBILENO));
                    cmd.Parameters.Add(new OracleParameter("address", student.ADDRESS));
                    cmd.Parameters.Add(new OracleParameter("id", student.ID));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("SubmitSuccess");
        }

        // Helper method to check if student already submitted
        private StudentDetails GetStudentDetailsByUserId(string userid)
        {
            StudentDetails student = null;

            using (OracleConnection con = new OracleConnection(cs))
            {
                string query = "SELECT ID, USERID, NAME, BRANCH, MOBILENO, ADDRESS FROM STUDENT_DETAILS WHERE USERID = :userid";
                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.Parameters.Add(new OracleParameter("userid", userid));
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            student = new StudentDetails
                            {
                                ID = dr.GetInt32(0),
                                USERID = dr.GetString(1),
                                NAME = dr.GetString(2),
                                BRANCH = dr.GetString(3),
                                MOBILENO = dr.GetString(4),
                                ADDRESS = dr.GetString(5)
                            };
                        }
                    }
                }
            }

            return student;
        }
    }
}
