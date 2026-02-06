using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using System.Configuration;
using Oracle.ManagedDataAccess.Client;


namespace MyLoginPage.Models
{
    public class UserDbContext
    {
        string cs = ConfigurationManager.ConnectionStrings["Smlhr_DbConnection"].ConnectionString;

        [Obsolete]
        public UserClass ValidateUser(string userid, string password)
        {
            string query = "SELECT USERID,ROLE FROM USERS WHERE USERID = :userid AND PASSWORD = :password";

            using (OracleConnection con = new OracleConnection(cs))
            using (OracleCommand cmd = new OracleCommand(query, con))
            {
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("userid", userid));
                cmd.Parameters.Add(new OracleParameter("password", password));

                con.Open();
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new UserClass
                        {
                            USERID = dr.GetString(0),
                            ROLE = dr.GetString(1)
                            
                        };
                        
                    }
                  
                }
            }
            return null;
        }
    

        [Obsolete]
        public bool RegisterUser(UserClass user)
        {
            string query = "INSERT INTO USERS (USERID,PASSWORD,ROLE) VALUES (:userid,:password,:role)";
            using (OracleConnection con = new OracleConnection(cs))
            using (OracleCommand cmd = new OracleCommand(query, con))
            {
                cmd.Parameters.Add("userid", user.USERID);
                cmd.Parameters.Add("password", user.PASSWORD);
                cmd.Parameters.Add("role", "student");

                con.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }

                catch(OracleException)
                {
                    return false;
                }
            }

        }

        [Obsolete]
        public List<UserClass> GetAllUsers()
        {
            List<UserClass> users = new List<UserClass>();
            string query = "SELECT USERID,ROLE FROM USERS";
            using(OracleConnection con = new OracleConnection(cs))
            using(OracleCommand cmd = new OracleCommand(query, con))
            {
                con.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    users.Add(new UserClass
                    {
                        USERID = dr.GetString(0),
                         ROLE = dr.GetString(1)
                    });
                }
            }
            return users;

        }
    }
}