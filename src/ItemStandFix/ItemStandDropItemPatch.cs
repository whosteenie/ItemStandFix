using HarmonyLib;

namespace ItemStandFix
{
    [HarmonyPatch(typeof(ItemStand), "DropItem")]
    internal static class ItemStandDropItemPatch
    {
        private static void Postfix(ZNetView ___m_nview)
        {
            if (___m_nview == null || !___m_nview.IsValid() || !___m_nview.IsOwner())
            {
                return;
            }

            ZDO zdo = ___m_nview.GetZDO();
            if (zdo == null || zdo.GetInt(ZDOVars.s_item, 0) != 0)
            {
                return;
            }

            if (ItemStandMetadata.ClearVisualMetadata(zdo))
            {
                Plugin.Log.LogDebug("Cleared visual metadata from an empty item stand.");
            }
        }
    }
}
