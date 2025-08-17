using BepInEx.Logging;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownOfUs.Modifiers.Game.Universal;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Universal;
using UnityEngine;

namespace TownOfUs.Events.Modifiers;

public static class InsaneEvents
{
    [RegisterEvent]
    public static void OnRoundStart(RoundStartEvent ev)
    {
        if (!PlayerControl.LocalPlayer.IsHost())
            return;

        if (!ev.TriggeredByIntro)
            return;

        int insaneAmount = (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.InsaneAmount;

        if (insaneAmount < 1)
            return;

        float insaneChance = OptionGroupSingleton<UniversalModifierOptions>.Instance.InsaneChance.Value;

        List<PlayerControl> possiblePlayers = PlayerControl.AllPlayerControls.ToArray().Where(x => InsaneOptions.IsEligibleForInsane(x)).ToList();

        for (int i = 0; i < insaneAmount; i++)
        {
            if (possiblePlayers.Count() < 1)
                return;

            if (UnityEngine.Random.Range(0, 101) > insaneChance)
                continue;

            PlayerControl player = possiblePlayers.Random();

            InsaneModifier.SetInsanePlayer(player);

            possiblePlayers.Remove(player);
        }
    }

    [RegisterEvent]
    public static void OnTaskCompleted(CompleteTaskEvent ev)
    {
        if (ev.Player != PlayerControl.LocalPlayer)
            return;

        if (!ev.Player.TryGetModifier<InsaneModifier>(out var insane))
            return;

        if (!ev.Player.AllTasksCompleted())
            return;

        InsaneModifier.RevealInsane(insane);
    }
}
