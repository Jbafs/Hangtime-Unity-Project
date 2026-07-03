using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BoldColorPassFeature : ScriptableRendererFeature
{
	[Serializable]
	public class Settings
	{
		public Material material;
	}

	private class BoldColorPass : ScriptableRenderPass
	{
		private static readonly string Tag = "Bold Color Pass";

		private Material mat;

		private RTHandle tempTexture;

		private RTHandle source;

		public BoldColorPass(Material material)
		{
			mat = material;
			base.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
		}

		public void Setup(RTHandle source)
		{
			this.source = source;
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData data)
		{
			if (!(mat == null))
			{
				CommandBuffer commandBuffer = CommandBufferPool.Get(Tag);
				RenderTextureDescriptor cameraTargetDescriptor = data.cameraData.cameraTargetDescriptor;
				cameraTargetDescriptor.depthBufferBits = 0;
				if (tempTexture == null || !tempTexture.rt.IsCreated())
				{
					tempTexture = RTHandles.Alloc(cameraTargetDescriptor.width, cameraTargetDescriptor.height, 1, (DepthBits)cameraTargetDescriptor.depthBufferBits, cameraTargetDescriptor.graphicsFormat, FilterMode.Bilinear, TextureWrapMode.Clamp, TextureDimension.Tex2D, enableRandomWrite: false, useMipMap: false, autoGenerateMips: true, isShadowMap: false, 1, 0f, MSAASamples.None, bindTextureMS: false, useDynamicScale: false, useDynamicScaleExplicit: false, RenderTextureMemoryless.None, VRTextureUsage.None, "_TempColorTexture");
				}
				Blitter.BlitCameraTexture(commandBuffer, source, tempTexture, mat, 0);
				Blitter.BlitCameraTexture(commandBuffer, tempTexture, source);
				context.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
			}
		}

		public void Cleanup()
		{
			tempTexture?.Release();
		}
	}

	public Settings settings = new Settings();

	private BoldColorPass pass;

	public override void Create()
	{
		if (settings.material != null)
		{
			pass = new BoldColorPass(settings.material);
		}
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData data)
	{
		if (pass != null)
		{
			RTHandle cameraColorTargetHandle = renderer.cameraColorTargetHandle;
			pass.Setup(cameraColorTargetHandle);
			renderer.EnqueuePass(pass);
		}
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		pass?.Cleanup();
	}
}
