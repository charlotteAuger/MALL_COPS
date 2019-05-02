// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_Gage"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_AngerValue("AngerValue", Range( 0 , 1)) = 0
		_GageColor01("GageColor01", Color) = (1,0.5365101,0.309804,1)
		_GageColor02("GageColor02", Color) = (0.127759,0.9339623,0.4704815,1)
		_T_Angry("T_Angry", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _GageColor01;
		uniform float4 _GageColor02;
		uniform float _AngerValue;
		uniform sampler2D _T_Angry;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 lerpResult24 = lerp( _GageColor01 , _GageColor02 , _AngerValue);
			o.Emission = lerpResult24.rgb;
			o.Alpha = 1;
			float2 temp_output_3_0 = (float2( -1,-1 ) + (i.uv_texcoord - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
			float2 break9 = temp_output_3_0;
			float temp_output_4_0 = length( temp_output_3_0 );
			float mulTime36 = _Time.y * 8.0;
			float temp_output_34_0 = (-0.05 + (sin( mulTime36 ) - -1.0) * (0.05 - -0.05) / (1.0 - -1.0));
			float2 appendResult35 = (float2(temp_output_34_0 , temp_output_34_0));
			clip( ( ( ( 1.0 - ceil( ( _AngerValue - (0.0 + (atan2( break9.y , break9.x ) - -3.5) * (1.0 - 0.0) / (3.5 - -3.5)) ) ) ) * ( floor( ( temp_output_4_0 + 0.3 ) ) * ( 1.0 - floor( temp_output_4_0 ) ) ) ) + tex2D( _T_Angry, ( i.uv_texcoord * ( appendResult35 + float2( 1,1 ) ) ) ).a ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1921;1;1598;837;3732.161;912.567;2.699239;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-2852.708,-7.965979;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;3;-2535.713,-7.965979;Float;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;9;-2282.597,-7.259898;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleTimeNode;36;-2087.643,1118.625;Float;False;1;0;FLOAT;8;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;33;-1905.97,1119.558;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;7;-2001.335,-9.719632;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;22;-1478.606,3.371359;Float;True;5;0;FLOAT;0;False;1;FLOAT;-3.5;False;2;FLOAT;3.5;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1509.352,-124.3879;Float;False;Property;_AngerValue;AngerValue;1;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;34;-1734.233,1119.767;Float;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;-0.05;False;4;FLOAT;0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;4;-1826.121,358.0837;Float;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;19;-1565.036,614.5914;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-1585.171,358.2131;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;35;-1525.664,1109.486;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;17;-1207.904,-59.3102;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;12;-1350.883,358.2132;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;15;-1045.403,-65.81019;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;-1364.502,1107.436;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;30;-1439.754,951.6757;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;20;-1350.536,610.6914;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1090.611,356.6609;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;16;-836.103,-67.11019;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1136.012,931.1375;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;1.1,1.1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-566.7083,231.5135;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;25;-997.7399,-371.5529;Float;False;Property;_GageColor02;GageColor02;3;0;Create;True;0;0;False;0;0.127759,0.9339623,0.4704815,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;-1000.548,-568.243;Float;False;Property;_GageColor01;GageColor01;2;0;Create;True;0;0;False;0;1,0.5365101,0.309804,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;26;-911.2263,923.506;Float;True;Property;_T_Angry;T_Angry;4;0;Create;True;0;0;False;0;6815dd0a89d75524a98a51818d1d84e8;6815dd0a89d75524a98a51818d1d84e8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-299.6195,363.4693;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;24;-707.3189,-344.6893;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;1;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;S_Gage;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;0
WireConnection;9;0;3;0
WireConnection;33;0;36;0
WireConnection;7;0;9;1
WireConnection;7;1;9;0
WireConnection;22;0;7;0
WireConnection;34;0;33;0
WireConnection;4;0;3;0
WireConnection;19;0;4;0
WireConnection;11;0;4;0
WireConnection;35;0;34;0
WireConnection;35;1;34;0
WireConnection;17;0;14;0
WireConnection;17;1;22;0
WireConnection;12;0;11;0
WireConnection;15;0;17;0
WireConnection;37;0;35;0
WireConnection;20;0;19;0
WireConnection;18;0;12;0
WireConnection;18;1;20;0
WireConnection;16;0;15;0
WireConnection;31;0;30;0
WireConnection;31;1;37;0
WireConnection;21;0;16;0
WireConnection;21;1;18;0
WireConnection;26;1;31;0
WireConnection;28;0;21;0
WireConnection;28;1;26;4
WireConnection;24;0;23;0
WireConnection;24;1;25;0
WireConnection;24;2;14;0
WireConnection;1;2;24;0
WireConnection;1;10;28;0
ASEEND*/
//CHKSM=3D2992CB025389394445DF6E73E3ED998CE22CA9