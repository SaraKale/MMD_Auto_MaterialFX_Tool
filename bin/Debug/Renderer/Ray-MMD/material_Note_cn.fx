//============================
//		ray材质翻译 v1.5.2
//============================

//----------------------------------------
//	albedo 物体贴图色
//----------------------------------------
// 基本颜色修改部分
// ALBEDO MAP FROM 指定单一色或纹理贴图默认时从PMX模型中的纹理插槽获取基本颜色
// 0：设置一个RGB的颜色到下方的"const float.3 albedo=1.0”来设置模型的颜色
// 1：使用bmp, png, jpg, tga, dds, gif, apng图片路径
// 2：使用GIF/APNG的路径
// 3：使用PMX模型中tex纹理图片
// 4：使用PMX模型中sp镜面图片
// 5：使用PMX模型中Toon图片
// 6：将AVI视频或渲染后结果作模型贴图纹理，需先放置Extension/DummyScreen/中DummyScreen.X
// 7：将PMX中的环境色用于替换模型的颜色
// 8：将PMX中的镜面色用于替换模型的颜色
// 9：将PMX中的光泽度用于替换模型的颜色（只能被光滑度使用）
// 提示1：ALBEDO是描述物体在消除了非金属材质的镜面反射后的基本颜色，在UE4或者其它引擎中同样也被称为底色或者固有色
// 提示2：HDR图片是工作在高动态的线性色彩空间中，所以不要将一个HDR文件用作ALBEDO,否则会丢失数据产生一些问题
// 提示3：一些(bmp,png,jpg,tga,dds,gif,apng)可能不是工作在sRGB的色域中的会产生一些问题，不过大部分图片都是工作在sRGB
// 提示4：GIF和APNG是占用CPU的开销
#define ALBEDO_MAP_FROM 3

// UV坐标翻转功能
// 1 : X轴水平翻转
// 2 : Y轴垂直翻转
// 3 : 水平+垂直同时反转
#define ALBEDO_MAP_UV_FLIP 0

// 更改const float3 albedo = 1.0;语句 修改颜色的模式
// 0：无效
// 1 : 乘算
// 2 : 指数乘算
#define ALBEDO_MAP_APPLY_SCALE 0

// 从PMX模型中的扩散色乘算到贴图上。
#define ALBEDO_MAP_APPLY_DIFFUSE 1

//  如果表情中有(R+/G+/B+)控制器，则可以用来乘算到贴图上修改模型颜色
#define ALBEDO_MAP_APPLY_MORPH_COLOR 0

//  贴图路径，如果设置了1或者2到`ALBEDO_MAP_FROM`时,需要将一个相对或者绝对的图片的路径输入到这里
// Tips : 父文件夹是 "../",  改变所有的 "\" to "/".
// 若 xxx.png  material.fx 在统一文件夹内                                  #define ALBEDO_MAP_FILE "xxx.png"
// 若 xxx.png 在 material.fx的子文件夹内                                   #define ALBEDO_MAP_FILE "../xxx.png"
// 若 xxx.png 在 其他目录                                                  #define ALBEDO_MAP_FILE "../other path/xxx.png"
// 若 xxx.png 在磁盘内任意位置                                             #define ALBEDO_MAP_FILE "C:/Users/User Name/Desktop/xxx.png"
#define ALBEDO_MAP_FILE "albedo.png"

// 改变纹理颜色的设置
//  当 ALBEDO_MAP_FROM 为`0`时，或者 ALBEDO_MAP_APPLY_SCALE 为`1`时, 你需要设置一个颜色到albedo，并且色彩范围是在0 ~ 1之间
//  例:
//  1. 如果红色是一个[归一化]后的数值, 则可以设置到albedo为:
//      const float3 albedo = float3(1.0, 0.0, 0.0);
//  2. 如果红色是一个[非归一化]后的数值, 则可以设置到albedo为:
//      const float3 albedo = float3(255, 0.0, 0.0) / 255.0;
//  3. 如果颜色是从显示器中提取的, 则需要将 [sRGB]转换到 [linear color-space]通过 `color ^ gamma`
//      转换一个归一化后的颜色到线性的色彩空间 为:
//      const float3 albedo = pow(float3(r, g, b), 2.2);
//      转换一个未归一化的颜色到线性的色彩空间 为:
//      const float3 albedo = pow(float3(r, g, b) / 255.0, 2.2);
const float3 albedo = 1.0;

