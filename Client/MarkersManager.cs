using GF.CrossCutting.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class MarkersManager
    {
        public MarkersManager()
        {
            this.Markers = new List<MarkerDto>();
        }

        public List<MarkerDto> Markers { get; set; }
    }
}
