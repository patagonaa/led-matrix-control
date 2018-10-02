using LedMatrixController.Host.Endpoints.MatrixPreview;
using LedMatrixController.Host.Endpoints.MixerControl;
using LedMatrixController.Host.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LedMatrixController.Host
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<ServerConfig>(x => new ServerConfig { Width = 32, Height = 8, FrameRate = 30 });
            services.AddTransient<MatrixPreviewOutput, MatrixPreviewOutput>();
            
            services.AddSingleton<MainMixerControls, MainMixerControls>();

            services.AddHostedService<MainService>();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseSignalR(x =>
            {
                x.MapHub<MatrixPreviewHub>("/matrixpreview");
                x.MapHub<MixerControlHub>("/mixercontrol");
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