//  修改这里的数值可以将图片以瓷砖的形式增加纹理的迭代次数, 默认数值是 1。即 1x1 的瓷砖贴图
//      例如:
//      1. 如果 `X` 和 `Y` 时相同数值时则可以简单的设置同一个数值为:
//      const flaot albedoMapLoopNum = 2;
//      或者
//      const flaot2 albedoMapLoopNum = 2;
//      2. 否者, 左边的数值2代表X轴，右边的数值3代表Y轴
//      const flaot2 albedoMapLoopNum = float2(2, 3);
const float2 albedoMapLoopNum = 1.0;

//----------------------------------------
//	albedo sub 次贴图色
//----------------------------------------
//   通过将不同数值设置到`ALBEDO_SUB_ENABLE`，可以进一步修改模型的颜色
//   0：无效  
//   1.：albedo * albedoSub乘算
//   2：albedo ^ albedoSub指数乘算
//   3：albedo + albedoSub加算
//   4：melanin黑色素  
//   5：Alpha Blend透明贴图混合  
#define ALBEDO_SUB_ENABLE 0

// 指定纹理贴图，具体看来源部分，设置为1是指定图片文件
#define ALBEDO_SUB_MAP_FROM 0

// UV坐标翻转功能
// 1 : X轴水平翻转
// 2 : Y轴垂直翻转
// 3 : 水平+垂直同时反转
#define ALBEDO_SUB_MAP_UV_FLIP 0

// 更改const float3 albedo = 1.0;语句 修改颜色的模式
// 0：无效
// 1：乘算
// 2：指数乘算
#define ALBEDO_SUB_MAP_APPLY_SCALE 0

// 贴图路径
#define ALBEDO_SUB_MAP_FILE "albedo.png"

// 色彩范围（0~1）
const float3 albedoSub = 1.0;

// 纹理迭代次数
const float2 albedoSubMapLoopNum = 1.0;

//----------------------------------------
//	Alpha透明贴图（该项对不透明物体无效）
//----------------------------------------
// 指定纹理贴图，具体看来源部分，设置为1是指定图片文件
#define ALPHA_MAP_FROM 3

// UV坐标翻转功能
// 1 : X轴水平翻转
// 2 : Y轴垂直翻转
// 3 : 水平+垂直同时反转
#define ALPHA_MAP_UV_FLIP 0

// 从纹理不同通道提取数据（R=0 G=1 B=2 A=3）
// 输入不同的数值用于从纹理中不同的通道提取参数需要的数据。
// 0 ： 从 `R` 通道中提取数据    
// 1 ：从 `G` 通道中提取数据    
// 2 ： 从 `B` 通道中提取数据    
// 3 ： 从 `A` 通道中提取数据    
#define ALPHA_MAP_SWIZZLE 3

// 贴图路径
#define ALPHA_MAP_FILE "alpha.png"

// 色彩范围（0~1）
const float alpha = 1.0;

// 纹理迭代次数
const float alphaMapLoopNum = 1.0;

//----------------------------------------
// 法线贴图
// 		法线贴图用于修改模型表面的凹凸以改变光照从而添加更多的阴影细节，默认时将总是使用带有三个通道的切线空间的法帖，同时还能够支持其它的一些类型的贴图。
// 		你可以修改`NORMAL_MAP_TYPE`来改变默认行为，由于计算光源时必须模型具有法线信息，所以所有被渲染的模型都必须具有法线否则模型或者场景边缘上会产生一些白边的现象。
// 		你可以尝试将场景模型放入PMXEditor，然后检查是否所有的法线的XYZ数值是否都不为0
//----------------------------------------
// 指定纹理贴图，具体看来源部分，设置为1是指定图片文件
#define NORMAL_MAP_FROM 0

// 法线贴图类型
// 0：将具有RGB的切线空间的法线贴图用于模型的法线。
// 1：将只有RG的压缩后的切线空间的法线贴图用于模型的法线。 
// 2：以PerturbNormalLQ的方式计算凹凸贴图用作模型的法线，在小的物体上工作的可能不是很好。
// 3：以PerturbNormalHQ的方式计算凹凸贴图用作模型的法线 (High Quality).  
// 4：将RGB的世界空间的法线贴图用于模型的法线。
#define NORMAL_MAP_TYPE 0

