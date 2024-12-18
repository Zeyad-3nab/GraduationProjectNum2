using GraduationProject.API.BLL.Interfaces;
using GraduationProject.API.BLL.Repositories;
using GraduationProject.API.DAL.Data.Contexts;
using GraduationProject.API.DAL.Models.IdentityModels;
using GraduationProject.API.PL.Errors;
using GraduationProject.API.PL.Extention;
using GraduationProject.API.PL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);

    #region Dependancy
// Add services to the container.

builder.Services.AddControllers();

//Connection String
builder.Services.AddDbContext<ApplicationDbContext>
	(
      options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
	);


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options => Options.SignIn.RequireConfirmedAccount = false)
			  .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


builder.Services.AddScoped<IApartmentRepository, ApartmentRepository>();
builder.Services.AddScoped<IApartmentImageRepository, ApartmentImageRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();


builder.Services.AddAutoMapper(M => M.AddProfile(new Applicationprofile(builder.Configuration)));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ApiBehaviorOptions>(Options =>
			    {
		    	    Options.InvalidModelStateResponseFactory = (actionContext) =>
		    	    {
		    	    	// ModelState => Dicsanary [KeyValuePair]
		    	    	// Key => Name Of Param
		    	    	// Value => Error
		    	    	//ValidationError هنا بكريت شكل جديد من ال
		    	    	// Note :Project بيتعمل مره واحده بس في ال ValidationError ال
		    	    	var error = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
		    	    										.SelectMany(P => P.Value.Errors)
		    	    										.Select(E => E.ErrorMessage)
		    	    										.ToArray();
		    	    
		    	    	var response = new ApiValidationErrorResponse()
		    	    	{
		    	    		Errors = error
		    	    	};
		    	    	return new BadRequestObjectResult(response);
		    	    };
		        });

//Add Swagger Extention
builder.Services.AddSwaggerGenJwtAuth();

//Add Custom Extention
builder.Services.AddCustomJwtAuth(builder.Configuration);

#endregion

var app = builder.Build();

   #region Update DataBase

using var scope = app.Services.CreateScope();

var Service = scope.ServiceProvider;

var context = Service.GetRequiredService<ApplicationDbContext>();

var LoggerFactory = Service.GetRequiredService<ILoggerFactory>();

try
{
	await context.Database.MigrateAsync();
}
catch (Exception ex)
{

	var logger = LoggerFactory.CreateLogger<Program>();
	logger.LogError(ex, "There are Probems during apply Migrations !!");
}

#endregion

   #region Configure 
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
	app.UseSwagger();
	app.UseSwaggerUI();
//}
app.UseStatusCodePagesWithReExecute("/error/{0}"); //تانيه end point ل Redirect هتروح تعمل <= StatusCode بحجز مكان ل

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

   app.Run();
