using EShop.Api.Billing.Builders;
using EShop.Api.Billing.Contracts.Builders;
using EShop.Api.Billing.Contracts.Services;
using EShop.Api.Billing.Contracts.Validators;
using EShop.Api.Billing.Services;
using EShop.Api.Billing.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EShop.Api.Billing
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
            services.AddScoped<IBillingService, BillingService>();
            services.AddScoped<IPaymentOrderBuilder, PaymentOrderBuilder>();
            services.AddScoped<IReceiptBuilder, ReceiptBuilder>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderValidator, OrderValidator>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
