using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using Reactor.Utilities.Extensions;
using TownOfUs.Modifiers.Game.Universal;

namespace TownOfUs.Events.Modifiers;

public static class SleuthEvents
{
    [RegisterEvent]
    public static void ReportBodyEventHandler(ReportBodyEvent @event)
    {
        var player = @event.Reporter;
        var target = @event.Target;

        if (player == null || target == null || !player.HasModifier<SleuthModifier>())
        {
            return;
        }

        var mod = player.GetModifier<SleuthModifier>();
        mod?.Reported.Add(target.PlayerId);

        if (!player.AmOwner)
            return;

        List<RoleBehaviour> availableRoles = new List<RoleBehaviour>();

        foreach (PlayerControl ctrl in Helpers.GetAlivePlayers())
        {
            if (ctrl == player)
                continue;

            availableRoles.Add(ctrl.Data.Role);
        }

        if (player.HasModifier<InsaneModifier>())
        {
            InsaneModifier modifier = player.GetModifier<InsaneModifier>();

            if (modifier.PlayerIdToFakeSleuthRole.ContainsKey(target.PlayerId))
                modifier.PlayerIdToFakeSleuthRole.Remove(target.PlayerId);

            RoleBehaviour role = availableRoles.Random();

            modifier.PlayerIdToFakeSleuthRole.Add(target.PlayerId, $"{role.TeamColor.ToTextColor()}{role.NiceName}</color>");
        }

        // Logger<TownOfUsPlugin>.Error($"SleuthEvents.ReportBodyEventHandler '{target.PlayerName}'");
    }
}