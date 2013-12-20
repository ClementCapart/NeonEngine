sampler s0;



float4 BaseTextureFunction(float4 color : COLOR0, float2 texCoord: TEXCOORD0) : COLOR
{
	return tex2D(s0, texCoord) * color;
}

technique Technique1
{
	pass BasicPass
	{
		PixelShader = compile ps_2_0 BaseTextureFunction();
	}
}
