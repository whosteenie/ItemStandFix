using System;
using HarmonyLib;
using UnityEngine;

namespace ItemStandFix
{
    [HarmonyPatch(typeof(ZDO), nameof(ZDO.Deserialize))]
    internal static class ZdoDeserializePatch
    {
        private static void Postfix(ZDO __instance)
        {
            if (ZNet.instance == null || !ZNet.instance.IsServer())
            {
                return;
            }

            int itemHash = __instance.GetInt(ZDOVars.s_item, 0);
            if (itemHash == 0)
            {
                ClearEmptyStand(__instance);
                return;
            }

            if (__instance.GetByteArray(ZDOVars.s_itemData, null) == null ||
                ObjectDB.instance == null ||
                !ItemStandMetadata.IsItemStand(__instance))
            {
                return;
            }

            GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(itemHash);
            if (itemPrefab == null)
            {
                return;
            }

            ItemDrop itemDrop = itemPrefab.GetComponent<ItemDrop>();
            if (itemDrop == null)
            {
                return;
            }

            try
            {
                ItemDrop.ItemData itemData = itemDrop.m_itemData.Clone();
                ItemDrop.LoadFromZDO(itemData, __instance, -1);

                int previousQuality = __instance.GetInt(ZDOVars.s_quality, 1);
                int previousVariant = __instance.GetInt(ZDOVars.s_variant, 0);

                if (previousQuality == itemData.m_quality && previousVariant == itemData.m_variant)
                {
                    return;
                }

                __instance.Set(ZDOVars.s_quality, itemData.m_quality, false);
                __instance.Set(ZDOVars.s_variant, itemData.m_variant, false);
                ItemStandMetadata.ForceServerSync(__instance);

                Plugin.Log.LogDebug(
                    $"Server repaired item stand visual metadata: " +
                    $"quality {previousQuality}->{itemData.m_quality}, " +
                    $"variant {previousVariant}->{itemData.m_variant}.");
            }
            catch (Exception exception)
            {
                Plugin.Log.LogWarning(
                    $"Server could not read item stand metadata: {exception.Message}");
            }
        }

        private static void ClearEmptyStand(ZDO zdo)
        {
            if (!ItemStandMetadata.HasVisualMetadata(zdo) ||
                !ItemStandMetadata.IsItemStand(zdo))
            {
                return;
            }

            if (ItemStandMetadata.ClearVisualMetadata(zdo))
            {
                ItemStandMetadata.ForceServerSync(zdo);
                Plugin.Log.LogDebug(
                    "Server cleared visual metadata from an empty item stand.");
            }
        }
    }
}
