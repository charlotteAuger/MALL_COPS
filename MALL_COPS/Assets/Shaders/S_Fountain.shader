// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_Fountain"
{
	Properties
	{
		_water_emissive("water_emissive", 2D) = "white" {}
		_water_opacity("water_opacity", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Tiling("Tiling", Vector) = (2,2,0,0)
		_PanningSpeed("Panning Speed", Vector) = (0,-0.75,0,0)
		_DisplacementMultiplier("DisplacementMultiplier", Float) = 0.1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float _DisplacementMultiplier;
		uniform sampler2D _water_emissive;
		uniform float2 _Tiling;
		uniform float2 _PanningSpeed;
		uniform sampler2D _water_opacity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 panner20 = ( 1.0 * _Time.y * float2( 0,-0.75 ) + float2( 0,0 ));
			float2 uv_TexCoord21 = v.texcoord.xy + panner20;
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( tex2Dlod( _TextureSample0, float4( uv_TexCoord21, 0, 0.0) ).g * _DisplacementMultiplier ) * ase_vertexNormal );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner6 = ( 1.0 * _Time.y * _PanningSpeed + float2( 0,0 ));
			float2 uv_TexCoord8 = i.uv_texcoord * _Tiling + panner6;
			float2 panner13 = ( 1.0 * _Time.y * float2( 0.25,0 ) + float2( 0,0 ));
			float2 uv_TexCoord14 = i.uv_texcoord + panner13;
			float4 tex2DNode11 = tex2D( _water_opacity, uv_TexCoord14 );
			o.Emission = ( tex2D( _water_emissive, uv_TexCoord8 ) + ( 1.0 - tex2DNode11.b ) ).rgb;
			o.Alpha = ( tex2DNode11.r * 0.75 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1920;1;1586;831;370.3344;84.38098;1;True;False
Node;AmplifyShaderEditor.PannerNode;13;-734.9138,461.3029;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.25,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;19;-800.7102,136.1178;Float;False;Property;_PanningSpeed;Panning Speed;4;0;Create;True;0;0;False;0;0,-0.75;0,-0.75;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;20;-1130.213,683.4715;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.75;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-501.3001,417.5164;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;15;-569.2332,-3.287267;Float;False;Property;_Tiling;Tiling;3;0;Create;True;0;0;False;0;2,2;3,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;6;-582.1812,119.627;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.75;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-864.2845,644.4876;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-546.0328,838.1637;Float;False;Property;_DisplacementMultiplier;DisplacementMultiplier;5;0;Create;True;0;0;False;0;0.1;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;-574.8789,618.0173;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;6c39c8096b4725142b0527c76a6b8255;6c39c8096b4725142b0527c76a6b8255;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-348.9423,34.62645;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-211.8944,391.0461;Float;True;Property;_water_opacity;water_opacity;1;0;Create;True;0;0;False;0;6c39c8096b4725142b0527c76a6b8255;6c39c8096b4725142b0527c76a6b8255;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-181.8049,669.5689;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;28;119.8039,469.831;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;25;-310.4587,942.4591;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-106.3969,5.639462;Float;True;Property;_water_emissive;water_emissive;0;0;Create;True;0;0;False;0;58887fa3ad71e09499db9d08b6d18532;58887fa3ad71e09499db9d08b6d18532;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;139.342,740.9588;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;365.6656,321.619;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.75;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;317.573,31.02126;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;613.6,-18.2;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;S_Fountain;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;True;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;1;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;14;1;13;0
WireConnection;6;2;19;0
WireConnection;21;1;20;0
WireConnection;22;1;21;0
WireConnection;8;0;15;0
WireConnection;8;1;6;0
WireConnection;11;1;14;0
WireConnection;23;0;22;2
WireConnection;23;1;24;0
WireConnection;28;0;11;3
WireConnection;5;1;8;0
WireConnection;26;0;23;0
WireConnection;26;1;25;0
WireConnection;30;0;11;1
WireConnection;29;0;5;0
WireConnection;29;1;28;0
WireConnection;0;2;29;0
WireConnection;0;9;30;0
WireConnection;0;11;26;0
ASEEND*/
//CHKSM=3AE979B8482F074E2676BD7EDCFFC2213DB7D210