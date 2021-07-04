using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System.Drawing;

namespace Client.Application
{
    public class DrawTextAPI : BaseClientScript
    {
        private SizeF resolution;
        private float height;
        private readonly float ratio;
        private readonly float width;

        public DrawTextAPI(bool ignoreFiveMInitialization)
        {
            this.resolution = GetScreenResolutionMaintainRatio();
            this.height = Screen.Resolution.Height;
            this.ratio = resolution.Width / resolution.Height;
            this.width = height * ratio;
        }

        public SizeF GetScreenResolutionMaintainRatio()
        {
            return new SizeF(Screen.Resolution.Height * ((float)Screen.Resolution.Width / (float)Screen.Resolution.Height), Screen.Resolution.Height);
        }

        public async void DrawGameSprite(string dict, string txtName, float xPos, float yPos, float width, float height, float heading, int r, int g, int b, int alpha, int vAlig = 0, int hAlig = 0)
        {
            if (!API.IsHudPreferenceSwitchedOn() || !Screen.Hud.IsVisible) return;

            if (!API.HasStreamedTextureDictLoaded(dict))
                API.RequestStreamedTextureDict(dict, true);

            float w = width / this.width;
            float h = height / this.height;
            float x = ToHorizontalAlignment(hAlig, xPos) / this.width + w * 0.5f;
            float y = ToVerticalAlignment(vAlig, yPos) / this.height + h * 0.5f;

            API.DrawSprite(dict, txtName, x, y, w, h, heading, r, g, b, alpha);
        }

        public void DrawRectangle(float xPos, float yPos, float wSize, float hSize, int r, int g, int b, int alpha, int vAlig = 0, int hAlig = 0)
        {
            if (!API.IsHudPreferenceSwitchedOn() || !Screen.Hud.IsVisible) return;

            float w = wSize / width;
            float h = hSize / height;
            float x = ToHorizontalAlignment(hAlig, xPos) / width + w * 0.5f;
            float y = ToVerticalAlignment(vAlig, yPos) / height + h * 0.5f;

            API.DrawRect(x, y, w, h, r, g, b, alpha);
        }

        public void DrawText(string caption, float xPos, float yPos, float scale, int r, int g, int b, int alpha, int font, int justify, bool shadow, bool outline, int wordWrap, int vAlig = 0, int hAlig = 0)
        {
            if (!API.IsHudPreferenceSwitchedOn() || !Screen.Hud.IsVisible) return;

            float x = ToHorizontalAlignment(hAlig, xPos) / width;
            float y = ToVerticalAlignment(vAlig, yPos) / height;

            API.SetTextFont(font);
            API.SetTextScale(1f, scale);
            API.SetTextColour(r, g, b, alpha);
            if (shadow) API.SetTextDropShadow();
            if (outline) API.SetTextOutline();

            switch (justify)
            {
                case 1:
                    API.SetTextCentre(true);
                    break;

                case 2:
                    API.SetTextRightJustify(true);
                    API.SetTextWrap(0, x);
                    break;
            }

            if (wordWrap != 0)
                API.SetTextWrap(x, (xPos + wordWrap) / width);

            API.BeginTextCommandDisplayText("STRING");

            const int maxStringLength = 99;
            for (int i = 0; i < caption.Length; i += maxStringLength)
                API.AddTextComponentSubstringPlayerName(caption.Substring(i, System.Math.Min(maxStringLength, caption.Length - i)));
            API.EndTextCommandDisplayText(x, y);
        }

        public float ToVerticalAlignment(int type, float x)
        {
            if (type == 2)
                return resolution.Height - x;
            if (type == 1)
                return resolution.Height / 2 + x;
            return x;
        }

        public float ToHorizontalAlignment(int type, float y)
        {
            if (type == 2)
                return resolution.Width - y;
            if (type == 1)
                return resolution.Width / 2 + y;
            return y;
        }
    }
}