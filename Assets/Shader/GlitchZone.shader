Shader "Custom/GlitchZone"
{
    Properties
    {
        _GlitchIntensity  ("Glitch Intensity",  Range(0, 1))    = 0.5
        _GlitchSpeed      ("Glitch Speed",       Range(0, 20))   = 5.0
        _BlockSize        ("Block Size",         Range(0, 0.5))  = 0.05
        _TearStrength     ("Tear Strength",      Range(0, 0.1))  = 0.02
        _ChromaOffset     ("Chroma Offset",      Range(0, 0.05)) = 0.008
        _DigitalNoise     ("Digital Noise",      Range(0, 1))    = 0.05
    }

    SubShader
    {
        Tags
        {
            "RenderType"     = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "Queue"          = "Transparent"
        }

        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "GlitchZonePass"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // URP provides this when Opaque Texture is enabled
            TEXTURE2D(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            float4 _CameraOpaqueTexture_TexelSize;

            float _GlitchIntensity;
            float _GlitchSpeed;
            float _BlockSize;
            float _TearStrength;
            float _ChromaOffset;
            float _DigitalNoise;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float4 screenPos   : TEXCOORD1;
            };

            float hash(float n)  { return frac(sin(n) * 43758.5453123); }
            float hash2(float2 p){ return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453); }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv          = IN.uv;
                OUT.screenPos   = ComputeScreenPos(OUT.positionHCS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Convert to screen UV (0-1 across the whole screen)
                float2 screenUV = IN.screenPos.xy / IN.screenPos.w;

                float time = _Time.y * _GlitchSpeed;

                // Block tearing (same logic as fullscreen shader)
                float rowId     = floor(screenUV.y / _BlockSize);
                float rowTime   = floor(time * 2.0);
                float tearChance = hash(rowId + rowTime * 0.37);
                float tearActive = step(1.0 - _GlitchIntensity, tearChance);
                float tearOffset = (hash(rowId + rowTime) * 2.0 - 1.0)
                                   * _TearStrength * tearActive;

                // Big jump
                float bigJump   = 0.0;
                float jumpChance = step(0.97, hash(floor(time * 0.5)));
                if (jumpChance > 0.5)
                {
                    float jumpRow = hash(floor(time * 0.5) + 0.1);
                    bigJump = step(jumpRow - 0.05, screenUV.y) *
                              step(screenUV.y, jumpRow + 0.05) *
                              (hash(floor(time * 0.5) + 0.2) * 2.0 - 1.0) * 0.08;
                }

                float2 distortedUV = screenUV + float2(tearOffset + bigJump, 0.0);

                // Chromatic aberration
                float chromaR = _ChromaOffset * (1.0 + _GlitchIntensity);
                float chromaB = _ChromaOffset * (1.0 + _GlitchIntensity) * 0.7;

                float r = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture,
                              distortedUV + float2( chromaR, 0.0)).r;
                float g = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture,
                              distortedUV).g;
                float b = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture,
                              distortedUV - float2( chromaB, 0.0)).b;

                half4 col = half4(r, g, b, 1.0);

                // Digital noise
                float noiseVal  = hash2(screenUV * float2(1920, 1080) + time);
                float noiseMask = step(1.0 - _DigitalNoise * _GlitchIntensity * 5.0, noiseVal);
                col.rgb = lerp(col.rgb, half3(noiseVal, noiseVal, noiseVal), noiseMask);

                // Scanlines
                float scanline = sin(screenUV.y * 800.0) * 0.03;
                col.rgb -= scanline * _GlitchIntensity;

                return col;
            }
            ENDHLSL
        }
    }
}