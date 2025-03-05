//-----------------------------------------------------------------------------
// 材質設置文件
// 用於非金屬。對於金屬來說指定Material_metal.fx。
// 在MME-ColorMapRT指定文件

#include "Sources/Material_header.fxsub"

// 素材類型
#define MATERIAL_TYPE		MT_NORMAL
/*
MT_NORMAL	: 普通(含金屬)
MT_FACE		: 皮膚(臉用)
MT_LEAF		: 用於樹葉和窗簾等背面能通透的材質。
MT_MASK		: 天空盒用。
*/

// 要使用的紋理文件名
#define TEXTURE_FILENAME_0	"textures/check.png"
#define TEXTURE_FILENAME_1	"textures/check.png"
#define TEXTURE_FILENAME_2	"textures/check.png"
#define TEXTURE_FILENAME_3	"textures/check.png"
#define TEXTURE_FILENAME_4	"textures/check.png"
#define TEXTURE_FILENAME_5	"textures/check.png"
// 下面的xxx_MAP_FILE中指定TEXTURE_FILENAME_x的數字。

// xxx_VALUE: 指定直接值。
// xxx_MAP_ENABLE: 數值為1時，從紋理讀取值。
// xxx_MAP_LOOPNUM: 重複紋理的次數。1的話是等倍。數字越大越細。
// xxx_MAP_SCALE, xxx_MAP_OFFSET: 值以（紋理值*scale+offset）計算。

// 覆蓋紋理顏色
#define	ALBEDO_MAP_ENABLE		0	// 0:使用原始顏色。1:使用此處指定的紋理。
#define ALBEDO_MAP_FILE			1	// 指定要使用的紋理文件編號。0-5
#define ALBEDO_MAP_LOOPNUM		1.0

// 汽車塗料：根據視線改變顏色的處理
#define	GRADIENT_MAP_ENABLE		0	// 1 :有效
#define GRADIENT_MAP_FILE			0
#define GRADIENT_POWER			1.0	//-4～4左右。坡度強度

// 指定紋理顏色的混合率
// 不使用ALBEDO_MAP時無效
#define BLEND_VALUE				0.0	// 與覆蓋紋理的混合率。0.0的話就用原來的顏色。
#define BLEND_MAP_ENABLE		0	// 使用混合貼圖與原始紋理合成？
#define BLEND_MAP_FILE			0	//指定要使用的紋理文件編號。0-5
#define BLEND_MAP_CHANNEL		R	// 要使用的紋理通道。R,G,B,A
#define BLEND_MAP_LOOPNUM		1.0
#define BLEND_MAP_SCALE			1.0
#define BLEND_MAP_OFFSET		0.0

// 是否為金屬。≈反射強度
// 數值越高，反射越強&受到原來顏色的影響。
// 0:非金屬，1:金屬。寶石等為0.1-0.2左右
// 如果調整過黑最好調整METALNESS_VALUE值，壹般0.1-0.5左右即可
#define	METALNESS_VALUE			0.0
#define	METALNESS_MAP_ENABLE	0	// 0:VALUE，1:在紋理中指定，啟動金屬貼圖的渲染
#define METALNESS_MAP_FILE		0	// 指定要使用的紋理文件編號。0-3  也就是金屬貼圖序號
#define METALNESS_MAP_CHANNEL	R	// 要使用的紋理通道。R,G,B,A，分別對應通道裏的紅/綠/藍/alpha通道
#define METALNESS_MAP_LOOPNUM	1.0
#define METALNESS_MAP_SCALE		1.0	// 控製效果大小
#define METALNESS_MAP_OFFSET	0.0	// 控製效果大小

// 表面光滑度
// SMOOTHNESS_TYPE = 1時，0:啞光，1:平滑。
// SMOOTHNESS_TYPE = 2時，0:平滑，1:啞光。

// Smoothness的指定方法：
// 0:根據模型的鏡面反射功率自動確定。
// 1:使用平滑度。
// 2:使用粗糙度。
// 頻繁閃光的話就改為0
#define SMOOTHNESS_TYPE		0

#define	SMOOTHNESS_VALUE		1.0
#define	SMOOTHNESS_MAP_ENABLE	0
#define SMOOTHNESS_MAP_FILE		0
#define SMOOTHNESS_MAP_CHANNEL	R
#define SMOOTHNESS_MAP_LOOPNUM	1.0
#define SMOOTHNESS_MAP_SCALE	1.0
#define SMOOTHNESS_MAP_OFFSET	0.0

// 只從底色求金屬的反射色？
// 0:根據底色*鏡面反射色確定。（ver0.16以前的方式）
// 1:僅用底色決定。
// 2:僅用鏡面反射色決定。
// ※ 非金屬時，與設定無關，為白色。
#define SPECULAR_COLOR_TYPE		1

// 鏡面反射強度

// Intensity的處理：
#define INTENSITY_TYPE		0
// 0: Specular Intensity. 鏡面反射強度的調整
// 1: Ambient Occlusion. 遮罩間接照明
// 2: Cavity. 遮蔽所有照明
// 3: Cavity (View Dependent). 遮擋所有照明（視線相關）

