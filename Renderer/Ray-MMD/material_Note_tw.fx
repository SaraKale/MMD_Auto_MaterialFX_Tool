//============================
//		ray材質翻譯 v1.5.2
//============================

//----------------------------------------
//	albedo 物體貼圖色
//----------------------------------------
// 基本顏色修改部分
// ALBEDO MAP FROM 指定單壹色或紋理貼圖默認時從PMX模型中的紋理插槽獲取基本顏色
// 0：設置壹個RGB的顏色到下方的"const float.3 albedo=1.0”來設置模型的顏色
// 1：使用bmp, png, jpg, tga, dds, gif, apng圖片路徑
// 2：使用GIF/APNG的路徑
// 3：使用PMX模型中tex紋理圖片
// 4：使用PMX模型中sp鏡面圖片
// 5：使用PMX模型中Toon圖片
// 6：將AVI視頻或渲染後結果作模型貼圖紋理，需先放置Extension/DummyScreen/中DummyScreen.X
// 7：將PMX中的環境色用於替換模型的顏色
// 8：將PMX中的鏡面色用於替換模型的顏色
// 9：將PMX中的光澤度用於替換模型的顏色（只能被光滑度使用）
// 提示1：ALBEDO是描述物體在消除了非金屬材質的鏡面反射後的基本顏色，在UE4或者其它引擎中同樣也被稱為底色或者固有色
// 提示2：HDR圖片是工作在高動態的線性色彩空間中，所以不要將壹個HDR文件用作ALBEDO,否則會丟失數據產生壹些問題
// 提示3：壹些(bmp,png,jpg,tga,dds,gif,apng)可能不是工作在sRGB的色域中的會產生壹些問題，不過大部分圖片都是工作在sRGB
// 提示4：GIF和APNG是占用CPU的開銷
#define ALBEDO_MAP_FROM 3

// UV坐標翻轉功能
// 1 : X軸水平翻轉
// 2 : Y軸垂直翻轉
// 3 : 水平+垂直同時反轉
#define ALBEDO_MAP_UV_FLIP 0

// 更改const float3 albedo = 1.0;語句 修改顏色的模式
// 0：無效
// 1 : 乘算
// 2 : 指數乘算
#define ALBEDO_MAP_APPLY_SCALE 0

// 從PMX模型中的擴散色乘算到貼圖上。
#define ALBEDO_MAP_APPLY_DIFFUSE 1

//  如果表情中有(R+/G+/B+)控製器，則可以用來乘算到貼圖上修改模型顏色
#define ALBEDO_MAP_APPLY_MORPH_COLOR 0

//  貼圖路徑，如果設置了1或者2到`ALBEDO_MAP_FROM`時,需要將壹個相對或者絕對的圖片的路徑輸入到這裏
// Tips : 父文件夾是 "../",  改變所有的 "\" to "/".
// 若 xxx.png  material.fx 在統壹文件夾內                                  #define ALBEDO_MAP_FILE "xxx.png"
// 若 xxx.png 在 material.fx的子文件夾內                                   #define ALBEDO_MAP_FILE "../xxx.png"
// 若 xxx.png 在 其他目錄                                                  #define ALBEDO_MAP_FILE "../other path/xxx.png"
// 若 xxx.png 在磁盤內任意位置                                             #define ALBEDO_MAP_FILE "C:/Users/User Name/Desktop/xxx.png"
#define ALBEDO_MAP_FILE "albedo.png"

// 改變紋理顏色的設置
//  當 ALBEDO_MAP_FROM 為`0`時，或者 ALBEDO_MAP_APPLY_SCALE 為`1`時, 妳需要設置壹個顏色到albedo，併且色彩範圍是在0 ~ 1之間
//  例:
//  1. 如果紅色是壹個[歸壹化]後的數值, 則可以設置到albedo為:
//      const float3 albedo = float3(1.0, 0.0, 0.0);
//  2. 如果紅色是壹個[非歸壹化]後的數值, 則可以設置到albedo為:
//      const float3 albedo = float3(255, 0.0, 0.0) / 255.0;
//  3. 如果顏色是從顯示器中提取的, 則需要將 [sRGB]轉換到 [linear color-space]通過 `color ^ gamma`
//      轉換壹個歸壹化後的顏色到線性的色彩空間 為:
//      const float3 albedo = pow(float3(r, g, b), 2.2);
//      轉換壹個未歸壹化的顏色到線性的色彩空間 為:
//      const float3 albedo = pow(float3(r, g, b) / 255.0, 2.2);
const float3 albedo = 1.0;

