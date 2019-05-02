Shader "Hidden/OrderedDither"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Size ("Size", float) = 1
		_ColorSteps("Color Steps", float ) = 1
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.0
			#pragma shader_feature USEPALETTE
			#pragma shader_feature DITHER2X2 DITHER3X3 DITHER4X4 DITHER8X8
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _LUT;
			half _Size;
			int _ColorSteps;
			float _CenterShift;
			float2 _Resolution;

			float Dither(int x, int y, float f){
				float l = 0.0;

				#if DITHER2X2
					int sum = 4;
					int ditherMatrix[2][2] = {
						{0,2},
						{3,1}
					};
				#endif
			
				#if DITHER3X3
					int sum = 9;
					int ditherMatrix[3][3] = {
						{7,2,6},
						{4,0,1},
						{3,8,5}
					};
				#endif

				#if DITHER4X4
					int sum = 16;
					int ditherMatrix[4][4] = {
						{ 0, 8, 2,10},
						{12, 4,14, 6},
						{ 3,11, 1, 9},
						{15, 7,13, 5}
					};
				#endif

				#if DITHER8X8
					int sum = 64;
					int ditherMatrix[8][8] = {
						{ 0, 32, 8, 40, 2, 34, 10, 42},
						{48, 16, 56, 24, 50, 18, 58, 26},
						{12, 44, 4, 36, 14, 46, 6, 38},
						{60, 28, 52, 20, 62, 30, 54, 22},
						{ 3, 35, 11, 43, 1, 33, 9, 41},
						{51, 19, 59, 27, 49, 17, 57, 25},
						{15, 47, 7, 39, 13, 45, 5, 37},
						{63, 31, 55, 23, 61, 29, 53, 21} 
					};
				#endif

				float f0 = max( 0.0,floor(f*_ColorSteps)/_ColorSteps );
				float f1 = min( f0 + (float)1.0/_ColorSteps, 1.0 );
				float d = smoothstep( f0,f1, f );

				if(f >= 1.0){
					l = f1;
				}else if( f == 0){
					l = f0;
				}else{
					if( d < (float)ditherMatrix[x][y] / sum ){
						l = f0;
					}else{ 
						l = f1; 
					}
				}
				return l;
			}

			float ApplyShift( float f ){
				float r = f;
				float d = 1.0 - abs(0.5 - f)*2.0;
				r += d*_CenterShift;
				return r;
			}

			fixed4 frag (v2f i) : SV_Target
			{

				float2 pixelXY = floor(_Resolution * i.uv);
				
				#ifdef DITHER2X2
					int px = int( pixelXY.x %  2);
					int py = int( pixelXY.y %  2);
				#endif

				#ifdef DITHER3X3
					int px = int( pixelXY.x %  3);
					int py = int( pixelXY.y %  3);
				#endif

				#ifdef DITHER4X4
					int px = int( pixelXY.x %  4);
					int py = int( pixelXY.y %  4);
				#endif

				#ifdef DITHER8X8
					int px = int( pixelXY.x %  8);
					int py = int( pixelXY.y %  8);
				#endif

				float3 lum = float3(0.299, 0.587, 0.144);

				fixed4 inputTex = tex2D(_MainTex, i.uv);
				float grayscale = dot(inputTex.rgb, lum);
				
				return fixed4( 
					Dither( px, py, ApplyShift( inputTex.r )),
					Dither( px, py, ApplyShift( inputTex.g )),
					Dither( px, py, ApplyShift( inputTex.b )),
					inputTex.a
				);
			}
			ENDCG
		}
	}
}
