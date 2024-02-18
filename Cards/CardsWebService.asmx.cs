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
    /// author Justus Kasyoki
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CardsWebService : System.Web.Services.WebService
    {
        private DbConnDetails CardConn;
        private Datamap _da;
        private JWTAuthService _authService;
        public CardsWebService()
        {
            CardConn = new DbConnDetails()
            {
                rawConnString = System.Configuration.ConfigurationManager.ConnectionStrings["CardApp"].ToString(),
                rPassword = System.Configuration.ConfigurationManager.AppSettings["CardDbPWD"].ToString(),
                rServerName = System.Configuration.ConfigurationManager.AppSettings["CardDbSV"].ToString(),
                rUserId = System.Configuration.ConfigurationManager.AppSettings["CardDbUI"].ToString()
            };
            CardConn.CompileConnectionString();
            _da = new Datamap(CardConn.ConnString);
            _authService = new JWTAuthService();
        }

        [WebMethod]
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
  
        [WebMethod]
        public void GetCards(string nameFilter = null, string colorFilter = null, CardStatus? statusFilter = null, DateTime? dateFilter = null, int? page = null, int? size = null, int? offset = null, int? limit = null, string sortBy = null)
        {
            string token = GetTokenFromHeader();
            if (_authService.ValidateToken(token))
            {
                IQueryable<Card> query = (IQueryable<Card>)_da.CardList();

                // Apply filters
                if (!string.IsNullOrEmpty(nameFilter))
                    query = query.Where(c => c.Name.Contains(nameFilter));

                if (!string.IsNullOrEmpty(colorFilter))
                    query = query.Where(c => c.Color == colorFilter);

                if (statusFilter.HasValue)
                    query = query.Where(c => c.Status == statusFilter.Value);

                if (dateFilter.HasValue)
                    query = query.Where(c => c.CreationDate.Date == dateFilter.Value.Date);

                // Apply sorting
                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "name":
                            query = query.OrderBy(c => c.Name);
                            break;
                        case "color":
                            query = query.OrderBy(c => c.Color);
                            break;
                        case "status":
                            query = query.OrderBy(c => c.Status);
                            break;
                        case "date":
                            query = query.OrderBy(c => c.CreationDate);
                            break;
                        default:
                            SetResponse(HttpStatusCode.BadRequest, "Invalid sort parameter");
                            return;
                    }
                }

                // Apply pagination
                if (page.HasValue && size.HasValue)
                {
                    int pageNumber = page.Value <= 0 ? 1 : page.Value;
                    int pageSize = size.Value <= 0 ? 10 : size.Value;

                    query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }
                else if (offset.HasValue && limit.HasValue)
                {
                    int offsetValue = offset.Value < 0 ? 0 : offset.Value;
                    int limitValue = limit.Value <= 0 ? 10 : limit.Value;

                    query = query.Skip(offsetValue).Take(limitValue);
                }

                var result = query.ToList();
                SetResponse(HttpStatusCode.OK, result.ToString());
            }
            else
            {
                SetResponse(HttpStatusCode.Unauthorized, "Invalid token");
            }
        }
      


        [WebMethod]
        public void GetCard(int cardId)
        {

            string token = GetTokenFromHeader();
            // VALIDATE THE TOKEN
            if (_authService.ValidateToken(token))
            {
                var User = _da.GetUserFromToken(token).UserId;
                // Retrieve cards from the database
                if (_da.HasAccess(User, cardId)){
                    var cards = _da.UserList().Where(p => p.UserId == User && cardId == cardId);
                    GetResponse(cards, "Success", HttpStatusCode.OK);
                }else
                {
                    GetResponse("", "FAILED", HttpStatusCode.Unauthorized);
                }

            }
            else
            {
                FailedTokenResponse();
            }

        }

     
        [WebMethod]
        public void CreateCard( string name, string description, string color)
        {
            //Get token from header
            string token = GetTokenFromHeader();
            if (_authService.ValidateToken(token))
            {
                var User = _da.GetUserFromToken(token).UserId;

                // Validate name is not empty
                if (string.IsNullOrEmpty(name))
                {
                    SetResponse(HttpStatusCode.BadRequest, "Card name is required.");
                    return;
                }

                // Validate color format (if provided)
                if (!IsValidColorFormat(color))
                {
                    SetResponse(HttpStatusCode.BadRequest, "Invalid color format. Color should be 6 alphanumeric characters prefixed with a #.");
                    return;
                }

                // Create a new card
                var newCard = new Card
                {
                    Name = name,
                    Description = description,
                    Color = color,
                    Status = CardStatus.ToDo,
                    UserId = User, // Set the user ID for the created card,
                    CreationDate =DateTime.Now,
                    
                };

                // Add the card to the database
                var res = _da.CreateCard(newCard);

                SetResponse(HttpStatusCode.Created, "Card created");
            }
            else
            {
                SetResponse(HttpStatusCode.Unauthorized, "Invalid token");
            }
        }
       
        [WebMethod]
        public void UpdateCard( int cardId, string name, string description, string color, CardStatus? status)
        {
            string token = GetTokenFromHeader();
            if (_authService.ValidateToken(token))
            {
                var user = _da.GetUserFromToken(token);

                // Retrieve the existing card from the database based on the cardId
                var existingCard = _da.CardList().Where(c => c.CardId== cardId).FirstOrDefault();

                if (existingCard != null)
                {
                    // Check if the user has access to update this card
                    if (user.Role == UserRole.Admin || (user.Role == UserRole.Member && existingCard.UserId== user.UserId))
                    {
                        // Update the card properties
                        existingCard.Name = !string.IsNullOrEmpty(name) ? name : existingCard.Name;
                        existingCard.Description = description;
                        existingCard.Color = !string.IsNullOrEmpty(color) ? color : existingCard.Color;

                        if (status.HasValue)
                        {
                            // Check if the provided status is a valid value
                            if (Enum.IsDefined(typeof(CardStatus), status.Value))
                            {
                                existingCard.Status = status.Value;
                            }
                            else
                            {
                                SetResponse(HttpStatusCode.BadRequest, "Invalid status");
                                return;
                            }
                        }

                        // Save changes to the database
                        _da.SaveCard(existingCard);

                        SetResponse(HttpStatusCode.OK, "Card updated");
                    }
                    else
                    {
                        SetResponse(HttpStatusCode.Forbidden, "You don't have permission to update this card");
                    }
                }
                else
                {
                    SetResponse(HttpStatusCode.NotFound, "Card not found");
                }
            }
            else
            {
                SetResponse(HttpStatusCode.Unauthorized, "Invalid token");
            }
        }
   

        [WebMethod]
        public void DeleteCard( int cardId)
        {
            string token = GetTokenFromHeader();
            if (_authService.ValidateToken(token))
            {
                var user = _da.GetUserFromToken(token);

                // Retrieve the existing card from the database based on the cardId
                var existingCard = _da.CardList().FirstOrDefault(c => c.CardId == cardId);

                if (existingCard != null)
                {
                    // Check if the user has access to delete this card
                    if (user.Role == UserRole.Admin || (user.Role == UserRole.Member && existingCard.UserId == user.UserId))
                    {
                        // Remove the card from the database
                        _da.CardList().Remove(existingCard);
                     

                        SetResponse(HttpStatusCode.NoContent, "Card deleted");
                    }
                    else
                    {
                        SetResponse(HttpStatusCode.Forbidden, "You don't have permission to delete this card");
                    }
                }
                else
                {
                    SetResponse(HttpStatusCode.NotFound, "Card not found");
                }
            }
            else
            {
                SetResponse(HttpStatusCode.Unauthorized, "Invalid token");
            }
        }
        private void SetResponse(HttpStatusCode statusCode, string message)
        {
            HttpContext.Current.Response.StatusCode = (int)statusCode;
            HttpContext.Current.Response.Write(message);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }


        /// <summary>
        /// return failed response
        /// </summary>
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


        private static void GetResponse(object res,string Msg,HttpStatusCode code)
        {
            var jsonObject = new Response
            {
                  Body=res,
                  Message=Msg,
                status = code
            };

            string json = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            HttpContext.Current.Response.StatusCode = 400;
            HttpContext.Current.Response.Write(json);
        }

        /// <summary>
        /// read the header  details to get the token
        /// </summary>
        /// <returns></returns>
        private string GetTokenFromHeader()
        {
            string authorizationHeader = WebOperationContext.Current?.IncomingRequest.Headers[HttpRequestHeader.Authorization];

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                return authorizationHeader.Substring("Bearer ".Length).Trim();
            }

            return null;
        }

        /// <summary>
        /// Can use the trim method to get token
        /// </summary>
        /// <returns>token</returns>
        private string GetTokenMethod()
        {
            var header = this.Context.Request.Headers["Authorization"];
            var token = (header != null && header.Trim().Length > 1 && header.Contains("Bearer")) ?
                header.Trim().Split(new char[] { ' ' })[1] : null;
            return token;
        }

        /// <summary>
        /// Check if the color starts with '#' and has 6 alphanumeric characters
        /// </summary>
        /// <param name="color">Color Value</param>
        /// <returns>bool</returns>
        private bool IsValidColorFormat(string color)
        {
            // Check if the color starts with '#' and has 6 alphanumeric characters
            return !string.IsNullOrEmpty(color) && color.Length == 7 && color[0] == '#' && color.Substring(1).All(char.IsLetterOrDigit);
        }
    }
}
