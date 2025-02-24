//-----------------------------------------------------------------------------
// 通用的預設。

//----------------------------------------------------------
// SSS（次表面散射）設置

// 是否啟用天鵝絨效果？
#define ENABLE_VELVET	0
const float VelvetExponent = 2.0;			// 邊緣的大小
const float VelvetBaseReflection = 0.01;	// 正面的亮度 
#define VELVET_MUL_COLOR		float3(0.50, 0.50, 0.50)	// 正面的顏色（乘法）
#define VELVET_MUL_RIM_COLOR	float3(1.00, 1.00, 1.00)	// 邊緣的顏色（乘法）
#define VELVET_ADD_COLOR		float3(0.00, 0.00, 0.00)	// 正面的顏色（加法）
#define VELVET_ADD_RIM_COLOR	float3(0.00, 0.00, 0.00)	// 邊緣的顏色（加法）

//----------------------------------------------------------
// 高光相關

// 是否使用模型的高光顏色？
#define ENABLE_SPECULAR_COLOR	0	// 0:禁用，1:啟用

// 清漆效果
// 在模型上添加透明層。
#define ENABLE_CLEARCOAT		0			// 0:禁用，1:啟用

const float USE_POLYGON_NORMAL = 1.0;		// 忽略清漆層的法線貼圖？
const float ClearcoatSmoothness =  0.95;		// 越接近1，高光越銳利。（0~1）
const float ClearcoatIntensity = 0.5;		// 高光強度。0為關閉。（0~1.0）
const float3 ClearcoatF0 = float3(0.05,0.05,0.05);	// 高光反射率
const float4 ClearcoatColor = float4(1,1,1, 0.0);	// 清漆的顏色


// 添加頭發專用的高光
#define ENABLE_HAIR_SPECULAR	0
// 頭發光澤
const float HairSmoothness = 0.5;	// (0~1)
// 頭發高光的強度
const float HairSpecularIntensity = 1.0;	// (0~1)
// 頭發方向的基準骨骼名稱
// #define HAIR_CENTER_BONE_NAME	"頭"


// 禁用球面貼圖。
#define IGNORE_SPHERE	1

// 球面貼圖的強度
float3 SphereScale = float3(1.0, 1.0, 1.0) * 0.1;

// 根據高光提高不透明度。
// 啟用後，玻璃等反射的高光會更強烈。
// 對於使用Alpha通道的草等物體，邊緣可能會出現強烈的高光。
#define ENABLE_SPECULAR_ALPHA	0


//----------------------------------------------------------
// 其他

#define ToonColor_Scale			0.5			// 卡通顏色的強調程度。（0.0~1.0）

// 裁剪Alpha
// 用於樹葉等鏤空紋理邊緣不清晰的情況。
#define Enable_Cutout	0
#define CutoutThreshold	0.5		// 透明/不透明的邊界值

#define IS_LIGHT	0		// 用於附加燈光

// 忽略自發光
// 為0時，(漫反射顏色 + 自發光顏色) * 紋理顏色 為基礎顏色。
// 為1時，漫反射顏色 * 紋理顏色 為基礎顏色。
#define IGNORE_EMISSIVE		0

// 從g-buffer中獲取顏色。
// 使用POM（視差遮蔽映射）時，由於高度變化，顏色位置會改變，因此需要從g-buffer中獲取顏色。
// 為0時，從模型的紋理中獲取顏色
#define USE_ALBEDO_MAP		0


//----------------------------------------------------------
// 加載通用處理
#include "Sources/PolishMain_common.fxsub"