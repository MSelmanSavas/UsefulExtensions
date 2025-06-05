using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace UsefulExtensions.Editor.Animator
{
    [CustomPreview(typeof(AnimatorState))]
    public class AnimatorStateObjectPreview : ObjectPreview
    {
        static FieldInfo _cachedAvatarPreviewField;
        static FieldInfo _cachedTimeControlField;
        static FieldInfo _cachedStopTimeField;

        UnityEditor.Editor _preview;
        int _animationClipId;

        public override void Initialize(Object[] targets)
        {
            base.Initialize(targets);

            if (targets.Length > 1 || Application.isPlaying)
            {
                return;
            }

            SourceAnimationClipEditorFields();

            AnimatorState state = target as AnimatorState;

            if (state == null)
            {
                return;
            }

            AnimationClip clip = GetAnimationClip(target as AnimatorState);

            if (clip != null)
            {
                _preview = UnityEditor.Editor.CreateEditor(clip);
                _animationClipId = clip.GetInstanceID();
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();
            CleanupPreviewEditor();
        }

        public override bool HasPreviewGUI()
        {
            return _preview?.HasPreviewGUI() ?? false;
        }

        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
        {
            base.OnInteractivePreviewGUI(r, background);

            AnimationClip currentClip = GetAnimationClip(target as AnimatorState);

            if (currentClip != null && currentClip.GetInstanceID() != _animationClipId)
            {
                CleanupPreviewEditor();
                _preview = UnityEditor.Editor.CreateEditor(currentClip);
                _animationClipId = currentClip.GetInstanceID();
                return;
            }

            UpdateAnimationClipEditor(_preview, currentClip);
            _preview?.OnInteractivePreviewGUI(r, background);
        }

        AnimationClip GetAnimationClip(AnimatorState animatorState)
        {
            return animatorState?.motion as AnimationClip;
        }

        void CleanupPreviewEditor()
        {
            if (_preview != null)
            {
                UnityEngine.Object.DestroyImmediate(_preview);
                _preview = null;
                _animationClipId = -1;
            }
        }

        static void SourceAnimationClipEditorFields()
        {
            if (_cachedAvatarPreviewField != null)
            {
                return;
            }

            _cachedAvatarPreviewField = System.Type.GetType("UnityEditor.AnimationClipEditor, UnityEditor").GetField("m_AvatarPreview", BindingFlags.NonPublic | BindingFlags.Instance);
            _cachedTimeControlField = System.Type.GetType("UnityEditor.AvatarPreview, UnityEditor").GetField("timeControl", BindingFlags.Public | BindingFlags.Instance);
            _cachedStopTimeField = System.Type.GetType("UnityEditor.TimeControl, UnityEditor").GetField("stopTime", BindingFlags.Public | BindingFlags.Instance);
        }

        void UpdateAnimationClipEditor(UnityEditor.Editor editor, AnimationClip clip)
        {
            if (_cachedAvatarPreviewField == null || _cachedTimeControlField == null || _cachedStopTimeField == null)
            {
                return;
            }

            var avatarPreview = _cachedAvatarPreviewField.GetValue(editor);
            var timeControl = _cachedTimeControlField.GetValue(avatarPreview);

            _cachedStopTimeField.SetValue(timeControl, clip.length);
        }
    }
}