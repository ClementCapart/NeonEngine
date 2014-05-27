sampler s0;

float2 PixelSize;
float Threshold = 0.5;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0  
{  
	float2 off = float2(PixelSize.x, 0); 

	float4 current = tex2D(s0, coords);
	float4 left = tex2D(s0, coords - off);
	float4 right = tex2D(s0, coords + off);

	if((left.a < Threshold && current.a > Threshold) && (right.a < Threshold && current.a > Threshold))
		return float4(0, 0, 0, 1);
	else
		return tex2D(s0, coords);
}  

technique Technique1  
{  
	pass Pass1  
	{  
		PixelShader = compile ps_3_0 PixelShaderFunction(); 
	}  
}  