using Logbound.PostProcessing;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

namespace Logbound.Rendering
{
    public class BlitMaterialRendererFeature : ScriptableRendererFeature
    {
        [SerializeField] private bool _enableCameraNormals;
        [SerializeField] Material _material;
        [SerializeField] private RenderPassEvent _injectionPoint = RenderPassEvent.AfterRenderingPostProcessing;

        private BlendPass _pass;

        public override void Create()
        {
            _pass = new BlendPass();
            _pass.renderPassEvent = _injectionPoint;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (_material == null)
            {
                Debug.LogWarning("Material is null");
                return;
            }

            _pass.Setup(_material, _enableCameraNormals);

            renderer.EnqueuePass(_pass);
        }

        class BlendPass : ScriptableRenderPass
        {
            public Material EffectMaterial;

            private string _passName = "Blit-";

            public void Setup(Material effectMaterial, bool cameraNormals)
            {
                EffectMaterial = effectMaterial;
                requiresIntermediateTexture = true;
                _passName = "PostProcessing-Blit-" + EffectMaterial.name;

                if (cameraNormals)
                {
                    ConfigureInput(ScriptableRenderPassInput.Normal); // all of this to just call this one line
                }
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                var stack = VolumeManager.instance.stack;
                var customEffect = stack.GetComponent<CustomVolumeComponent>();

                if (!customEffect.Active())
                {
                    return;
                }

                UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

                if (resourceData.isActiveTargetBackBuffer)
                {
                    return;
                }

                var src = resourceData.activeColorTexture;

                var dstDesc = renderGraph.GetTextureDesc(src);
                dstDesc.name = _passName;
                dstDesc.clearBuffer = false;

                TextureHandle dst = renderGraph.CreateTexture(dstDesc);

                if (!src.IsValid() || !dst.IsValid())
                {
                    return;
                }

                RenderGraphUtils.BlitMaterialParameters p = new(src, dst, EffectMaterial, 0);
                renderGraph.AddBlitPass(p, passName);

                resourceData.cameraColor = dst;
            }
        }
    }
}