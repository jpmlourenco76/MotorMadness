// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DSRC/PC/Screens"
{
	Properties
	{
		_Screens01("Screens01", 2D) = "white" {}
		_Screens02("Screens02", 2D) = "white" {}
		_Screens03("Screens03", 2D) = "white" {}
		_FPS("FPS", Float) = 8
		_MaxFrames("MaxFrames", Float) = 144
		_Width("Width", Float) = 9
		_Height("Height", Float) = 16
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Screens01;
		uniform float _Width;
		uniform float _Height;
		uniform float _FPS;
		uniform float _MaxFrames;
		uniform sampler2D _Screens02;
		uniform sampler2D _Screens03;
		uniform float4 _Screens03_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float temp_output_4_0_g1 = _Width;
			float temp_output_5_0_g1 = _Height;
			float2 appendResult7_g1 = (float2(temp_output_4_0_g1 , temp_output_5_0_g1));
			float totalFrames39_g1 = ( temp_output_4_0_g1 * temp_output_5_0_g1 );
			float2 appendResult8_g1 = (float2(totalFrames39_g1 , temp_output_5_0_g1));
			float clampResult42_g1 = clamp( 0.0 , 0.0001 , ( totalFrames39_g1 - 1.0 ) );
			float temp_output_35_0_g1 = frac( ( ( floor( ( ( _FPS * _Time.y ) % _MaxFrames ) ) + clampResult42_g1 ) / totalFrames39_g1 ) );
			float2 appendResult29_g1 = (float2(temp_output_35_0_g1 , ( 1.0 - temp_output_35_0_g1 )));
			float2 temp_output_15_0_g1 = ( ( i.uv_texcoord / appendResult7_g1 ) + ( floor( ( appendResult8_g1 * appendResult29_g1 ) ) / appendResult7_g1 ) );
			float2 temp_output_10_0 = temp_output_15_0_g1;
			float4 lerpResult15 = lerp( tex2D( _Screens01, temp_output_10_0 ) , tex2D( _Screens02, temp_output_10_0 ) , i.vertexColor.g);
			float2 uv_Screens03 = i.uv_texcoord * _Screens03_ST.xy + _Screens03_ST.zw;
			float4 lerpResult18 = lerp( lerpResult15 , tex2D( _Screens03, uv_Screens03 ) , i.vertexColor.b);
			o.Emission = lerpResult18.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18102
1920;187;1734;760;4749.761;1106.528;2.339742;True;True
Node;AmplifyShaderEditor.TimeNode;3;-3522.502,42.22059;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-3370.502,-98.77946;Inherit;False;Property;_FPS;FPS;3;0;Create;True;0;0;False;0;False;8;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-3170.502,-7.779409;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-3156.798,107.0168;Inherit;False;Property;_MaxFrames;MaxFrames;4;0;Create;True;0;0;False;0;False;144;144;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleRemainderNode;6;-2862.502,1.220591;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;9;-2617.502,3.220591;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-2456.502,67.22059;Inherit;False;Property;_Width;Width;5;0;Create;True;0;0;False;0;False;9;9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2451.502,138.2206;Inherit;False;Property;_Height;Height;6;0;Create;True;0;0;False;0;False;16;16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;16;-2072.696,70.10355;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;10;-2180.502,-198.7794;Inherit;False;Flipbook;-1;;1;53c2488c220f6564ca6c90721ee16673;2,71,0,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;3;False;5;FLOAT;3;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.WireNode;21;-1561.924,382.7911;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-1508.755,-55.92254;Inherit;True;Property;_Screens02;Screens02;1;0;Create;True;0;0;False;0;False;-1;None;efc6387874342c44d8eba79b05f73a1e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1524.956,-279.4267;Inherit;True;Property;_Screens01;Screens01;0;0;Create;True;0;0;False;0;False;-1;None;a3238cbb37037954f8f7937d086c515a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-1534.674,204.8394;Inherit;True;Property;_Screens03;Screens03;2;0;Create;True;0;0;False;0;False;-1;None;2822dd41420b564418ec6ad4663fec3e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;17;-1146.167,134.6429;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;19;-939.3776,202.1218;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;20;-1231.06,389.3214;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;15;-1013.715,13.67574;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;18;-742.7082,157.1732;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;;0;0;Unlit;DSRC/PC/Screens;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;5;0
WireConnection;4;1;3;2
WireConnection;6;0;4;0
WireConnection;6;1;8;0
WireConnection;9;0;6;0
WireConnection;10;4;11;0
WireConnection;10;5;12;0
WireConnection;10;2;9;0
WireConnection;21;0;16;3
WireConnection;13;1;10;0
WireConnection;1;1;10;0
WireConnection;17;0;16;2
WireConnection;19;0;14;0
WireConnection;20;0;21;0
WireConnection;15;0;1;0
WireConnection;15;1;13;0
WireConnection;15;2;17;0
WireConnection;18;0;15;0
WireConnection;18;1;19;0
WireConnection;18;2;20;0
WireConnection;0;2;18;0
ASEEND*/
//CHKSM=9FDE055716CC903EE02846A5668D9CA16A27D1AC