// UV坐标翻转功能
// 1 : X轴水平翻转
// 2 : Y轴垂直翻转
// 3 : 水平+垂直同时反转
#define NORMAL_MAP_UV_FLIP 0

// 法线贴图路径
#define NORMAL_MAP_FILE "normal.png"

// 法线强度 ≥ 0
const float normalMapScale = 1.0;

// 纹理迭代次数
const float normalMapLoopNum = 1.0;

//-----------------------------------
// 子法线
//　　子法线主要用于在原始的基本法线上添加而外的细节，将两个法线贴图组合成为一个法线贴图，从而不需要修改原始贴图。
//-----------------------------------
// 指定纹理贴图，具体看来源部分，设置为1是指定图片文件
#define NORMAL_SUB_MAP_FROM 0

// 法线贴图类型
// 0：将具有RGB的切线空间的法线贴图用于模型的法线。
// 1：将只有RG的压缩后的切线空间的法线贴图用于模型的法线。
// 2：以PerturbNormalLQ的方式计算凹凸贴图用作模型的法线. 在小的物体上工作的可能不是很好。
// 3：以PerturbNormalHQ的方式计算凹凸贴图用作模型的法线 (High Quality).  
// 4：将RGB的世界空间的法线贴图用于模型的法线。
#define NORMAL_SUB_MAP_TYPE 0

// UV坐标翻转功能
// 1 : X轴水平翻转
// 2 : Y轴垂直翻转
// 3 : 水平+垂直同时反转
#define NORMAL_SUB_MAP_UV_FLIP 0

// 法线贴图路径
#define NORMAL_SUB_MAP_FILE "normal.png"

// 法线强度 ≥ 0
const float normalSubMapScale = 1.0;

// 纹理迭代次数
const float normalSubMapLoopNum = 1.0;

//----------------------------------------
//	光滑度、粗糙度
// 　　光滑度用于描述模型表面的不均匀性,数值越大时模型反射周围越清晰,并且该帖图总是一个灰度贴图。
// 　　如果输入的贴图是彩色,将会从`R`通道中提取数据,或者可以通过修改`SMOOTHNESS_MAP_SWIZZLE`来指定其它通道。
// 　　默认时光滑度的计算是将PMX模型的高光转换成光滑度来使用的，因此你需要先修改`SMOOTHNESS_MAP_FROM`。
//----------------------------------------
// 指定纹理贴图，噪点等贴图
#define SMOOTHNESS_MAP_FROM 9

// 光滑粗糙类型
// 描述光滑度贴图是使用光滑度还是粗糙度，以及如何转换粗糙度为光滑度
// 0: 光滑	(来自寒霜引擎/CE5)
// 1: 粗糙~光滑 by 1.0-粗糙度 ^ 0.5	(来自 UE4/GGX/SubstancePainter2)
// 2: 粗糙~光滑 by 1.0-粗糙度 	(来自 UE4/GGX/SubstancePainter.2 具有粗糙度线性度)
#define SMOOTHNESS_MAP_TYPE 0

// UV坐标翻转功能
// 1 : X轴水平翻转
// 2 : Y轴垂直翻转
// 3 : 水平+垂直同时反转
#define SMOOTHNESS_MAP_UV_FLIP 0

// 指定不同参数贴图使用的颜色通道（R=0,G=1,B=2,A=3,灰度图不需要指定） 
#define SMOOTHNESS_MAP_SWIZZLE 0

// 改变纹理颜色，详见f1oat3数值设定
// 1：乘算 
// 2：指致乘算
#define SMOOTHNESS_MAP_APPLY_SCALE 0

// 贴图路径
#define SMOOTHNESS_MAP_FILE "smoothness.png"

// 色彩范围(0~1) 类型不同时色彩范围不同
const float smoothness = 0.0;

// 迭代次数
const float smoothnessMapLoopNum = 1.0;

