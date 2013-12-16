sampler s0;
float WhiteIntensity = 0.5f;

float4 WhiteBlinkFunction(float2 texCoord: TEXCOORD0) : COLOR
{
    float4 color = tex2D(s0, texCoord);

	if(color.a == 1)
	{
		color.r = (WhiteIntensity * 1 + color.r);
	
		color.g = (WhiteIntensity * 1 + color.g);

		color.b = (WhiteIntensity * 1 + color.b);
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

