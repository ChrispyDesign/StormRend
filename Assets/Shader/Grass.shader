// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Test/Grass"
{
	Properties
	{
		_Intesity("Intesity", Float) = 0
		_Speed("Speed", Float) = 1
		_Direction("Direction", Range( -1 , 1)) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_AlbedoMultiplyer("Albedo Multiplyer", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Intesity;
		uniform float _Speed;
		uniform float _Direction;
		uniform float _AlbedoMultiplyer;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime8 = _Time.y * _Speed;
			float temp_output_17_0 = ( _Intesity * ( v.texcoord.xy.y * ( cos( mulTime8 ) * _Direction ) ) );
			float4 appendResult19 = (float4(temp_output_17_0 , 0.0 , temp_output_17_0 , 0.0));
			float4 Movement49 = appendResult19;
			v.vertex.xyz += ( v.color * Movement49 ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 myVarName52 = ( _AlbedoMultiplyer * tex2D( _TextureSample0, uv_TextureSample0 ) );
			o.Albedo = myVarName52.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16900
1927;7;1906;1044;2105.058;636.0496;2.104185;True;True
Node;AmplifyShaderEditor.CommentaryNode;47;-2507.911,62.00231;Float;False;1298.586;797.718;Moves The Grass;17;27;8;9;40;10;39;12;7;17;35;34;20;33;32;19;36;37;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-2457.911,607.9183;Float;False;Property;_Speed;Speed;1;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;8;-2304.516,613.2209;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-2155.714,744.7203;Float;False;Property;_Direction;Direction;2;0;Create;True;0;0;False;0;0;-1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;9;-2133.199,611.5052;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-2200.714,381.3971;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-1973.245,612.3651;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1699.351,243.5336;Float;False;Property;_Intesity;Intesity;0;0;Create;True;0;0;False;0;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1866.243,495.6681;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1707.079,396.8774;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;35;-1567.286,503.7589;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;34;-1563.439,367.8398;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;33;-1469.834,508.8879;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;32;-1471.116,369.122;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1549.599,423.6619;Float;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;46;-830.9869,-907.5828;Float;False;567.235;513.8707;Comment;3;41;43;42;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-732.1572,-857.5828;Float;False;Property;_AlbedoMultiplyer;Albedo Multiplyer;4;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;41;-780.9869,-623.7122;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;19;-1376.325,401.4963;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;-1136.475,395.0763;Float;False;Movement;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-432.7519,-562.0325;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;48;-806.2169,-24.55991;Float;False;317;275;Comment;1;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.VertexColorNode;5;-756.2169,25.44003;Float;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;52;-150.9177,-549.2951;Float;False;myVarName;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;51;-477.6435,286.7523;Float;False;49;Movement;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;-326.1173,6.518542;Float;False;52;myVarName;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1866.716,373.5817;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-231.8155,262.7539;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;36;-1999.509,112.0023;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;1;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Test/Grass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.2;0.8113208,0.6738453,0.4247953,0;VertexScale;False;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;27;0
WireConnection;9;0;8;0
WireConnection;39;0;9;0
WireConnection;39;1;40;0
WireConnection;12;0;10;2
WireConnection;12;1;39;0
WireConnection;17;0;7;0
WireConnection;17;1;12;0
WireConnection;35;0;17;0
WireConnection;34;0;17;0
WireConnection;33;0;35;0
WireConnection;32;0;34;0
WireConnection;19;0;32;0
WireConnection;19;1;20;0
WireConnection;19;2;33;0
WireConnection;49;0;19;0
WireConnection;42;0;43;0
WireConnection;42;1;41;0
WireConnection;52;0;42;0
WireConnection;37;0;36;3
WireConnection;6;0;5;0
WireConnection;6;1;51;0
WireConnection;1;0;56;0
WireConnection;1;11;6;0
ASEEND*/
//CHKSM=6AC4EB2A7543FEE36E2BF1824451D029F8FE11D6