using BepInEx.Logging;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Roles.Crewmate;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Universal;

public class InsaneModifier : BaseModifier
{
    public override string ModifierName => "Insane";

    public void AddMysticDeathWithDelay(PlayerControl target, PlayerControl mystic)
    {
        IEnumerator AddWithDelay()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 10f));
            target?.AddModifier<MysticDeathNotifierModifier>(mystic);
        }

        Coroutines.Start(AddWithDelay());
    }

    [MethodRpc((uint)TownOfUsRpc.ReportAsAnotherPlayer, SendImmediately = true)]
    public static void ForceAnotherPlayerToReport(PlayerControl forcer, PlayerControl player, NetworkedPlayerInfo target, bool baitReport = false)
    {
        player.ReportDeadBody(target);

        if (player.AmOwner)
        {
            if (baitReport)
            {
                var notif = Helpers.CreateAndShowNotification(
                    $"<b>{TownOfUsColors.Bait.ToTextColor()}{target.PlayerName} was a Bait, causing you to self report.</color></b>",
                    Color.white, spr: TouModifierIcons.Bait.LoadAsset());

                notif.Text.SetOutlineThickness(0.35f);
                notif.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
        }
    }

    [MethodRpc((uint)TownOfUsRpc.SetInsane, SendImmediately = true)]
    public static void SetInsanePlayer(PlayerControl target)
    {
        if (PlayerControl.LocalPlayer.IsHost())
        {
            if (TownOfUsPlugin.IsDevBuild) Logger.GlobalInstance.Info("I'm the host!");
        }

        if (TownOfUsPlugin.IsDevBuild) Logger.GlobalInstance.Info($"Recevied message to turn {target.Data.PlayerName} into Insane!");

        target.AddModifier<InsaneModifier>();
    }
}
