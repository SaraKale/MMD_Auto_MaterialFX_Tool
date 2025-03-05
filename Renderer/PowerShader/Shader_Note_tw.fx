
////////////////////////////////////////////////////////////////////////////////////////////////
// 1. 陰影設定 /////////////////////////////////////////////////////////////////////////////////

// 光的倍率
#define LightPower 2.0

// 影的倍率
#define ShadowPower 0.75

// 光源強時的發光抑製
#define Saturate 0.0



////////////////////////////////////////////////////////////////////////////////////////////////
// 2. 發光設定 /////////////////////////////////////////////////////////////////////////////////

// 發光紋理參考源
// 0: 單色（忽略紋理發光）
// 1: 模型紋理
// 2: 在效果側指定
#define EMISSIVE_FROM 0

// 發光紋理
#define EMISSIVE_TEXTURE "emissive.png"

// 發光倍率
#define EmissiveScale 0.0



////////////////////////////////////////////////////////////////////////////////////////////////
// 3. 法線映射設置 ///////////////////////////////////////////////////////////////////////

// 正常地圖的獲取源
// 0: 無
// 1: 球形地圖
// 2: 在效果側指定
#define NORMAL_FROM 0

// 法線貼圖紋理
#define NORMAL_TEXTURE "normal.png"

// 法線映射倍率
#define NormalScale 1.0

// 紋理循環數
#define NormalLoop 1.0



////////////////////////////////////////////////////////////////////////////////////////////////
// 4. Toon設置 ///////////////////////////////////////////////////////////////////////

// 是否進行Toon着色
// 0: 不做
// 1: 做
#define TOON_ENABLE 1

// 影子範圍
#define ToonThreshold 0.2

// 陰影提示紋理
// 與AlternativeFull的着色提示規格相同，可替換
#define THRESHOLD_TEXTURE "shading_hint_toon.png"

//有無陰影
//0: 無陰影
//1: 有陰影
#define SHADOW_ENABLE 1



#include "Common_Shader.fxsub"
