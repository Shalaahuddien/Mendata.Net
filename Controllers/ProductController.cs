using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mendata.Net.Models;
using Mendata.Net.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Mendata.Net.Controllers;

[Authorize(Roles = "Penjual")]

public class ProductController : Controller
{

    //    static List<ProductInput> produk = new List<ProductInput>(){
    //         new ProductInput {code = "01", nama = "laptop keren", harga = "5000", deskripsi = "laptop keren bisa gaming" }
    //     };


    private readonly ILogger<ProductController> _logger;
    private readonly dbcontext _dbContext;


    public ProductController(ILogger<ProductController> logger, dbcontext dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;

    }
    [HttpGet]
    public IActionResult Index()
    {
        List<Barang> barangs = _dbContext.Barangs.ToList();
        return View(barangs);
    }

    //buat delete
    [HttpGet]
    public IActionResult Delete(int Id)
    {
        Barang br = _dbContext.Barangs.Find(Id);
        _dbContext.Barangs.Remove(br);
        _dbContext.SaveChanges();
        // List<Barang> barangs = _dbContext.Barangs.ToList();
        return RedirectToAction("Index");
    }

    //input data

    public IActionResult InputData()
    {
        //  Barang br = _dbContext.Barangs.First(x => x.Id == barangs);
        return View();
    }

    [HttpPost]
    public IActionResult InputData(RequestBarang br)
    {
        var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        var filename = $"{br.Kode}-{br.imgname.FileName}";
        var filepath = Path.Combine(folder, filename);
        using var stream = System.IO.File.Create(filepath);
        if (br.imgname != null)
        {
            br.imgname.CopyTo(stream);
        }
        var url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/images/{filename}";

        Barang input = new Barang
        {
            Kode = br.Kode,
            Nama = br.Nama,
            Harga = br.Harga,
            Description = br.Description,
            Stok = br.Stok,
            FileName = filename,
            Url = url,
            IdPenjual = 5
        };
        _dbContext.Barangs.Add(input);
        _dbContext.SaveChanges();
        List<Barang> data = _dbContext.Barangs.ToList();
        return View("Index", data);

    }

    //buat edit data
    [HttpGet]
    public IActionResult Editdata(int id)
    {
        Barang br = _dbContext.Barangs.First(x => x.Id == id);

        RequestBarang data = new RequestBarang
        {
            Nama = br.Nama,
            Id = br.Id,
            Description = br.Description,
            Harga = br.Harga,
            Stok = br.Stok,
            Kode = br.Kode
        };
        return View(data);
    }

    [HttpPost]
    public IActionResult Editdata(RequestBarang br)
    {
        var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        var filename = $"{br.Kode}-{br.imgname.FileName}";
        var filepath = Path.Combine(folder, filename);
        using var stream = System.IO.File.Create(filepath);
        if (br.imgname != null)
        {
            br.imgname.CopyTo(stream);
        }
        var url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/images/{filename}";

        Barang updated = _dbContext.Barangs.First(x => x.Id == br.Id);

        var Deletedfilepath = Path.Combine(folder, updated.FileName);
        System.IO.File.Delete(Deletedfilepath);

        updated.Kode = br.Kode;
        updated.Harga = br.Harga;
        updated.Nama = br.Nama;
        updated.Stok = br.Stok;
        updated.Description = br.Description;
        updated.Url = url;
        updated.FileName = filename;
        _dbContext.SaveChanges();
        return RedirectToAction("Index");
    }

    // [HttpPost]
    // public IActionResult UploadFoto(RequestBarang br)
    // {
    //     var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
    //     var filename = $"{br.Kode}-{br.imgname.FileName}";
    //     var filepath = Path.Combine(folder, filename);
    //     using var stream = System.IO.File.Create(filepath);
    //     if (br.imgname != null)
    //     {
    //         br.imgname.CopyTo(stream);
    //     }
    //     var url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/images/{filename}";

    //     Barang input = new Barang
    //     {
    //         Kode = br.Kode,
    //         Nama = br.Nama,
    //         Harga = br.Harga,
    //         Description = br.Description,
    //         Stok = br.Stok,
    //         FileName = filename,
    //         Url = url,
    //         IdPenjual = 5
    //     };
    //     _dbContext.Barangs.Add(input);
    //     _dbContext.SaveChanges();
    //     List<Barang> data = _dbContext.Barangs.ToList();
    //     return View("Index", data);

    // }




    // [HttpPost, ActionName("Delete")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> DeleteConfirmed(int id)
    // {
    //     var Barang = await _dbcontext.Barang.FindAsync(id);
    //     _dbcontext.Transactions.Remove(Barang);
    //     await _dbcontext.SaveChangesAsync();
    //     return RedirectToAction(nameof(Index));
    // }



    // [HttpDelete]
    // public IActionResult Delete(int id)
    // {
    //     _dbContext.Destroy(id);
    //     return Ok(new { message = "User deleted" });
    // }


    // [HttpDelete]
    // [ProducesResponseType((int)dbcontext.NotFound)]
    // [ProducesResponseType((int)dbcontext.NoContent)]
    // public async Task<IActionResult> Delete(int id)
    // {
    //     var existingItem = await dbcontext.Customer.SingleOrDefaultAsync((System.Linq.Expressions.Expression<System.Func<Customer, bool>>)(x => x.Id == id));
    //     if (existingItem == null)
    //     {
    //         return NotFound();
    //     }

    //     dbcontext.Customer.Remove(existingItem);
    //     await _dbcontext.SaveChangesAsync();
    //     return NoContent();
    // }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}

