using BL;
using Cards.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Cards
{
    /// <summary>
    /// Summary description for CardsWebService
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
        [System.Web.Script.Services.ScriptService]
    public class CardsWebService : System.Web.Services.WebService
    {
        //private BL _da;
        private JWTAuthService _authService;
        public CardsWebService()
        {
           
            _authService = new JWTAuthService();
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetAuthToken(AuthPayload authenticate)
        {

            // validate payload
            if (authenticate.User == null || authenticate.Password == null ||
                (authenticate.User.Trim().ToLower() != JWTAuthService.USER.ToLower()
                || authenticate.Password.Trim() != JWTAuthService.PASSWORD))
            {
                var jsonObject = new TokenResponse
                {
                    token = null,
                    status = "Invalid Username or Password"
                };
                string json = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                HttpContext.Current.Response.StatusCode = 400;
                HttpContext.Current.Response.Write(json);
                return;
            }

            var token = _authService.CreateToken(authenticate.User); // GENERATE THE TOKEN

            if (token != null && token.Trim().Length > 1)
            {
                var jsonObject = new TokenResponse
                {
                    token = token,
                    status = "Success"
                };
                string json = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.Response.Write(json);
            }
            else
            {
                var jsonObject = new TokenResponse
                {
                    token = null,
                    status = "Bad request."
                };

                string json = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                HttpContext.Current.Response.StatusCode = 400;
                HttpContext.Current.Response.Write(json);
            }


        }
        private const string ConnectionString = "your_database_connection_string";
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetCards()
        {
            string token = GetTokenFromHeader();
            // VALIDATE THE TOKEN
            if (_authService.ValidateToken(token))
            {
                // Retrieve cards from the database
                return _context.Cards.ToList().ToString();

            }
            else
            {
                FailedTokenResponse();
            }
           
        }

        [WebMethod]
        public string GetCard(int cardId)
        {

            string token = GetTokenFromHeader();
            // VALIDATE THE TOKEN
            if (_authService.ValidateToken(token))
            {
                // Retrieve cards from the database
                return _context.Cards.ToList().ToString();

            }
            else
            {
                FailedTokenResponse();
            }

        }

        [WebMethod]
        public void CreateCard(string name, string description, string color)
        {
            string token = GetTokenFromHeader();
            // VALIDATE THE TOKEN
            if (_authService.ValidateToken(token))
            {
                // Retrieve cards from the database
                return _context.Cards.ToList().ToString();

            }
            else
            {
                FailedTokenResponse();
            }

        }

        [WebMethod]
        public void UpdateCard(int cardId, string name, string description, string color, string status)
        {
            string token = GetTokenFromHeader();
            // VALIDATE THE TOKEN
            if (_authService.ValidateToken(token))
            {
                // Retrieve cards from the database
                return _context.Cards.ToList().ToString();

            }
            else
            {
                FailedTokenResponse();
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"UPDATE Cards SET Name = '{name}', Description = '{description}', Color = '{color}', Status = '{status}' WHERE Id = {cardId}", connection);
                command.ExecuteNonQuery();
            }

            return "Card updated";
        }

        [WebMethod]
        public void DeleteCard(int cardId)
        {
            string token = GetTokenFromHeader();
            // VALIDATE THE TOKEN
            if (_authService.ValidateToken(token))
            {
                // Retrieve the card from the database
                var cardToDelete = _context.Cards.FirstOrDefault(c => c.Id == cardId);

                if (cardToDelete != null)
                {
                    // Remove the card from the database
                    _context.Cards.Remove(cardToDelete);
                    _context.SaveChanges();

                    return "Card deleted";
                }
                else
                {
                    return "Card not found";
                }

            }
            else
            {
                FailedTokenResponse();
            }


        }

        private static void FailedTokenResponse()
        {
            var jsonObject = new TokenResponse
            {
                token = "Invalid token.",
                status = "Bad request."
            };

            string json = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            HttpContext.Current.Response.StatusCode = 400;
            HttpContext.Current.Response.Write(json);
        }

        private string GetTokenFromHeader()
        {
            string authorizationHeader = WebOperationContext.Current?.IncomingRequest.Headers[HttpRequestHeader.Authorization];

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                return authorizationHeader.Substring("Bearer ".Length).Trim();
            }

            return null;
        }
        private string GetTokenMethod()
        {
            var header = this.Context.Request.Headers["Authorization"];
            var token = (header != null && header.Trim().Length > 1 && header.Contains("Bearer")) ?
                header.Trim().Split(new char[] { ' ' })[1] : null;
            return token;
        }
    }
}
