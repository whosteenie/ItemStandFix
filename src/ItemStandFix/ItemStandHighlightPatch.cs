using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace ItemStandFix
{
    internal static class ItemStandStyleRestore
    {
        private static readonly HashSet<WearNTear> Pending = new HashSet<WearNTear>();
        private static readonly MaterialPropertyBlock PropertyBlock = new MaterialPropertyBlock();
        private static readonly int StyleProperty = Shader.PropertyToID("_Style");

        internal static void Queue(WearNTear wearNTear)
        {
            if (wearNTear != null && wearNTear.GetComponent<ItemStand>() != null)
            {
                Pending.Add(wearNTear);
            }
        }

        internal static void RestorePending()
        {
            foreach (WearNTear wearNTear in Pending)
            {
                if (wearNTear == null)
                {
                    continue;
                }

                ZNetView netView = wearNTear.GetComponent<ZNetView>();
                ItemStyle itemStyle = wearNTear.GetComponentInChildren<ItemStyle>();
                if (netView == null || !netView.IsValid() || itemStyle == null)
                {
                    continue;
                }

                int variant = netView.GetZDO().GetInt(ZDOVars.s_variant, 0);
                Renderer[] renderers = itemStyle.GetComponentsInChildren<Renderer>(true);
                foreach (Renderer renderer in renderers)
                {
                    PropertyBlock.Clear();
                    renderer.GetPropertyBlock(PropertyBlock);
                    PropertyBlock.SetInt(StyleProperty, variant);
                    renderer.SetPropertyBlock(PropertyBlock);
                }
            }

            Pending.Clear();
        }
    }

    [HarmonyPatch(typeof(ItemStand), "SetVisualItem")]
    internal static class ItemStandSetVisualItemPatch
    {
        private static readonly MethodInfo RegisterRenderers = AccessTools.Method(
            typeof(MaterialMan),
            "RegisterRenderers",
            new[] { typeof(GameObject), typeof(bool) });

        private static void Prefix(GameObject ___m_visualItem, out GameObject __state)
        {
            __state = ___m_visualItem;
        }

        private static void Postfix(
            ItemStand __instance,
            GameObject ___m_visualItem,
            GameObject __state)
        {
            if (__state == ___m_visualItem || MaterialMan.instance == null)
            {
                return;
            }

            RegisterRenderers.Invoke(
                MaterialMan.instance,
                new object[] { __instance.gameObject, true });
        }
    }

    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.Highlight))]
    internal static class WearNTearHighlightPatch
    {
        private static void Postfix(WearNTear __instance)
        {
            ItemStandStyleRestore.Queue(__instance);
        }
    }

    [HarmonyPatch(typeof(WearNTear), "ResetHighlight")]
    internal static class WearNTearResetHighlightPatch
    {
        private static void Postfix(WearNTear __instance)
        {
            ItemStandStyleRestore.Queue(__instance);
        }
    }

    [HarmonyPatch(typeof(MaterialMan), "Update")]
    internal static class MaterialManUpdatePatch
    {
        private static void Postfix()
        {
            ItemStandStyleRestore.RestorePending();
        }
    }
}
