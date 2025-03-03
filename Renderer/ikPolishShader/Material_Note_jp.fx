//-----------------------------------------------------------------------------
// 材質設定ファイル
// 非金属用。金属の場合は Material_metal.fx を割り当てる。

#include "Sources/Material_header.fxsub"

// マテリアルタイプ
#define MATERIAL_TYPE		MT_NORMAL
/*
MT_NORMAL	: 通常 (金属を含む)
MT_FACE		: 肌 (顔用)
MT_LEAF		: 葉やカーテンなど、裏が透ける材質用。
MT_MASK		: スカイドーム用。
*/

// 使用するテクスチャファイル名
#define TEXTURE_FILENAME_0	"textures/check.png"
#define TEXTURE_FILENAME_1	"textures/check.png"
#define TEXTURE_FILENAME_2	"textures/check.png"
#define TEXTURE_FILENAME_3	"textures/check.png"
#define TEXTURE_FILENAME_4	"textures/check.png"
#define TEXTURE_FILENAME_5	"textures/check.png"
// TEXTURE_FILENAME_x の数字を、以下の xxx_MAP_FILE で指定する。

// xxx_VALUE: 直接値を指定する。
// xxx_MAP_ENABLE: 1の場合、テクスチャから値を読み込む。
// xxx_MAP_LOOPNUM: テクスチャの繰り返す回数。1なら等倍。数字が大きいほど細かくなる。
// xxx_MAP_SCALE, xxx_MAP_OFFSET: 値は (テクスチャの値 * scale + offset) で計算する。


// テクスチャの色を上書きする
#define	ALBEDO_MAP_ENABLE		0	// 0:元の色を使う。1:ここで指定したテクスチャを使う。
#define ALBEDO_MAP_FILE			1	// 使用するテクスチャファイル番号を指定。0-5
#define ALBEDO_MAP_LOOPNUM		1.0

// カーペイント：視線に応じて色を変える処理
#define	GRADIENT_MAP_ENABLE		0	// 1:有効
#define GRADIENT_MAP_FILE		0
#define GRADIENT_POWER			1.0	// -4〜4程度。勾配の強さ

// テクスチャ色のブレンド率を指定する
// ALBEDO_MAPを使わない場合は無効
#define BLEND_VALUE				0.0	// 上書きテクスチャとのブレンド率。0.0なら元の色を使う。
#define BLEND_MAP_ENABLE		0	// ブレンドマップを使って元のテクスチャと合成する?
#define BLEND_MAP_FILE			0	// 使用するテクスチャファイル番号を指定。0-5
#define BLEND_MAP_CHANNEL		R	// 使用するテクスチャのチャンネル。R,G,B,A
#define BLEND_MAP_LOOPNUM		1.0
#define BLEND_MAP_SCALE			1.0
#define BLEND_MAP_OFFSET		0.0


// 金属かどうか。≒ 反射の強さ
// 数値が高いほど反射が強くなる＆元の色の影響を受けるようになる。
// 0: 非金属、1:金属。宝石などは0.1-0.2程度。
#define	METALNESS_VALUE			0.0
#define	METALNESS_MAP_ENABLE	0	// 0:VALUEを使う、1: テクスチャで指定する
#define METALNESS_MAP_FILE		0	// 使用するテクスチャファイル番号を指定。0-3
#define METALNESS_MAP_CHANNEL	R	// 使用するテクスチャのチャンネル。R,G,B,A
#define METALNESS_MAP_LOOPNUM	1.0
#define METALNESS_MAP_SCALE		1.0
#define METALNESS_MAP_OFFSET	0.0


// 表面の滑らかさ
// SMOOTHNESS_TYPE = 1 の場合、0:マット、1:滑らか。
// SMOOTHNESS_TYPE = 2 の場合、0:滑らか、1:マット。

// Smoothnessの指定方法：
// 0: モデルのスペキュラパワーから自動で決定する。
// 1: スムースネスを使用。
// 2: ラフネスを使用。
#define SMOOTHNESS_TYPE		0

#define	SMOOTHNESS_VALUE		1.0
#define	SMOOTHNESS_MAP_ENABLE	0
#define SMOOTHNESS_MAP_FILE		0
#define SMOOTHNESS_MAP_CHANNEL	R
#define SMOOTHNESS_MAP_LOOPNUM	1.0
#define SMOOTHNESS_MAP_SCALE	1.0
#define SMOOTHNESS_MAP_OFFSET	0.0

// 金属の反射色をベース色だけから求める?
// 0: ベース色 * スペキュラ色で決定。(ver0.16以前の方式)
// 1: ベース色のみで決定。
// 2: スペキュラ色のみで決定。
// ※ 非金属の場合は、設定とは無関係に白になる。
#define SPECULAR_COLOR_TYPE		1


// スペキュラ強度

// Intensityの扱い：
#define INTENSITY_TYPE		0
// 0: Specular Intensity. スペキュラ強度の調整
// 1: Ambient Occlusion. 間接光へのマスク
// 2: Cavity. 全てのライティングを遮蔽
// 3: Cavity (View Dependent). 全てのライティングを遮蔽(視線依存)

