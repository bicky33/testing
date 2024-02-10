using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NorthwindWebMvc.Basic.Models;
using NorthwindWebMvc.Basic.Models.Dto;
using NorthwindWebMvc.Basic.Repository;
using NorthwindWebMvc.Basic.RepositoryContext;
using NorthwindWebMvc.Basic.Service;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        //register interface IRepositoryBase<Category> & CategoryRepository Impl
        builder.Services.AddScoped<IRepositoryBase<Category>, CategoryRepository>();
        builder.Services.AddScoped<ICategoryService<CategoryDto>, CategoryService>();

        builder.Services.AddScoped<IRepositoryBase<Product>, ProductRepository>();
        builder.Services.AddScoped<IProductService<ProductDto>, ProductService>();

        //register AutoMapper
        builder.Services.AddAutoMapper(typeof(Program));

        //register dbcontext to services
        builder.Services.AddDbContext<RepositoryDbContext>(opts =>
        {
            opts.UseSqlServer(builder.Configuration["ConnectionStrings:ShopeeConnection"]);
        });

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
        // set folder resources to static file
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
            RequestPath = new PathString("/Resources")
        });

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}