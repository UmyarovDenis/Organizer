namespace DataTransferService.DataAccessLayer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            Mails = new HashSet<Mail>();
        }

        public int Id { get; set; }

        public int? Organization_Id { get; set; }

        [StringLength(100)]
        public string Label { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public decimal? ProjectCost { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? Deadline { get; set; }

        public int? Status { get; set; }

        [StringLength(50)]
        public string ImplementationPeriod { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mail> Mails { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
