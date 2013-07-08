using System.Collections.Generic;

namespace RegExpose.Api.Services
{
    public class RegexNodeDto
    {
        public RegexNodeDto()
        {
            Children = new List<RegexNodeDto>();
        }

        public int Id { get; set; }
        public int Index { get; set; }
        public string Pattern { get; set; }
        public string NodeType { get; set; }
        public List<RegexNodeDto> Children { get; set; }
    }
}