//----------------------------------------
//	金属度
// 金属性仅仅只是修改模型的反射率，用于替代老式的渲染管线，是一个在绝缘体和导体的插值过程，数值越大时其反射色依赖`Albedo`贴图色，并且该帖图总是一个灰度贴图。
// 如果输入的贴图是彩色,将会从`R`通道中提取数据,或者可以通过修改`METALNESS_MAP_SWIZZLE`来指定其它通道。
//----------------------------------------
// 指定纹理贴图，具体看来源部分，设置为1是指定图片文件
#define METALNESS_MAP_FROM 0

// UV坐标翻转功能
// 1 : X轴水平翻转
// 2 : Y轴垂直翻转
// 3 : 水平+垂直同时反转
#define METALNESS_MAP_UV_FLIP 0

// 指定不同参数贴图使用的颜色通道（R=0,G=1,B=2,A=3,灰度图不需要指定） 
#define METALNESS_MAP_SWIZZLE 0

// 改变纹理颜色，详见f1oat3数值设定
// 1：乘算 
// 2：指致乘算
#define METALNESS_MAP_APPLY_SCALE 0

// 贴图路径
#define METALNESS_MAP_FILE "metalness.png"

// 色彩范围(0~1) 
const float metalness = 0.0;

// 迭代次数
const float metalnessMapLoopNum = 1.0;

//----------------------------------------
//	镜面色转换成反射率
// 该选项是对metalness不大于0时提供一个基本反射率，因此该选项不是MMD的高光贴图, 贴图允许支持彩色和灰色，但彩色的RGB贴图不允许和CUSTOM_ENABLE一起使用。
// 不过你可以修改SPECULAR_MAP_TYPE,将其设置成灰度图, 如果你不希望模型反射镜面色，则可以设置数值0到`const float3 specular = 0.0;`
//----------------------------------------
// 指定纹理贴图，具体看来源部分，设置为1是指定图片文件
#define SPECULAR_MAP_FROM 0

// 镜面色转换成反射率类型
// 0：从镜面颜色计算反时系数by F(x)=0.08*(x)（来自UE4）
// 1：从镜面颜色计算反时系数by F(x)=0.16*(x^2)（来自寒霜引擎）
// 2：从镜面灰色计算反时系数by F(x)=0.08*(x)（来自UE4）
// 3：从镜面灰色计算反时系数by F((x)=0.16*(x2)（来自寒霜引擎）
// 4：使用反射系数 (0.04) 代替镜面值 (0.5) 一 镜面图从0点开始时可用
// 提示：高光贴图不是环境贴图，它仅修改模型基本反射率，使改变环境反射颜色，且词反射的光强大于环境反射时该参数贡献很小
// 如果你不期望你的模型反时天空的颜色可设置 const float3 specular=0.0 让模型不具有镜面反射
#define SPECULAR_MAP_TYPE 0

// UV坐标翻转功能
// 1 : X轴水平翻转
// 2 : Y轴垂直翻转
// 3 : 水平+垂直同时反转
#define SPECULAR_MAP_UV_FLIP 0

// 指定不同参数贴图使用的颜色通道（R=0,G=1,B=2,A=3，灰度图不需要指定） 
#define SPECULAR_MAP_SWIZZLE 0

// 改变纹理颜色，详见f1oat3数值设定
// 1：乘算 
// 2：指致乘算
#define SPECULAR_MAP_APPLY_SCALE 0

// 贴图路径
#define SPECULAR_MAP_FILE "specular.png"

// 色彩范围(0~1)  默认值0.5
const float3 specular = 0.5;

// 迭代次数
const float2 specularMapLoopNum = 1.0;

//----------------------------------------
//	环境光遮蔽
// 　　由于天顶光源是由无数方向发射的光线,因此无法实时的计算出环境光的遮蔽,一种简单的方式则是使用`SSAO`或者`环境光遮蔽贴图`来代替。
// 　　而实时中的`SSAO`只能模拟小范围的闭塞，所以可能需要离线烘培出`环境光遮蔽贴图`，此贴图是一种非常近视的手法用于模拟环境光的大范围闭塞。
// 　　所以能够产生更真实的效果，如果你不希望某个物体反射天空中的漫反射以及镜面反射，你可以将该参数设置为0。
//----------------------------------------
// 指定纹理贴图，具体看来源部分，设置为1是指定图片文件
#define OCCLUSION_MAP_FROM 0

