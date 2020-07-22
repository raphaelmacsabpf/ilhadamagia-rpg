using Shared.CrossCutting.Dto;
using System.Collections.Generic;

namespace Client.Application
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