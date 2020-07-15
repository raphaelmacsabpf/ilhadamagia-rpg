namespace GF.CrossCutting.Dto
{
    public class InteractionTargetDto
    {
        public InteractionTargetDto(float x, float y, float z, string actionName, string onInteractionPayload)
        {
            X = x;
            Y = y;
            Z = z;
            ActionName = actionName;
            OnInteractionPayload = onInteractionPayload;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public string ActionName { get; set; }
        public string OnInteractionPayload { get; set; }
    }
}