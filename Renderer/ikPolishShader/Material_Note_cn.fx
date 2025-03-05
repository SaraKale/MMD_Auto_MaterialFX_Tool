//-----------------------------------------------------------------------------
// 材质设置文件
// 用于非金属。对于金属来说指定Material_metal.fx。
// 在MME-ColorMapRT指定文件

#include "Sources/Material_header.fxsub"

// 素材类型
#define MATERIAL_TYPE		MT_NORMAL
/*
MT_NORMAL	: 普通(含金属)
MT_FACE		: 皮肤(脸用)
MT_LEAF		: 用于树叶和窗帘等背面能通透的材质。
MT_MASK		: 天空盒用。
*/

// 要使用的纹理文件名
#define TEXTURE_FILENAME_0	"textures/check.png"
#define TEXTURE_FILENAME_1	"textures/check.png"
#define TEXTURE_FILENAME_2	"textures/check.png"
#define TEXTURE_FILENAME_3	"textures/check.png"
#define TEXTURE_FILENAME_4	"textures/check.png"
#define TEXTURE_FILENAME_5	"textures/check.png"
// 下面的xxx_MAP_FILE中指定TEXTURE_FILENAME_x的数字。

// xxx_VALUE: 指定直接值。
// xxx_MAP_ENABLE: 数值为1时，从纹理读取值。
// xxx_MAP_LOOPNUM: 重复纹理的次数。1的话是等倍。数字越大越细。
// xxx_MAP_SCALE, xxx_MAP_OFFSET: 值以（纹理值*scale+offset）计算。

// 覆盖纹理颜色
#define	ALBEDO_MAP_ENABLE		0	// 0:使用原始颜色。1:使用此处指定的纹理。
#define ALBEDO_MAP_FILE			1	// 指定要使用的纹理文件编号。0-5
#define ALBEDO_MAP_LOOPNUM		1.0

// 汽车涂料：根据视线改变颜色的处理
#define	GRADIENT_MAP_ENABLE		0	// 1 :有效
#define GRADIENT_MAP_FILE			0
#define GRADIENT_POWER			1.0	//-4～4左右。坡度强度

// 指定纹理颜色的混合率
// 不使用ALBEDO_MAP时无效
#define BLEND_VALUE				0.0	// 与覆盖纹理的混合率。0.0的话就用原来的颜色。
#define BLEND_MAP_ENABLE		0	// 使用混合贴图与原始纹理合成？
#define BLEND_MAP_FILE			0	//指定要使用的纹理文件编号。0-5
#define BLEND_MAP_CHANNEL		R	// 要使用的纹理通道。R,G,B,A
#define BLEND_MAP_LOOPNUM		1.0
#define BLEND_MAP_SCALE			1.0
#define BLEND_MAP_OFFSET		0.0

// 是否为金属。≈反射强度
// 数值越高，反射越强&受到原来颜色的影响。
// 0:非金属，1:金属。宝石等为0.1-0.2左右
// 如果调整过黑最好调整METALNESS_VALUE值，一般0.1-0.5左右即可
#define	METALNESS_VALUE			0.0
#define	METALNESS_MAP_ENABLE	0	// 0:VALUE，1:在纹理中指定，启动金属贴图的渲染
#define METALNESS_MAP_FILE		0	// 指定要使用的纹理文件编号。0-3  也就是金属贴图序号
#define METALNESS_MAP_CHANNEL	R	// 要使用的纹理通道。R,G,B,A，分别对应通道里的红/绿/蓝/alpha通道
#define METALNESS_MAP_LOOPNUM	1.0
#define METALNESS_MAP_SCALE		1.0	// 控制效果大小
#define METALNESS_MAP_OFFSET	0.0	// 控制效果大小

// 表面光滑度
// SMOOTHNESS_TYPE = 1时，0:哑光，1:平滑。
// SMOOTHNESS_TYPE = 2时，0:平滑，1:哑光。

// Smoothness的指定方法：
// 0:根据模型的镜面反射功率自动确定。
// 1:使用平滑度。
// 2:使用粗糙度。
// 频繁闪光的话就改为0
#define SMOOTHNESS_TYPE		0

#define	SMOOTHNESS_VALUE		1.0
#define	SMOOTHNESS_MAP_ENABLE	0
#define SMOOTHNESS_MAP_FILE		0
#define SMOOTHNESS_MAP_CHANNEL	R
#define SMOOTHNESS_MAP_LOOPNUM	1.0
#define SMOOTHNESS_MAP_SCALE	1.0
#define SMOOTHNESS_MAP_OFFSET	0.0

// 只从底色求金属的反射色？
// 0:根据底色*镜面反射色确定。（ver0.16以前的方式）
// 1:仅用底色决定。
// 2:仅用镜面反射色决定。
// ※ 非金属时，与设定无关，为白色。
#define SPECULAR_COLOR_TYPE		1

// 镜面反射强度

// Intensity的处理：
#define INTENSITY_TYPE		0
// 0: Specular Intensity. 镜面反射强度的调整
// 1: Ambient Occlusion. 遮罩间接照明
// 2: Cavity. 遮蔽所有照明
// 3: Cavity (View Dependent). 遮挡所有照明（视线相关）

