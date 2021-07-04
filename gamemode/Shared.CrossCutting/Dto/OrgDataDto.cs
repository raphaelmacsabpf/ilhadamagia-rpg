using System.Collections.Generic;

namespace Shared.CrossCutting.Dto
{
    public class OrgDataDto
    {
        public string Name { get; set; }
        public string Leader { get; set; }
        public List<string> Members { get; set; }
    }
}