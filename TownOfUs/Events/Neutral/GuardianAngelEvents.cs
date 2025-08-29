﻿using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfUs.Buttons;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Game.Universal;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Options;
using TownOfUs.Options.Modifiers.Universal;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;

namespace TownOfUs.Events.Neutral;

public static class GuardianAngelEvents
{
    [RegisterEvent]
    public static void MiraButtonClickEventHandler(MiraButtonClickEvent @event)
    {
        var button = @event.Button as CustomActionButton<PlayerControl>;
        var target = button?.Target;

        if (target == null || button == null || !button.CanClick() || button is not IKillButton)
        {
            return;
        }

        CheckForGaProtection(@event, target);
    }

    [RegisterEvent]
    public static void MiraButtonCancelledEventHandler(MiraButtonCancelledEvent @event)
    {
        var source = PlayerControl.LocalPlayer;
        var button = @event.Button as CustomActionButton<PlayerControl>;
        var target = button?.Target;
        if (target == null || button is not IKillButton)
        {
            return;
        }

        if (target && !target!.HasModifier<GuardianAngelProtectModifier>())
        {
            return;
        }

        ResetButtonTimer(source, button);
    }

    [RegisterEvent]
    public static void BeforeMurderEventHandler(BeforeMurderEvent @event)
    {
        var source = @event.Source;
        var target = @event.Target;

        if (!CheckForGaProtection(@event, target, source))
        {
            return;
        }

        ResetButtonTimer(source);
    }

    [RegisterEvent]
    public static void PlayerDeathEventHandler(PlayerDeathEvent @event)
    {
        foreach (var ga in CustomRoleUtils.GetActiveRolesOfType<GuardianAngelTouRole>())
        {
            ga.CheckTargetDeath(@event.Player);
        }
    }

    private static bool CheckForGaProtection(MiraCancelableEvent @event, PlayerControl target,
        PlayerControl? source = null)
    {
        if (MeetingHud.Instance || ExileController.Instance)
        {
            return false;
        }

        if (!target.HasModifier<GuardianAngelProtectModifier>() ||
            source == null ||
            source.PlayerId == target.PlayerId ||
            (source.TryGetModifier<IndirectAttackerModifier>(out var indirect) && indirect.IgnoreShield))
        {
            return false;
        }

        if (target.HasModifier<GuardianAngelProtectModifier>())
        {
            if (target.GetModifier<GuardianAngelProtectModifier>().Guardian.HasModifier<InsaneModifier>())
            {
                return false;
            }
        }

        @event.Cancel();

        return true;
    }

    private static void ResetButtonTimer(PlayerControl source, CustomActionButton<PlayerControl>? button = null)
    {
        var reset = OptionGroupSingleton<GeneralOptions>.Instance.TempSaveCdReset;

        button?.SetTimer(reset);

        // Reset impostor kill cooldown if they attack a shielded player
        if (!source.AmOwner || !source.IsImpostor())
        {
            return;
        }

        source.SetKillTimer(reset);
    }
}