sampler s0;
float WhiteIntensity = 0.5f;

float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : COLOR
{
    float4 color = tex2D(s0, texCoord);

	if(color.a > 0.5f)
	{
		color.r = (WhiteIntensity * 1 + color.r);
	
		color.g = (WhiteIntensity * 1 + color.g);

		color.b = (WhiteIntensity * 1 + color.b);
	}
	
	return color;
}

technique Technique1  
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction(); 
	}
}

