#if OPENGL
#define SV_POSITION POSITION
#define COLOR0 COLOR
#define PIXEL_OUTPUT COLOR0
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define PIXEL_OUTPUT SV_TARGET0
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

texture Texture;
sampler TextureSampler = sampler_state
{
    Texture = <Texture>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

float4x4 MatrixTransform;
float2 ViewportSize;
float2 LightPosition;
float LightRadius;
float LightIntensity;
float3 LightColor;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput SpriteVertexShader(VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Position = mul(input.Position, MatrixTransform);
    output.TexCoord = input.TexCoord;
    return output;
}

float4 CornerLightPixelShader(VertexShaderOutput input) : PIXEL_OUTPUT
{
    // 将纹理坐标还原为屏幕像素坐标，以便光源位置和半径使用像素单位。
    float2 pixelPosition = input.TexCoord * ViewportSize;
    float distanceToLight = length(pixelPosition - LightPosition);

    // 使用平方衰减让光线从屏幕外的光源位置向画面内部柔和扩散。
    float attenuation = saturate(1.0 - distanceToLight / LightRadius);
    float lightAmount = attenuation * attenuation * LightIntensity;
    // 直接读取场景纹理，避免额外的顶点颜色通道改变原画面的颜色。
    float4 sceneColor = tex2D(TextureSampler, input.TexCoord);
    sceneColor.rgb = saturate(sceneColor.rgb * (1.0 + LightColor * lightAmount));
    return sceneColor;
}

technique CornerLight
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL SpriteVertexShader();
        PixelShader = compile PS_SHADERMODEL CornerLightPixelShader();
    }
}
