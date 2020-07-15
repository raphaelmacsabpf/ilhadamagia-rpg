namespace GF.CrossCutting.Dto
{
    public class ProximityTargetDto
    {
        public ProximityTargetDto(float x, float y, float z, float radius, int periodInMs, string actionName, string onEnterActionPayload, string onExitActionPayload)
        {
            X = x;
            Y = y;
            Z = z;
            Radius = radius;
            PeriodInMs = periodInMs;
            ActionName = actionName;
            OnEnterActionPayload = onEnterActionPayload;
            OnExitActionPayload = onExitActionPayload;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Radius { get; set; }
        public int PeriodInMs { get; set; }
        public string ActionName { get; set; }
        public string OnEnterActionPayload { get; set; }
        public string OnExitActionPayload { get; set; }
    }
}