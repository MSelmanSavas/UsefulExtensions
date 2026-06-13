using UnityEngine;
using UnityEngine.UI;

namespace UsefulExtensions.ScrollRectExtensions
{
    public static class ScrollRectExtensions
    {
        /// <summary>
        /// Calculates the vertical normalized position required to center a specific child RectTransform within the ScrollRect's viewport.
        /// </summary>
        /// <param name="scrollRect">The ScrollRect being extended.</param>
        /// <param name="targetChild">The RectTransform of the child element you want to center.</param>
        /// <param name="clamp">If true, clamps the result between 0 (bottom) and 1 (top). If false, allows over-scrolling values.</param>
        /// <returns>The normalized Y position (0 to 1) to apply to the ScrollRect.</returns>
        public static void SetVerticalNormalizedPositionForCentering(this ScrollRect scrollRect, RectTransform targetChild, bool clamp = true)
        {
            scrollRect.verticalNormalizedPosition = GetNormalizedPositionToCenter(scrollRect, targetChild).y;
        }

        /// <summary>
        /// Returns the ScrollRect.normalizedPosition that would center `target` in the viewport.
        /// Works for any content pivot/anchors and for both vertical & horizontal scroll.
        /// </summary>
        public static UnityEngine.Vector2 GetNormalizedPositionToCenter(ScrollRect sr, RectTransform target)
        {
            RectTransform content = sr.content;
            RectTransform viewport = sr.viewport != null ? sr.viewport : (RectTransform)sr.transform;

            // 1) Target center in Content local space
            UnityEngine.Vector3 worldCenter = target.TransformPoint(target.rect.center);
            UnityEngine.Vector3 localCenter = content.InverseTransformPoint(worldCenter);

            float cw = content.rect.width;
            float ch = content.rect.height;
            float vw = viewport.rect.width;
            float vh = viewport.rect.height;

            float scrollableW = Mathf.Max(0f, cw - vw);
            float scrollableH = Mathf.Max(0f, ch - vh);

            // 2) Content edges in Content local space (depend on pivot)
            float left = -content.pivot.x * cw;
            float right = (1f - content.pivot.x) * cw;
            float bottom = -content.pivot.y * ch;
            float top = (1f - content.pivot.y) * ch;

            // 3) Distances from left/top to the item center
            float fromLeft = localCenter.x - left;
            float fromTop = top - localCenter.y;

            // 4) Desired content offsets so the item center sits at viewport center
            float targetX = Mathf.Clamp(fromLeft - vw * 0.5f, 0f, scrollableW);
            float targetY = Mathf.Clamp(fromTop - vh * 0.5f, 0f, scrollableH);

            // 5) Convert to normalized (Unity: H 0=left 1=right, V 1=top 0=bottom)
            float hNorm = scrollableW > 0f ? targetX / scrollableW : 0f;
            float vNorm = scrollableH > 0f ? 1f - (targetY / scrollableH) : 1f;

            return new UnityEngine.Vector2(hNorm, vNorm);
        }
    }
}