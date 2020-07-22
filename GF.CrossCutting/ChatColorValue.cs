namespace Shared.CrossCutting
{
    public static class ChatColorValue
    {
        public static dynamic GetRGBA(ChatColor chatColor)
        {
            switch (chatColor)
            {
                case ChatColor.COLOR_GRAD1: return COLOR_GRAD1;
                case ChatColor.COLOR_GRAD2: return COLOR_GRAD2;
                case ChatColor.COLOR_GRAD3: return COLOR_GRAD3;
                case ChatColor.COLOR_GRAD4: return COLOR_GRAD4;
                case ChatColor.COLOR_GRAD5: return COLOR_GRAD5;
                case ChatColor.COLOR_GRAD6: return COLOR_GRAD6;
                case ChatColor.COLOR_GREY: return COLOR_GREY;
                case ChatColor.COLOR_GREEN: return COLOR_GREEN;
                case ChatColor.COLOR_RED: return COLOR_RED;
                case ChatColor.COLOR_LIGHTRED: return COLOR_LIGHTRED;
                case ChatColor.COLOR_VERMELHO: return COLOR_VERMELHO;
                case ChatColor.COLOR_LIGHTBLUE: return COLOR_LIGHTBLUE;
                case ChatColor.COLOR_LIGHTGREEN: return COLOR_LIGHTGREEN;
                case ChatColor.COLOR_YELLOW: return COLOR_YELLOW;
                case ChatColor.COLOR_YELLOW2: return COLOR_YELLOW2;
                case ChatColor.COLOR_WHITE: return COLOR_WHITE;
                case ChatColor.COLOR_FADE1: return COLOR_FADE1;
                case ChatColor.COLOR_FADE2: return COLOR_FADE2;
                case ChatColor.COLOR_FADE3: return COLOR_FADE3;
                case ChatColor.COLOR_FADE4: return COLOR_FADE4;
                case ChatColor.COLOR_FADE5: return COLOR_FADE5;
                case ChatColor.COLOR_PURPLE: return COLOR_PURPLE;
                case ChatColor.COLOR_DBLUE: return COLOR_DBLUE;
                case ChatColor.COLOR_ALLDEPT: return COLOR_ALLDEPT;
                case ChatColor.COLOR_NEWS: return COLOR_NEWS;
                case ChatColor.COLOR_ROSA: return COLOR_ROSA;
                case ChatColor.COLOR_SABRINA: return COLOR_SABRINA;
                case ChatColor.COLOR_HELPER: return COLOR_HELPER;
                case ChatColor.COLOR_OOC: return COLOR_OOC;
                case ChatColor.TEAM_ALIANCA_COLOR: return TEAM_ALIANCA_COLOR;
                case ChatColor.OBJECTIVE_COLOR: return OBJECTIVE_COLOR;
                case ChatColor.TEAM_PC_COLOR: return TEAM_PC_COLOR;
                case ChatColor.TEAM_GREEN_COLOR: return TEAM_GREEN_COLOR;
                case ChatColor.TEAM_JOB_COLOR: return TEAM_JOB_COLOR;
                case ChatColor.TEAM_HIT_COLOR: return TEAM_HIT_COLOR;
                case ChatColor.TEAM_BLUE_COLOR: return TEAM_BLUE_COLOR;
                case ChatColor.COLOR_ADD: return COLOR_ADD;
                case ChatColor.TEAM_GROVE_COLOR: return TEAM_GROVE_COLOR;
                case ChatColor.COLOR_ANG: return COLOR_ANG;
                case ChatColor.TEAM_VAGOS_COLOR: return TEAM_VAGOS_COLOR;
                case ChatColor.TEAM_BALLAS_COLOR: return TEAM_BALLAS_COLOR;
                case ChatColor.TEAM_AZTECAS_COLOR: return TEAM_AZTECAS_COLOR;
                case ChatColor.TEAM_TRIAD_COLOR: return TEAM_TRIAD_COLOR;
                case ChatColor.TEAM_MS13_COLOR: return TEAM_MS13_COLOR;
                case ChatColor.TEAM_TATTAGLIA_COLOR: return TEAM_TATTAGLIA_COLOR;
                case ChatColor.TEAM_CORLEONE_COLOR: return TEAM_CORLEONE_COLOR;
                case ChatColor.TEAM_CYAN_COLOR: return TEAM_CYAN_COLOR;
                case ChatColor.TEAM_ROTAM_COLOR: return TEAM_ROTAM_COLOR;
                default: return COLOR_WHITE;
            }
        }

        private static readonly dynamic COLOR_GRAD1 = new[] { 180, 181, 183, 1.000 };
        private static readonly dynamic COLOR_GRAD2 = new[] { 191, 192, 194, 1.000 };
        private static readonly dynamic COLOR_GRAD3 = new[] { 203, 204, 206, 1.000 };
        private static readonly dynamic COLOR_GRAD4 = new[] { 216, 216, 216, 1.000 };
        private static readonly dynamic COLOR_GRAD5 = new[] { 227, 227, 227, 1.000 };
        private static readonly dynamic COLOR_GRAD6 = new[] { 240, 240, 240, 1.000 };
        private static readonly dynamic COLOR_GREY = new[] { 175, 175, 175, 0.667 };
        private static readonly dynamic COLOR_GREEN = new[] { 51, 170, 51, 0.667 };
        private static readonly dynamic COLOR_RED = new[] { 170, 51, 51, 0.667 };
        private static readonly dynamic COLOR_LIGHTRED = new[] { 255, 99, 71, 0.667 };
        private static readonly dynamic COLOR_VERMELHO = new[] { 170, 51, 51, 0.667 };
        private static readonly dynamic COLOR_LIGHTBLUE = new[] { 51, 204, 255, 0.667 };
        private static readonly dynamic COLOR_LIGHTGREEN = new[] { 154, 205, 50, 0.667 };
        private static readonly dynamic COLOR_YELLOW = new[] { 255, 255, 0, 0.667 };
        private static readonly dynamic COLOR_YELLOW2 = new[] { 245, 222, 179, 0.667 };
        private static readonly dynamic COLOR_WHITE = new[] { 255, 255, 255, 0.667 };
        private static readonly dynamic COLOR_FADE1 = new[] { 230, 230, 230, 0.902 };
        private static readonly dynamic COLOR_FADE2 = new[] { 200, 200, 200, 0.784 };
        private static readonly dynamic COLOR_FADE3 = new[] { 170, 170, 170, 0.667 };
        private static readonly dynamic COLOR_FADE4 = new[] { 140, 140, 140, 0.549 };
        private static readonly dynamic COLOR_FADE5 = new[] { 110, 110, 110, 0.431 };
        private static readonly dynamic COLOR_PURPLE = new[] { 194, 162, 218, 0.667 };
        private static readonly dynamic COLOR_DBLUE = new[] { 38, 65, 254, 0.667 };
        private static readonly dynamic COLOR_ALLDEPT = new[] { 255, 130, 130, 0.667 };
        private static readonly dynamic COLOR_NEWS = new[] { 255, 165, 0, 0.667 };
        private static readonly dynamic COLOR_ROSA = new[] { 254, 175, 238, 0.588 };
        private static readonly dynamic COLOR_SABRINA = new[] { 248, 16, 167, 0.667 };
        private static readonly dynamic COLOR_HELPER = new[] { 121, 255, 202, 0.588 };
        private static readonly dynamic COLOR_OOC = new[] { 224, 255, 255, 0.667 };
        private static readonly dynamic TEAM_ALIANCA_COLOR = new[] { 143, 186, 169, 0.588 };
        private static readonly dynamic OBJECTIVE_COLOR = new[] { 100, 0, 0, 0.392 };
        private static readonly dynamic TEAM_PC_COLOR = new[] { 145, 118, 98, 0.588 };
        private static readonly dynamic TEAM_GREEN_COLOR = new[] { 255, 255, 255, 0.667 };
        private static readonly dynamic TEAM_JOB_COLOR = new[] { 255, 182, 193, 0.667 };
        private static readonly dynamic TEAM_HIT_COLOR = new[] { 48, 48, 48, 0.588 };
        private static readonly dynamic TEAM_BLUE_COLOR = new[] { 82, 158, 252, 0.588 };
        private static readonly dynamic COLOR_ADD = new[] { 99, 255, 96, 0.667 };
        private static readonly dynamic TEAM_GROVE_COLOR = new[] { 0, 217, 0, 0.502 };
        private static readonly dynamic COLOR_ANG = new[] { 255, 96, 210, 0.667 };
        private static readonly dynamic TEAM_VAGOS_COLOR = new[] { 255, 200, 1, 0.784 };
        private static readonly dynamic TEAM_BALLAS_COLOR = new[] { 128, 0, 255, 0.588 };
        private static readonly dynamic TEAM_AZTECAS_COLOR = new[] { 1, 252, 255, 0.784 };
        private static readonly dynamic TEAM_TRIAD_COLOR = new[] { 164, 209, 255, 0.588 };
        private static readonly dynamic TEAM_MS13_COLOR = new[] { 204, 177, 124, 0.588 };
        private static readonly dynamic TEAM_TATTAGLIA_COLOR = new[] { 223, 107, 38, 0.588 };
        private static readonly dynamic TEAM_CORLEONE_COLOR = new[] { 61, 107, 154, 0.588 };
        private static readonly dynamic TEAM_CYAN_COLOR = new[] { 255, 130, 130, 0.667 };
        private static readonly dynamic TEAM_ROTAM_COLOR = new[] { 108, 108, 249, 0.588 };
    }
}