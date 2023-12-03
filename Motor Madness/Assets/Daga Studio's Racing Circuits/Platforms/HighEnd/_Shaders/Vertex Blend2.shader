// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DSRC/PC/Vertex Blend (RG)"
{
	Properties
	{
		_AlbedoR("Albedo (R)", 2D) = "white" {}
		_AlbedoG("Albedo (G)", 2D) = "white" {}
		_NormalR("Normal (R)", 2D) = "bump" {}
		_MRAOR("MRAO (R)", 2D) = "white" {}
		_RoughnessValue("Roughness Value", Range( 0 , 1)) = 0
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

		uniform sampler2D _NormalR;
		uniform float4 _NormalR_ST;
		uniform sampler2D _AlbedoR;
		uniform float4 _AlbedoR_ST;
		uniform sampler2D _AlbedoG;
		uniform float4 _AlbedoG_ST;
		uniform sampler2D _MRAOR;
		uniform float4 _MRAOR_ST;
		uniform float _RoughnessValue;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalR = i.uv_texcoord * _NormalR_ST.xy + _NormalR_ST.zw;
			o.Normal = UnpackNormal( tex2D( _NormalR, uv_NormalR ) );
			float2 uv_AlbedoR = i.uv_texcoord * _AlbedoR_ST.xy + _AlbedoR_ST.zw;
			float2 uv_AlbedoG = i.uv_texcoord * _AlbedoG_ST.xy + _AlbedoG_ST.zw;
			float4 lerpResult6 = lerp( tex2D( _AlbedoR, uv_AlbedoR ) , tex2D( _AlbedoG, uv_AlbedoG ) , i.vertexColor.g);
			o.Albedo = lerpResult6.rgb;
			float2 uv_MRAOR = i.uv_texcoord * _MRAOR_ST.xy + _MRAOR_ST.zw;
			float4 tex2DNode13 = tex2D( _MRAOR, uv_MRAOR );
			o.Metallic = tex2DNode13.r;
			o.Smoothness = ( tex2DNode13.g * _RoughnessValue );
			o.Occlusion = tex2DNode13.b;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18102
1920;286;1734;754;1777.264;226.7541;1;True;True
Node;AmplifyShaderEditor.VertexColorNode;5;-1323.251,317.4828;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-793.5578,473.2135;Inherit;False;Property;_RoughnessValue;Roughness Value;4;0;Create;True;0;0;False;0;False;0;0.253;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-1440.704,-116.9037;Inherit;True;Property;_AlbedoR;Albedo (R);0;0;Create;True;0;0;False;0;False;-1;7f215f780e973e34d996b5f3cd1958bf;7f215f780e973e34d996b5f3cd1958bf;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-1446.269,82.53258;Inherit;True;Property;_AlbedoG;Albedo (G);1;0;Create;True;0;0;False;0;False;-1;3b64c62bb2d0a8b4294e35d9800b88d0;3b64c62bb2d0a8b4294e35d9800b88d0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-818.9238,275.9771;Inherit;True;Property;_MRAOR;MRAO (R);3;0;Create;True;0;0;False;0;False;-1;c7b5eacf4b5e44f41ac578650855b3b5;c7b5eacf4b5e44f41ac578650855b3b5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;6;-1028.815,-8.839561;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-337.2054,337.3545;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-826.254,70.17278;Inherit;True;Property;_NormalR;Normal (R);2;0;Create;True;0;0;False;0;False;-1;5f20d118c78ec984f88b23570699f1de;5f20d118c78ec984f88b23570699f1de;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;;0;0;Standard;DSRC/PC/Vertex Blend (RG);False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;10;0
WireConnection;6;1;11;0
WireConnection;6;2;5;2
WireConnection;15;0;13;2
WireConnection;15;1;14;0
WireConnection;0;0;6;0
WireConnection;0;1;12;0
WireConnection;0;3;13;1
WireConnection;0;4;15;0
WireConnection;0;5;13;3
ASEEND*/
//CHKSM=460033712C8F9F8603636382CD7A63539AFB372B