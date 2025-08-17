using BepInEx.Logging;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.ModifierDisplay;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
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
using TownOfUs.Options.Modifiers.Universal;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Universal;

public class InsaneModifier : BaseModifier
{
    public override string ModifierName => "Insane";
    public override string GetDescription() => "Your abilities cannot be trusted. Your modifiers and role attributes were turned against you!";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Insane;

    public override bool HideOnUi => !WasRevealed;

    public Dictionary<byte, string> PlayerIdToFakeSleuthRole = new Dictionary<byte, string>();

    // Make it so CONFIRMED dead players have this set to true!
    public bool WasRevealed = false;

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

    [MethodRpc((uint)TownOfUsRpc.RevealInsane, SendImmediately = true)]
    public static void RevealInsane(InsaneModifier insaneToReveal)
    {
        InsaneOptions options = OptionGroupSingleton<InsaneOptions>.Instance;

        switch (options.InsaneRevealsTo)
        {
            case InsaneRevealsTo.Self:
                if (insaneToReveal.Player != PlayerControl.LocalPlayer)
                    break;

                insaneToReveal.WasRevealed = true;
                break;
            case InsaneRevealsTo.Others:
                if (insaneToReveal.Player == PlayerControl.LocalPlayer)
                    break;

                insaneToReveal.WasRevealed = true;
                break;
            case InsaneRevealsTo.Everyone:
                insaneToReveal.WasRevealed = true;
                break;
        }

        Logger.GlobalInstance.Info($"Insane {insaneToReveal.Player.Data.PlayerName} was revealed to {options.InsaneRevealsTo}!");

        if (insaneToReveal.WasRevealed && insaneToReveal.Player == PlayerControl.LocalPlayer)
        {
            var modsTab = ModifierDisplayComponent.Instance;

            if (modsTab != null)
                modsTab.RefreshModifiers();

            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Insane, alpha: 0.05f));
        }
    }
}
