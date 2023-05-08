

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CRTFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class CustomPassSettings
    {
        public RenderPassEvent rpe = RenderPassEvent.BeforeRenderingPostProcessing;
        public float curvature = 5.0f;
        public float vignette_width = 50.0f;
    }

    [SerializeField] private CustomPassSettings settings;
    private CRTPass custom_pass;

    public override void Create()
    {
        custom_pass = new CRTPass(settings);
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {

#if UNITY_EDITOR
        if (renderingData.cameraData.isSceneViewCamera) return;
#endif
        renderer.EnqueuePass(custom_pass);
    }
}


