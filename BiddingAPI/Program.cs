using BiddingAPI.Processors;
using BiddingAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS so that web UI can call this API: (don't try this at home)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Configure bids provider (provides ability to add bid to stream of bids and to consume stream of bids):
builder.Services.AddSingleton<BidsProvider>();
builder.Services.AddSingleton<IBidsProvider>(svc => svc.GetRequiredService<BidsProvider>());
builder.Services.AddSingleton<IBidsHandler>(svc => svc.GetRequiredService<BidsProvider>());

// Configure processor service that prepares all processing services, handling the stream of bids:
builder.Services.AddSingleton<ProcessorService>();

// Configure processing services:
// Note: the below 3 processors are various examples/scenarios of how bids can be processed; you may comment out what you don't need or want to test
   //builder.Services.AddSingleton<IProcessor, TestProcessor>();
   builder.Services.AddSingleton<IProcessor, MostActiveAuctionsProcessor>();
   // builder.Services.AddSingleton<IProcessor, ReduceToHighestBidPerLotProcessor>();

builder.Host.ConfigureLogging(logging => {
    logging.ClearProviders();
    logging.AddConsole();
});

var app = builder.Build();

// Use CORS:
app.UseCors("AllowAll");

// Use bidding processors: IProcessor services that each process the stream of incoming bids in parallel:
app.UseProcessors();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

// disable HTTPS redirection as TLS is terminated earlier by a proxy
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