// 0:無高光，1:有高光
#define	INTENSITY_VALUE			1.0
#define	INTENSITY_MAP_ENABLE	0
#define INTENSITY_MAP_FILE		0
#define INTENSITY_MAP_CHANNEL	R
#define INTENSITY_MAP_LOOPNUM	1.0
#define INTENSITY_MAP_SCALE		1.0
#define INTENSITY_MAP_OFFSET	0.0

// 發光度
// ※發光度和皮下散射度不能共享。
#define	EMISSIVE_TYPE			0
// 0:AL對應
// 1:不發光（輕）
// 2:在此指定。EMISSIVE_VALUE or EMISSIVE_MAP
// 3:追加照明用
// 4:追加照明用（屏幕）
// 5:將模型的emissive作為發光色處理

// 以下為EMISSIVE_TYPE2、5時的設置：
#define	EMISSIVE_VALUE			1.0 // 0.0〜8.0
#define	EMISSIVE_MAP_ENABLE		0
#define EMISSIVE_MAP_FILE		0
#define EMISSIVE_MAP_CHANNEL	R
#define EMISSIVE_MAP_LOOPNUM	1.0
#define EMISSIVE_MAP_SCALE		1.0 // 0.0〜8.0
#define EMISSIVE_MAP_OFFSET		0.0

// 忽略模型的emissive
#define IGNORE_EMISSIVE		0

// 皮下散亂度：指定皮膚、塑料等半透明物質。
// 0:不透明。1:半透明。
// 金屬的情況下被忽略。
#define	SSS_VALUE			0.0
#define	SSS_MAP_ENABLE		0
#define SSS_MAP_FILE		0
#define SSS_MAP_CHANNEL		R
#define SSS_MAP_LOOPNUM		1.0
#define SSS_MAP_SCALE		1.0
#define SSS_MAP_OFFSET		0.0

// 邊緣照明的抑製
#define DISABLE_RIM			0

//-----------------------------------------------------------------------------
// 其他

// 忽略sphere地圖（0：不忽略，1：忽略）
#define	IGNORE_SPHERE	1
// 只使用黑手黨中央的顏色。（很少有用球體設定顏色的材質）
#define USE_ONLY_CENTER_OF_SPHERE	0

// 如果是該值以下的半透明度，則在材質上為透明處理。
#define AlphaThreshold		0.5

// 啟用偽半透明處理（0：無效，1：有效，2：有效+有背面繪製）
#define	ENABLE_FAKE_SEMITRANSPARENT	0

// 用於計算偽半透明顏色的系數
#define SURFACE_ABSORPTION_RATE			0.1 // 統壹的吸收率（0.0～1.0）
#define BODY_ABSORPTION_RATE			1.0 // 厚度變化的吸收率（0.0～）

//-----------------------------------------------------------------------------
// 法線貼圖

// 是否使用法線貼圖？0：未使用。1：使用
#define NORMALMAP_ENABLE		0

// 主法線映射
#define NORMALMAP_MAIN_FILENAME  	"textures/dummy_n.bmp"
#define NORMALMAP_MAIN_LOOPNUM	1.0		//法線位置
#define NORMALMAP_MAIN_HEIGHT	1.0  		//調節深淺，可以負數
// 方向反轉
#define	NORMALMAP_MAIN_FLIP		FLIP_NONE
// FLIP_NONE:無反轉
// 反轉FLIP_X:x
// 反轉FLIP_Y:y
// 反轉FLIP_XY:x，y

// 子法線貼圖
#define NORMALMAP_SUB_ENABLE	0
#define NORMALMAP_SUB_FILENAME  "textures/dummy_n.bmp"
#define NORMALMAP_SUB_LOOPNUM	1.0
#define NORMALMAP_SUB_HEIGHT	1.0
#define	NORMALMAP_SUB_FLIP		FLIP_NONE

// 上下反轉紋理使用的模型為1
// 人型模型左右翻轉較多。
#define NORMALMAP_FLIP_VERTICAL	0

// 在計算法線映射時抑製方向的修正。
// 僅在使用法線貼圖時照明變得奇怪時
// 將其設為1可以改善。
#define DISABLE_ALIGNMENT_CORRECTION	0

//-----------------------------------------------------------------------------
// 視差遮擋(parallax occlusion mapping) 
// 使用parallax occlusion時，將main設為PolishMain_pom.fx等
// 必須為USE_ALBEDO_MAP1。

#define PARALLAX_ENABLE		0
#define PARALLAX_FILENAME	"textures/white.png"
#define PARALLAX_LOOPNUM	1.0		// 紋理重複次數

// 深度調整量（mmd單位）
// 深度圖中0-1處的高度用mmd表示多高。
#define PARALLAX_HEIGHT		1.0

// 紋理上的參照距離
// （參考像素/紋理大小）
#define PARALLAX_LENGTH		(32.0/512.0)

#define PARALLAX_ITERATION	8	// 檢索次數(1〜16)

//-----------------------------------------------------------------------------
#include "Sources/Material_body.fxsub"
