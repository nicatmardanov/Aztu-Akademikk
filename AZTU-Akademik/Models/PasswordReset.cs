using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class PasswordReset
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Hash { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }

        public virtual User User { get; set; }
    }
}
