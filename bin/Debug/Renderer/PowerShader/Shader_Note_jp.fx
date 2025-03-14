
////////////////////////////////////////////////////////////////////////////////////////////////
// 1. 陰影設定 /////////////////////////////////////////////////////////////////////////////////

// 光の倍率
#define LightPower 2.0

// 影の倍率
#define ShadowPower 0.75

// 光源が強い時の発光抑え
#define Saturate 0.0



////////////////////////////////////////////////////////////////////////////////////////////////
// 2. 発光設定 /////////////////////////////////////////////////////////////////////////////////

// 発光テクスチャの参照元
// 0 : 単色(テクスチャを無視して発光)
// 1 : モデルのテクスチャ
// 2 : エフェクト側(EMISSIVE_TEXTURE)で指定
#define EMISSIVE_FROM 0

// 発光テクスチャ
#define EMISSIVE_TEXTURE "emissive.png"

// 発光倍率
#define EmissiveScale 0.0



////////////////////////////////////////////////////////////////////////////////////////////////
// 3. ノーマルマップ設定 ///////////////////////////////////////////////////////////////////////

// ノーマルマップの取得元
// 0 : なし
// 1 : スフィアマップ
// 2 : エフェクト側(NORMAL_TEXTURE)で指定
#define NORMAL_FROM 0

// ノーマルマップのテクスチャ
#define NORMAL_TEXTURE "normal.png"

// ノーマルマップの倍率
#define NormalScale 1.0

// テクスチャのループ数
#define NormalLoop 1.0



////////////////////////////////////////////////////////////////////////////////////////////////
// 4. シェーディング設定 ///////////////////////////////////////////////////////////////////////

// トゥーンシェーディングをするか
// 0 : しない
// 1 : する
#define TOON_ENABLE 0

// 影範囲
#define ToonThreshold 0.0

// シェーディングヒント用テクスチャ
// AlternativeFullのシェーディングヒントと同じ仕様なので置き換え可
#define THRESHOLD_TEXTURE "shading_hint_toon.png"

// 影の有無
// 0 : 影なし
// 1 : 影あり
#define SHADOW_ENABLE 1



#include "Common_Shader.fxsub"
