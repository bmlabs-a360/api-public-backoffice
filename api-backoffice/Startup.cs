using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using api_public_backOffice.Repository;
using api_public_backOffice.Service;
using Microsoft.EntityFrameworkCore;
using neva.entities;
using System.Net.Security;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Polly.Extensions.Http;
using Polly;
using System.Net.Http;
using api_public_backOffice.Clients;
using api_public_backOffice.Helpers;
using Elastic.Apm.NetCoreAll;

namespace api_public_backOffice
{
    public class Startup
    {
        private readonly string _SERVICENAME = "api-public-backoffice";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddCors();
            services.AddControllers();
            services.AddDbContext<neva.entities.Context>(
                options => options.UseNpgsql(Configuration.GetConnectionString("Context"), 
                o => o.RemoteCertificateValidationCallback(CertificateValidation())));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = _SERVICENAME, Version = "v1", Description = _SERVICENAME });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    LifetimeValidator = LifetimeValidator,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            /*.oOo.oOo.oOo.oOo.oOo. ADD REST CLIENTS .oOo.oOo.oOo.oOo.oOo.*/
            services.AddHttpClient<MsMailClient>((client) =>
            {
                client.BaseAddress = new Uri(Configuration.GetSection("Configuration").GetValue<string>("msMail.urlbase"));
            }).AddPolicyHandler(GetRetryPolicy())
              .AddPolicyHandler(GetCircuitBreakerPolicy())
              .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            /*Inyection Repository*/
           
            services.AddScoped<AlternativaRepository>();
            services.AddScoped<BitacoraRepository>();
            services.AddScoped<ControlTokenRepository>();
            services.AddScoped<EmpresaRepository>();
            services.AddScoped<EvaluacionEmpresaRepository>();
            services.AddScoped<EvaluacionRepository>();
            services.AddScoped<ImportanciaEstrategicaRepository>();
            services.AddScoped<ImportanciaRelativaRepository>();
            services.AddScoped<PerfilPermisoRepository>();
            services.AddScoped<PerfilRepository>();
            services.AddScoped<PlanMejoraRepository>();
            services.AddScoped<PreguntaRepository>();
            services.AddScoped<ReporteAreaRepository>();
            services.AddScoped<ReporteItemRepository>();
            services.AddScoped<ReporteRepository>();
            services.AddScoped<SegmentacionAreaRepository>();
            services.AddScoped<SegmentacionSubAreaRepository>();
            services.AddScoped<TipoCantidadEmpleadoRepository>();
            services.AddScoped<TipoDiferenciaRelacionadaRepository>();
            services.AddScoped<TipoImportanciaRepository>();
            services.AddScoped<TipoItemReporteRepository>();
            services.AddScoped<TipoNivelVentaRepository>();
            services.AddScoped<TipoRubroRepository>();
            services.AddScoped<TipoSubRubroRepository>();
            services.AddScoped<TipoTamanoEmpresaRepository>();
            services.AddScoped<UsuarioAreaRepository>();
            services.AddScoped<UsuarioEmpresaRepository>();
            services.AddScoped<UsuarioEvaluacionRepository>();
            services.AddScoped<UsuarioSuscripcionRepository>();
            services.AddScoped<UsuarioRepository>();

            /*Inyection Service*/

            services.AddScoped<AlternativaService>();
            services.AddScoped<BitacoraService>();
            services.AddScoped<ControlTokenService>();
            services.AddScoped<EmpresaService>();
            services.AddScoped<EvaluacionEmpresaService>();
            services.AddScoped<EvaluacionService>();
            services.AddScoped<ImportanciaEstrategicaService>();
            services.AddScoped<ImportanciaRelativaService>();
            services.AddScoped<PerfilPermisoService>();
            services.AddScoped<PerfilService>();
            services.AddScoped<PlanMejoraService>();
            services.AddScoped<PreguntaService>();
            services.AddScoped<ReporteAreaService>();
            services.AddScoped<ReporteItemService>();
            services.AddScoped<ReporteService>();
            services.AddScoped<SegmentacionAreaService>();
            services.AddScoped<SegmentacionSubAreaService>();
            services.AddScoped<TipoCantidadEmpleadoService>();
            services.AddScoped<TipoDiferenciaRelacionadaService>();
            services.AddScoped<TipoImportanciaService>();
            services.AddScoped<TipoItemReporteService>();
            services.AddScoped<TipoNivelVentaService>();
            services.AddScoped<TipoRubroService>();
            services.AddScoped<TipoSubRubroService>();
            services.AddScoped<TipoTamanoEmpresaService>();
            services.AddScoped<UsuarioAreaService>();
            services.AddScoped<UsuarioEmpresaService>();
            services.AddScoped<UsuarioEvaluacionService>();
            services.AddScoped<UsuarioSuscripcionService>();
            services.AddScoped<UsuarioService>();
            services.AddScoped<MailService>();
            services.AddScoped<TokenService>();

            /*Inyection Helpper*/
            services.AddScoped<EmailHelper>();
            services.AddScoped<SecurityHelper>();
            services.AddScoped<UrlHelper>();


            /*Inyection Client*/
           // services.AddScoped<MsMailClient>();
            services.AddHttpClient<MsMailClient>((client) =>
            {
                client.BaseAddress = new Uri(Configuration.GetSection("Configuration").GetValue<string>("msmail.urlbase"));
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var tenant = Configuration["Configuration:tenant"];
            if (string.IsNullOrEmpty(tenant))
            {
                throw new Exception("Se debe configurar el parametro tenant, en el appsettings.json");
            }

            var context = $"/{tenant}/{_SERVICENAME}";
            //Console.WriteLine($"context: {context}");
            //app.UsePathBase($"{context}");
            app.UsePathBase("/api-public-backoffice");

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c => { c.SerializeAsV2 = true; c.RouteTemplate = "swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", _SERVICENAME);
                c.RoutePrefix = "swagger";
                c.DefaultModelsExpandDepth(-1);  //desactiva models de swagger
            });
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }
        
        private RemoteCertificateValidationCallback CertificateValidation() => (sender, certificate, chain, errors) =>
        {
            if (errors == SslPolicyErrors.None) return true;
            if ((errors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                var allErrorsIsUntrustedRoot = chain?.ChainStatus.All(o => o.Status == X509ChainStatusFlags.UntrustedRoot);
                return allErrorsIsUntrustedRoot ?? true;
            }
            return true;
        };

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
