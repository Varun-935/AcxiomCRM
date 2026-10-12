using AcxiomCRM.Data;
using AcxiomCRM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder=WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDefaultIdentity<IdentityUser>(options=>{
    options.SignIn.RequireConfirmedAccount=false;
    options.Password.RequiredLength=8;
    options.Lockout.MaxFailedAccessAttempts=5;
    options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(10);
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.ConfigureApplicationCookie(options=>{options.LoginPath="/Identity/Account/Login";options.AccessDeniedPath="/Home/AccessDenied";});
var app=builder.Build();
using(var scope=app.Services.CreateScope()){
    var db=scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
    var roles=scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    foreach(var role in new[]{"Admin","Manager","SalesExecutive"}) if(!await roles.RoleExistsAsync(role)) await roles.CreateAsync(new IdentityRole(role));
    var users=scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var admin=await users.FindByEmailAsync("admin@acxiomcrm.local");
    if(admin==null){admin=new IdentityUser{UserName="admin@acxiomcrm.local",Email="admin@acxiomcrm.local",EmailConfirmed=true};await users.CreateAsync(admin,"Admin@12345");await users.AddToRoleAsync(admin,"Admin");}
}
if(!app.Environment.IsDevelopment()){app.UseExceptionHandler("/Home/Error");app.UseHsts();}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name:"default",pattern:"{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
