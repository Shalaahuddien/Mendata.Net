using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mendata.Net.Models.Entities;

public class Penjual
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [ForeignKey("User")]
    public int IdUser { get; set; }
    public string? NamaToko { get; set; }
    public string? Alamat { get; set; }

    public virtual User User { get; set; }
    public virtual ICollection<Barang> Barangs { get; set; }
}