//  修改這裏的數值可以將圖片以瓷磚的形式增加紋理的疊代次數, 默認數值是 1。即 1x1 的瓷磚貼圖
//      例如:
//      1. 如果 `X` 和 `Y` 時相同數值時則可以簡單的設置同壹個數值為:
//      const flaot albedoMapLoopNum = 2;
//      或者
//      const flaot2 albedoMapLoopNum = 2;
//      2. 否者, 左邊的數值2代表X軸，右邊的數值3代表Y軸
//      const flaot2 albedoMapLoopNum = float2(2, 3);
const float2 albedoMapLoopNum = 1.0;

//----------------------------------------
//	albedo sub 次貼圖色
//----------------------------------------
//   通過將不同數值設置到`ALBEDO_SUB_ENABLE`，可以進壹步修改模型的顏色
//   0：無效  
//   1.：albedo * albedoSub乘算
//   2：albedo ^ albedoSub指數乘算
//   3：albedo + albedoSub加算
//   4：melanin黑色素  
//   5：Alpha Blend透明貼圖混合  
#define ALBEDO_SUB_ENABLE 0

// 指定紋理貼圖，具體看來源部分，設置為1是指定圖片文件
#define ALBEDO_SUB_MAP_FROM 0

// UV坐標翻轉功能
// 1 : X軸水平翻轉
// 2 : Y軸垂直翻轉
// 3 : 水平+垂直同時反轉
#define ALBEDO_SUB_MAP_UV_FLIP 0

// 更改const float3 albedo = 1.0;語句 修改顏色的模式
// 0：無效
// 1：乘算
// 2：指數乘算
#define ALBEDO_SUB_MAP_APPLY_SCALE 0

// 貼圖路徑
#define ALBEDO_SUB_MAP_FILE "albedo.png"

// 色彩範圍（0~1）
const float3 albedoSub = 1.0;

// 紋理疊代次數
const float2 albedoSubMapLoopNum = 1.0;

//----------------------------------------
//	Alpha透明貼圖（該項對不透明物體無效）
//----------------------------------------
// 指定紋理貼圖，具體看來源部分，設置為1是指定圖片文件
#define ALPHA_MAP_FROM 3

// UV坐標翻轉功能
// 1 : X軸水平翻轉
// 2 : Y軸垂直翻轉
// 3 : 水平+垂直同時反轉
#define ALPHA_MAP_UV_FLIP 0

// 從紋理不同通道提取數據（R=0 G=1 B=2 A=3）
// 輸入不同的數值用於從紋理中不同的通道提取參數需要的數據。
// 0 ： 從 `R` 通道中提取數據    
// 1 ：從 `G` 通道中提取數據    
// 2 ： 從 `B` 通道中提取數據    
// 3 ： 從 `A` 通道中提取數據    
#define ALPHA_MAP_SWIZZLE 3

// 貼圖路徑
#define ALPHA_MAP_FILE "alpha.png"

// 色彩範圍（0~1）
const float alpha = 1.0;

// 紋理疊代次數
const float alphaMapLoopNum = 1.0;

//----------------------------------------
// 法線貼圖
// 		法線貼圖用於修改模型表面的凹凸以改變光照從而添加更多的陰影細節，默認時將總是使用帶有三個通道的切線空間的法帖，同時還能夠支持其它的壹些類型的貼圖。
// 		妳可以修改`NORMAL_MAP_TYPE`來改變默認行為，由於計算光源時必須模型具有法線信息，所以所有被渲染的模型都必須具有法線否則模型或者場景邊緣上會產生壹些白邊的現象。
// 		妳可以嘗試將場景模型放入PMXEditor，然後檢查是否所有的法線的XYZ數值是否都不為0
//----------------------------------------
// 指定紋理貼圖，具體看來源部分，設置為1是指定圖片文件
#define NORMAL_MAP_FROM 0

// 法線貼圖類型
// 0：將具有RGB的切線空間的法線貼圖用於模型的法線。
// 1：將只有RG的壓縮後的切線空間的法線貼圖用於模型的法線。 
// 2：以PerturbNormalLQ的方式計算凹凸貼圖用作模型的法線，在小的物體上工作的可能不是很好。
// 3：以PerturbNormalHQ的方式計算凹凸貼圖用作模型的法線 (High Quality).  
// 4：將RGB的世界空間的法線貼圖用於模型的法線。
#define NORMAL_MAP_TYPE 0

