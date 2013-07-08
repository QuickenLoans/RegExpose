using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;

namespace RegExpose.Api.Services
{
    public class StructureService : Service
    {
        public object Any(StructureRequest request)
        {
            var compiler = new RegexCompiler();

            StructureResponse response;

            try
            {
                var regex = compiler.Compile(request.Pattern);
                var regexDto = DtoMapper.Map<RegexNodeDto>(regex);
                response = new StructureResponse { Regex = regexDto };
            }
            catch (Exception ex)
            {
                response = new StructureResponse { ResponseStatus = new ResponseStatus(ex.GetType().Name, ex.Message) { StackTrace = ex.StackTrace } };
            }

            return new HttpResult(response, ContentType.Json);
        }
    }
}