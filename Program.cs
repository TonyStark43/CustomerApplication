using CustomerApplication.Data;
using CustomerApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CustomerApplication.Repository;
using CustomerApplication.Services;
using CustomerApplication.Validators;

namespace CustomerApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionMySql");
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.
            builder.Services.AddRazorPages();

    //        builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 35))));




                    builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

                                builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
                                {
                                    options.SignIn.RequireConfirmedAccount = false;
                                    options.Password.RequireDigit = false;      // Disable digit requirement
                                    options.Password.RequireLowercase = false;  // Disable lowercase requirement
                                    options.Password.RequireUppercase = false;  // Disable uppercase requirement
                                    options.Password.RequireNonAlphanumeric = false;
                                })
                .AddEntityFrameworkStores<ApplicationDbContext>();


            //DI container register
            builder.Services.AddScoped<ApplicationDbContext>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<CustomerService>();
            builder.Services.AddScoped<AddressService>();
            builder.Services.AddTransient<CustomerEditModelValidator>();

            //builder.WebHost.ConfigureKestrel(serverOptions =>
            //{
            //    serverOptions.ListenAnyIP(7248); // Listen on port 7248
            //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
                        app.UseAuthentication();;

            app.UseAuthorization();

            app.MapRazorPages();
            

            app.Run();
        }
    }
}