// UV坐標翻轉功能
// 1 : X軸水平翻轉
// 2 : Y軸垂直翻轉
// 3 : 水平+垂直同時反轉
#define NORMAL_MAP_UV_FLIP 0

// 法線貼圖路徑
#define NORMAL_MAP_FILE "normal.png"

// 法線強度 ≥ 0
const float normalMapScale = 1.0;

// 紋理疊代次數
const float normalMapLoopNum = 1.0;

//-----------------------------------
// 子法線
//　　子法線主要用於在原始的基本法線上添加而外的細節，將兩個法線貼圖組合成為壹個法線貼圖，從而不需要修改原始貼圖。
//-----------------------------------
// 指定紋理貼圖，具體看來源部分，設置為1是指定圖片文件
#define NORMAL_SUB_MAP_FROM 0

// 法線貼圖類型
// 0：將具有RGB的切線空間的法線貼圖用於模型的法線。
// 1：將只有RG的壓縮後的切線空間的法線貼圖用於模型的法線。
// 2：以PerturbNormalLQ的方式計算凹凸貼圖用作模型的法線. 在小的物體上工作的可能不是很好。
// 3：以PerturbNormalHQ的方式計算凹凸貼圖用作模型的法線 (High Quality).  
// 4：將RGB的世界空間的法線貼圖用於模型的法線。
#define NORMAL_SUB_MAP_TYPE 0

// UV坐標翻轉功能
// 1 : X軸水平翻轉
// 2 : Y軸垂直翻轉
// 3 : 水平+垂直同時反轉
#define NORMAL_SUB_MAP_UV_FLIP 0

// 法線貼圖路徑
#define NORMAL_SUB_MAP_FILE "normal.png"

// 法線強度 ≥ 0
const float normalSubMapScale = 1.0;

// 紋理疊代次數
const float normalSubMapLoopNum = 1.0;

//----------------------------------------
//	光滑度、粗糙度
// 　　光滑度用於描述模型表面的不均勻性,數值越大時模型反射周圍越清晰,併且該帖圖總是壹個灰度貼圖。
// 　　如果輸入的貼圖是彩色,將會從`R`通道中提取數據,或者可以通過修改`SMOOTHNESS_MAP_SWIZZLE`來指定其它通道。
// 　　默認時光滑度的計算是將PMX模型的高光轉換成光滑度來使用的，因此妳需要先修改`SMOOTHNESS_MAP_FROM`。
//----------------------------------------
// 指定紋理貼圖，噪點等貼圖
#define SMOOTHNESS_MAP_FROM 9

// 光滑粗糙類型
// 描述光滑度貼圖是使用光滑度還是粗糙度，以及如何轉換粗糙度為光滑度
// 0: 光滑	(來自寒霜引擎/CE5)
// 1: 粗糙~光滑 by 1.0-粗糙度 ^ 0.5	(來自 UE4/GGX/SubstancePainter2)
// 2: 粗糙~光滑 by 1.0-粗糙度 	(來自 UE4/GGX/SubstancePainter.2 具有粗糙度線性度)
#define SMOOTHNESS_MAP_TYPE 0

// UV坐標翻轉功能
// 1 : X軸水平翻轉
// 2 : Y軸垂直翻轉
// 3 : 水平+垂直同時反轉
#define SMOOTHNESS_MAP_UV_FLIP 0

// 指定不同參數貼圖使用的顏色通道（R=0,G=1,B=2,A=3,灰度圖不需要指定） 
#define SMOOTHNESS_MAP_SWIZZLE 0

// 改變紋理顏色，詳見f1oat3數值設定
// 1：乘算 
// 2：指致乘算
#define SMOOTHNESS_MAP_APPLY_SCALE 0

// 貼圖路徑
#define SMOOTHNESS_MAP_FILE "smoothness.png"

// 色彩範圍(0~1) 類型不同時色彩範圍不同
const float smoothness = 0.0;

// 疊代次數
const float smoothnessMapLoopNum = 1.0;

