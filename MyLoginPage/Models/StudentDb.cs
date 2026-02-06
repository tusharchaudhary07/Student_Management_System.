using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace MyLoginPage.Models
{ 
    public class StudentDb
    {
        string cs = ConfigurationManager.ConnectionStrings["Smlhr_DbConnection"].ConnectionString;
        public List<StudentDetails> GetAllStudentDetails()
        {
            List<StudentDetails> list = new List<StudentDetails>();

            using (OracleConnection con = new OracleConnection(cs))
            {
                string query = "SELECT ID, USERID, NAME, BRANCH, MOBILENO, ADDRESS FROM STUDENT_DETAILS";
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new StudentDetails
                    {
                        ID = dr.GetInt32(0),
                        USERID = dr.GetString(1),
                        NAME = dr.GetString(2),
                        BRANCH = dr.GetString(3),
                        MOBILENO = dr.GetString(4),
                        ADDRESS = dr.GetString(5)
                    });
                }
            }

            return list;
        }

        public StudentDetails GetStudentDetailsByUserId(string userid)
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

        public void UpdateStudentDetails(StudentDetails student)
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
        }




    }
}