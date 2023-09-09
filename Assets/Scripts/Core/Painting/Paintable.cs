using UnityEngine;

namespace Core.Painting
{
	public class Paintable : MonoBehaviour
	{
		private const int TEXTURE_SIZE = 1024;

		public float extendsIslandOffset = 1;

		private readonly int maskTextureID = Shader.PropertyToID("_MaskTexture");

		private RenderTexture extendIslandsRenderTexture;
		private RenderTexture maskRenderTexture;

		private Renderer rend;
		private RenderTexture supportTexture;
		private RenderTexture uvIslandsRenderTexture;

		private void Start()
		{
			maskRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
			maskRenderTexture.filterMode = FilterMode.Bilinear;

			extendIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
			extendIslandsRenderTexture.filterMode = FilterMode.Bilinear;

			uvIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
			uvIslandsRenderTexture.filterMode = FilterMode.Bilinear;

			supportTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
			supportTexture.filterMode = FilterMode.Bilinear;

			rend = GetComponent<Renderer>();
			rend.material.SetTexture(maskTextureID, extendIslandsRenderTexture);

			PaintManager.Instance.initTextures(this);
		}

		private void OnDisable()
		{
			maskRenderTexture.Release();
			uvIslandsRenderTexture.Release();
			extendIslandsRenderTexture.Release();
			supportTexture.Release();
		}

		public Vector3 GetClosestPointOnBounds(Vector3 toPoint) => rend.bounds.ClosestPoint(toPoint);

		public RenderTexture getMask() => maskRenderTexture;
		public RenderTexture getUVIslands() => uvIslandsRenderTexture;
		public RenderTexture getExtend() => extendIslandsRenderTexture;
		public RenderTexture getSupport() => supportTexture;
		public Renderer getRenderer() => rend;
	}
}