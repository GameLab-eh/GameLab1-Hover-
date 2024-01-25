Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (.5,.5,.5,1)
    }

    SubShader
    {
        Tags {"Queue" = "Overlay" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        // Dichiara le proprietà
        struct Input
        {
            float2 uv_MainTex;
        };

        // Dichiarazioni delle texture
        sampler2D _MainTex;

        // Proprietà dichiarate all'inizio del file
        fixed4 _Color;

        // Funzione surface shader
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Calcola il colore emissivo basato sul colore principale del materiale
            fixed4 emission = _Color;

            // Applica il colore principale del materiale
            o.Albedo = _Color.rgb;

            // Applica il colore emissivo
            o.Emission = emission.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
