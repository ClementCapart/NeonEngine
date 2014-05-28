sampler s0;

float4 BaseTextureFunction(float4 color : COLOR0, float2 texCoord: TEXCOORD0) : COLOR
{
	float4 pixel = 1.0f - tex2D(s0, texCoord) * color;
	pixel.a = 1.0f;
	return pixel;
}

technique Technique1
{
	pass BasicPass
	{
		PixelShader = compile ps_2_0 BaseTextureFunction();
	}
}
