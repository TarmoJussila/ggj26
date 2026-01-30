using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

namespace Rendering
{
    public class SceneNormalsFeature : ScriptableRendererFeature
    {
        public class MyCustomData : ContextItem
        {
            public TextureHandle textureToTransfer;
            
            public override void Reset()
            {
                textureToTransfer = TextureHandle.nullHandle;
            }
        }
        
        class PassData
        {
            internal TextureHandle copySourceTexture;
        }

        class SceneNormalsPass : ScriptableRenderPass
        {
            private Material _material;
            private string _passName = "DepthNormalsPass";

            public RenderTexture RendTexture;
            public RTHandle DstHandle;

            public void Setup()
            {
                _material = new Material(Shader.Find("Hidden/Internal-DepthNormalsTexture"));
                //ConfigureInput(ScriptableRenderPassInput.Normal); // all of this to just call this one line

                DstHandle = RTHandles.Alloc(RendTexture);
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

                if (resourceData.isActiveTargetBackBuffer)
                {
                    return;
                }
                
                using (var builder = renderGraph.AddRasterRenderPass<PassData>("SceneNormalsFeature", out var passData))
                {
                    RenderTextureDescriptor textureProperties = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.Default, 0);
                    TextureHandle texture = UniversalRenderer.CreateRenderGraphTexture(renderGraph, textureProperties, "NormalFeatureTexture", false);
                    MyCustomData customData = frameData.Create<MyCustomData>();
                    customData.textureToTransfer = texture;
                    //builder.SetRenderAttachmentDepth(frameData);
                    builder.SetRenderAttachment(texture, 0, AccessFlags.Write);
    
                    builder.AllowPassCulling(false);

                  //  builder.SetRenderFunc((PassData data, RasterGraphContext context) => ExecutePass(data, context));
                }

                var src = resourceData.activeColorTexture;

                if (!src.IsValid())
                {
                    return;
                }

               // RenderGraphUtils.BlitMaterialParameters p = new(src, texture, _material, 0);
              //  renderGraph.AddBlitPass(p, passName);
            }

            public override void OnCameraCleanup(CommandBuffer cmd)
            {
                RTHandles.Release(DstHandle);
                base.OnCameraCleanup(cmd);
            }
        }

        [SerializeField] private RenderTexture _renderTexture;
        [SerializeField] private RenderPassEvent _injectionPoint = RenderPassEvent.AfterRenderingPrePasses;

        SceneNormalsPass m_ScriptablePass;

        /// <inheritdoc/>
        public override void Create()
        {
            if (m_ScriptablePass == null)
            {
                m_ScriptablePass = new SceneNormalsPass();
                m_ScriptablePass.renderPassEvent = _injectionPoint;
                m_ScriptablePass.RendTexture = _renderTexture;
            }
        }

        // Here you can inject one or multiple render passes in the renderer.
        // This method is called when setting up the renderer once per-camera.
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            m_ScriptablePass.Setup();
            renderer.EnqueuePass(m_ScriptablePass);
        }
    }
}