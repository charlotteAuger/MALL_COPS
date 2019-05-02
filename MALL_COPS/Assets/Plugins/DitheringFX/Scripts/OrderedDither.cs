using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[AddComponentMenu("DitherFX")]
public class OrderedDither : MonoBehaviour {

	public enum DitherType { DITHER2X2, DITHER3X3, DITHER4X4, DITHER8X8 }
	public DitherType dithering = DitherType.DITHER2X2;
	[Range(1,16)]
	public int colorSteps = 1;
	public float shiftCenter = 0;
	Shader postFXShader;
	Material shaderMat;

	[Range(0,1)]
	public float resolutionScale = 1f;
	RenderTexture resized;

	int dithersize = 2;

	private void Awake() {
		if (postFXShader == null) { postFXShader = Shader.Find("Hidden/OrderedDither"); }	
		shaderMat = new Material(postFXShader);
	}

	void DisableKeywords() {
		shaderMat.DisableKeyword("DITHER2X2");
		shaderMat.DisableKeyword("DITHER3X3");
		shaderMat.DisableKeyword("DITHER4X4");
		shaderMat.DisableKeyword("DITHER8X8");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination) {
		if (resolutionScale > 0) {
			shaderMat.SetInt("_ColorSteps", colorSteps);
			switch (dithering) {
				case DitherType.DITHER2X2:
					if (!shaderMat.IsKeywordEnabled("DITHER2X2")) {
						DisableKeywords();
						shaderMat.EnableKeyword("DITHER2X2");
					}
					dithersize = 2;
					break;
				case DitherType.DITHER3X3:
					if (!shaderMat.IsKeywordEnabled("DITHER3X3")) {
						DisableKeywords();
						shaderMat.EnableKeyword("DITHER3X3");
					}
					dithersize = 3;
					break;
				case DitherType.DITHER4X4:
					if (!shaderMat.IsKeywordEnabled("DITHER4X4")) {
						DisableKeywords();
						shaderMat.EnableKeyword("DITHER4X4");
					}
					dithersize = 4;
					break;
				case DitherType.DITHER8X8:
					if (!shaderMat.IsKeywordEnabled("DITHER8X8")) {
						DisableKeywords();
						shaderMat.EnableKeyword("DITHER8X8");
					}
					dithersize = 8;
					break;
			}
			shaderMat.SetFloat("_Size", resolutionScale);
			shaderMat.SetFloat("_CenterShift", shiftCenter);
			
			resized = new RenderTexture(
				Mathf.Min(source.width, 
					Mathf.Max(
						2,
						Mathf.FloorToInt(
							(float)source.width * resolutionScale
						)
					)
				),
				Mathf.Min(source.height, 
					Mathf.Max(
						2, 
						Mathf.FloorToInt(
							(float)source.height * resolutionScale
						)
					)
				),
				0
			);
			source.filterMode = FilterMode.Point;
			shaderMat.SetVector("_Resolution", new Vector2( resized.width, resized.height ));
			resized.filterMode = FilterMode.Point;
			Graphics.Blit(source, resized);
			Graphics.Blit(resized, destination, shaderMat);
			resized.Release();
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof( OrderedDither ))]
public class OrderedDitherEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		EditorGUILayout.HelpBox("Resolution Scale does not change the actual rendering resolution! To gain performance, lower your camera resolution globally.", MessageType.Info);
	}
}
#endif
