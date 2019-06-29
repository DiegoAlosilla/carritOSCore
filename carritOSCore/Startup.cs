﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using carritOSCore.Model.Context;
using carritOSCore.Model.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
/**
* --
* @author Juan Diego Alosilla
* @email diegoalosillagmail.com
*/
namespace carritOSCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer("Data Source =.; Initial Catalog = carritOSDataBase; Integrated Security = True"));

            services.AddIdentity<AplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = "alosilla.com",
                     ValidAudience = "alosilla.com",
                     IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("Alosilla")),
                     ClockSkew = TimeSpan.Zero
                 });


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (!context.BusinessOwners.Any())
            {
                context.BusinessOwners.AddRange(new List<BusinessOwner>()
                {
                    new BusinessOwner()
                    {

                        FirstName = "Diego",
                        LastName = "Alosilla",
                        Dni ="12345678",
                        Email = "deigoalosilla@gmail.com",
                        Movil = "966450252",
                        Password = "1234",
                        City = "Lima",
                        Country = "Peru"
                     },

                    new BusinessOwner()
                    {

                        FirstName = "Giannela",
                        LastName = "Peramas",
                        Dni ="12345678",
                        Email = "deigoalosilla@gmail.com",
                        Movil = "966450252",
                        Password = "1234",
                        City = "Lima",
                        Country = "Peru"
                     }
                });
                context.SaveChanges();
            }


        }
    }
}
