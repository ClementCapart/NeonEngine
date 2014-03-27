sampler s0;

float2 GDistance;
float2 RDistance;
float2 BDistance;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0  
{  
	float4 rColor = tex2D(s0, coords + RDistance);
	float4 gColor = tex2D(s0, coords + GDistance);
	float4 bColor = tex2D(s0, coords + BDistance);

	return float4(rColor.r, gColor.g, bColor.b, 1.0);
}  



technique Technique1  
{  
	pass Pass1  
	{  
		PixelShader = compile ps_2_0 PixelShaderFunction(); 
	}  
}  