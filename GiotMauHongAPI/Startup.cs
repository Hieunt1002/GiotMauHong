using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.Repository;
using GiotMauHongAPI.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Text;

namespace GiotMauHongAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddOData(option => option.Select().Filter().Count().OrderBy().Expand()
            .SetMaxTop(100).AddRouteComponents("odata", GetEdmModel()));
            services.AddDbContext<ConnectDB>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("GiotMauHong"));
            });
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.AddScoped<IHospitalRepository, HospitalRepository>();
            services.AddScoped<IVolunteerRepository, VolunteerRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            var secretKey = Configuration["AppSettings:ScretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddCors();
            services.AddControllersWithViews()
                .AddNewtonsoftJson()
                .AddXmlDataContractSerializerFormatters();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project", Version = "v1" });
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project v1"));
            }
            app.UseHttpsRedirection();
            app.UseODataBatching();
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
            app.UseRouting();
            app.Use(next => context =>
            {
                var endpoint = context.GetEndpoint();
                if (endpoint == null)
                {
                    return next(context);
                }
                IEnumerable<string> templates;
                IODataRoutingMetadata metadata = endpoint.Metadata.GetMetadata<IODataRoutingMetadata>();
                if (metadata != null)
                {
                    templates = metadata.Template.GetTemplates();
                }
                return next(context);
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(enpoints =>
            {
                enpoints.MapControllers();
            });
        }
        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Requests>("Requests");

            builder.EntityType<Requests>().HasKey(e => e.Requestid);
            return builder.GetEdmModel();
        }
    }
}
