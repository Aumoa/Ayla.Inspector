using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ayla
{
    [Serializable]
    public class PreviewScene
    {
        [SerializeField]
        private Scene m_Scene;
        [SerializeField]
        private Camera m_Camera;
        [SerializeField]
        private RenderTexture m_RenderTexture;
        [SerializeField]
        private Light m_DefaultLight;

        public Camera Camera => m_Camera;
        public Scene Scene => m_Scene;
        public Light DefaultLight => m_DefaultLight;

        public static PreviewScene NewScene()
        {
            var s = new PreviewScene
            {
                m_Scene = EditorSceneManager.NewPreviewScene(),
                m_Camera = EditorUtility.CreateGameObjectWithHideFlags("Preview Camera", HideFlags.HideAndDontSave, typeof(Camera)).GetComponent<Camera>(),
                m_DefaultLight = EditorUtility.CreateGameObjectWithHideFlags("Default Directional Light", HideFlags.HideAndDontSave, typeof(Light)).GetComponent<Light>()
            };

            SceneManager.MoveGameObjectToScene(s.m_Camera.gameObject, s.m_Scene);
            s.m_Camera.cameraType = CameraType.Preview;
            s.m_Camera.scene = s.m_Scene;
            s.m_Camera.enabled = false;
            SceneManager.MoveGameObjectToScene(s.m_DefaultLight.gameObject, s.m_Scene);
            s.m_DefaultLight.type = LightType.Directional;
            s.m_DefaultLight.lightmapBakeType = LightmapBakeType.Realtime;
            s.m_DefaultLight.color = new Color(1.0f, 0.956862f, 0.839215f);
            s.m_DefaultLight.intensity = 1;
            return s;
        }

        public void Cleanup()
        {
            EditorSceneManager.ClosePreviewScene(m_Scene);

            if (m_RenderTexture != null)
            {
                RenderTexture.ReleaseTemporary(m_RenderTexture);
                m_RenderTexture = null;
            }
        }

        public Texture RenderScene(Vector2 size)
        {
            TryReallocateRenderTexture(size);
            m_Camera.Render();
            return m_RenderTexture;
        }

        private void TryReallocateRenderTexture(Vector2 size)
        {
            if (m_RenderTexture == null || m_RenderTexture.texelSize != size)
            {
                if (m_RenderTexture != null)
                {
                    RenderTexture.ReleaseTemporary(m_RenderTexture);
                }

                m_RenderTexture = RenderTexture.GetTemporary((int)size.x, (int)size.y, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
                m_Camera.targetTexture = m_RenderTexture;
                m_Camera.enabled = true;
            }
        }
    }
}