//----------------------------------------
//	金屬度
// 金屬性僅僅只是修改模型的反射率，用於替代老式的渲染管線，是壹個在絕緣體和導體的插值過程，數值越大時其反射色依賴`Albedo`貼圖色，併且該帖圖總是壹個灰度貼圖。
// 如果輸入的貼圖是彩色,將會從`R`通道中提取數據,或者可以通過修改`METALNESS_MAP_SWIZZLE`來指定其它通道。
//----------------------------------------
// 指定紋理貼圖，具體看來源部分，設置為1是指定圖片文件
#define METALNESS_MAP_FROM 0

// UV坐標翻轉功能
// 1 : X軸水平翻轉
// 2 : Y軸垂直翻轉
// 3 : 水平+垂直同時反轉
#define METALNESS_MAP_UV_FLIP 0

// 指定不同參數貼圖使用的顏色通道（R=0,G=1,B=2,A=3,灰度圖不需要指定） 
#define METALNESS_MAP_SWIZZLE 0

// 改變紋理顏色，詳見f1oat3數值設定
// 1：乘算 
// 2：指致乘算
#define METALNESS_MAP_APPLY_SCALE 0

// 貼圖路徑
#define METALNESS_MAP_FILE "metalness.png"

// 色彩範圍(0~1) 
const float metalness = 0.0;

// 疊代次數
const float metalnessMapLoopNum = 1.0;

//----------------------------------------
//	鏡面色轉換成反射率
// 該選項是對metalness不大於0時提供壹個基本反射率，因此該選項不是MMD的高光貼圖, 貼圖允許支持彩色和灰色，但彩色的RGB貼圖不允許和CUSTOM_ENABLE壹起使用。
// 不過妳可以修改SPECULAR_MAP_TYPE,將其設置成灰度圖, 如果妳不希望模型反射鏡面色，則可以設置數值0到`const float3 specular = 0.0;`
//----------------------------------------
// 指定紋理貼圖，具體看來源部分，設置為1是指定圖片文件
#define SPECULAR_MAP_FROM 0

// 鏡面色轉換成反射率類型
// 0：從鏡面顏色計算反時系數by F(x)=0.08*(x)（來自UE4）
// 1：從鏡面顏色計算反時系數by F(x)=0.16*(x^2)（來自寒霜引擎）
// 2：從鏡面灰色計算反時系數by F(x)=0.08*(x)（來自UE4）
// 3：從鏡面灰色計算反時系數by F((x)=0.16*(x2)（來自寒霜引擎）
// 4：使用反射系數 (0.04) 代替鏡面值 (0.5) 壹 鏡面圖從0點開始時可用
// 提示：高光貼圖不是環境貼圖，它僅修改模型基本反射率，使改變環境反射顏色，且詞反射的光強大於環境反射時該參數貢獻很小
// 如果妳不期望妳的模型反時天空的顏色可設置 const float3 specular=0.0 讓模型不具有鏡面反射
#define SPECULAR_MAP_TYPE 0

// UV坐標翻轉功能
// 1 : X軸水平翻轉
// 2 : Y軸垂直翻轉
// 3 : 水平+垂直同時反轉
#define SPECULAR_MAP_UV_FLIP 0

// 指定不同參數貼圖使用的顏色通道（R=0,G=1,B=2,A=3，灰度圖不需要指定） 
#define SPECULAR_MAP_SWIZZLE 0

// 改變紋理顏色，詳見f1oat3數值設定
// 1：乘算 
// 2：指致乘算
#define SPECULAR_MAP_APPLY_SCALE 0

// 貼圖路徑
#define SPECULAR_MAP_FILE "specular.png"

// 色彩範圍(0~1)  默認值0.5
const float3 specular = 0.5;

// 疊代次數
const float2 specularMapLoopNum = 1.0;

//----------------------------------------
//	環境光遮蔽
// 　　由於天頂光源是由無數方向發射的光線,因此無法實時的計算出環境光的遮蔽,壹種簡單的方式則是使用`SSAO`或者`環境光遮蔽貼圖`來代替。
// 　　而實時中的`SSAO`只能模擬小範圍的閉塞，所以可能需要離線烘培出`環境光遮蔽貼圖`，此貼圖是壹種非常近視的手法用於模擬環境光的大範圍閉塞。
// 　　所以能夠產生更真實的效果，如果妳不希望某個物體反射天空中的漫反射以及鏡面反射，妳可以將該參數設置為0。
//----------------------------------------
// 指定紋理貼圖，具體看來源部分，設置為1是指定圖片文件
#define OCCLUSION_MAP_FROM 0

