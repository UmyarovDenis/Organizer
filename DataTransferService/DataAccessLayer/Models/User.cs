using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferService.DataAccessLayer.Models
{
    public partial class User
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string FIO { get; set; }

        [StringLength(70)]
        public string Login { get; set; }

        [StringLength(70)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
    }
}