// 环境光遮蔽类型
// 0：从sRGB的色彩空间中提取
// 1：从线性的色彩空间中提取
// 2：从sRGB的色彩空间以及使用模型的第二组UV提取
// 3：从线性的色彩空间以及使用模型的第二组UV提取
#define OCCLUSION_MAP_TYPE 0

// UV坐标翻转功能
// 1 : X轴水平翻转
// 2 : Y轴垂直翻转
// 3 : 水平+垂直同时反转
#define OCCLUSION_MAP_UV_FLIP 0

// 指定不同参数贴图使用的颜色通道（R=0,G=1,B=2,A=3，灰度图不需要指定） 
#define OCCLUSION_MAP_SWIZZLE 0

// 改变纹理颜色，详见f1oat3数值设定
// 1：乘算 
// 2：指致乘算
#define OCCLUSION_MAP_APPLY_SCALE 0 

// 贴图路径
#define OCCLUSION_MAP_FILE "occlusion.png"

// 色彩范围(0~1) 
const float occlusion = 1.0;

// 迭代次数
const float occlusionMapLoopNum = 1.0;

//----------------------------------------
//	视差贴图
//  你可以将高度贴图用于此处, 但在DX9中视差贴图无法和顶点位移同时工作
//----------------------------------------
// 指定纹理贴图，具体看来源部分，设置为1是指定图片文件
#define PARALLAX_MAP_FROM 0

// 类型
// 0：计算缺乏透明度
// 1：用透明和最好的SSDO来计算
#define PARALLAX_MAP_TYPE 0

// UV坐标翻转功能
// 1 : X轴水平翻转
// 2 : Y轴垂直翻转
// 3 : 水平+垂直同时反转
#define PARALLAX_MAP_UV_FLIP 0

// 指定不同参数贴图使用的颜色通道（R=0,G=1,B=2,A=3,灰度图不需要指定） 
#define PARALLAX_MAP_SWIZZLE 0

// 贴图路径
#define PARALLAX_MAP_FILE "height.png"

// 必须≥0
const float parallaxMapScale = 1.0;

// 迭代次数
const float parallaxMapLoopNum = 1.0;

//----------------------------------------
// 自发光
//  你可以添加一个光源到场景并且绑定该光源到自发光的材质骨骼，用于制造自发光的照明
//----------------------------------------
// 启动开关
// 0：无效
// 1：启动
#define EMISSIVE_ENABLE 0

// 指定纹理贴图，具体看来源部分，设置为1是指定图片文件
#define EMISSIVE_MAP_FROM 0

// UV坐标翻转功能
// 1 : X轴水平翻转
// 2 : Y轴垂直翻转
// 3 : 水平+垂直同时反转
#define EMISSIVE_MAP_UV_FLIP 0

// 改变纹理颜色，详见f1oat3数值设定
// 1：乘算 
// 2：指致乘算
#define EMISSIVE_MAP_APPLY_SCALE 0

// 乘算到贴图改变颜色
// 表情中有（R+/G+/B+）控制器时
#define EMISSIVE_MAP_APPLY_MORPH_COLOR 0

// 纹理的强度从形态改变（Intensity+/-）控制器
#define EMISSIVE_MAP_APPLY_MORPH_INTENSITY 0

// 闪烁频率
// 0：无效
// 1：设置emissiveBlink
// 自发光详细介绍
// 1：颜色乘以频率，从发射闪烁。例如：const float3 emissiveBlink = float3(1.0,20,3.0):
// 2：颜色乘以变形控制器的频率，请参考 点光源控制器 的闪烁变形
// 控制器设置色彩范围例：=mEmissiveColor + f1loat3(10, 10, 10);
#define EMISSIVE_MAP_APPLY_BLINK 0

// 贴图路径
#define EMISSIVE_MAP_FILE "emissive.png"

// 色彩范围（0~1）自发光颜色
const float3 emissive = 1.0;
// 闪烁色彩范围（0~10）频率
const float3 emissiveBlink = 1.0;
// 自发光强度，越大Bloom越大（0~100）或更高
const float  emissiveIntensity = 1.0;
// 迭代次数
const float2 emissiveMapLoopNum = 1.0;

