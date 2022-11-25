using System.Threading;
using Server.Utils;
using Server.Database;
using Server.Auth;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Server.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(opt => {
    opt.ListenAnyIP(8080, opt => { });
    opt.ListenAnyIP(8443, opt =>
    {
        opt.UseHttps(
            System.Environment.GetEnvironmentVariable("CERTIFICATE_FILE") ?? "",
            System.Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD" ?? "")
        );
    });
});

builder.Services.AddControllers();

builder.Services.AddAuthentication("Basic").AddScheme<AuthOptions, BasicAuthenticationHandler>("Basic", null);

var app = builder.Build();

Template.TemplateDirectory = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Templates");
HtmlController.HtmlFile = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Static/index.html");

app.UseHttpsRedirection();

app.UseStaticFiles(
    new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Static")
        ),
        RequestPath = "/static",
    }
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Thread mailThread = new Thread(Email.SendMessages);
mailThread.IsBackground = true;
mailThread.Start();

app.Run();
