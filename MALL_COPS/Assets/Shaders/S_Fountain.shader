// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_Fountain"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
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
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
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
		uniform float _Cutoff = 0.5;

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
			o.Alpha = 1;
			clip( tex2DNode11.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1921;1;1598;837;2699.257;169.1455;1.733724;True;False
Node;AmplifyShaderEditor.PannerNode;20;-1955.365,802.2454;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.75;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;13;-1949.8,242.1021;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.25,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;19;-1418.53,240.9137;Float;False;Property;_PanningSpeed;Panning Speed;5;0;Create;True;0;0;False;0;0,-0.75;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-1689.437,763.2615;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;15;-1187.053,82.10247;Float;False;Property;_Tiling;Tiling;4;0;Create;True;0;0;False;0;2,2;2,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;6;-1200.001,205.0167;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.75;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1119.12,502.9061;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-1218.811,984.5327;Float;False;Property;_DisplacementMultiplier;DisplacementMultiplier;6;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;-1400.031,736.7912;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;6c39c8096b4725142b0527c76a6b8255;6c39c8096b4725142b0527c76a6b8255;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-966.7621,120.0162;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-829.7142,476.4358;Float;True;Property;_water_opacity;water_opacity;2;0;Create;True;0;0;False;0;6c39c8096b4725142b0527c76a6b8255;6c39c8096b4725142b0527c76a6b8255;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1000.411,772.6327;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;25;-1135.611,1061.233;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-752.2167,99.0292;Float;True;Property;_water_emissive;water_emissive;1;0;Create;True;0;0;False;0;58887fa3ad71e09499db9d08b6d18532;58887fa3ad71e09499db9d08b6d18532;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;28;-486.0858,344.4527;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-300.2468,116.411;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-685.8102,859.7327;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;S_Fountain;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;21;1;20;0
WireConnection;6;2;19;0
WireConnection;14;1;13;0
WireConnection;22;1;21;0
WireConnection;8;0;15;0
WireConnection;8;1;6;0
WireConnection;11;1;14;0
WireConnection;23;0;22;2
WireConnection;23;1;24;0
WireConnection;5;1;8;0
WireConnection;28;0;11;3
WireConnection;29;0;5;0
WireConnection;29;1;28;0
WireConnection;26;0;23;0
WireConnection;26;1;25;0
WireConnection;0;2;29;0
WireConnection;0;10;11;1
WireConnection;0;11;26;0
ASEEND*/
//CHKSM=06DBFCEBDF3B019E7706C4BAF07EB738A1DBD90A