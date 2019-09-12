
Shader "Unlit/NoiseWater"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_NoiseTex("Noise Map", 2D) = "bump" {}
		_Mitigation("Mitigation", Range(1, 50)) = 20
		_SpeedX("Speed X", Range(0, 5)) = 1
		_SpeedY("Speed Y", Range(0, 5)) = 1

		_Color("Color", Color) = (0,0,0,1)
		_Strength("Strength", Range(0,2)) = 1.0
		_Speed("Speed", Range(-200,200)) = 100
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
 
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
 
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			float _Mitigation;
			float _SpeedX;
			float _SpeedY;

			float4 _Color;
			float _Strength;
			float _Speed;
 
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
 
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
									
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

				float displacement = (cos(worldPos.y)+cos(worldPos.x + (_Speed*_Time)));
				worldPos.y = worldPos.y + (displacement * _Strength);
				o.vertex = mul(UNITY_MATRIX_VP, worldPos);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.uv;
 
				float2 speed = (_SpeedX, _SpeedY); 
 
				fixed noise = tex2D(_NoiseTex, uv).r;
 
				noise = noise / _Mitigation;
				
				uv += noise* sin(_Time.y * speed);
				
				return tex2D(_MainTex, uv);
			}
			ENDCG
		}
	}
}