using Patterns.Singelton;
using UnityEngine;
using UnityEngine.Rendering;

namespace Core.Painting
{
	public class PaintManager : Singleton<PaintManager>
	{
		public Shader texturePaint;
		public Shader extendIslands;
		private readonly int colorID = Shader.PropertyToID("_PainterColor");
		private readonly int hardnessID = Shader.PropertyToID("_Hardness");
		private readonly int positionID = Shader.PropertyToID("_PainterPosition");

		private readonly int prepareUVID = Shader.PropertyToID("_PrepareUV");
		private readonly int radiusID = Shader.PropertyToID("_Radius");
		private readonly int strengthID = Shader.PropertyToID("_Strength");
		private readonly int textureID = Shader.PropertyToID("_MainTex");
		private readonly int uvIslandsID = Shader.PropertyToID("_UVIslands");
		private readonly int uvOffsetID = Shader.PropertyToID("_OffsetUV");
		private int blendOpID = Shader.PropertyToID("_BlendOp");

		private CommandBuffer command;
		private Material extendMaterial;

		private Material paintMaterial;

		public void Awake()
		{
			paintMaterial = new Material(texturePaint);
			extendMaterial = new Material(extendIslands);
			command = new CommandBuffer();
			command.name = "CommmandBuffer - " + gameObject.name;
		}

		public void initTextures(Paintable paintable)
		{
			RenderTexture mask = paintable.getMask();
			RenderTexture uvIslands = paintable.getUVIslands();
			RenderTexture extend = paintable.getExtend();
			RenderTexture support = paintable.getSupport();
			Renderer rend = paintable.getRenderer();

			command.SetRenderTarget(mask);
			command.SetRenderTarget(extend);
			command.SetRenderTarget(support);

			paintMaterial.SetFloat(prepareUVID, 1);
			command.SetRenderTarget(uvIslands);
			command.DrawRenderer(rend, paintMaterial, 0);

			Graphics.ExecuteCommandBuffer(command);
			command.Clear();
		}


		public void Paint(Paintable paintable, Vector3 pos, float radius = 1f, float hardness = .5f, float strength = .5f,
			Color? color = null)
		{
			RenderTexture mask = paintable.getMask();
			RenderTexture uvIslands = paintable.getUVIslands();
			RenderTexture extend = paintable.getExtend();
			RenderTexture support = paintable.getSupport();
			Renderer rend = paintable.getRenderer();

			paintMaterial.SetFloat(prepareUVID, 0);
			paintMaterial.SetVector(positionID, pos);
			paintMaterial.SetFloat(hardnessID, hardness);
			paintMaterial.SetFloat(strengthID, strength);
			paintMaterial.SetFloat(radiusID, radius);
			paintMaterial.SetTexture(textureID, support);
			paintMaterial.SetColor(colorID, color ?? Color.red);
			extendMaterial.SetFloat(uvOffsetID, paintable.extendsIslandOffset);
			extendMaterial.SetTexture(uvIslandsID, uvIslands);

			command.SetRenderTarget(mask);
			command.DrawRenderer(rend, paintMaterial, 0);

			command.SetRenderTarget(support);
			command.Blit(mask, support);

			command.SetRenderTarget(extend);
			command.Blit(mask, extend, extendMaterial);

			Graphics.ExecuteCommandBuffer(command);
			command.Clear();
		}
	}
}