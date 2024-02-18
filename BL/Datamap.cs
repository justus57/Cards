
using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BL
{

    #region Enum
    //if you have multiple db
    public enum DatabaseType { Oracle = 0, MsSql = 1 }
    public enum UserRole
    {
        Member = 0,
        Admin = 1
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
        public HttpStatusCode status { get; set; }
        public string Message { get; set; }
        public object Body { get; set; }

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

        public int UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual User User { get; set; }
    }

    #endregion

    public class Datamap
    {
        public DataAccess _da;
        private List<CARDS> test;

        public Datamap(string conn)
        {
            _da = new DataAccess(conn);
        }

        #region Access

        public bool HasAccess(int userid, int cardid)
        {

            if (isAdmin(userid))
            {
                test = _da.Search<CARDS>(x => x.CARDID == cardid).ToList();
            }
            else
            {
                test = _da.Search<CARDS>(x => x.CARDID == cardid & x.STATUSID == 3 & x.USERID == userid).ToList();
            }



            if (test.Count > 0)
            {
                return true;
            }
            return false;

            //Include("MENU").ToList()
        }

        private bool isAdmin(int userid)
        {
            var IsAdmin = UserList().Where(X => X.UserId == userid).ToList();
            if (IsAdmin.Count > 0)
            {
                return true;
            }
            return false;
        }
        #endregion
        #region Cards
        private Card AppObject(CARDS u)
        {
            return new Card
            {
                Color = u.COLOR,
                Name = u.NAME,
                Description = u.DESCRIPTION,
                CardId = u.CARDID,
                UserId = (int)u.USERID,
                Status = (CardStatus)u.STATUSID,
                CreationDate = (DateTime)u.CREATIONDATE

            };
        }
        private CARDS CardObj(Card u)
        {
            return new CARDS
            {
                COLOR = u.Color,
                NAME = u.Name,
                DESCRIPTION = u.Description,
                STATUSID = (int)CardStatus.ToDo,
                USERID = u.UserId,


            };
        }

        private int? GetuserId()
        {
            throw new NotImplementedException();
        }

        public List<Card> CardList()
        {
            return (from p in _da.Fetch<CARDS>().ToList() select AppObject(p)).ToList();
        }

        public Card SaveCard(Card o)
        {

            CARDS u = CardObj(o);
            _da.Save(u);
            return AppObject(u);
        }
        public string CreateCard(Card card)
        {
            try
            {
                //with out mekarchecker process
                var sql = $"INSERT INTO [dbo].[CARDS] ([NAME], [DESCRIPTION], [COLOR], [STATUSID], [USERID], [CREATIONDATE]) VALUES ('{card.Name}', '{card.Description}', '{card.Color}', { (int)card.Status }, {card.UserId}, GETDATE())";
                _da.Exec(sql);
                // can create a maker checker  below
                return "success";
            }
            catch (Exception es)
            {
                return es.ToString();
            }
        }
        #endregion

        public User GetUserFromToken(string token)
        {


            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken != null)
                {
                    var userEmail = jsonToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        // Retrieve user information based on the email
                        var user = UserList().Where(u => u.Email == userEmail).FirstOrDefault();

                        if (user != null)
                        {
                            return user;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return null;
        }
        #region User

        private User UserObject(USERS u)
        {
            return new User
            {
                UserId = u.USERID,
                Email = u.EMAIL,
                Password = u.PASSWORD,
                Role = (UserRole)Enum.Parse(typeof(UserRole), u.ROLE)

            };
        }
        public List<User> UserList()
        {
            return (from p in _da.Fetch<USERS>().ToList() select UserObject(p)).ToList();
        }


        #endregion
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
