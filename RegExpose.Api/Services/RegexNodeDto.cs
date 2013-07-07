using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegExpose.Web.Services
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