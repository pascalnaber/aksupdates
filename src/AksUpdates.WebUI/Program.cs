using AksUpdates.Api.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAzureTableReadStorage, AzureTableStorage>();

builder.Services.Configure<AzureTableStorageConfiguration>(builder.Configuration.GetSection("azureTableStorageConfiguration"));

builder.Services.AddResponseCaching();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCaching();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
//app.Run("http://*:8080");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
