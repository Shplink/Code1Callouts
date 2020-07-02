using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using FivePD.API;

namespace NoiseComplaint
{
    [CalloutProperties("NoiseComplaint", "Shplink", "0.1")]
    public class NoiseComplaint : FivePD.API.Callout
    {
        Ped victim;
        Ped suspect;

        private bool dialogueFinished = false;

        Random random = new Random();

        private Vector3[] coordinates =
        {
            new Vector3(1645.59f, 3729f, 34.39f),
        };

        public NoiseComplaint()
        {
            Vector3 location = coordinates.OrderBy(x => World.GetDistance(x, Game.PlayerPed.Position)).Skip(1).First();

            InitInfo(location);
            ShortName = "Noise Complaint";
            CalloutDescription = "A neighbor has reported a Noise complaint coming from the house next to them. Respond Code 1";
            ResponseCode = 1;
            StartDistance = 30f;
        }


        public override async Task OnAccept()
        {
            InitBlip();
            suspect = await SpawnPed(GetRandomPed(), new Vector3(1645.59f, 3729f, 34.39f));
            victim = await SpawnPed(GetRandomPed(), new Vector3(1659.75f, 3751.38f, 34.63f));

            suspect.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
            victim.AlwaysKeepTask = true;
            victim.BlockPermanentEvents = true;
        }

        public override void OnStart(Ped player)
        {
            base.OnStart(player);

            int x = random.Next(1, 100 + 1);
            if (x < 50)
            {
                LoudNoise();
            }
            else if (x >= 50)
            {
                NoNoise();
            }
        }

        private void LoudNoise()
        {
            DrawSubtitle("As you approach the neighbor's house, you would notice ~r~LOUD MUSIC~w~ from the house", 5000);
            Wait(5000);
            DrawSubtitle("As you arrive on scene, the neigbhor turns the music off.", 5000);
            dialogueFinished = true;
        }

        private void NoNoise()
        {
            DrawSubtitle("As you approach the neighbor's house, you would not notice any loud noise.", 5000);
            dialogueFinished = true;
        }

        private void Notify(string message)
        {
            SetNotificationTextEntry("STRING");
            AddTextComponentString(message);
            DrawNotification(false, false);
        }

        private void DrawSubtitle(string message, int duration)
        {
            BeginTextCommandPrint("STRING");
            AddTextComponentSubstringPlayerName(message);
            EndTextCommandPrint(duration, false);
        }
    }
}
