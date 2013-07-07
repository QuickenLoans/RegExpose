using System.Web.Mvc;
using RegExpose.Web;
using ServiceStack.Mvc;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using RegExpose.Web.Services;
using ServiceStack.ServiceHost;
using ServiceStack.Common;
using ServiceStack.Common.Web;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AppHost), "Start")]

namespace RegExpose.Web
{
    public class AppHost
        : AppHostBase
    {
        public AppHost()
            : base("RegExpose API", typeof(AppHost).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

            Routes
              .Add<PatternRequest>("/getstructure", "GET,POST")
              .Add<ParseStepsRequest>("/getparsesteps", "GET,POST");

            // Disabling Html seems to make it so browsers can't hit the service.
            var disableFeatures = Feature.Xml | Feature.Jsv | Feature.Csv | Feature.Soap;// | Feature.Html;
            SetConfig(new EndpointHostConfig
            {
                EnableFeatures = Feature.All.Remove(disableFeatures),
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