using AcxiomCRM.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace AcxiomCRM.Controllers;
public class HomeController:Controller{
    private readonly ApplicationDbContext _db;
    public HomeController(ApplicationDbContext db){_db=db;}
    public async Task<IActionResult> Index(){
        ViewBag.Customers=await _db.Customers.CountAsync();
        ViewBag.Leads=await _db.Leads.CountAsync();
        ViewBag.Opportunities=await _db.Opportunities.CountAsync(x=>x.Status=="Open");
        ViewBag.FollowUps=await _db.FollowUps.CountAsync(x=>x.Status=="Planned");
        ViewBag.Pipeline=(await _db.Opportunities.Where(x=>x.Status=="Open").Select(x=>x.Amount).ToListAsync()).Sum();
        ViewBag.Won=await _db.Opportunities.CountAsync(x=>x.Stage=="Won");
        ViewBag.Lost=await _db.Opportunities.CountAsync(x=>x.Stage=="Lost");
        ViewBag.Audit=await _db.AuditLogs.OrderByDescending(x=>x.CreatedDate).Take(8).ToListAsync();
        return View();
    }
    public IActionResult AccessDenied()=>View();
}
