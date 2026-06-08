#更新20220727
修正matcap的错误方向，对齐进行垂直翻转以保证和PmxEditor中效果一致。

#更新20240322
追加ALBEDO_MATCAP_MAP_AFFECT_SWIZZLE参数，可以自由选择使用哪个通道作为影响值

#更新20240704
修复ALBEDO_MATCAP_MAP_AFFECT_SWIZZLE参数为3时报错问题


##################
SJ_Matcap for ray
ver1.1(152)
##################

【材质说明】
本材质用于在Ray渲染中使用加算/乘算的Matcap
同时可以指定材质受到Matcap影响的程度

###基于Ray1.5.2，其他版本可能会有出错，如有可以联系我进行适配###

【使用规约】
1.请在借物表显眼处标注我的名字（三金络合物）
2.请不要宣称此材质是自己制作的
3.材质【允许】二次配布，但是配布时必须包含这份规约和完整原始材质的压缩包，且借物表必须书写完整
4.【禁止】售卖本材质！

【参数定义说明】

#define ALBEDO_MATCAP_ENABLE 0/1/2
	当此常量为0时，禁用Matcap功能。
	当此常量为1时，启用Matcap功能，且此时计算方式为乘算。
	当此常量为2时，启用Matcap功能，且此时计算方式为加算。
	
#define ALBEDO_MATCAP_MAP_FILE "matcap.png"
	[当ALBEDO_MATCAP_ENABLE为1或2时] 在此处填写Matcap图片的路径名。
	
#define ALBEDO_MATCAP_MAP_AFFECT_FROM 0/1
	当此常量为0时，禁用读取影响程度的通道图片。
	当此常量为1时，启用读取影响程度的通道图片。

#define ALBEDO_MATCAP_MAP_AFFECT_SWIZZLE 0/1/2/3/4
	当此常量为0时，读取影响程度的通道图片的R通道。
	当此常量为1时，读取影响程度的通道图片的G通道。
	当此常量为2时，读取影响程度的通道图片的B通道。
	当此常量为3时，读取影响程度的通道图片的A通道。
	当此常量为4或未定义时，读取影响程度的通道图片的RGB通道。
	
#define ALBEDO_MATCAP_MAP_AFFECT_FILE "affect.png"
	[当ALBEDO_MATCAP_MAP_AFFECT_FROM为1时] 在此处填写影响程度的通道图片的路径名。
	
const float3 albedoMatcap = [float3 value];
	此处填写的是Matcap的强度(颜色)。
	注意可以使用float3来控制RGB。
	
const float3 albedoMatcap_Affect = [float3 value];
	此处填写的是材质受影响的程度。
	当这个值越接近0时代表越不受Matcap的影响(无论乘算加算)。
	当这个值为1时代表完全受Matcap的影响(无论乘算加算)。
	注意可以使用float3来控制RGB。
	如果ALBEDO_MATCAP_MAP_AFFECT_FROM为1，那么这个值会与影响程度的通道图片进行乘算。




其余常规材质操作，可以自行修改fx文件

联系bilibili@三金络合物
uid：1223127584

QQ：3371741288 / 467440249