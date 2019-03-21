namespace DataTransferService.DataAccessLayer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Mail
    {
        public int Id { get; set; }

        public int? Tasks_Id { get; set; }

        [StringLength(100)]
        public string Sender { get; set; }

        [StringLength(100)]
        public string MailTheme { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public DateTime? DepartingDate { get; set; }

        public virtual Task Tasks { get; set; }
    }
}
