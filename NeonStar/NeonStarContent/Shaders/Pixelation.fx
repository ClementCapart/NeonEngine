sampler tex;

float4 BaseTextureFunction(float2 texCoord : TEXCOORD0) : COLOR
{
	 float dx = 5.*(1./256.);
	 float dy = 5.*(1./256.);
	 float2 coord;
	 coord.x = dx*floor(texCoord.x/dx);
	 coord.y = dy*floor(texCoord.y/dy);
	 return tex2D(tex, coord);
}

technique Technique1
{
	pass PixelationPass
	{
		PixelShader = compile ps_2_0 BaseTextureFunction();
	}
}
