namespace Mendata.Net.Models;

public class RequestBarang
{
    public int Id { get; set; }
    public string? Kode { get; set; }
    public string? Nama { get; set; }
    public string? Description { get; set; }
    public decimal Harga { get; set; }
    public int Stok { get; set; }
    public IFormFile? imgname { get; set; }
    public string? url { get; set; }

}