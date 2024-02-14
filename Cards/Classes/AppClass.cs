using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cards.Classes
{

    #region Enum
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

    #region  public Class
    public class AppClass
    {
    }
 
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public CardStatus Status { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }

    #endregion public Class
}