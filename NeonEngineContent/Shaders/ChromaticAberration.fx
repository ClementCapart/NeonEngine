sampler s0;

float Distance;

float4 PixelShaderFunctionRed(float2 coords: TEXCOORD0) : COLOR0  
{  
	float4 color = tex2D(s0, coords + (0, -Distance));

	color.g = 0;
	color.b = 0;
	if(color.a > 0)
		color.a = 0.2;
	return color;
}  

float4 PixelShaderFunctionGreen(float2 coords: TEXCOORD0) : COLOR0  
{  
	float4 color = tex2D(s0, coords + (0 , Distance ));
	color.r = 0;
	color.b = 0;
	if(color.a > 0)
		color.a = 0.2;
	return color;
}  

float4 PixelShaderFunctionBlue(float2 coords: TEXCOORD0) : COLOR0  
{  
	float4 color = tex2D(s0, coords + (Distance, -Distance));

	color.g = 0;
	color.r = 0;
	
	if(color.a > 0)
		color.a = 0.2;

	return color;
} 

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
		return color;
}

technique Technique1  
{  
	pass Pass1  
	{  
		PixelShader = compile ps_2_0 PixelShaderFunction(); 
	}  

	pass Pass2
	{
		PixelShader = compile ps_2_0 PixelShaderFunctionGreen();
	}

	pass Pass3
	{
		PixelShader = compile ps_2_0 PixelShaderFunctionBlue();
	}

	pass Pass4
	{
		PixelShader = compile ps_2_0 PixelShaderFunctionRed();
	}
}  