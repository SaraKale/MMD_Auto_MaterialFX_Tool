//-----------------------------------------------------------------------------
// 通用的预设。

//----------------------------------------------------------
// SSS（次表面散射）设置

// 是否启用天鹅绒效果？
#define ENABLE_VELVET	0
const float VelvetExponent = 2.0;			// 边缘的大小
const float VelvetBaseReflection = 0.01;	// 正面的亮度 
#define VELVET_MUL_COLOR		float3(0.50, 0.50, 0.50)	// 正面的颜色（乘法）
#define VELVET_MUL_RIM_COLOR	float3(1.00, 1.00, 1.00)	// 边缘的颜色（乘法）
#define VELVET_ADD_COLOR		float3(0.00, 0.00, 0.00)	// 正面的颜色（加法）
#define VELVET_ADD_RIM_COLOR	float3(0.00, 0.00, 0.00)	// 边缘的颜色（加法）

//----------------------------------------------------------
// 高光相关

// 是否使用模型的高光颜色？
#define ENABLE_SPECULAR_COLOR	0	// 0:禁用，1:启用

// 清漆效果
// 在模型上添加透明层。
#define ENABLE_CLEARCOAT		0			// 0:禁用，1:启用

const float USE_POLYGON_NORMAL = 1.0;		// 忽略清漆层的法线贴图？
const float ClearcoatSmoothness =  0.95;		// 越接近1，高光越锐利。（0~1）
const float ClearcoatIntensity = 0.5;		// 高光强度。0为关闭。（0~1.0）
const float3 ClearcoatF0 = float3(0.05,0.05,0.05);	// 高光反射率
const float4 ClearcoatColor = float4(1,1,1, 0.0);	// 清漆的颜色


// 添加头发专用的高光
#define ENABLE_HAIR_SPECULAR	0
// 头发光泽
const float HairSmoothness = 0.5;	// (0~1)
// 头发高光的强度
const float HairSpecularIntensity = 1.0;	// (0~1)
// 头发方向的基准骨骼名称
// #define HAIR_CENTER_BONE_NAME	"头"


// 禁用球面贴图。
#define IGNORE_SPHERE	1

// 球面贴图的强度
float3 SphereScale = float3(1.0, 1.0, 1.0) * 0.1;

// 根据高光提高不透明度。
// 启用后，玻璃等反射的高光会更强烈。
// 对于使用Alpha通道的草等物体，边缘可能会出现强烈的高光。
#define ENABLE_SPECULAR_ALPHA	0


//----------------------------------------------------------
// 其他

#define ToonColor_Scale			0.5			// 卡通颜色的强调程度。（0.0~1.0）

// 裁剪Alpha
// 用于树叶等镂空纹理边缘不清晰的情况。
#define Enable_Cutout	0
#define CutoutThreshold	0.5		// 透明/不透明的边界值

#define IS_LIGHT	0		// 用于附加灯光

// 忽略自发光
// 为0时，(漫反射颜色 + 自发光颜色) * 纹理颜色 为基础颜色。
// 为1时，漫反射颜色 * 纹理颜色 为基础颜色。
#define IGNORE_EMISSIVE		0

// 从g-buffer中获取颜色。
// 使用POM（视差遮蔽映射）时，由于高度变化，颜色位置会改变，因此需要从g-buffer中获取颜色。
// 为0时，从模型的纹理中获取颜色
#define USE_ALBEDO_MAP		0


//----------------------------------------------------------
// 加载通用处理
#include "Sources/PolishMain_common.fxsub"