// 環境光遮蔽類型
// 0：從sRGB的色彩空間中提取
// 1：從線性的色彩空間中提取
// 2：從sRGB的色彩空間以及使用模型的第二組UV提取
// 3：從線性的色彩空間以及使用模型的第二組UV提取
#define OCCLUSION_MAP_TYPE 0

// UV坐標翻轉功能
// 1 : X軸水平翻轉
// 2 : Y軸垂直翻轉
// 3 : 水平+垂直同時反轉
#define OCCLUSION_MAP_UV_FLIP 0

// 指定不同參數貼圖使用的顏色通道（R=0,G=1,B=2,A=3，灰度圖不需要指定） 
#define OCCLUSION_MAP_SWIZZLE 0

// 改變紋理顏色，詳見f1oat3數值設定
// 1：乘算 
// 2：指致乘算
#define OCCLUSION_MAP_APPLY_SCALE 0 

// 貼圖路徑
#define OCCLUSION_MAP_FILE "occlusion.png"

// 色彩範圍(0~1) 
const float occlusion = 1.0;

// 疊代次數
const float occlusionMapLoopNum = 1.0;

//----------------------------------------
//	視差貼圖
//  妳可以將高度貼圖用於此處, 但在DX9中視差貼圖無法和頂點位移同時工作
//----------------------------------------
// 指定紋理貼圖，具體看來源部分，設置為1是指定圖片文件
#define PARALLAX_MAP_FROM 0

// 類型
// 0：計算缺乏透明度
// 1：用透明和最好的SSDO來計算
#define PARALLAX_MAP_TYPE 0

// UV坐標翻轉功能
// 1 : X軸水平翻轉
// 2 : Y軸垂直翻轉
// 3 : 水平+垂直同時反轉
#define PARALLAX_MAP_UV_FLIP 0

// 指定不同參數貼圖使用的顏色通道（R=0,G=1,B=2,A=3,灰度圖不需要指定） 
#define PARALLAX_MAP_SWIZZLE 0

// 貼圖路徑
#define PARALLAX_MAP_FILE "height.png"

// 必須≥0
const float parallaxMapScale = 1.0;

// 疊代次數
const float parallaxMapLoopNum = 1.0;

//----------------------------------------
// 自發光
//  妳可以添加壹個光源到場景併且綁定該光源到自發光的材質骨骼，用於製造自發光的照明
//----------------------------------------
// 啟動開關
// 0：無效
// 1：啟動
#define EMISSIVE_ENABLE 0

// 指定紋理貼圖，具體看來源部分，設置為1是指定圖片文件
#define EMISSIVE_MAP_FROM 0

// UV坐標翻轉功能
// 1 : X軸水平翻轉
// 2 : Y軸垂直翻轉
// 3 : 水平+垂直同時反轉
#define EMISSIVE_MAP_UV_FLIP 0

// 改變紋理顏色，詳見f1oat3數值設定
// 1：乘算 
// 2：指致乘算
#define EMISSIVE_MAP_APPLY_SCALE 0

// 乘算到貼圖改變顏色
// 表情中有（R+/G+/B+）控製器時
#define EMISSIVE_MAP_APPLY_MORPH_COLOR 0

// 紋理的強度從形態改變（Intensity+/-）控製器
#define EMISSIVE_MAP_APPLY_MORPH_INTENSITY 0

// 閃爍頻率
// 0：無效
// 1：設置emissiveBlink
// 自發光詳細介紹
// 1：顏色乘以頻率，從發射閃爍。例如：const float3 emissiveBlink = float3(1.0,20,3.0):
// 2：顏色乘以變形控製器的頻率，請參考 點光源控製器 的閃爍變形
// 控製器設置色彩範圍例：=mEmissiveColor + f1loat3(10, 10, 10);
#define EMISSIVE_MAP_APPLY_BLINK 0

// 貼圖路徑
#define EMISSIVE_MAP_FILE "emissive.png"

// 色彩範圍（0~1）自發光顏色
const float3 emissive = 1.0;
// 閃爍色彩範圍（0~10）頻率
const float3 emissiveBlink = 1.0;
// 自發光強度，越大Bloom越大（0~100）或更高
const float  emissiveIntensity = 1.0;
// 疊代次數
const float2 emissiveMapLoopNum = 1.0;

