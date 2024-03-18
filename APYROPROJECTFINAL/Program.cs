using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using APYROPROJECTFINAL.Data;
using APYROPROJECTFINAL.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(connectionString));


/*
builder.Services.AddDefaultIdentity<ADMIN_STUDENTUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ADMIN_STUDENTContext>();
*/



builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();



builder.Services.AddIdentityCore<Educator>().AddEntityFrameworkStores<AuthDbContext>();
builder.Services.AddIdentityCore<Student>().AddEntityFrameworkStores<AuthDbContext>();





// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

//Validations for password can edit right click RequireUppercase - go to definition
//builder.Services.Configure<IdentityOptions>(options =>
//{
//  options.Password.RequireUppercase = false;
//});


//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2V1hhQlJAfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5UdkJjXn9XdHBTRmJc");
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cWWBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH5ecnVVRmReWEJ3WUM=");

//Register Syncfusion license
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2V1hhQlJAfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5UdkJjXn9XdHBTRmJc");
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cWWBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH5ecnVVRmReWEJ3WUM=");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();



using (var scope = app.Services.CreateScope())
{
    await DBSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
}




app.MapControllerRoute(
    name: "default",
           //pattern: "{controller=MainPage}/{action=MainHomePage}/{id?}");
           pattern: "{controller=MainPage}/{action=MainHomePage}/{id?}");







app.MapRazorPages();



app.Run();
