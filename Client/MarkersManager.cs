using GF.CrossCutting.Dto;
using System.Collections.Generic;

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