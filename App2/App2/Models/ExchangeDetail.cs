using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App2.Models
{
    public class ExchangeDetail
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string Code { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(18,5)")]
        public decimal RateFormated { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(18,5)")]
        public decimal DiffFormated { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(18,5)")]
        public decimal Rate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(18,5)")]
        public decimal Diff { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime ValidFromDate { get; set; }
    }
}
