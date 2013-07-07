using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegExpose.Web.Services
{
    public class ParseStepsService : Service
    {
        private const int MaxStepCountPerRequest = 1000;

        public object Any(ParseStepsRequest request)
        {
            var compiler = new RegexCompiler(request.IgnoreCase, request.SingleLine, request.MultiLine);

            ParseStepsResponse response;

            try
            {
                var regex = compiler.Compile(request.Pattern);
                var engine = regex.Parse(request.Input);
                var parseSteps =
                    engine.GetParseSteps()
                        .Select((parseStep, stepIndex) =>
                        {
                            var parseStepDto = DtoMapper.Map<ParseStepDto>(parseStep);
                            parseStepDto.StepIndex = stepIndex;
                            return parseStepDto;
                        })
                        .Skip(request.Skip ?? 0)
                        .Take(request.Take == null ? MaxStepCountPerRequest : Math.Min(request.Take.Value, MaxStepCountPerRequest));

                var regexDto = DtoMapper.Map<RegexNodeDto>(regex);

                response = new ParseStepsResponse
                {
                    Regex = regexDto,
                    ParseSteps = parseSteps.ToList()
                };
            }
            catch (Exception ex)
            {
                response = new ParseStepsResponse { ResponseStatus = new ResponseStatus(ex.GetType().Name, ex.Message) { StackTrace = ex.StackTrace } };
            }

            return new HttpResult(response, ContentType.Json);
        }
    }
}