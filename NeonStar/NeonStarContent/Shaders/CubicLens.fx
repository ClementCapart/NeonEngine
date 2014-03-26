uniform extern texture ScreenTexture;    

float k = -0.15;
float kcube = 0.5;

sampler ScreenS = sampler_state
{
    Texture = <ScreenTexture>;    
};

float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : COLOR
{
    float r2 = (texCoord.x-0.5) * (texCoord.x-0.5) + (texCoord.y-0.5) * (texCoord.y-0.5);
    float f = 0;

    if(kcube == 0.0)
    {
        f = 1 + r2 * k;
    }
    else
    {
        f = 1 + r2 * (k + kcube * sqrt(r2));
    }

    float x = f*(texCoord.x-0.5)+0.5;
    float y = f*(texCoord.y-0.5)+0.5;
    float3 inputDistord = tex2D(ScreenS, float2(x,y));

	return float4(inputDistord.r, inputDistord.g, inputDistord.b,1);
}

technique CubicLens
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}