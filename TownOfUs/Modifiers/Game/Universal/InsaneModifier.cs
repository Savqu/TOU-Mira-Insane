using BepInEx.Logging;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownOfUs.Modifiers.Game.Universal;

public class InsaneModifier : BaseModifier
{
    public override string ModifierName => "Insane";

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
