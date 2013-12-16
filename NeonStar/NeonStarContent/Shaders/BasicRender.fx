sampler s0;

float4 BaseTextureFunction(float2 texCoord: TEXCOORD0) : COLOR
{
	return tex2D(s0, texCoord);
}

technique Technique1
{
	pass BasicPass
	{
		PixelShader = compile ps_2_0 BaseTextureFunction();
	}
}
