using UnityEngine;

namespace ItemStandFix
{
    internal static class ItemStandMetadata
    {
        internal static bool IsItemStand(ZDO zdo)
        {
            if (zdo == null || ZNetScene.instance == null)
            {
                return false;
            }

            GameObject standPrefab = ZNetScene.instance.GetPrefab(zdo.GetPrefab());
            return standPrefab != null && standPrefab.GetComponent<ItemStand>() != null;
        }

        internal static bool HasVisualMetadata(ZDO zdo)
        {
            return zdo.GetInt(ZDOVars.s_quality, out _) ||
                   zdo.GetInt(ZDOVars.s_variant, out _);
        }

        internal static bool ClearVisualMetadata(ZDO zdo)
        {
            bool removedQuality = zdo.RemoveInt(ZDOVars.s_quality);
            bool removedVariant = zdo.RemoveInt(ZDOVars.s_variant);
            return removedQuality || removedVariant;
        }

        internal static void ForceServerSync(ZDO zdo)
        {
            if (ZDOMan.instance != null)
            {
                ZDOMan.instance.ForceSendZDO(zdo.m_uid);
            }
        }
    }
}
