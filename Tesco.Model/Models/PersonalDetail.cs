namespace Tesco.Framework.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
using Tesco.Framework.Models;

    public partial class PersonalDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PersonalDetail()
        {
            Accounts = new HashSet<Account>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string Title { get; set; }

        public string Title_NoFullStop => Title.Replace(".", "");

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string FullTitleName => $"{Title_NoFullStop} {FirstName} {LastName}";

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(30)]
        public string PhoneNumber { get; set; }

        [StringLength(30)]
        public string MobileNumber { get; set; }

        [Required]
        [StringLength(300)]
        public string EmailAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