// 0:无高光，1:有高光
#define	INTENSITY_VALUE			1.0
#define	INTENSITY_MAP_ENABLE	0
#define INTENSITY_MAP_FILE		0
#define INTENSITY_MAP_CHANNEL	R
#define INTENSITY_MAP_LOOPNUM	1.0
#define INTENSITY_MAP_SCALE		1.0
#define INTENSITY_MAP_OFFSET	0.0

// 发光度
// ※发光度和皮下散射度不能共享。
#define	EMISSIVE_TYPE			0
// 0:AL对应
// 1:不发光（轻）
// 2:在此指定。EMISSIVE_VALUE or EMISSIVE_MAP
// 3:追加照明用
// 4:追加照明用（屏幕）
// 5:将模型的emissive作为发光色处理

// 以下为EMISSIVE_TYPE2、5时的设置：
#define	EMISSIVE_VALUE			1.0 // 0.0〜8.0
#define	EMISSIVE_MAP_ENABLE		0
#define EMISSIVE_MAP_FILE		0
#define EMISSIVE_MAP_CHANNEL	R
#define EMISSIVE_MAP_LOOPNUM	1.0
#define EMISSIVE_MAP_SCALE		1.0 // 0.0〜8.0
#define EMISSIVE_MAP_OFFSET		0.0

// 忽略模型的emissive
#define IGNORE_EMISSIVE		0

// 皮下散乱度：指定皮肤、塑料等半透明物质。
// 0:不透明。1:半透明。
// 金属的情况下被忽略。
#define	SSS_VALUE			0.0
#define	SSS_MAP_ENABLE		0
#define SSS_MAP_FILE		0
#define SSS_MAP_CHANNEL		R
#define SSS_MAP_LOOPNUM		1.0
#define SSS_MAP_SCALE		1.0
#define SSS_MAP_OFFSET		0.0

// 边缘照明的抑制
#define DISABLE_RIM			0

//-----------------------------------------------------------------------------
// 其他

// 忽略sphere地图（0：不忽略，1：忽略）
#define	IGNORE_SPHERE	1
// 只使用黑手党中央的颜色。（很少有用球体设定颜色的材质）
#define USE_ONLY_CENTER_OF_SPHERE	0

// 如果是该值以下的半透明度，则在材质上为透明处理。
#define AlphaThreshold		0.5

// 启用伪半透明处理（0：无效，1：有效，2：有效+有背面绘制）
#define	ENABLE_FAKE_SEMITRANSPARENT	0

// 用于计算伪半透明颜色的系数
#define SURFACE_ABSORPTION_RATE			0.1 // 统一的吸收率（0.0～1.0）
#define BODY_ABSORPTION_RATE			1.0 // 厚度变化的吸收率（0.0～）

//-----------------------------------------------------------------------------
// 法线贴图

// 是否使用法线贴图？0：未使用。1：使用
#define NORMALMAP_ENABLE		0

// 主法线映射
#define NORMALMAP_MAIN_FILENAME  	"textures/dummy_n.bmp"
#define NORMALMAP_MAIN_LOOPNUM	1.0		//法线位置
#define NORMALMAP_MAIN_HEIGHT	1.0  		//调节深浅，可以负数
// 方向反转
#define	NORMALMAP_MAIN_FLIP		FLIP_NONE
// FLIP_NONE:无反转
// 反转FLIP_X:x
// 反转FLIP_Y:y
// 反转FLIP_XY:x，y

// 子法线贴图
#define NORMALMAP_SUB_ENABLE	0
#define NORMALMAP_SUB_FILENAME  "textures/dummy_n.bmp"
#define NORMALMAP_SUB_LOOPNUM	1.0
#define NORMALMAP_SUB_HEIGHT	1.0
#define	NORMALMAP_SUB_FLIP		FLIP_NONE

// 上下反转纹理使用的模型为1
// 人型模型左右翻转较多。
#define NORMALMAP_FLIP_VERTICAL	0

// 在计算法线映射时抑制方向的修正。
// 仅在使用法线贴图时照明变得奇怪时
// 将其设为1可以改善。
#define DISABLE_ALIGNMENT_CORRECTION	0

//-----------------------------------------------------------------------------
// 视差遮挡(parallax occlusion mapping) 
// 使用parallax occlusion时，将main设为PolishMain_pom.fx等
// 必须为USE_ALBEDO_MAP1。

#define PARALLAX_ENABLE		0
#define PARALLAX_FILENAME	"textures/white.png"
#define PARALLAX_LOOPNUM	1.0		// 纹理重复次数

// 深度调整量（mmd单位）
// 深度图中0-1处的高度用mmd表示多高。
#define PARALLAX_HEIGHT		1.0

// 纹理上的参照距离
// （参考像素/纹理大小）
#define PARALLAX_LENGTH		(32.0/512.0)

#define PARALLAX_ITERATION	8	// 检索次数(1〜16)

//-----------------------------------------------------------------------------
#include "Sources/Material_body.fxsub"
