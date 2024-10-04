using SsoWebApp.Contacts;
using SsoWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add session services
//builder.Services.AddDistributedMemoryCache(); // Enables session state in memory
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Set the cookie to be HttpOnly
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    var baseAddress = builder.Configuration["APIs:AuthUrl"];
    Console.WriteLine($"Configured BaseAddress: {baseAddress}"); // Check the value here
    client.BaseAddress = new Uri(baseAddress!);
});

//builder.Services.AddSingleton<IAuthService, AuthService>();

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
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
