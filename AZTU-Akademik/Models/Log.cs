using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Log
    {
        public long Id { get; set; }
        public string TableName { get; set; }
        public string IpAddress { get; set; }
        public string Description { get; set; }
        public string AdditionalInformation { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? RefId { get; set; }
        public int? UserId { get; set; }
        public byte? OperationId { get; set; }

        public virtual Operation Operation { get; set; }
        public virtual User User { get; set; }
    }
}
