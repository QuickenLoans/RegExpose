using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegExpose.Web.Services
{
    public class PatternService : Service
    {
        public object Any(PatternRequest request)
        {
            var compiler = new RegexCompiler(request.IgnoreCase, request.SingleLine, request.MultiLine);

            PatternResponse response;

            try
            {
                var regex = compiler.Compile(request.Pattern);
                var regexDto = DtoMapper.Map<RegexNodeDto>(regex);
                response = new PatternResponse { Regex = regexDto };
            }
            catch (Exception ex)
            {
                response = new PatternResponse { ResponseStatus = new ResponseStatus(ex.GetType().Name, ex.Message) { StackTrace = ex.StackTrace } };
            }

            return new HttpResult(response, ContentType.Json);
        }
    }
}