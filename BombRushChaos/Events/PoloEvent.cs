using BombRushChaos.Scripts;
using Reptile;
using UnityEngine;

namespace BombRushChaos.Events;

public class PoloEvent : IEvent {
    public string Name => "Polo.";
    public float Duration => 30f;

    private GameObject polo;

    public PoloEvent() {
        var assets = Core.Instance.Assets;
        const string bundle = "city_assets";
        if (!assets.availableBundles.ContainsKey(bundle)) assets.LoadAssetBundleByName(bundle);
        var assetBundle = assets.availableBundles[bundle].AssetBundle;

        var prefab = assetBundle.LoadAsset<GameObject>("Mascot_Polo_street");
        var material = assetBundle.LoadAsset<Material>("MascotAtlas_MAT");

        this.polo = Object.Instantiate(prefab);
        this.polo.GetComponent<MeshRenderer>().material = material;
        this.polo.AddComponent<ChasingPolo>();
    }

    public void Update() { }

    public void Dispose() {
        Object.Destroy(this.polo);
    }
}
