
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApi.Models
{
    [Table("tbl_PRODUCTS")]
    public class Product
    {
        [Key]
        public int PID { get; set; }

        [MaxLength(50)]
        public string? PCode { get; set; }

        [MaxLength(200)]
        public string? PDesc { get; set; }

        [Column(TypeName = "decimal(13,2)")]
        public decimal? PPrice { get; set; }

        public int? PStock { get; set; }

        public int? PLocationID { get; set; }

        public DateTime PSysInsDt { get; set; }

        public int? PSysInsUserID { get; set; }
    }
}