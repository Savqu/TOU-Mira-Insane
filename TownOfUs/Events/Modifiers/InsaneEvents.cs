using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownOfUs.Events.Modifiers;

public static class InsaneEvents
{
    [RegisterEvent]
    public static void OnRoundStart(RoundStartEvent ev)
    {
        // Assign all insanes.
    }
}
