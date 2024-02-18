
using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// Author: Justus
    /// </summary>
    #region Enum
    //if you have multiple db
    public enum DatabaseType { Oracle = 0, MsSql = 1 }
    public enum UserRole
    {
        Member,
        Admin
    }

    public enum CardStatus
    {
        ToDo,
        InProgress,
        Done
    }
    #endregion Enum
    #region classes
    public class DbConnDetails
    {
        public string rawConnString { get; set; }
        public string ConnString { get; set; }
        public string ConnString2 { get; set; }
        public string rServerName { get; set; }
        public string rUserId { get; set; }
        public string rPassword { get; set; }
        public DatabaseType dbType { get; set; }
        public string Error { get; set; }

    
        public DbConnDetails()
        {
            dbType = DatabaseType.MsSql;
            this.rawConnString = "";
            this.Error = "";
        }

        public bool CompileConnectionString()
        {
            this.Error = "";
            try
            {
                if (!string.IsNullOrEmpty(rawConnString))
                {
                    //----- Validate Connection string
                    if (!this.rawConnString.Contains("{UI}") || !this.rawConnString.Contains("{SV}") || !this.rawConnString.Contains("{PD}"))
                    {
                        this.Error = "Invalid connection string!";
                        return false;
                    }
                    //-----Add server
                    if (!string.IsNullOrEmpty(this.rServerName))
                    {
                        //---- Decrypt value and assign
                        this.ConnString = this.rawConnString.Replace("{SV}", Crypto.DecryptString(this.rServerName));
                    }
                    //-----Add User Name
                    if (!string.IsNullOrEmpty(this.rUserId))
                    {
                        //---- Decrypt value and assign
                        this.ConnString = this.ConnString.Replace("{UI}", Crypto.DecryptString(this.rUserId));
                    }
                    //-----Add Password
                    if (!string.IsNullOrEmpty(this.rPassword))
                    {
                        //---- Decrypt value and assign
                        this.ConnString = this.ConnString.Replace("{PD}", Crypto.DecryptString(this.rPassword));
                    }
                    //------Get connection 2
                    if (this.ConnString.Trim().StartsWith("metadata"))
                    {
                        this.ConnString2 = this.ConnString.Substring(this.ConnString.IndexOf("Data Source"));
                        if (this.ConnString2.EndsWith("\""))
                            this.ConnString2 = this.ConnString2.Substring(0, this.ConnString2.Length - 1);
                    }
                    else
                        this.ConnString2 = this.ConnString;
                }
                return true;
            }
            catch (Exception ex)
            {
                this.Error = string.Format("Error! {0}", ex.Message);
                return false;
            }
        }
    }
    public class AuthPayload
    {
        public string User { get; set; }
        public string Password { get; set; }
   
    }

    public class TokenResponse
    {
        public string token { get; set; }
        public string status { get; set; }
    }

    public class Response
    {
        public string status { get; set; }
        public string Message { get; set; }
        public string Body { get; set; }
     
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
    }

    public class Card
    {
        [Key]
        public int CardId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        [Required]
        public CardStatus Status { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }

    #endregion

    public class Datamap
    {
        public DataAccess _da;
        public Datamap(string conn)
        {
            _da = new DataAccess(conn);
        }

        /// <summary>
        /// used to excute all Sql Queries to Database
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public int ExecuteQuery(string sqlQuery)
        {
            try
            {
                _da.Exec(sqlQuery);
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        /// <summary>
        /// used when exceuting Stored Procedures
        /// </summary>
        /// <param name="sql">Query String</param>
        /// <param name="param">Parameters need by the Store procedure</param>
        public void ExecuteSql(string sql, SqlParameter[] param)
        {
            _da.Exec(sql, param);
        }

    }
}