// 0:ハイライトなし、1:ハイライトあり
#define	INTENSITY_VALUE			1.0
#define	INTENSITY_MAP_ENABLE	0
#define INTENSITY_MAP_FILE		0
#define INTENSITY_MAP_CHANNEL	R
#define INTENSITY_MAP_LOOPNUM	1.0
#define INTENSITY_MAP_SCALE		1.0
#define INTENSITY_MAP_OFFSET	0.0


// 発光度
// ※発光度と皮下散乱度は共有できない。
#define	EMISSIVE_TYPE			0
// 0: AL対応
// 1: 発光しない (軽い)
// 2: ここで指定。EMISSIVE_VALUE or EMISSIVE_MAP
// 3: 追加ライト用
// 4: 追加ライト用(スクリーン)
// 5: モデルのemissiveを発光色として扱う

// 以下は EMISSIVE_TYPE 2, 5の場合の設定：
#define	EMISSIVE_VALUE			1.0 // 0.0〜8.0
#define	EMISSIVE_MAP_ENABLE		0
#define EMISSIVE_MAP_FILE		0
#define EMISSIVE_MAP_CHANNEL	R
#define EMISSIVE_MAP_LOOPNUM	1.0
#define EMISSIVE_MAP_SCALE		1.0 // 0.0〜8.0
#define EMISSIVE_MAP_OFFSET		0.0

// モデルのemissiveを無視する
#define IGNORE_EMISSIVE		0


// 皮下散乱度：肌、プラスチックなどの半透明なものに指定。
// 0:不透明。1:半透明。
// 金属の場合は無視される。
#define	SSS_VALUE			0.0
#define	SSS_MAP_ENABLE		0
#define SSS_MAP_FILE		0
#define SSS_MAP_CHANNEL		R
#define SSS_MAP_LOOPNUM		1.0
#define SSS_MAP_SCALE		1.0
#define SSS_MAP_OFFSET		0.0


// リムライトの抑制
#define DISABLE_RIM			0


//-----------------------------------------------------------------------------
// その他

// スフィアマップを無視する(0:無視しない、1:無視する)
#define	IGNORE_SPHERE	1
// スフィアの中央の色だけを使う。(スフィアで色を設定する材質がまれにある)
#define USE_ONLY_CENTER_OF_SPHERE	0

// この値以下の半透明度ならマテリアル的には透明扱いにする。
#define AlphaThreshold		0.5


// 疑似半透明処理を有効にする(0:無効、1:有効、2:有効+裏面描画あり)
#define	ENABLE_FAKE_SEMITRANSPARENT	0

// 疑似半透明の色計算用の係数
#define SURFACE_ABSORPTION_RATE			0.1 // 一律の吸収率 (0.0〜1.0)
#define BODY_ABSORPTION_RATE			1.0 // 厚みで変わる吸収率 (0.0〜)


//-----------------------------------------------------------------------------
// 法線マップ

// 法線マップを使用するか? 0:未使用。1:使用
#define NORMALMAP_ENABLE		0

// メイン法線マップ
#define NORMALMAP_MAIN_FILENAME  	"textures/dummy_n.bmp"
#define NORMALMAP_MAIN_LOOPNUM	1.0
#define NORMALMAP_MAIN_HEIGHT	1.0
// 方向の反転
#define	NORMALMAP_MAIN_FLIP		FLIP_NONE
// FLIP_NONE: 反転なし
// FLIP_X: xを反転
// FLIP_Y: yを反転
// FLIP_XY: x,yを反転

// サブ法線マップ
#define NORMALMAP_SUB_ENABLE	0
#define NORMALMAP_SUB_FILENAME "textures/dummy_n.bmp"
#define NORMALMAP_SUB_LOOPNUM	1.0
#define NORMALMAP_SUB_HEIGHT	1.0
#define	NORMALMAP_SUB_FLIP		FLIP_NONE


// テクスチャを上下反転して使用するモデルなら1
// 人型モデルは左右反転が多い。
#define NORMALMAP_FLIP_VERTICAL	0

// 法線マップ計算時に向きの補正を抑制する。
// 法線マップ使用時だけライティングがおかしくなる場合、
// これを1にすることで改善できることがある。
#define DISABLE_ALIGNMENT_CORRECTION	0


//-----------------------------------------------------------------------------
// 視差遮蔽(parallax occlusion mapping) 
// parallax occlusionを使用する場合、mainをPolishMain_pom.fxなど、
// USE_ALBEDO_MAP 1にしたものにする必要がある。

#define PARALLAX_ENABLE		0
#define PARALLAX_FILENAME	"textures/white.png"
#define PARALLAX_LOOPNUM	1.0		// テクスチャの繰り返し回数

// 深度の調整量(mmd単位)
// 深度マップでの0-1での高さが、mmdでどれくらいの高さを表すか。
#define PARALLAX_HEIGHT		1.0

// テクスチャ上での参照距離
// (参照ピクセル/テクスチャサイズ)
#define PARALLAX_LENGTH		(32.0/512.0)

#define PARALLAX_ITERATION	8	// 検索回数(1〜16)


//-----------------------------------------------------------------------------
#include "Sources/Material_body.fxsub"
