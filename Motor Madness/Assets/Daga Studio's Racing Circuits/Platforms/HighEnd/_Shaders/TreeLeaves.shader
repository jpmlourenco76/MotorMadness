// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DSRC/PC/TreeLeaves"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.25
		_BaseLight("Base Light", Color) = (0,0,0,0)
		_DirectLight("Direct Light", Float) = 0
		_Base("Base", 2D) = "white" {}
		_Fade("Fade", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Lambert keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform half _DirectLight;
		uniform half4 _BaseLight;
		uniform sampler2D _Base;
		uniform half4 _Base_ST;
		uniform half _Fade;
		uniform float _Cutoff = 0.25;

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Base = i.uv_texcoord * _Base_ST.xy + _Base_ST.zw;
			half4 tex2DNode16 = tex2D( _Base, uv_Base );
			half4 temp_output_14_0 = ( _BaseLight * tex2DNode16 );
			o.Albedo = ( ( i.vertexColor * _DirectLight ) * temp_output_14_0 ).rgb;
			o.Emission = temp_output_14_0.rgb;
			o.Alpha = 1;
			float3 ase_worldPos = i.worldPos;
			clip( ( tex2DNode16.a * ( 1.0 - saturate( ( _Fade * distance( _WorldSpaceCameraPos , ase_worldPos ) ) ) ) ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18102
1920;187;1734;760;3292.664;1248.364;3.038823;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;29;-1944.753,275.2589;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceCameraPos;34;-1953.287,100.7268;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;31;-1647.406,433.2746;Inherit;False;Property;_Fade;Fade;4;0;Create;True;0;0;False;0;False;1;0.0015;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;30;-1588.515,143.9463;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1277.171,254.8009;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1268.239,-578.1458;Inherit;False;Property;_DirectLight;Direct Light;2;0;Create;True;0;0;False;0;False;0;2.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;12;-1747.82,-519.4708;Inherit;False;Property;_BaseLight;Base Light;1;0;Create;True;0;0;False;0;False;0,0,0,0;0.3953124,0.4599997,0.3485936,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-1806.153,-331.5132;Inherit;True;Property;_Base;Base;3;0;Create;True;0;0;False;0;False;-1;None;503437ceeee13f74cb4c574f8faa0b4b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;33;-1074.105,261.2747;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;17;-1282.683,-752.4572;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;28;-807.7866,285.4464;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-1192.734,-485.7377;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;35;-1235.942,119.8933;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1055.889,-679.572;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-568.3679,208.8931;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-574.4454,-477.9921;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Half;False;True;-1;2;;0;0;Lambert;DSRC/PC/TreeLeaves;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.25;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Spherical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;30;0;34;0
WireConnection;30;1;29;0
WireConnection;32;0;31;0
WireConnection;32;1;30;0
WireConnection;33;0;32;0
WireConnection;28;0;33;0
WireConnection;14;0;12;0
WireConnection;14;1;16;0
WireConnection;35;0;16;4
WireConnection;22;0;17;0
WireConnection;22;1;23;0
WireConnection;27;0;35;0
WireConnection;27;1;28;0
WireConnection;21;0;22;0
WireConnection;21;1;14;0
WireConnection;0;0;21;0
WireConnection;0;2;14;0
WireConnection;0;10;27;0
ASEEND*/
//CHKSM=5BF31260EFC7C369C1F1F14A09AB29ABA77D9D13