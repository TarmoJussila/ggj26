using System.Collections.Generic;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

namespace Rendering
{
    public class DepthNormalsFeature : ScriptableRendererFeature
    {
        class DepthNormalsPass : ScriptableRenderPass
        {
            private Material _material;
            private List<ShaderTagId> _shaderTags = new();
            private FilteringSettings _filteringSettings;
            private TextureHandle _destHandle = TextureHandle.nullHandle;
            private string _passName = "DepthNormalsPass";

            private string _textureName = "_DepthTexture";

            private int globalTextureID;

            private RenderTexture _externalTexture;
            private RTHandle _externalTextureHandle;
            
            private class PassData
            {
                // Create a field to store the list of objects to draw
                public RendererListHandle rendererListHandle;
            }

            public DepthNormalsPass(Material mat, RenderTexture rt)
            {
                _material = mat;
                globalTextureID = Shader.PropertyToID(_textureName);
                _shaderTags = new List<ShaderTagId>()
                {
                    new ShaderTagId("DepthOnly")
                };
                this._filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
                //_destHandle =  RTHandles.Alloc("_DepthNormalsTexture", name: "_DepthNormalsTexture");
                _externalTexture = rt;
            }

            // RecordRenderGraph is where the RenderGraph handle can be accessed, through which render passes can be added to the graph.
            // FrameData is a context container through which URP resources can be accessed and managed.
            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameContext)
            {
                using (var builder = renderGraph.AddRasterRenderPass<PassData>(_passName, out var passData))
                {
                    // Get the data needed to create the list of objects to draw
                    UniversalRenderingData renderingData = frameContext.Get<UniversalRenderingData>();
                    UniversalCameraData cameraData = frameContext.Get<UniversalCameraData>();
                    UniversalLightData lightData = frameContext.Get<UniversalLightData>();
                    SortingCriteria sortFlags = cameraData.defaultOpaqueSortFlags;
                    FilteringSettings filterSettings = _filteringSettings;
                    UniversalResourceData resourceData = frameContext.Get<UniversalResourceData>();

                    if (resourceData.isActiveTargetBackBuffer)
                    {
                        return;
                    }

                    RenderTextureDescriptor textureProperties =
                        new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.Default, 0);
                    _destHandle =
                        UniversalRenderer.CreateRenderGraphTexture(renderGraph, textureProperties, _textureName, false);

                    // Create drawing settings
                    DrawingSettings drawSettings = RenderingUtils.CreateDrawingSettings(_shaderTags,
                        renderingData, cameraData, lightData, sortFlags);
                
                    _externalTextureHandle = RTHandles.Alloc(_externalTexture);

                    TextureHandle rtHandle = renderGraph.ImportTexture(_externalTextureHandle);

                    // Add the override material to the drawing settings
                    drawSettings.overrideMaterial = _material;

                    // Create the list of objects to draw
                    var rendererListParameters =
                        new RendererListParams(renderingData.cullResults, drawSettings, filterSettings);

                    // Convert the list to a list handle that the render graph system can use
                    passData.rendererListHandle = renderGraph.CreateRendererList(rendererListParameters);

                    //builder.UseTexture(_destHandle, AccessFlags.ReadWrite);
                    builder.AllowGlobalStateModification(true);
                    builder.UseRendererList(passData.rendererListHandle);
                    builder.SetRenderAttachment(rtHandle, 0, AccessFlags.ReadWrite);
                    
                    builder.SetRenderAttachmentDepth(resourceData.activeDepthTexture, AccessFlags.Write);
                    builder.AllowPassCulling(false);

                    builder.SetRenderFunc((PassData data, RasterGraphContext context) => ExecutePass(data, context));
                    builder.Dispose();
                    
                }
            }

            static void ExecutePass(PassData data, RasterGraphContext context)
            {
                // Clear the render target to black
                context.cmd.ClearRenderTarget(true, true, Color.white);

                // Draw the objects in the list
                context.cmd.DrawRendererList(data.rendererListHandle);
            }

            // public override void OnCameraCleanup(CommandBuffer cmd)
            // {
            //     RTHandles.Release(_destHandle);
            //
            //     base.OnCameraCleanup(cmd);
            // }
        }

        DepthNormalsPass m_ScriptablePass;

        [SerializeField] private RenderTexture _renderTexture;
        [SerializeField] private RenderPassEvent _injectionPoint = RenderPassEvent.AfterRenderingPrePasses;

        /// <inheritdoc/>
        public override void Create()
        {
            //Material mat = new Material(Shader.Find("Hidden/ViewSpaceNormalsShader"));
            Material mat = new Material(Shader.Find("Hidden/Internal-DepthNormalsTexture"));
            m_ScriptablePass = new DepthNormalsPass(mat, _renderTexture);
            
            // Configures where the render pass should be injected.
            m_ScriptablePass.renderPassEvent = _injectionPoint;
        }

        // Here you can inject one or multiple render passes in the renderer.
        // This method is called when setting up the renderer once per-camera.
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(m_ScriptablePass);
        }
    }
}