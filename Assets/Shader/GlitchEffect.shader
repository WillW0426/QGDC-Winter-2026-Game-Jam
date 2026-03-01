Shader "Custom/GlitchEffect"
{
    Properties
    {
        // Exposed in the Material inspector for tuning
        _GlitchIntensity   ("Glitch Intensity",   Range(0, 1))   = 0.3
        _GlitchSpeed       ("Glitch Speed",        Range(0, 20))  = 5.0
        _BlockSize         ("Block Size",          Range(0, 0.5)) = 0.05
        _TearStrength      ("Tear Strength",       Range(0, 0.1)) = 0.02
        _ChromaOffset      ("Chroma Offset",       Range(0, 0.05))= 0.008
        _DigitalNoise      ("Digital Noise",       Range(0, 1))   = 0.05
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        ZWrite Off
        Cull Off

        Pass
        {
            Name "GlitchPass"

            HLSLPROGRAM
            #pragma vertex   Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            // ---- Uniforms -------------------------------------------------------
            float _GlitchIntensity;
            float _GlitchSpeed;
            float _BlockSize;
            float _TearStrength;
            float _ChromaOffset;
            float _DigitalNoise;

            // ---- Hash / noise helpers -------------------------------------------
            float hash(float n) { return frac(sin(n) * 43758.5453123); }

            float hash2(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            // ---- Main fragment --------------------------------------------------
            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv   = input.texcoord;
                float  time = _Time.y * _GlitchSpeed;

                // --- 1. Block-based horizontal tearing ---
                // Snap UV.y to discrete rows of height _BlockSize
                float rowId   = floor(uv.y / _BlockSize);
                float rowTime = floor(time * 2.0); // changes twice per second

                // Only tear some rows, driven by noise
                float tearChance = hash(rowId + rowTime * 0.37);
                float tearActive = step(1.0 - _GlitchIntensity, tearChance);

                float tearOffset = (hash(rowId + rowTime) * 2.0 - 1.0)
                                   * _TearStrength * tearActive;

                // --- 2. Large scan-line jump (less frequent) ---
                float bigJump = 0.0;
                float jumpChance = step(0.97, hash(floor(time * 0.5)));
                if (jumpChance > 0.5)
                {
                    float jumpRow = hash(floor(time * 0.5) + 0.1);
                    bigJump = step(jumpRow - 0.05, uv.y) *
                              step(uv.y, jumpRow + 0.05) *
                              (hash(floor(time * 0.5) + 0.2) * 2.0 - 1.0) * 0.08;
                }

                float totalOffset = tearOffset + bigJump;
                float2 distortedUV = uv + float2(totalOffset, 0.0);

                // --- 3. RGB chromatic aberration (channel split) ---
                float chromaR = _ChromaOffset * (1.0 + _GlitchIntensity);
                float chromaB = _ChromaOffset * (1.0 + _GlitchIntensity) * 0.7;

                float r = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp,
                              distortedUV + float2( chromaR, 0.0)).r;
                float g = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp,
                              distortedUV).g;
                float b = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp,
                              distortedUV - float2( chromaB, 0.0)).b;
                float a = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp,
                              distortedUV).a;

                half4 col = half4(r, g, b, a);

                // --- 4. Digital pixel noise (random black/white flicker) ---
                float noiseVal = hash2(uv * float2(1920, 1080) + time);
                float noiseMask = step(1.0 - _DigitalNoise * _GlitchIntensity * 5.0,
                                       noiseVal);
                col.rgb = lerp(col.rgb, half3(noiseVal, noiseVal, noiseVal), noiseMask);

                // --- 5. Scanline darkening (subtle CRT feel) ---
                float scanline = sin(uv.y * 800.0) * 0.03;
                col.rgb -= scanline * _GlitchIntensity;

                return col;
            }
            ENDHLSL
        }
    }
}