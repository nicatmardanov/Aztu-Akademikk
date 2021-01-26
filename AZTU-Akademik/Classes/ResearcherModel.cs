using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AZTU_Akademik.Classes
{
    public class Internal
    {
        public int Id { get; set; }
        public bool Type { get; set; }
    }

    public class External
    {
        public int Id { get; set; }
        public bool Type { get; set; }
    }
    public class Researchers
    {
        public List<Internal> Internals { get; set; }
        public List<External> Externals { get; set; }
    }
}
