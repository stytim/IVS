Shader "Unlit/PointCloudStereo"
{
    Properties
    {
        _PointSize("Point Size", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float _PointSize;

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR; // Add color attribute

                UNITY_VERTEX_INPUT_INSTANCE_ID // Add instance ID support
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR; // Pass color to fragment shader
                float pointSize : PSIZE;    // Point size semantic

                UNITY_VERTEX_OUTPUT_STEREO // Add stereo output support
            };

            v2f vert(appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v); // Initialize instance ID
                UNITY_INITIALIZE_OUTPUT(v2f, o); // Initialize output
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); // Initialize stereo output

                o.vertex = UnityObjectToClipPos(v.vertex); // Transform vertex position to clip space
                o.color = v.color; // Pass vertex color to the fragment shader
                o.pointSize = _PointSize;                   // Set point size from property
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); // Setup stereo eye index

                // Use the point's actual color
                return i.color;
            }
            ENDCG
        }
    }
}
