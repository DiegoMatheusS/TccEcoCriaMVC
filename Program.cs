using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; 
});


builder.Services.AddHttpClient(); 


if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddExceptionHandler(options =>
    {
        options.ExceptionHandlingPath = "/Home/Error"; 
    });
}

var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();


app.UseSession();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
