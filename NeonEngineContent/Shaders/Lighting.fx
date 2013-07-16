uniform extern texture ScreenTexture;    
texture lightMask;

sampler ScreenS = sampler_state { Texture = <ScreenTexture>; };
sampler LightS = sampler_state { Texture = <lightMask>; };

float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : COLOR
{
    float4 color = tex2D(ScreenS, texCoord);
	float4 light = tex2D(LightS, texCoord);
	return color * light;
}

technique Lighting
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}