using System.Text;
using HarmonyLib;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Items;
using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;

namespace lorex.patches
{
  [HarmonyPatch(typeof(Item), nameof(Item.GetExtendedText))]
  public static class Item_GetExtendedText_Patch
  {

    private static readonly HashSet<string> IgnoredPrefabs = new()
    {
      "ItemPortablesPackage",
      "ItemInsulatedCanisterPackage",
      "ItemWaterBottlePackage",
      "ItemWaterBottleBag",
      "ItemResidentialPackage",
      "ItemMiningPackage",
      "ItemCerealBarBox",
      "ItemCerealBarBag",
    };
    public static void Postfix(Item __instance, ref StringBuilder __result)
    {
      // Only apply this to CardboardBoxes
      if (__instance is not CardboardBox box)
        return;

      string prefabName = __instance.GetPrefabName();
      if (IgnoredPrefabs.Contains(prefabName))
        return;

      try
      {
        if (__result == null)
          __result = new StringBuilder();

        __result.AppendLine("Contents:");

        var slots = box.Slots;
        if (slots == null || slots.Count == 0)
        {
          __result.AppendLine("  (No storage slots)");
          return;
        }

        bool hasItems = false;
        int itemCount = 0;
        const int maxDisplayCount = 20; // Keep maxDisplayCount higher than default in case someone makes a mod that increases box slots.

        foreach (var slot in slots)
        {
          if (slot == null || slot.IsEmpty())
            continue;

          hasItems = true;
          itemCount++;

          // Retrieve item from slot
          var item = slot.Get<Item>() as Item;
          if (item == null)
            continue;

          string itemName = item.DisplayName ?? item.name ?? "Unknown Item";
          int quantity = (item is Stackable s && s.Quantity > 1) ? s.Quantity : 1;

          string colorItemHex = "#66FF00";    // Green for item name
          string colorQuantityHex = "#FFED29"; // Yellow for quantity

          string coloredItemName = $"<color={colorItemHex}>{itemName}</color>";
          string coloredQuantity = $"<color={colorQuantityHex}>{quantity}</color>";

          if (quantity > 1)
            __result.AppendLine($"  • {coloredItemName} x{coloredQuantity}");
          else
            __result.AppendLine($"  • {coloredItemName}");

          if (itemCount >= maxDisplayCount)
          {
            __result.AppendLine("  ...and more");
            break;
          }
        }

        if (!hasItems)
          __result.AppendLine("  (Empty)");
      }
      catch (System.Exception ex)
      {
        UnityEngine.Debug.Log($"[CardboardBoxToolTipMod] Error in Item_GetExtendedText_Patch: {ex}");
      }
    }
  }
}
