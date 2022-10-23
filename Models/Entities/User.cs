using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mendata.Net.Models.Entities;

public class User {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get;set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Tipe { get; set; }
}