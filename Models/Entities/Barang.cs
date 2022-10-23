using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mendata.Net.Models.Entities;

public class Barang
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [StringLength(10)]
    public string? Kode { get; set; }
    [StringLength(100)]
    public string? Nama { get; set; }
    public string? Description { get; set; }
    public decimal Harga { get; set; }
    public int Stok { get; set; }
    [ForeignKey("Penjual")]
    public int IdPenjual { get; set; }

    [StringLength(250)]
    public string? FileName { get; set; }

    [StringLength(250)]
    public string? Url { get; set; }

    public virtual Penjual Penjual { get; set; }
}