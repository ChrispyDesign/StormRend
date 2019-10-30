// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Test/Grass"
{
	Properties
	{
		_Intesity("Intesity", Float) = 0
		_Speed("Speed", Float) = 1
		_SnowOpacity("Snow Opacity", Range( 0 , 1)) = 0
		_Direction("Direction", Range( -1 , 1)) = 0
		_Vector0("Vector 0", Vector) = (0,1,0,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float _Intesity;
		uniform float _Speed;
		uniform float _Direction;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float3 _Vector0;
		uniform float _SnowOpacity;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime8 = _Time.y * _Speed;
			float temp_output_17_0 = ( _Intesity * ( v.texcoord.xy.y * ( cos( mulTime8 ) * _Direction ) ) );
			float4 appendResult19 = (float4(temp_output_17_0 , 0.0 , temp_output_17_0 , 0.0));
			float4 Movement49 = appendResult19;
			v.vertex.xyz += ( v.color * Movement49 ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float temp_output_63_0 = ( ase_worldPos.x * ase_worldPos.z );
			float2 temp_cast_0 = (temp_output_63_0).xx;
			float simplePerlin2D69 = snoise( temp_cast_0*0.01 );
			simplePerlin2D69 = simplePerlin2D69*0.5 + 0.5;
			float2 temp_cast_1 = (temp_output_63_0).xx;
			float simplePerlin2D66 = snoise( temp_cast_1*100.0 );
			simplePerlin2D66 = simplePerlin2D66*0.5 + 0.5;
			float NoiseGenerator74 = ( (0.5 + (simplePerlin2D69 - 0.0) * (1.0 - 0.5) / (1.0 - 0.0)) * (0.7 + (simplePerlin2D66 - 0.0) * (1.0 - 0.7) / (1.0 - 0.0)) );
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float dotResult67 = dot( ase_vertexNormal , _Vector0 );
			float clampResult71 = clamp( dotResult67 , 0.0 , 1.0 );
			float DotMatrix75 = ( clampResult71 * _SnowOpacity );
			float4 myVarName52 = ( tex2D( _TextureSample0, uv_TextureSample0 ) + ( NoiseGenerator74 * DotMatrix75 ) );
			o.Albedo = myVarName52.rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
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
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17101
2086;122;1906;1050;2175.572;1642.089;1.229219;True;False
Node;AmplifyShaderEditor.CommentaryNode;47;-2507.911,62.00231;Inherit;False;1298.586;797.718;Moves The Grass;17;27;8;9;40;10;39;12;7;17;20;19;36;37;58;59;60;61;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-2457.911,607.9183;Float;False;Property;_Speed;Speed;1;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;8;-2304.516,613.2209;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;62;-2784.91,-1033.801;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;40;-2155.714,744.7203;Float;False;Property;_Direction;Direction;3;0;Create;True;0;0;False;0;0;-1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;9;-2133.199,611.5052;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;65;-3044.832,-244.2951;Inherit;False;Property;_Vector0;Vector 0;4;0;Create;True;0;0;False;0;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalVertexDataNode;64;-3057.19,-414.312;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-2379.276,-965.9197;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-2200.714,381.3971;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-1973.245,612.3651;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;67;-2733.987,-262.454;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;66;-2269.388,-751.9913;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;69;-2230.622,-1283.995;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1866.243,495.6681;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1699.351,243.5336;Float;False;Property;_Intesity;Intesity;0;0;Create;True;0;0;False;0;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;83;-1919.899,-1283.157;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;71;-2521.532,-262.9955;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-2323.12,-144.9136;Inherit;False;Property;_SnowOpacity;Snow Opacity;2;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;68;-2022.598,-751.5347;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.7;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1707.079,396.8774;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-1820.455,-1125.544;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-1988.964,-297.0145;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;59;-1537.527,393.2306;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;74;-1521.4,-1126.769;Inherit;False;NoiseGenerator;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;60;-1538.527,478.2306;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;75;-1701.669,-402.6597;Inherit;False;DotMatrix;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;77;-1123.883,-448.3833;Inherit;False;75;DotMatrix;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;61;-1435.527,486.2306;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;58;-1444.527,390.2306;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1529.599,421.6619;Float;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;76;-1161.297,-553.686;Inherit;False;74;NoiseGenerator;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;46;-1200.42,-1632.853;Inherit;False;567.235;513.8707;Comment;1;41;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;48;-806.2169,-24.55991;Inherit;False;317;275;Comment;1;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-736.1688,-555.7278;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;-1376.325,401.4963;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;41;-1150.42,-1348.982;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;-1136.475,395.0763;Float;False;Movement;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.VertexColorNode;5;-756.2169,25.44003;Inherit;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-379.5986,-589.989;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;57;-293.5273,110.2306;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;52;-208.9177,-562.2951;Float;False;myVarName;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;51;-477.6435,286.7523;Inherit;False;49;Movement;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;-274.4084,-8.190378;Inherit;False;52;myVarName;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;36;-1999.509,112.0023;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-231.8155,262.7539;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1866.716,373.5817;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;79;-2875.349,-706.0109;Float;False;Constant;_Vector1;Vector 1;1;0;Create;True;0;0;False;0;100,100;500,500;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;78;-2653.786,-697.8241;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;1;0,0;Float;False;True;2;ASEMaterialInspector;0;0;StandardSpecular;Test/Grass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.2;0.8113208,0.6738453,0.4247953,0;VertexScale;False;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;27;0
WireConnection;9;0;8;0
WireConnection;63;0;62;1
WireConnection;63;1;62;3
WireConnection;39;0;9;0
WireConnection;39;1;40;0
WireConnection;67;0;64;0
WireConnection;67;1;65;0
WireConnection;66;0;63;0
WireConnection;69;0;63;0
WireConnection;12;0;10;2
WireConnection;12;1;39;0
WireConnection;83;0;69;0
WireConnection;71;0;67;0
WireConnection;68;0;66;0
WireConnection;17;0;7;0
WireConnection;17;1;12;0
WireConnection;72;0;83;0
WireConnection;72;1;68;0
WireConnection;73;0;71;0
WireConnection;73;1;70;0
WireConnection;59;0;17;0
WireConnection;74;0;72;0
WireConnection;60;0;17;0
WireConnection;75;0;73;0
WireConnection;61;0;60;0
WireConnection;58;0;59;0
WireConnection;80;0;76;0
WireConnection;80;1;77;0
WireConnection;19;0;58;0
WireConnection;19;1;20;0
WireConnection;19;2;61;0
WireConnection;49;0;19;0
WireConnection;82;0;41;0
WireConnection;82;1;80;0
WireConnection;57;0;5;0
WireConnection;52;0;82;0
WireConnection;6;0;57;0
WireConnection;6;1;51;0
WireConnection;37;0;36;3
WireConnection;78;0;79;0
WireConnection;1;0;56;0
WireConnection;1;11;6;0
ASEEND*/
//CHKSM=2257CD82D0E796489D5CCC4463B397F4D680AA29