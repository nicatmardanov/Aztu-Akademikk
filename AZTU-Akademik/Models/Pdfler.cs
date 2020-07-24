using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Pdfler
    {
        public int Id { get; set; }
        public int MeqaleId { get; set; }
        public string PdfLocation { get; set; }

        public virtual Meqaleler Meqale { get; set; }
    }
}
