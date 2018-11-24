using LedMatrixController.Host.ConfigControllers;
using LedMatrixController.Host.Endpoints;
using LedMatrixController.Host.Endpoints.MatrixPreview;
using LedMatrixController.Host.Endpoints.MixerControl;
using LedMatrixController.Host.Models;
using LedMatrixController.Host.Server;
using LedMatrixController.Server;
using LedMatrixController.Server.Config;
using LedMatrixController.Server.PipelineElements.Source;
using LedMatrixController.Server.Queue;
using LedMatrixController.Server.Queue.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

            services.Configure<ServerConfig>(x =>
            {
                x.Width = 32;
                x.Height = 8;
                x.FrameRate = 30;
            });
            services.AddTransient<MatrixPreviewOutput, MatrixPreviewOutput>();

            services.AddSingleton<MainMixerControl, MainMixerControl>();

            services.AddSingleton<IDataService<FlatColorConfig>, MemoryDataService<FlatColorConfig>>();
            services.AddSingleton<IDataService<SourcesModel>, MemoryDataService<SourcesModel>>();
            services.AddSingleton<IDataService<QueueConfigModel>, MemoryDataService<QueueConfigModel>>();
            services.AddSingleton<IDataService<ServerConfig>, ServerConfigDataService>();

            services.AddSingleton<IQueueElementFactory, QueueElementFactory>();
            services.AddSingleton<ISourceFactory, SourceFactory>();

            services.AddSingleton<IOutputSize>(x =>
            {
                var config = x.GetRequiredService<IOptions<ServerConfig>>().Value;
                return new OutputSize(config.Width, config.Height);
            });

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
                x.MapHub<SliderControlHub<MainMixerControl>>("/slider/mixer");
                x.MapHub<ConfigController<FlatColorConfig>>("/FlatColor");
                x.MapHub<ConfigController<SourcesModel>>("/SourcesList");
                x.MapHub<ConfigController<QueueConfigModel>>("/Queue");
                x.MapHub<ConfigController<ServerConfig>>("/ServerConfig");
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
