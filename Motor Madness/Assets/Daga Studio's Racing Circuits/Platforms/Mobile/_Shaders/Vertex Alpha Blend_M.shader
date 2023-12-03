// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DSRC/Mobile/Vertex Alpha Blend"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_BaseR("Base (R)", 2D) = "white" {}
		_BaseG("Base (G)", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _BaseR;
		uniform float4 _BaseR_ST;
		uniform sampler2D _BaseG;
		uniform float4 _BaseG_ST;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_BaseR = i.uv_texcoord * _BaseR_ST.xy + _BaseR_ST.zw;
			float4 tex2DNode1 = tex2D( _BaseR, uv_BaseR );
			float2 uv_BaseG = i.uv_texcoord * _BaseG_ST.xy + _BaseG_ST.zw;
			float4 tex2DNode2 = tex2D( _BaseG, uv_BaseG );
			float4 lerpResult4 = lerp( tex2DNode1 , tex2DNode2 , i.vertexColor.g);
			o.Albedo = lerpResult4.rgb;
			o.Alpha = 1;
			float lerpResult5 = lerp( tex2DNode1.a , tex2DNode2.a , i.vertexColor.g);
			clip( lerpResult5 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18102
1950;361;1920;647;2112.771;312.5551;1.541445;True;True
Node;AmplifyShaderEditor.VertexColorNode;3;-1161.964,254.8667;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-883.7164,-162.3573;Inherit;True;Property;_BaseR;Base (R);1;0;Create;True;0;0;False;0;False;-1;21222edd9419479459e63a7ea057e18d;21222edd9419479459e63a7ea057e18d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-905.9166,88.34267;Inherit;True;Property;_BaseG;Base (G);2;0;Create;True;0;0;False;0;False;-1;45e5bb31f0a438a4e92b8fa9a3224937;45e5bb31f0a438a4e92b8fa9a3224937;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;4;-414.1339,175.891;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;5;-411.9716,301.2993;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;;0;0;Lambert;DSRC/Mobile/Vertex Alpha Blend;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;1;0
WireConnection;4;1;2;0
WireConnection;4;2;3;2
WireConnection;5;0;1;4
WireConnection;5;1;2;4
WireConnection;5;2;3;2
WireConnection;0;0;4;0
WireConnection;0;10;5;0
ASEEND*/
//CHKSM=E0B915D91290B7E19F7BB8C574BE561326C466F4