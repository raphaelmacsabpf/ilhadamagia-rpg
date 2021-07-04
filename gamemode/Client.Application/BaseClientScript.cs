using CitizenFX.Core;
using GF.CrossCutting.Enums;

namespace Client
{
    public class BaseClientScript : BaseScript
    {
        public void CallServerAction(ServerEvent serverEvent, params object[] args)
        {
            TriggerServerEvent(serverEvent.ToString(), args);
        }
    }
}
