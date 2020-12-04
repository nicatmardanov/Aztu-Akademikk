using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Operation
    {
        public Operation()
        {
            Log = new HashSet<Log>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Log> Log { get; set; }
    }
}
