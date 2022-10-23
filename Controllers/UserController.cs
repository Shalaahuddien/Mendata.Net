using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Mendata.Net.Models;
using Mendata.Net.Models.Entities;
using Mendata.Net.Models.Request;

namespace Mendata.Net.Controllers;

public class UserController : Controller
{
    private readonly dbcontext _dbContext;

    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, dbcontext dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;

    }
    [HttpGet]
    public IActionResult IndexPenjual()
    {
        List<Penjual> penjuals = _dbContext.Penjuals.ToList();
        return View(penjuals);
    }

    //buat delete data Penjual
    [HttpGet]
    public IActionResult DeletePenjual(Penjual pj)
    {

        Penjual deleted = _dbContext.Penjuals.First(x => x.Id == pj.Id);
        _dbContext.Penjuals.Remove(deleted);
        _dbContext.SaveChanges();
        List<Penjual> penjuals = _dbContext.Penjuals.ToList();
        return View("IndexPenjual", penjuals);
    }

    //input Data Penjual
    [HttpGet]
    public IActionResult InputPenjual()
    {
        return View();
    }

    [HttpPost]
    public IActionResult InputPenjual(Penjual pj)
    {
        try
        {
            // produk.Add(pr);
            pj.IdUser = 1;
            _dbContext.Penjuals.Add(pj);
            _dbContext.SaveChanges();
            List<Penjual> penjuals = _dbContext.Penjuals.ToList();
            return View("IndexPenjual", penjuals);
            // return View("Index");
        }
        catch
        {
            return View("IndexPenjual");
        }

    }

    //buat edit 
    [HttpGet]
    public IActionResult EditPenjual(int id)
    {
        Penjual pj = _dbContext.Penjuals.First(x => x.Id == id);
        return View(pj);
    }

    // [HttpPost]
    // public IActionResult EditPenjual(Penjual pj)
    // {
    //     try
    //     {
    //         Penjual updated = _dbContext.Penjuals.First(x => x.Id == pj.Id);
    //         updated.NamaToko = pj.NamaToko;
    //         updated.AlamatToko = pj.AlamatToko;
    //         updated.NoHP = pj.NoHP;
    //         _dbContext.SaveChanges();
    //         return RedirectToAction("IndexPenjual");
    //     }
    //     catch
    //     {
    //         return View();
    //     }

    // }
    // BUAT PEMBELI YGY

    public IActionResult IndexPembeli()
    {
        List<Pembeli> pembelis = _dbContext.Pembelis.ToList();
        return View(pembelis);
    }

    //buat delete 
    [HttpGet]
    public IActionResult DeletePembeli(Pembeli pb)
    {

        Pembeli deleted = _dbContext.Pembelis.First(x => x.Id == pb.Id);
        _dbContext.Pembelis.Remove(deleted);
        _dbContext.SaveChanges();
        List<Pembeli> pembelis = _dbContext.Pembelis.ToList();
        return View("IndexPembeli", pembelis);
    }

    //input 
    [HttpGet]
    public IActionResult InputPembeli()
    {
        return View();
    }

    [HttpPost]
    public IActionResult InputPembeli(Pembeli pb)
    {
        try
        {
            // produk.Add(pr);
            pb.IdUser = 1;
            _dbContext.Pembelis.Add(pb);
            _dbContext.SaveChanges();
            List<Pembeli> pembelis = _dbContext.Pembelis.ToList();
            return View("IndexPembeli", pembelis);
            // return View("Index");
        }
        catch
        {
            return View("IndexPembeli");
        }

    }

    //buat edit 
    // [HttpGet]
    // public IActionResult EditPembeli(int id)
    // {
    //     Pembeli pb = _dbContext.Pembelis.First(x => x.Id == id);
    //     return View(pb);
    // }

    // [HttpPost]
    // public IActionResult EditPembeli(Pembeli pb)
    // {
    //     try
    //     {
    //         Pembeli updated = _dbContext.Pembelis.First(x => x.Id == pb.Id);
    //         updated.NamaPembeli = pb.NamaPembeli;
    //         updated.AlamatPembeli = pb.AlamatPembeli;
    //         updated.NoHP = pb.NoHP;
    //         _dbContext.SaveChanges();
    //         return RedirectToAction("IndexPembeli");
    //     }
    //     catch
    //     {
    //         return View();
    //     }

    // }

    //BUAT USER
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest login)
    {
        if (!ModelState.IsValid)
        {
            return View(login);
        }

        var user = _dbContext.Users.FirstOrDefault(x => x.Username == login.Username);

        if (user == null)
        {
            ViewBag.ErrorMessage = "Invalid Username or Password";
            return View(login);
        }

        if (user.Tipe == "Pembeli")
        {
            ViewBag.ErrorMessage = "You're not admin or seller";
            return View(login);
        }

        var claims = new List<Claim>{
            new Claim(ClaimTypes.Name,user.Username),
            new Claim("Fullname",user.Username),
            new Claim(ClaimTypes.Role,user.Tipe)
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme
        );

        var authProperties = new AuthenticationProperties()
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return RedirectToAction("Login");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {

            return View(request);
        }

        var newUser = new Models.Entities.User
        {
            Username = request.Username,
            Password = request.Password,
            Tipe = request.Tipe
        };

        var penjual = new Models.Entities.Penjual
        {
            IdUser = newUser.Id,
            Alamat = request.Alamat,
            NamaToko = $"{request.FullName} Store",
            User = newUser
        };

        _dbContext.Users.Add(newUser);
        _dbContext.Penjuals.Add(penjual);

        _dbContext.SaveChanges();

        return RedirectToAction("Login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}