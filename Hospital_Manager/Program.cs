using Data_Hospital_Manager;
using Hospital_Manager.Core.Interfaces;
using Hospital_Manager.Core.Services;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HospitalDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddScoped<IHospitalService, HospitalService>();
builder.Services.AddScoped<IDoctorInterface, DoctorService>();
builder.Services.AddScoped<IPatientInterface, PatientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Hospital}/{action=Index}/{id?}");

app.Run();
