
////////////////////////////////////////////////////////////////////////////////////////////////
// 1. 阴影设定 /////////////////////////////////////////////////////////////////////////////////

// 光的倍率
#define LightPower 2.0

// 影的倍率
#define ShadowPower 0.75

// 光源强时的发光抑制
#define Saturate 0.0



////////////////////////////////////////////////////////////////////////////////////////////////
// 2. 发光设定 /////////////////////////////////////////////////////////////////////////////////

// 发光纹理参考源
// 0: 单色（忽略纹理发光）
// 1: 模型纹理
// 2: 在效果侧指定
#define EMISSIVE_FROM 0

// 发光纹理
#define EMISSIVE_TEXTURE "emissive.png"

// 发光倍率
#define EmissiveScale 0.0



////////////////////////////////////////////////////////////////////////////////////////////////
// 3. 法线映射设置 ///////////////////////////////////////////////////////////////////////

// 正常地图的获取源
// 0: 无
// 1: 球形地图
// 2: 在效果侧指定
#define NORMAL_FROM 0

// 法线贴图纹理
#define NORMAL_TEXTURE "normal.png"

// 法线映射倍率
#define NormalScale 1.0

// 纹理循环数
#define NormalLoop 1.0



////////////////////////////////////////////////////////////////////////////////////////////////
// 4. Toon设置 ///////////////////////////////////////////////////////////////////////

// 是否进行Toon着色
// 0: 不做
// 1: 做
#define TOON_ENABLE 1

// 影子范围
#define ToonThreshold 0.2

// 阴影提示纹理
// 与AlternativeFull的着色提示规格相同，可替换
#define THRESHOLD_TEXTURE "shading_hint_toon.png"

//有无阴影
//0: 无阴影
//1: 有阴影
#define SHADOW_ENABLE 1



#include "Common_Shader.fxsub"
