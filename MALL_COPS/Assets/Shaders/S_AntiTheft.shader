// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_AntiTheft"
{
	Properties
	{
		[Toggle(_ANTITHEFT_ON)] _AntiTheft("AntiTheft", Float) = 0
		_AntiTheftColor1("AntiTheftColor 1", Color) = (1,0,0.07238674,0)
		_multAntiTheftColor("multAntiTheftColor", Float) = 1
		_Color0("Color 0", Color) = (0,1,0.5261211,0)
		_BlinkSpeed("BlinkSpeed", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _ANTITHEFT_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			half filler;
		};

		uniform float4 _Color0;
		uniform float _BlinkSpeed;
		uniform float _multAntiTheftColor;
		uniform float4 _AntiTheftColor1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime14 = _Time.y * _BlinkSpeed;
			#ifdef _ANTITHEFT_ON
				float4 staticSwitch1 = ( sin( mulTime14 ) * ( _multAntiTheftColor * _AntiTheftColor1 ) );
			#else
				float4 staticSwitch1 = _Color0;
			#endif
			o.Emission = staticSwitch1.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
48;77;1862;988;1961.16;726.7139;1.534366;True;False
Node;AmplifyShaderEditor.RangedFloatNode;10;-1270.747,-50.16119;Float;False;Property;_BlinkSpeed;BlinkSpeed;4;0;Create;True;0;0;False;0;0;30;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-1105,130.5;Float;False;Property;_AntiTheftColor1;AntiTheftColor 1;1;0;Create;True;0;0;False;0;1,0,0.07238674,0;1,0,0.07238661,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;14;-1072.747,-47.16119;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1113,39.5;Float;False;Property;_multAntiTheftColor;multAntiTheftColor;2;0;Create;True;0;0;False;0;1;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-799,115.5;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;11;-871.7469,-45.16119;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-510,312.5;Float;False;Property;_Color0;Color 0;3;0;Create;True;0;0;False;0;0,1,0.5261211,0;0,1,0.526121,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-546.7469,67.83881;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;1;-265,82.5;Float;False;Property;_AntiTheft;AntiTheft;0;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;36,41;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;S_AntiTheft;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;14;0;10;0
WireConnection;5;0;6;0
WireConnection;5;1;4;0
WireConnection;11;0;14;0
WireConnection;7;0;11;0
WireConnection;7;1;5;0
WireConnection;1;1;3;0
WireConnection;1;0;7;0
WireConnection;0;2;1;0
ASEEND*/
//CHKSM=1BAF924F27D9FB2C08C9C98FD2BB5F241A857082