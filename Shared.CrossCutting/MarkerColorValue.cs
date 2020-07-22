namespace Shared.CrossCutting
{
    public static class MarkerColorValue
    {
        public static dynamic GetRGBA(MarkerColor markerColor)
        {
            switch (markerColor)
            {
                case MarkerColor.COLOR_ORANGE: return COLOR_ORANGE;
                case MarkerColor.COLOR_BLUE: return COLOR_BLUE;
                case MarkerColor.COLOR_DARK_BLUE: return COLOR_DARK_BLUE;
                case MarkerColor.COLOR_GREEN: return COLOR_GREEN;
                case MarkerColor.COLOR_DARK_GREEN: return COLOR_DARK_GREEN;
                case MarkerColor.COLOR_YELLOW: return COLOR_YELLOW;
                case MarkerColor.COLOR_DARK_YELLOW: return COLOR_DARK_YELLOW;

                default: return COLOR_ORANGE;
            }
        }

        private static readonly dynamic COLOR_ORANGE = new[] { 255, 128, 0, 120 };
        private static readonly dynamic COLOR_BLUE = new[] { 0, 172, 255, 120 };
        private static readonly dynamic COLOR_DARK_BLUE = new[] { 0, 126, 187, 120 };
        private static readonly dynamic COLOR_GREEN = new[] { 26, 255, 0, 120 };
        private static readonly dynamic COLOR_DARK_GREEN = new[] { 16, 159, 0, 120 };
        private static readonly dynamic COLOR_YELLOW = new[] { 249, 255, 0, 120 };
        private static readonly dynamic COLOR_DARK_YELLOW = new[] { 190, 196, 0, 120 };
    }
}