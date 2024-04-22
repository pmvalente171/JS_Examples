Shader "Unlit/TerrainHeightMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        [Header(Voronoi)]
        _CellSize ("Cell Size", Range(0.01, 2)) = 2
        _Offset ("Noise Offset ", Range(0.01, 0.99)) = 0.5
        _NoiseStrength ("Noise Strength ", Range(0.01, 1.5)) = 1
        _MinDistanceToCell ("Min Distance To Cell", Range(0.01, 1)) = 1

        [Header (Smoothstep Voronoi)]
        _Smoothstep0("Smoothstep 0", Range(0, 5)) = 0.01
        _Smoothstep1("Smoothstep 1", Range(0, 5)) = 0.01

        [Header (Noise)]
        _offsetX ("OffsetX",Float) = 0.0
        _offsetY ("OffsetY",Float) = 0.0
        _octaves ("Octaves",Int) = 7
        _lacunarity("Lacunarity", Range( 1.0 , 5.0)) = 2
        _gain("Gain", Range( 0.0 , 1.0)) = 0.5
        _value("Value", Range( -2.0 , 2.0)) = 0.0
        _amplitude("Amplitude", Range( 0.0 , 5.0)) = 1.5
        _frequency("Frequency", Range( 0.0 , 6.0)) = 2.0
        _power("Power", Range( 0.1 , 5.0)) = 1.0
        _scale("Scale", Float) = 1.0

        [Header(Final Blend)]
        _Blend1("First Blend value", Range(0, 1)) = 0.5
        _Blend2("Second Blend value", Range(0, 1)) = 0.5

        [Header(Border)]
        _DarkBorder("Border", Range(0.01, 5)) = 0.01
        _DarkBorderIntensity("Border Intensity", Range(0.01, 5)) = 0.01
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;

            // Voronoi
            float _CellSize;
            float _Offset;
            float _NoiseStrength;
            float _MinDistanceToCell;

            // Smooth step
            float _Smoothstep0;
            float _Smoothstep1;

            // Noise
            float _octaves, _lacunarity, _gain, _value, _amplitude, _frequency, _offsetX, _offsetY, _power, _scale,
                  _range;

            // Blend
            float _Blend1, _Blend2;

            // Border
            float _DarkBorder;
            float _DarkBorderIntensity;


            // uniform declaration
            // xy values are 1 / resolution.xy
            // zw values are resolution.xy
            float4 _MainTex_TexelSize;


            float random(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float2 voronoiNoise(float2 value)
            {
                const float2 baseCell = floor(value);

                float min_dist_to_cell = _MinDistanceToCell;
                float2 closest_cell;
                [unroll]
                for (int x = -1; x < 2; x++)
                {
                    [unroll]
                    for (int y = -1; y < 2; y++)
                    {
                        const float2 cell = baseCell + float2(x, y);
                        const float2 rand = float2(random(cell), random(cell + float2(_Offset, 0))) * _NoiseStrength;
                        const float2 cell_position = cell + rand;

                        const float2 toCell = cell_position - value;

                        const float distToCell = length(toCell);
                        const int t = distToCell <= min_dist_to_cell;

                        min_dist_to_cell = t * distToCell + (1 - t) * min_dist_to_cell;
                        closest_cell = t * cell + (1 - t) * float2(0, 0);
                    }
                }
                float rand_2 = random(closest_cell * _Offset);
                return float2(min_dist_to_cell, rand_2);
            }

            float fbm(float2 p)
            {
                p = p * _scale + float2(_offsetX, _offsetY);
                for (int i = 0; i < _octaves; i++)
                {
                    float2 i = floor(p * _frequency);
                    float2 f = frac(p * _frequency);
                    float2 t = f * f * f * (f * (f * 6.0 - 15.0) + 10.0);
                    float2 a = i + float2(0.0, 0.0);
                    float2 b = i + float2(1.0, 0.0);
                    float2 c = i + float2(0.0, 1.0);
                    float2 d = i + float2(1.0, 1.0);
                    a = -1.0 + 2.0 * frac(
                        sin(float2(dot(a, float2(127.1, 311.7)), dot(a, float2(269.5, 183.3)))) * 43758.5453123);
                    b = -1.0 + 2.0 * frac(
                        sin(float2(dot(b, float2(127.1, 311.7)), dot(b, float2(269.5, 183.3)))) * 43758.5453123);
                    c = -1.0 + 2.0 * frac(
                        sin(float2(dot(c, float2(127.1, 311.7)), dot(c, float2(269.5, 183.3)))) * 43758.5453123);
                    d = -1.0 + 2.0 * frac(
                        sin(float2(dot(d, float2(127.1, 311.7)), dot(d, float2(269.5, 183.3)))) * 43758.5453123);
                    float A = dot(a, f - float2(0.0, 0.0));
                    float B = dot(b, f - float2(1.0, 0.0));
                    float C = dot(c, f - float2(0.0, 1.0));
                    float D = dot(d, f - float2(1.0, 1.0));
                    float noise = (lerp(lerp(A, B, t.x), lerp(C, D, t.x), t.y));
                    _value += _amplitude * noise;
                    _frequency *= _lacunarity;
                    _amplitude *= _gain;
                }
                _value = clamp(_value, -1.0, 1.0);
                return pow(_value * 0.5 + 0.5, _power);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                const float2 uv_coord = i.uv;
                const float2 value = uv_coord / _CellSize;

                float noise = voronoiNoise(value).x;
                float4 voronoi_col = float4(noise, noise, noise, 1.0f);
                voronoi_col = smoothstep(_Smoothstep0, _Smoothstep1, voronoi_col);

                // Noise 
                float perlin_val = fbm(uv_coord);
                float4 perlin_col = float4(perlin_val, perlin_val, perlin_val, 1.0f);
                float4 final_col = lerp(lerp(perlin_col, voronoi_col, _Blend1), perlin_col * voronoi_col, _Blend2);

                // Calculate the border values
                float val1 = -4 * _DarkBorderIntensity * ((uv_coord.x - 0.5) * (uv_coord.x - 0.5)) + 1.0;
                float val2 = -4 * _DarkBorderIntensity * ((uv_coord.y - 0.5) * (uv_coord.y - 0.5)) + 1.0;
                float val = (val1 + val2) * _DarkBorder;

                return 1 - final_col * val;
            }
            ENDCG
        }
    }
}