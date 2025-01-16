using GF.CrossCutting.Dto;
using System.Collections.Generic;

namespace Shared.CrossCutting.Dto
{
    public class OrgDataDto
    {
        public string Name { get; set; }
        public string Leader { get; set; }
        public List<OrgMembershipDto> Members { get; set; }
    }
}