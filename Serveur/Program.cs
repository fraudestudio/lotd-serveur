using DotLiquid.FileSystems;
using DotLiquid;
using System.Threading;
using Server.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(opt => {
    opt.ListenAnyIP(5080, opt => { });
    opt.ListenAnyIP(5443, opt =>
    {
        opt.UseHttps(
            System.Environment.GetEnvironmentVariable("DEPLOY_CERTIFICATE") ?? "",
            System.Environment.GetEnvironmentVariable("DEPLOY_CERTIFICATE_PASSWORD" ?? "")
        );
    });
});

builder.Services.AddControllers();

var app = builder.Build();

Template.FileSystem = new LocalFileSystem(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "html", "partial"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Thread mailThread = new Thread(Email.SendMessages);
mailThread.IsBackground = true;
mailThread.Start();

app.Run();
