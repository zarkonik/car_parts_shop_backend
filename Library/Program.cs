var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
	options.AddPolicy("CORS", builder =>
	{
builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();          
	}
	);
}
 );

var app = builder.Build();

// Configure the HTTP request pipeline.

	app.UseSwagger();
	app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("CORS");

app.UseAuthorization();

app.MapControllers();


app.Run();
