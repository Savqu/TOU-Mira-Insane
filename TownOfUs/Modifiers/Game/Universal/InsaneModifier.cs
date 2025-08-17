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

    public static void RandomizeInsaneSwaps()
    {
        foreach (SwapperRole swapper in CustomRoleUtils.GetActiveRolesOfType<SwapperRole>())
        {
            if (!swapper.Player.HasModifier<InsaneModifier>())
                continue;

            if (swapper.Swap1 == null || swapper.Swap2 == null)
                continue;

            byte swap1 = swapper.Swap1.TargetPlayerId;
            byte swap2 = swapper.Swap2.TargetPlayerId;

            swap1 = Helpers.GetAlivePlayers().Random().PlayerId;
            swap2 = Helpers.GetAlivePlayers().Where(x => x.PlayerId != swap1).Random().PlayerId;

            SwapperRole.RpcSyncSwaps(swapper.Player, swap1, swap2);
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
