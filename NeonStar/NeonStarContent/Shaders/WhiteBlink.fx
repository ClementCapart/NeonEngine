sampler s0;

float4 WhiteBlinkFunction(float2 texCoord: TEXCOORD0) : COLOR
{
    float4 color = tex2D(s0, texCoord);

	if(color.a == 1)
	{
		color.r = (0.5 + color.r);
	
		color.g = (0.5 + color.g);

		color.b = (0.5 + color.b);
	}
	
	return color;
}

technique Technique1  
{
	pass PassBlink
	{
		PixelShader = compile ps_2_0 WhiteBlinkFunction(); 
	}
}

