using RegExpose.Api;
using RegExpose.Api.Services;
using ServiceStack.Api.Swagger;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.ServiceHost;
using ServiceStack.Common;
using ServiceStack.Common.Web;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AppHost), "Start")]

namespace RegExpose.Api
{
    public class AppHost
        : AppHostBase
    {
        public AppHost()
            : base("RegExpose API", typeof(AppHost).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

            Plugins.Add(new SwaggerFeature());

            // Disabling Html seems to make it so browsers can't hit the service.
            const Feature DisableFeatures = Feature.Xml | Feature.Jsv | Feature.Csv | Feature.Soap; // | Feature.Html;
            SetConfig(new EndpointHostConfig
            {
                EnableFeatures = Feature.All.Remove(DisableFeatures),
                DefaultContentType = ContentType.Json
            });

            DtoMapper.Init();
        }

        public static void Start()
        {
            new AppHost().Init();
        }
    }
}