using AcxiomCRM.Data;
using AcxiomCRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcxiomCRM.Controllers;
[Authorize]
public class CustomersController:Controller{
    private readonly ApplicationDbContext db;
    public CustomersController(ApplicationDbContext context){db=context;}
    public async Task<IActionResult> Index(string? q)=>View(await db.Customers.Where(x=>q==null||x.Name.Contains(q)||x.Email.Contains(q)||x.CompanyName.Contains(q)).OrderBy(x=>x.Name).ToListAsync());
    public IActionResult Create()=>View(new Customer());
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(Customer model){if(await db.Customers.AnyAsync(x=>x.Email==model.Email||x.Phone==model.Phone))ModelState.AddModelError("","Email or phone already exists.");if(!ModelState.IsValid)return View(model);db.Customers.Add(model);await db.SaveChangesAsync();await Log("Create","Customer",model.Id.ToString());return RedirectToAction(nameof(Index));}
    public async Task<IActionResult> Edit(int id){var x=await db.Customers.FindAsync(id);return x==null?NotFound():View(x);}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,Customer model){if(id!=model.Id)return BadRequest();if(!ModelState.IsValid)return View(model);db.Customers.Update(model);await db.SaveChangesAsync();await Log("Update","Customer",id.ToString());return RedirectToAction(nameof(Index));}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id){var x=await db.Customers.FindAsync(id);if(x!=null){x.Status="Inactive";await db.SaveChangesAsync();await Log("Deactivate","Customer",id.ToString());}return RedirectToAction(nameof(Index));}
    private async Task Log(string action,string entity,string id){db.AuditLogs.Add(new AuditLog{UserName=User.Identity?.Name??"system",Action=action,EntityName=entity,RecordId=id});await db.SaveChangesAsync();}
}
[Authorize]
public class LeadsController:Controller{
    private readonly ApplicationDbContext db;
    public LeadsController(ApplicationDbContext context){db=context;}
    public async Task<IActionResult> Index(string? q)=>View(await db.Leads.Where(x=>q==null||x.Name.Contains(q)||x.CompanyName.Contains(q)||x.Status.Contains(q)).OrderByDescending(x=>x.CreatedDate).ToListAsync());
    public IActionResult Create()=>View(new Lead());
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(Lead model){if(!ModelState.IsValid)return View(model);db.Leads.Add(model);await db.SaveChangesAsync();db.AuditLogs.Add(new AuditLog{UserName=User.Identity?.Name??"system",Action="Create",EntityName="Lead",RecordId=model.Id.ToString()});await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
    public async Task<IActionResult> Edit(int id){var x=await db.Leads.FindAsync(id);return x==null?NotFound():View(x);}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,Lead model){if(id!=model.Id)return BadRequest();if(!ModelState.IsValid)return View(model);db.Leads.Update(model);await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id){var x=await db.Leads.FindAsync(id);if(x!=null){db.Leads.Remove(x);await db.SaveChangesAsync();}return RedirectToAction(nameof(Index));}
}
[Authorize]
public class OpportunitiesController:Controller{
    private readonly ApplicationDbContext db;
    public OpportunitiesController(ApplicationDbContext context){db=context;}
    public async Task<IActionResult> Index()=>View(await db.Opportunities.OrderBy(x=>x.ExpectedCloseDate).ToListAsync());
    public async Task<IActionResult> Create(){ViewBag.Customers=await db.Customers.Where(x=>x.Status=="Active").ToListAsync();return View(new Opportunity());}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(Opportunity model){ViewBag.Customers=await db.Customers.Where(x=>x.Status=="Active").ToListAsync();if(model.ExpectedCloseDate.Date<DateTime.Today)ModelState.AddModelError(nameof(model.ExpectedCloseDate),"Expected close date cannot be in the past.");if(!await db.Customers.AnyAsync(x=>x.Id==model.CustomerId))ModelState.AddModelError(nameof(model.CustomerId),"Select a valid customer.");if(!ModelState.IsValid)return View(model);db.Opportunities.Add(model);await db.SaveChangesAsync();db.AuditLogs.Add(new AuditLog{UserName=User.Identity?.Name??"system",Action="Create",EntityName="Opportunity",RecordId=model.Id.ToString()});await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Stage(int id,string stage){var x=await db.Opportunities.FindAsync(id);if(x==null)return NotFound();if(!new[]{"Qualification","Proposal","Negotiation","Won","Lost"}.Contains(stage))return BadRequest();x.Stage=stage;if(stage=="Won"||stage=="Lost")x.Status=stage;await db.SaveChangesAsync();db.AuditLogs.Add(new AuditLog{UserName=User.Identity?.Name??"system",Action="StageChange",EntityName="Opportunity",RecordId=id.ToString(),Details=stage});await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
}
[Authorize]
public class FollowUpsController:Controller{
    private readonly ApplicationDbContext db;
    public FollowUpsController(ApplicationDbContext context){db=context;}
    public async Task<IActionResult> Index()=>View(await db.FollowUps.OrderBy(x=>x.FollowUpDate).ToListAsync());
    public IActionResult Create()=>View(new FollowUp());
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(FollowUp model){if(model.FollowUpDate.Date<DateTime.Today)ModelState.AddModelError(nameof(model.FollowUpDate),"Follow-up date cannot be earlier than today.");if(!ModelState.IsValid)return View(model);db.FollowUps.Add(model);await db.SaveChangesAsync();db.AuditLogs.Add(new AuditLog{UserName=User.Identity?.Name??"system",Action="Create",EntityName="FollowUp",RecordId=model.Id.ToString()});await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Complete(int id){var x=await db.FollowUps.FindAsync(id);if(x!=null){x.Status="Completed";await db.SaveChangesAsync();}return RedirectToAction(nameof(Index));}
}
[Authorize(Roles="Admin")]
public class AuditController:Controller{
    private readonly ApplicationDbContext db;
    public AuditController(ApplicationDbContext context){db=context;}
    public async Task<IActionResult> Index()=>View(await db.AuditLogs.OrderByDescending(x=>x.CreatedDate).Take(300).ToListAsync());
}
