using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace TownOfUs.Assets;

public static class TouModifierIcons
{
    public static LoadableAsset<Sprite> Aftermath { get; } = new LoadableBundleAsset<Sprite>("Aftermath", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Bait { get; } = new LoadableBundleAsset<Sprite>("Bait", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> ButtonBarry { get; } = new LoadableBundleAsset<Sprite>("ButtonBarry", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Celebrity { get; } = new LoadableBundleAsset<Sprite>("Celebrity", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Diseased { get; } = new LoadableBundleAsset<Sprite>("Diseased", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Egotist { get; } = new LoadableBundleAsset<Sprite>("Egotist", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Frosty { get; } = new LoadableBundleAsset<Sprite>("Frosty", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Multitasker { get; } = new LoadableBundleAsset<Sprite>("Multitasker", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Noisemaker { get; } = new LoadableBundleAsset<Sprite>("Noisemaker", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Operative { get; } = new LoadableBundleAsset<Sprite>("Operative", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Rotting { get; } = new LoadableBundleAsset<Sprite>("Rotting", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Scientist { get; } = new LoadableBundleAsset<Sprite>("Scientist", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Scout { get; } = new LoadableBundleAsset<Sprite>("Scout", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Taskmaster { get; } = new LoadableBundleAsset<Sprite>("Taskmaster", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Torch { get; } = new LoadableBundleAsset<Sprite>("Torch", TouAssets.MainBundle);

    public static LoadableAsset<Sprite> Disperser { get; } = new LoadableBundleAsset<Sprite>("Disperser", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> DoubleShot { get; } = new LoadableBundleAsset<Sprite>("DoubleShot", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Saboteur { get; } = new LoadableBundleAsset<Sprite>("Saboteur", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Telepath { get; } = new LoadableBundleAsset<Sprite>("Telepath", TouAssets.MainBundle);
    public static LoadableAsset<Sprite> Underdog { get; } = new LoadableBundleAsset<Sprite>("Underdog", TouAssets.MainBundle);

    public static LoadableAsset<Sprite> Insane { get; } = new LoadableResourceAsset($"Insane", TouAssets.MainBundle);

    public static LoadableAsset<Sprite> FirstRoundShield { get; } =
        new LoadableBundleAsset<Sprite>("FirstRoundShield", TouAssets.MainBundle);
}