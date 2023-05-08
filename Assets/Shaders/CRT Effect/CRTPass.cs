
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CRTPass : ScriptableRenderPass
{
    private CRTFeature.CustomPassSettings settings;

    private RenderTargetIdentifier colour_buffer, pixel_buffer;
    private int pb_ID = Shader.PropertyToID("_PixelBuffer");

    private Material material;

    public CRTPass(CRTFeature.CustomPassSettings settings)
    {
        this.settings = settings;
        this.renderPassEvent = settings.rpe;
        if (material == null) material = CoreUtils.CreateEngineMaterial("Hidden/CRT");
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        colour_buffer = renderingData.cameraData.renderer.cameraColorTarget;
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

        material.SetFloat("_Curvature", settings.curvature);
        material.SetFloat("_VignetteWidth", settings.vignette_width);

        cmd.GetTemporaryRT(pb_ID, descriptor, FilterMode.Point);
        pixel_buffer = new RenderTargetIdentifier(pb_ID);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("CRT Pass")))
        {
            Blit(cmd, colour_buffer, pixel_buffer, material);
            Blit(cmd, pixel_buffer, colour_buffer);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if (cmd == null) throw new System.ArgumentNullException("cmd");
        cmd.ReleaseTemporaryRT(pb_ID);
        //cmd.ReleaseTemporaryRT(pointBufferID);
    }

}
