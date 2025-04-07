using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOBR.Models.LOBRCOnfiguration
{
    [Table("HRData", Schema = "dbo")]
    public partial class Hrdatum
    {
        [Key]
        [Column("EMPLID")]
        [Required]
        public int Emplid { get; set; }

        [Column("EMPLIZ_NLZ")]
        public string EmplizNlz { get; set; }

        [Column("ELID_NW")]
        public string ElidNw { get; set; }

        [Column("TCField_DisplayNameLFM")]
        public string TcfieldDisplayNameLfm { get; set; }
    }
}