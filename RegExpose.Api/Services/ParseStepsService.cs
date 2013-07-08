using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Linq;

namespace RegExpose.Api.Services
{
    public class ParseStepsService : Service
    {
        private static readonly int MaxStepCountPerRequest = int.Parse(ParseStepsRequest.MaxStepCountPerRequest);

        public object Any(ParseStepsRequest request)
        {
            var skip = request.Skip.HasValue ? Math.Max(0, request.Skip.Value) : 0;
            var take = request.Take.HasValue ? Math.Min(MaxStepCountPerRequest, request.Take.Value) : MaxStepCountPerRequest;

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
                        .Skip(skip)
                        .Take(take);

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