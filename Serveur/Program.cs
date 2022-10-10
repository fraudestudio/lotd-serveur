using DotLiquid.FileSystems;
using DotLiquid;
using System.Threading;
using Server.Utils;
using Server.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

Template.FileSystem = new LocalFileSystem(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "html", "partial"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Thread mailThread = new Thread(EMail.SendMessages);
mailThread.IsBackground = true;
mailThread.Start();

app.Run();
