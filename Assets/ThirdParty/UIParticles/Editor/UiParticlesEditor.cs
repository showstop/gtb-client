using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace UiParticles
{
    [CustomEditor(typeof(UiParticles))]
    public class UiParticlesEditor : GraphicEditor
    {


        private SerializedProperty m_RenderMode;
        private SerializedProperty m_StretchedSpeedScale;
        private SerializedProperty m_StretchedLenghScale;

        private SerializedProperty m_FrameOverLifetime;

        private SerializedProperty m_VelocityOverLifetimeX;
        private SerializedProperty m_VelocityOverLifetimeY;
        private SerializedProperty m_VelocityOverLifetimeZ;



        protected override void OnEnable()
        {
            base.OnEnable();

            m_RenderMode = serializedObject.FindProperty("m_RenderMode");
            m_StretchedSpeedScale = serializedObject.FindProperty("m_StretchedSpeedScale");
            m_StretchedLenghScale = serializedObject.FindProperty("m_StretchedLenghScale");

            m_FrameOverLifetime = serializedObject.FindProperty("m_FrameOverLifetime");

            m_VelocityOverLifetimeX = serializedObject.FindProperty("m_VelocityOverLifetimeX");
            m_VelocityOverLifetimeY = serializedObject.FindProperty("m_VelocityOverLifetimeY");
            m_VelocityOverLifetimeZ = serializedObject.FindProperty("m_VelocityOverLifetimeZ");


        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UiParticles uiParticleSystem = target as UiParticles;

            if (GUILayout.Button("Apply to nested particle systems"))
            {
                var nested = uiParticleSystem.gameObject.GetComponentsInChildren<ParticleSystem>();
                foreach (var particleSystem in nested)
                {
                    if (particleSystem.GetComponent<UiParticles>() == null)
                        particleSystem.gameObject.AddComponent<UiParticles>();
                }
            }

            EditorGUILayout.PropertyField(m_RenderMode);

            if (uiParticleSystem.RenderMode == UiParticleRenderMode.StreachedBillboard)
            {
                EditorGUILayout.PropertyField(m_StretchedSpeedScale);
                EditorGUILayout.PropertyField(m_StretchedLenghScale);

                #if !UNITY_5_4_OR_NEWER
                if (uiParticleSystem.ParticleSystem.velocityOverLifetime.enabled)
                {
                    EditorGUILayout.PropertyField(m_VelocityOverLifetimeX);
                    EditorGUILayout.PropertyField(m_VelocityOverLifetimeY);
                    EditorGUILayout.PropertyField(m_VelocityOverLifetimeZ);

                }
                #endif
            }

            #if !UNITY_5_4_OR_NEWER


            if (uiParticleSystem.ParticleSystem.textureSheetAnimation.enabled)
            {
                EditorGUILayout.PropertyField(m_FrameOverLifetime);

            }
            #endif

            serializedObject.ApplyModifiedProperties();
        }
    }
}
