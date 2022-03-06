namespace Tesco.Framework.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Account")]
    public partial class Account
    {
        public Account()
        {
        }

        public Account(string username, string password)
        {
            Username = username;
            Password = password;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int PersonalDetailsID { get; set; }

        public int AddressID { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string ClubCardNumber { get; set; }

        public int ClubCardStatus { get; set; }

        public int MarketingCommunication { get; set; }

        public virtual Address Address { get; set; }

        public virtual ClubCardStatu ClubCardStatu { get; set; }

        public virtual MarketingCommunication MarketingCommunication1 { get; set; }

        public virtual PersonalDetail PersonalDetail { get; set; }
    }
}