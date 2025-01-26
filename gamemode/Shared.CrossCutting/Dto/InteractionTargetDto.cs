using GF.CrossCutting.Enums;

namespace Shared.CrossCutting.Dto
{
    public class InteractionTargetDto
    {
        public InteractionTargetDto(float x, float y, float z, float radius, InteractionTargetAction interactionTargetAction, string onInteractionPayload)
        {
            X = x;
            Y = y;
            Z = z;
            this.Radius = radius;
            InteractionTargetAction = interactionTargetAction;
            OnInteractionPayload = onInteractionPayload;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Radius { get; set; }
        public InteractionTargetAction InteractionTargetAction { get; set; }
        public string OnInteractionPayload { get; set; }
    }
}