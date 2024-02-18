using Cards.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Datamap
    {
        public DataAccess _da;
        public Datamap(string conn)
        {
            _da = new DataAccess(conn);
        }
    }
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
    #region classes
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
}
