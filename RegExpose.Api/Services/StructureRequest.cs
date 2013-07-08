using ServiceStack.ServiceHost;

namespace RegExpose.Api.Services
{
    [Api("Gets the tree structure of the specifiec regular expression pattern.")]
    [Route("/regex/structure", "GET", Summary = @"Regex Tree Structure", Notes = "Gets the tree structure of the specifiec regular expression pattern.")]
    public class StructureRequest : IReturn<StructureResponse>
    {
        [ApiMember(Name = "Pattern", Description = "The regular expression pattern to match.", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string Pattern { get; set; }
    }
}