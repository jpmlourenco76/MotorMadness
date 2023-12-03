// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DSRC/PC/Vertex Blend"
{
	Properties
	{
		_AlbedoR("Albedo (R)", 2D) = "white" {}
		_AlbedoG("Albedo (G)", 2D) = "white" {}
		_AlbedoB("Albedo (B)", 2D) = "white" {}
		_AlbedoA("Albedo (A)", 2D) = "white" {}
		_NormalR("Normal (R)", 2D) = "bump" {}
		_NormalG("Normal (G)", 2D) = "bump" {}
		_NormalB("Normal (B)", 2D) = "bump" {}
		_NormalA("Normal (A)", 2D) = "bump" {}
		_MRAOR("MRAO (R)", 2D) = "white" {}
		_MRAOG("MRAO (G)", 2D) = "white" {}
		_MRAOB("MRAO (B)", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _NormalA;
		uniform float4 _NormalA_ST;
		uniform sampler2D _NormalR;
		uniform float4 _NormalR_ST;
		uniform sampler2D _NormalG;
		uniform float4 _NormalG_ST;
		uniform sampler2D _NormalB;
		uniform float4 _NormalB_ST;
		uniform sampler2D _AlbedoA;
		uniform float4 _AlbedoA_ST;
		uniform sampler2D _AlbedoR;
		uniform float4 _AlbedoR_ST;
		uniform sampler2D _AlbedoG;
		uniform float4 _AlbedoG_ST;
		uniform sampler2D _AlbedoB;
		uniform float4 _AlbedoB_ST;
		uniform sampler2D _MRAOR;
		uniform float4 _MRAOR_ST;
		uniform sampler2D _MRAOG;
		uniform float4 _MRAOG_ST;
		uniform sampler2D _MRAOB;
		uniform float4 _MRAOB_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalA = i.uv_texcoord * _NormalA_ST.xy + _NormalA_ST.zw;
			float2 uv_NormalR = i.uv_texcoord * _NormalR_ST.xy + _NormalR_ST.zw;
			float2 uv_NormalG = i.uv_texcoord * _NormalG_ST.xy + _NormalG_ST.zw;
			float Green20 = i.vertexColor.g;
			float3 lerpResult50 = lerp( UnpackNormal( tex2D( _NormalR, uv_NormalR ) ) , UnpackNormal( tex2D( _NormalG, uv_NormalG ) ) , Green20);
			float2 uv_NormalB = i.uv_texcoord * _NormalB_ST.xy + _NormalB_ST.zw;
			float Blue21 = i.vertexColor.b;
			float3 lerpResult54 = lerp( lerpResult50 , UnpackNormal( tex2D( _NormalB, uv_NormalB ) ) , Blue21);
			float A24 = i.vertexColor.a;
			float3 lerpResult55 = lerp( UnpackNormal( tex2D( _NormalA, uv_NormalA ) ) , lerpResult54 , A24);
			float3 Normal56 = lerpResult55;
			o.Normal = Normal56;
			float2 uv_AlbedoA = i.uv_texcoord * _AlbedoA_ST.xy + _AlbedoA_ST.zw;
			float2 uv_AlbedoR = i.uv_texcoord * _AlbedoR_ST.xy + _AlbedoR_ST.zw;
			float2 uv_AlbedoG = i.uv_texcoord * _AlbedoG_ST.xy + _AlbedoG_ST.zw;
			float4 lerpResult6 = lerp( tex2D( _AlbedoR, uv_AlbedoR ) , tex2D( _AlbedoG, uv_AlbedoG ) , Green20);
			float2 uv_AlbedoB = i.uv_texcoord * _AlbedoB_ST.xy + _AlbedoB_ST.zw;
			float4 lerpResult7 = lerp( lerpResult6 , tex2D( _AlbedoB, uv_AlbedoB ) , Blue21);
			float4 lerpResult8 = lerp( tex2D( _AlbedoA, uv_AlbedoA ) , lerpResult7 , A24);
			float4 Albedo18 = lerpResult8;
			o.Albedo = Albedo18.rgb;
			float2 uv_MRAOR = i.uv_texcoord * _MRAOR_ST.xy + _MRAOR_ST.zw;
			float4 tex2DNode10 = tex2D( _MRAOR, uv_MRAOR );
			float2 uv_MRAOG = i.uv_texcoord * _MRAOG_ST.xy + _MRAOG_ST.zw;
			float4 tex2DNode12 = tex2D( _MRAOG, uv_MRAOG );
			float lerpResult26 = lerp( tex2DNode10.r , tex2DNode12.r , Green20);
			float2 uv_MRAOB = i.uv_texcoord * _MRAOB_ST.xy + _MRAOB_ST.zw;
			float4 tex2DNode14 = tex2D( _MRAOB, uv_MRAOB );
			float lerpResult29 = lerp( lerpResult26 , tex2DNode14.r , Blue21);
			float Metal32 = lerpResult29;
			o.Metallic = Metal32;
			float lerpResult33 = lerp( tex2DNode10.g , tex2DNode12.g , Green20);
			float lerpResult34 = lerp( lerpResult33 , tex2DNode14.g , Blue21);
			float Roughness39 = lerpResult34;
			o.Smoothness = Roughness39;
			float lerpResult44 = lerp( tex2DNode10.b , tex2DNode12.b , Green20);
			float lerpResult45 = lerp( lerpResult44 , tex2DNode14.b , Blue21);
			float AO46 = lerpResult45;
			o.Occlusion = AO46;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18102
1920;187;1734;760;4444.206;1310.487;3.11833;True;True
Node;AmplifyShaderEditor.VertexColorNode;5;-3386.065,-230.8333;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;20;-3111.272,-243.3062;Inherit;False;Green;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1849.189,-1526.459;Inherit;True;Property;_AlbedoR;Albedo (R);0;0;Create;True;0;0;False;0;False;-1;91812541033fb894b879106d331f8ea1;91812541033fb894b879106d331f8ea1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;22;-1550.468,-1200.769;Inherit;False;20;Green;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-1769.031,747.4216;Inherit;True;Property;_NormalR;Normal (R);4;0;Create;True;0;0;False;0;False;-1;4f4b03d3bf52faf49b2a24f5590a7e3d;4f4b03d3bf52faf49b2a24f5590a7e3d;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-1763.126,973.6501;Inherit;True;Property;_NormalG;Normal (G);5;0;Create;True;0;0;False;0;False;-1;222effe73ce01a744af73acffd457a5f;222effe73ce01a744af73acffd457a5f;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-1855.547,-1313.302;Inherit;True;Property;_AlbedoG;Albedo (G);1;0;Create;True;0;0;False;0;False;-1;a9f50243c4a538d4b9723bb58ea0f2b9;a9f50243c4a538d4b9723bb58ea0f2b9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;51;-1395.74,935.8207;Inherit;False;20;Green;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;21;-3108.272,-143.3062;Inherit;False;Blue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;-3108.189,-47.95767;Inherit;False;A;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;50;-1219.74,743.8206;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;12;-2200.644,-79.78989;Inherit;True;Property;_MRAOG;MRAO (G);9;0;Create;True;0;0;False;0;False;-1;6d3ec5fef33720740bd988f4a4164500;6d3ec5fef33720740bd988f4a4164500;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;43;-1656.05,485.465;Inherit;False;20;Green;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-2209.853,-290.6161;Inherit;True;Property;_MRAOR;MRAO (R);8;0;Create;True;0;0;False;0;False;-1;3819f1c2409dd8d489215908792a90ea;3819f1c2409dd8d489215908792a90ea;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;35;-1706.787,76.84251;Inherit;False;20;Green;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;15;-1762.784,1169.456;Inherit;True;Property;_NormalB;Normal (B);6;0;Create;True;0;0;False;0;False;-1;None;ed324f58410937e4eb36f4f282a62580;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;6;-1373.458,-1397.603;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-1868.408,-1095.599;Inherit;True;Property;_AlbedoB;Albedo (B);2;0;Create;True;0;0;False;0;False;-1;533281ccdab7b284b8a6a38bac5c746b;533281ccdab7b284b8a6a38bac5c746b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;27;-1690.416,-234.5367;Inherit;False;20;Green;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;23;-1547.349,-1016.05;Inherit;False;21;Blue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;-1203.74,1031.821;Inherit;False;21;Blue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;26;-1507.965,-410.4014;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;44;-1480.05,293.465;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;36;-1488,208;Inherit;False;21;Blue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;25;-1377.147,-791.2136;Inherit;False;24;A;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;33;-1504,-80;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;53;-1011.741,1047.821;Inherit;False;24;A;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;14;-2190.215,118.6242;Inherit;True;Property;_MRAOB;MRAO (B);10;0;Create;True;0;0;False;0;False;-1;2722dd4207f1f1c4b8f63cd9a86b6eed;2722dd4207f1f1c4b8f63cd9a86b6eed;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;42;-1464.05,581.4648;Inherit;False;21;Blue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-1773.084,1374.546;Inherit;True;Property;_NormalA;Normal (A);7;0;Create;True;0;0;False;0;False;-1;b78df171921143c40b6b40048e343b03;b78df171921143c40b6b40048e343b03;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;28;-1505.034,-230.5996;Inherit;False;21;Blue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-1866.584,-871.5011;Inherit;True;Property;_AlbedoA;Albedo (A);3;0;Create;True;0;0;False;0;False;-1;cfc3f2964fce23e46a845957ea7cd390;cfc3f2964fce23e46a845957ea7cd390;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;54;-1035.743,803.8555;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;7;-1240.958,-1116.636;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;45;-1296.052,353.4998;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;29;-1333.907,-354.5574;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;34;-1320.002,-19.96515;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;8;-1014.939,-886.7408;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;55;-803.7407,919.8207;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;32;-886.8615,-361.8881;Inherit;True;Metal;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;-895.0974,-9.418154;Inherit;True;Roughness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;-769.7294,-882.7529;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;-584.2401,910.1014;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;46;-895.0959,344.3359;Inherit;True;AO;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;49;-237.9464,-128.3224;Inherit;False;46;AO;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-226.6141,-457.1857;Inherit;False;18;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;-236.9464,-288.3224;Inherit;False;32;Metal;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;-253.9464,-208.3224;Inherit;False;39;Roughness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;57;-230.7476,-370.0161;Inherit;False;56;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;16.99684,-444.346;Float;False;True;-1;2;;0;0;Standard;DSRC/PC/Vertex Blend;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;5;2
WireConnection;21;0;5;3
WireConnection;24;0;5;4
WireConnection;50;0;11;0
WireConnection;50;1;13;0
WireConnection;50;2;51;0
WireConnection;6;0;1;0
WireConnection;6;1;2;0
WireConnection;6;2;22;0
WireConnection;26;0;10;1
WireConnection;26;1;12;1
WireConnection;26;2;27;0
WireConnection;44;0;10;3
WireConnection;44;1;12;3
WireConnection;44;2;43;0
WireConnection;33;0;10;2
WireConnection;33;1;12;2
WireConnection;33;2;35;0
WireConnection;54;0;50;0
WireConnection;54;1;15;0
WireConnection;54;2;52;0
WireConnection;7;0;6;0
WireConnection;7;1;3;0
WireConnection;7;2;23;0
WireConnection;45;0;44;0
WireConnection;45;1;14;3
WireConnection;45;2;42;0
WireConnection;29;0;26;0
WireConnection;29;1;14;1
WireConnection;29;2;28;0
WireConnection;34;0;33;0
WireConnection;34;1;14;2
WireConnection;34;2;36;0
WireConnection;8;0;4;0
WireConnection;8;1;7;0
WireConnection;8;2;25;0
WireConnection;55;0;17;0
WireConnection;55;1;54;0
WireConnection;55;2;53;0
WireConnection;32;0;29;0
WireConnection;39;0;34;0
WireConnection;18;0;8;0
WireConnection;56;0;55;0
WireConnection;46;0;45;0
WireConnection;0;0;19;0
WireConnection;0;1;57;0
WireConnection;0;3;47;0
WireConnection;0;4;48;0
WireConnection;0;5;49;0
ASEEND*/
//CHKSM=1F7225EE04613EB5CE758D41A2B8691EFBC32F01