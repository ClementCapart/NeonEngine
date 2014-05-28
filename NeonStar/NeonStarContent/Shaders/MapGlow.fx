sampler s0;

float4 ColorToUse;

float4 BaseTextureFunction(float4 color : COLOR0, float2 texCoord: TEXCOORD0) : COLOR
{
	float4 pixelColor = tex2D(s0, texCoord);
	if(pixelColor.r == 0 && pixelColor.g == 0 && pixelColor.b == 0 && pixelColor.a == 1)
		return ColorToUse;
	else
		return pixelColor;
}

technique Technique1
{
	pass BasicPass
	{
		PixelShader = compile ps_2_0 BaseTextureFunction();
	}
}