//----------------------------------------
//	自定义（皮肤等）
//----------------------------------------
// 自定义设置
//     | ID | Material     | CustomA   | CustomB |
//     | :- |:------------|:----------|:--------|
//     | 0  | 默认 | 无效 | 无效 |
//     | 1   | 皮肤 | 曲率 | 散射色 |
//     | 2  | 自发光 | 无效 | 无效 |
//     | 3  | 各向异性 | 各项异性成都 | 切线扰动 |
//     | 4  | 玻璃 | 曲率 | 散射色 |
//     | 5  | 布料 | 光泽度 | 绒毛颜色 |
//     | 6  | 清漆 | 光滑度 | 无效 |
//     | 7  | 次表面（玉器、皮肤） | 曲率 | 散射色 |
//     | 8  | 卡通着色 | 阴影阈值  | 阴影色 |
//     | 9  | 卡通 基于阴影 | 阴影阈值  | 阴影色 |
// Tips:  
//		Subsurface : `曲率`也被称为`不透明度`,定义了材质的模糊强度以及透射强度,更多信息可以查看UE4 [docs]
//		Glass : 为了使材质的折射能够工作，必须让`PMX`的透明度小于`0.999`  
//		Cloth : `光泽度`是一个在`GGX`和`InvGGX`的插值系数,更多信息可以查看[link]
//		Cloth : `毛绒色`是fresnel中f0的参数,定义了材质的基本反射色使用sRGB的色彩  
//		Toon : 更多信息可以查看[link]
#define CUSTOM_ENABLE 0

// 指定贴图，具体看来源部分，设置为1是指定图片文件
#define CUSTOM_A_MAP_FROM 0

// UV坐标翻转功能
// 1 : X轴水平翻转
// 2 : Y轴垂直翻转
// 3 : 水平+垂直同时反转
#define CUSTOM_A_MAP_UV_FLIP 0

// 0：正常
// 1：贴图色使用反相色
#define CUSTOM_A_MAP_COLOR_FLIP 0

// 指定不同参数贴图使用的颜色通道（R=0,G=1,B=2,A=3,灰度图不需要指定） 
#define CUSTOM_A_MAP_SWIZZLE 0

// 改变纹理颜色，详见f1oat3数值设定
// 1：乘算 
// 2：指致乘算
#define CUSTOM_A_MAP_APPLY_SCALE 0

// 贴图路径
#define CUSTOM_A_MAP_FILE "custom.png"

// 工作在线性颜色空间，色彩范围（0~1）
const float customA = 0.0;
// 迭代次数
const float customAMapLoopNum = 1.0;

// 指定贴图
#define CUSTOM_B_MAP_FROM 0
// UV坐标翻转功能
// 1 : X轴水平翻转
// 2 : Y轴垂直翻转
// 3 : 水平+垂直同时反转
#define CUSTOM_B_MAP_UV_FLIP 0
// 0：正常
// 1：贴图色使用反相色
#define CUSTOM_B_MAP_COLOR_FLIP 0
// 改变纹理颜色，详见f1oat3数值设定
// 1：乘算 
// 2：指致乘算
#define CUSTOM_B_MAP_APPLY_SCALE 0
// 贴图路径
#define CUSTOM_B_MAP_FILE "custom.png"

//----------------------------------------
// SSS（次表面散射）
//
// 这种现象是由于材质内部散射光线的作用而形成的，即当光线进入之后在物体内部不断折射而形成的效果。
// 例如蜡，人的皮肤，果冻，拨下皮的简萄种材质
// 是指光线在物体内部的色散而呈现的半送明效果。
//直观例子：在黑暗的环境下把手电筒的光线对准手掌，这时手掌呈半送明状，手掌内的血管隐约可见，这就是3S材质，通常用这种材质来表现蜡烛、玉器和皮肤等半透明的材质。
// 带控制器则为：static const float.3 customB = mCustomBColor+SSS_SKIN_TRANSMITTANCE(O.75);
// #define SSS_SKIN_TRANSMITTANCE(x) exp((1-saturate(x))*float3(-8,-40,-64))
//----------------------------------------

// 工作在线性颜色空间，色彩范围（0~1）
const float3 customB = 0.0;
// 迭代次数
const float2 customBMapLoopNum = 1.0;

// ray材质控制主文件
#include "material_common_2.0.fxsub"