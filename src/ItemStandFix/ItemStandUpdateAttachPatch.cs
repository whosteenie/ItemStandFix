using HarmonyLib;
using UnityEngine;

namespace ItemStandFix
{
    [HarmonyPatch(typeof(ItemStand), "UpdateAttach")]
    internal static class ItemStandUpdateAttachPatch
    {
        private sealed class PlacementState
        {
            public ZDO Zdo;
            public int PreviousItemHash;
            public GameObject ExpectedItemPrefab;
            public int Quality;
            public int Variant;
        }

        private static void Prefix(
            ItemDrop.ItemData ___m_queuedItem,
            ZNetView ___m_nview,
            out PlacementState __state)
        {
            __state = null;

            if (___m_queuedItem == null ||
                ___m_queuedItem.m_dropPrefab == null ||
                ___m_nview == null ||
                !___m_nview.IsValid())
            {
                return;
            }

            ZDO zdo = ___m_nview.GetZDO();
            if (zdo == null)
            {
                return;
            }

            __state = new PlacementState
            {
                Zdo = zdo,
                PreviousItemHash = zdo.GetInt(ZDOVars.s_item, 0),
                ExpectedItemPrefab = ___m_queuedItem.m_dropPrefab,
                Quality = ___m_queuedItem.m_quality,
                Variant = ___m_queuedItem.m_variant
            };
        }

        private static void Postfix(PlacementState __state)
        {
            if (__state == null || __state.PreviousItemHash != 0)
            {
                return;
            }

            int attachedItemHash = __state.Zdo.GetInt(ZDOVars.s_item, 0);
            if (attachedItemHash == 0 || ObjectDB.instance == null)
            {
                return;
            }

            GameObject attachedItemPrefab = ObjectDB.instance.GetItemPrefab(attachedItemHash);
            if (attachedItemPrefab == null || attachedItemPrefab != __state.ExpectedItemPrefab)
            {
                return;
            }

            int previousQuality = __state.Zdo.GetInt(ZDOVars.s_quality, 1);
            int previousVariant = __state.Zdo.GetInt(ZDOVars.s_variant, 0);

            __state.Zdo.Set(ZDOVars.s_quality, __state.Quality, false);
            __state.Zdo.Set(ZDOVars.s_variant, __state.Variant, false);

            Plugin.Log.LogDebug(
                $"Stored item stand visual metadata: " +
                $"quality {previousQuality}->{__state.Quality}, " +
                $"variant {previousVariant}->{__state.Variant}.");
        }
    }
}