//----------------------------------------
//	自定義（皮膚等）
//----------------------------------------
// 自定義設置
//     | ID | Material     | CustomA   | CustomB |
//     | :- |:------------|:----------|:--------|
//     | 0  | 默認 | 無效 | 無效 |
//     | 1   | 皮膚 | 曲率 | 散射色 |
//     | 2  | 自發光 | 無效 | 無效 |
//     | 3  | 各向異性 | 各項異性成都 | 切線擾動 |
//     | 4  | 玻璃 | 曲率 | 散射色 |
//     | 5  | 布料 | 光澤度 | 絨毛顏色 |
//     | 6  | 清漆 | 光滑度 | 無效 |
//     | 7  | 次表面（玉器、皮膚） | 曲率 | 散射色 |
//     | 8  | 卡通着色 | 陰影閾值  | 陰影色 |
//     | 9  | 卡通 基於陰影 | 陰影閾值  | 陰影色 |
// Tips:  
//		Subsurface : `曲率`也被稱為`不透明度`,定義了材質的模糊強度以及透射強度,更多信息可以查看UE4 [docs]
//		Glass : 為了使材質的折射能夠工作，必須讓`PMX`的透明度小於`0.999`  
//		Cloth : `光澤度`是壹個在`GGX`和`InvGGX`的插值系數,更多信息可以查看[link]
//		Cloth : `毛絨色`是fresnel中f0的參數,定義了材質的基本反射色使用sRGB的色彩  
//		Toon : 更多信息可以查看[link]
#define CUSTOM_ENABLE 0

// 指定貼圖，具體看來源部分，設置為1是指定圖片文件
#define CUSTOM_A_MAP_FROM 0

// UV坐標翻轉功能
// 1 : X軸水平翻轉
// 2 : Y軸垂直翻轉
// 3 : 水平+垂直同時反轉
#define CUSTOM_A_MAP_UV_FLIP 0

// 0：正常
// 1：貼圖色使用反相色
#define CUSTOM_A_MAP_COLOR_FLIP 0

// 指定不同參數貼圖使用的顏色通道（R=0,G=1,B=2,A=3,灰度圖不需要指定） 
#define CUSTOM_A_MAP_SWIZZLE 0

// 改變紋理顏色，詳見f1oat3數值設定
// 1：乘算 
// 2：指致乘算
#define CUSTOM_A_MAP_APPLY_SCALE 0

// 貼圖路徑
#define CUSTOM_A_MAP_FILE "custom.png"

// 工作在線性顏色空間，色彩範圍（0~1）
const float customA = 0.0;
// 疊代次數
const float customAMapLoopNum = 1.0;

// 指定貼圖
#define CUSTOM_B_MAP_FROM 0
// UV坐標翻轉功能
// 1 : X軸水平翻轉
// 2 : Y軸垂直翻轉
// 3 : 水平+垂直同時反轉
#define CUSTOM_B_MAP_UV_FLIP 0
// 0：正常
// 1：貼圖色使用反相色
#define CUSTOM_B_MAP_COLOR_FLIP 0
// 改變紋理顏色，詳見f1oat3數值設定
// 1：乘算 
// 2：指致乘算
#define CUSTOM_B_MAP_APPLY_SCALE 0
// 貼圖路徑
#define CUSTOM_B_MAP_FILE "custom.png"

//----------------------------------------
// SSS（次表面散射）
//
// 這種現象是由於材質內部散射光線的作用而形成的，即當光線進入之後在物體內部不斷折射而形成的效果。
// 例如蠟，人的皮膚，果凍，撥下皮的簡萄種材質
// 是指光線在物體內部的色散而呈現的半送明效果。
//直觀例子：在黑暗的環境下把手電筒的光線對準手掌，這時手掌呈半送明狀，手掌內的血管隱約可見，這就是3S材質，通常用這種材質來表現蠟燭、玉器和皮膚等半透明的材質。
// 帶控製器則為：static const float.3 customB = mCustomBColor+SSS_SKIN_TRANSMITTANCE(O.75);
// #define SSS_SKIN_TRANSMITTANCE(x) exp((1-saturate(x))*float3(-8,-40,-64))
//----------------------------------------

// 工作在線性顏色空間，色彩範圍（0~1）
const float3 customB = 0.0;
// 疊代次數
const float2 customBMapLoopNum = 1.0;

// ray材質控製主文件
#include "material_common_2.0.fxsub"