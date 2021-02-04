Shader "Unlit/PolarCoordinatesGrad"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Data ("Data",vector) = (1,1,1,1)
        _Percent("Percent",float) = 1
        _Color("Color",color) = (1,1,1,1)
        _Color2("Color2",color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent""Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Data;
            float _Percent;
            fixed4 _Color;
            fixed4 _Color2;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {

                float r_inner=0.0; 
                float t_outer=0.5; 

                float2 x = i.uv - float2(0.5,0.5);
                float radius = length(x);
                float angle = atan2(x.y, x.x);

                float2 tc_polar; // the new polar texcoords
                // map radius so that for r=r_inner -> 0 and r=r_outer -> 1
                tc_polar.x = ( radius - r_inner) / (t_outer - r_inner);
                tc_polar.x += _Data.x + _Time.x * _Data.z;
                //tc_polar.x *= _Data.x;
                // map angle from [-PI,PI] to [0,1]
                tc_polar.y = angle * 0.5 / 3.1415 + 0.5;
                //tc_polar.y *= _Repeat.y;
                // texture mapping
                //gl_FragColor = texture2D(tex, tc_polar);
                //fixed4 dis = tex2D(_Distort,TRANSFORM_TEX(i.uv, _Distort));
                // sample the texture
                //fixed4 col = tex2D(_MainTex, dis.xyz*_DistortAmount+tc_polar * float2(-1,1));
                // apply fog

                float dist = distance(i.uv,float2(.5,.5));
                float c = cos((dist+_Data.y)*6.28);
                c*=_Data.z;
                float dist2 = 1-smoothstep(_Data.x,_Data.x+.001,c);
                float percent = smoothstep(_Percent,_Percent+.001,tc_polar.y);
                //UNITY_APPLY_FOG(i.fogCoord, col);
                //float alpha = lerp(col.r,col.b,_Mix);
                //fixed4 col2 = fixed4(_Color.r,_Color.g,_Color.b,alpha*_Color.a*dist);
                return dist2*percent*lerp(_Color,_Color2,pow(dist*2,4));
            }
            ENDCG
        }
    }
}
