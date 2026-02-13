using Assets.Scripts;
using HarmonyLib;
using lorex;
using StationeersMods.Interface;
using System;
[StationeersMod("CardboardBoxToolTipMod", "CardboardBoxToolTipMod [StationeersLaunchPad]", "0.2.5919.26060")]
public class CardboardBoxToolTipMod : ModBehaviour
{
  public override void OnLoaded(ContentHandler contentHandler)
  {
    UnityEngine.Debug.Log("CardboardBoxToolTipMod says: Cardboard Boxes should have tooltips!");

    Harmony harmony = new Harmony("CardboardBoxToolTipMod");
    PrefabPatch.prefabs = contentHandler.prefabs;
    harmony.PatchAll();
    UnityEngine.Debug.Log("CardboardBoxToolTipMod Loaded with " + contentHandler.prefabs.Count + " prefab(s)");
  }
}
