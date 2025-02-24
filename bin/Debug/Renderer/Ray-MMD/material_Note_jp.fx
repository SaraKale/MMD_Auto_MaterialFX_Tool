//============================
//		rayマテリアル翻訳 v1.5.2
//============================

//----------------------------------------
//	albedo 物体テクスチャ色
//----------------------------------------
// 基本色変更部分
// ALBEDO MAP FROM 単一色またはテクスチャを指定。デフォルトではPMXモデルのテクスチャスロットから基本色を取得
// 0：下記の"const float.3 albedo=1.0"にRGB色を設定してモデルの色を設定
// 1：bmp, png, jpg, tga, dds, gif, apng画像パスを使用
// 2：GIF/APNGのパスを使用
// 3：PMXモデルのtexテクスチャ画像を使用
// 4：PMXモデルのspスペキュラ画像を使用
// 5：PMXモデルのToon画像を使用
// 6：AVIビデオまたはレンダリング結果をモデルのテクスチャとして使用。事前にExtension/DummyScreen/内のDummyScreen.Xを配置する必要あり
// 7：PMXの環境色を使用してモデルの色を置換
// 8：PMXのスペキュラ色を使用してモデルの色を置換
// 9：PMXの光沢度を使用してモデルの色を置換（光沢度のみ使用可能）
// ヒント1：ALBEDOは非金属材質のスペキュラ反射を除去した後の物体の基本色を表し、UE4や他のエンジンではベースカラーまたはディフューズカラーとも呼ばれる
// ヒント2：HDR画像は高ダイナミックレンジのリニアカラースペースで動作するため、HDRファイルをALBEDOとして使用するとデータが失われ問題が発生する可能性あり
// ヒント3：一部の(bmp,png,jpg,tga,dds,gif,apng)はsRGB色域で動作しないため問題が発生する可能性あり。ただし、ほとんどの画像はsRGBで動作する
// ヒント4：GIFとAPNGはCPUの負荷が高い
#define ALBEDO_MAP_FROM 3

// UV座標反転機能
// 1 : X軸水平反転
// 2 : Y軸垂直反転
// 3 : 水平+垂直同時反転
#define ALBEDO_MAP_UV_FLIP 0

// const float3 albedo = 1.0;文を変更して色を変更するモード
// 0：無効
// 1 : 乗算
// 2 : 指数乗算
#define ALBEDO_MAP_APPLY_SCALE 0

// PMXモデルのディフューズ色をテクスチャに乗算
#define ALBEDO_MAP_APPLY_DIFFUSE 1

// 表情に(R+/G+/B+)コントローラーがある場合、テクスチャに乗算してモデルの色を変更可能
#define ALBEDO_MAP_APPLY_MORPH_COLOR 0

// テクスチャパス。`ALBEDO_MAP_FROM`に1または2を設定した場合、相対または絶対パスの画像パスをここに入力する必要あり
// ヒント : 親フォルダは "../", すべての "\" を "/" に変更
// xxx.png が material.fx と同じフォルダ内にある場合                                  #define ALBEDO_MAP_FILE "xxx.png"
// xxx.png が material.fx のサブフォルダ内にある場合                                   #define ALBEDO_MAP_FILE "../xxx.png"
// xxx.png が他のディレクトリにある場合                                                  #define ALBEDO_MAP_FILE "../other path/xxx.png"
// xxx.png がディスク内の任意の場所にある場合                                             #define ALBEDO_MAP_FILE "C:/Users/User Name/Desktop/xxx.png"
#define ALBEDO_MAP_FILE "albedo.png"

// テクスチャ色の変更設定
// ALBEDO_MAP_FROMが`0`の場合、またはALBEDO_MAP_APPLY_SCALEが`1`の場合、albedoに色を設定する必要があり、色の範囲は0〜1
// 例:
// 1. 赤が[正規化]された値の場合、albedoに以下のように設定可能:
//      const float3 albedo = float3(1.0, 0.0, 0.0);
// 2. 赤が[非正規化]された値の場合、albedoに以下のように設定可能:
//      const float3 albedo = float3(255, 0.0, 0.0) / 255.0;
// 3. 色がディスプレイから抽出された場合、[sRGB]から[linear color-space]に変換する必要あり。`color ^ gamma`を使用
//      正規化された色をリニアカラースペースに変換する場合:
//      const float3 albedo = pow(float3(r, g, b), 2.2);
//      非正規化された色をリニアカラースペースに変換する場合:
//      const float3 albedo = pow(float3(r, g, b) / 255.0, 2.2);
const float3 albedo = 1.0;

// ここで値を変更すると、テクスチャをタイル状に繰り返すことが可能。デフォルト値は1。つまり1x1のタイルテクスチャ
// 例:
// 1. `X`と`Y`が同じ値の場合、以下のように簡単に設定可能:
//      const float albedoMapLoopNum = 2;
//      または
//      const float2 albedoMapLoopNum = 2;
// 2. それ以外の場合、左の値2はX軸、右の値3はY軸を表す:
//      const float2 albedoMapLoopNum = float2(2, 3);
const float2 albedoMapLoopNum = 1.0;

//----------------------------------------
//	albedo sub サブテクスチャ色
//----------------------------------------
// `ALBEDO_SUB_ENABLE`に異なる値を設定することで、モデルの色をさらに変更可能
// 0：無効  
// 1.：albedo * albedoSub乗算
// 2：albedo ^ albedoSub指数乗算
// 3：albedo + albedoSub加算
// 4：メラニン  
// 5：アルファブレンド  
#define ALBEDO_SUB_ENABLE 0

// テクスチャを指定。詳細はソース部分を参照。1に設定すると画像ファイルを指定
#define ALBEDO_SUB_MAP_FROM 0

// UV座標反転機能
// 1 : X軸水平反転
// 2 : Y軸垂直反転
// 3 : 水平+垂直同時反転
#define ALBEDO_SUB_MAP_UV_FLIP 0

// const float3 albedo = 1.0;文を変更して色を変更するモード
// 0：無効
// 1：乗算
// 2：指数乗算
#define ALBEDO_SUB_MAP_APPLY_SCALE 0

// テクスチャパス
#define ALBEDO_SUB_MAP_FILE "albedo.png"

// 色範囲（0~1）
const float3 albedoSub = 1.0;

// テクスチャ繰り返し回数
const float2 albedoSubMapLoopNum = 1.0;

//----------------------------------------
//	アルファ透明テクスチャ（不透明物体には無効）
//----------------------------------------
// テクスチャを指定。詳細はソース部分を参照。1に設定すると画像ファイルを指定
#define ALPHA_MAP_FROM 3

// UV座標反転機能
// 1 : X軸水平反転
// 2 : Y軸垂直反転
// 3 : 水平+垂直同時反転
#define ALPHA_MAP_UV_FLIP 0

// テクスチャの異なるチャンネルからデータを抽出（R=0 G=1 B=2 A=3）
// 異なる値を入力して、テクスチャの異なるチャンネルから必要なデータを抽出
// 0 ： `R`チャンネルからデータを抽出    
// 1 ：`G`チャンネルからデータを抽出    
// 2 ： `B`チャンネルからデータを抽出    
// 3 ： `A`チャンネルからデータを抽出    
#define ALPHA_MAP_SWIZZLE 3

// テクスチャパス
#define ALPHA_MAP_FILE "alpha.png"

// 色範囲（0~1）
const float alpha = 1.0;

// テクスチャ繰り返し回数
const float alphaMapLoopNum = 1.0;

//----------------------------------------
// 法線マップ
// 法線マップはモデル表面の凹凸を変更し、照明を変更してより多くの影のディテールを追加するために使用される。デフォルトでは、3チャンネルの接空間法線マップを使用し、他のタイプのマップもサポート
// `NORMAL_MAP_TYPE`を変更してデフォルトの動作を変更可能。光源計算時にモデルが法線を持っている必要があるため、すべてのレンダリングされるモデルは法線を持っている必要あり。そうでない場合、モデルやシーンのエッジに白い縁が発生する可能性あり
// PMXEditorにシーンモデルを配置し、すべての法線のXYZ値が0でないか確認することを試みる
//----------------------------------------
// テクスチャを指定。詳細はソース部分を参照。1に設定すると画像ファイルを指定
#define NORMAL_MAP_FROM 0

// 法線マップタイプ
// 0：RGBの接空間法線マップを使用
// 1：RGの圧縮された接空間法線マップを使用
// 2：PerturbNormalLQを使用してバンプマップを計算し、モデルの法線として使用。小さなオブジェクトではうまく動作しない可能性あり
// 3：PerturbNormalHQを使用してバンプマップを計算し、モデルの法線として使用 (高品質)
// 4：RGBのワールド空間法線マップを使用
#define NORMAL_MAP_TYPE 0

// UV座標反転機能
// 1 : X軸水平反転
// 2 : Y軸垂直反転
// 3 : 水平+垂直同時反転
#define NORMAL_MAP_UV_FLIP 0

// 法線マップパス
#define NORMAL_MAP_FILE "normal.png"

// 法線強度 ≥ 0
const float normalMapScale = 1.0;

// テクスチャ繰り返し回数
const float normalMapLoopNum = 1.0;

//-----------------------------------
// サブ法線
// サブ法線は、元の基本法線に追加のディテールを追加するために使用され、2つの法線マップを組み合わせて1つの法線マップとする。これにより、元のテクスチャを変更する必要がなくなる
//-----------------------------------
// テクスチャを指定。詳細はソース部分を参照。1に設定すると画像ファイルを指定
#define NORMAL_SUB_MAP_FROM 0

// 法線マップタイプ
// 0：RGBの接空間法線マップを使用
// 1：RGの圧縮された接空間法線マップを使用
// 2：PerturbNormalLQを使用してバンプマップを計算し、モデルの法線として使用。小さなオブジェクトではうまく動作しない可能性あり
// 3：PerturbNormalHQを使用してバンプマップを計算し、モデルの法線として使用 (高品質)
// 4：RGBのワールド空間法線マップを使用
#define NORMAL_SUB_MAP_TYPE 0

// UV座標反転機能
// 1 : X軸水平反転
// 2 : Y軸垂直反転
// 3 : 水平+垂直同時反転
#define NORMAL_SUB_MAP_UV_FLIP 0

// 法線マップパス
#define NORMAL_SUB_MAP_FILE "normal.png"

// 法線強度 ≥ 0
const float normalSubMapScale = 1.0;

// テクスチャ繰り返し回数
const float normalSubMapLoopNum = 1.0;

//----------------------------------------
//	滑らかさ、粗さ
// 滑らかさはモデル表面の不均一性を表し、値が大きいほどモデルが周囲をより鮮明に反射する。このマップは常にグレースケールマップ
// 入力テクスチャがカラーの場合、`R`チャンネルからデータを抽出するか、`SMOOTHNESS_MAP_SWIZZLE`を変更して他のチャンネルを指定可能
// デフォルトでは、滑らかさの計算はPMXモデルのハイライトを滑らかさに変換して使用するため、`SMOOTHNESS_MAP_FROM`を先に変更する必要あり
//----------------------------------------
// テクスチャを指定。ノイズなどのマップ
#define SMOOTHNESS_MAP_FROM 9

// 滑らかさ/粗さタイプ
// 滑らかさマップが滑らかさまたは粗さを使用するか、および粗さを滑らかさに変換する方法を指定
// 0: 滑らかさ (Frostbiteエンジン/CE5から)
// 1: 粗さ~滑らかさ by 1.0-粗さ ^ 0.5 (UE4/GGX/SubstancePainter2から)
// 2: 粗さ~滑らかさ by 1.0-粗さ (UE4/GGX/SubstancePainter.2 粗さの線形性あり)
#define SMOOTHNESS_MAP_TYPE 0

// UV座標反転機能
// 1 : X軸水平反転
// 2 : Y軸垂直反転
// 3 : 水平+垂直同時反転
#define SMOOTHNESS_MAP_UV_FLIP 0

// 異なるパラメータマップに使用するカラーチャンネルを指定（R=0,G=1,B=2,A=3,グレースケールマップは指定不要）
#define SMOOTHNESS_MAP_SWIZZLE 0

// テクスチャ色を変更。詳細はf1oat3値設定を参照
// 1：乗算 
// 2：指数乗算
#define SMOOTHNESS_MAP_APPLY_SCALE 0

// テクスチャパス
#define SMOOTHNESS_MAP_FILE "smoothness.png"

// 色範囲(0~1) タイプによって色範囲が異なる
const float smoothness = 0.0;

// 繰り返し回数
const float smoothnessMapLoopNum = 1.0;

//----------------------------------------
//	メタリック
// メタリックはモデルの反射率を変更するだけで、古いレンダリングパイプラインを置き換えるために使用される。これは絶縁体と導体の間の補間プロセスであり、値が大きいほど反射色が`Albedo`テクスチャ色に依存する。このマップは常にグレースケールマップ
// 入力テクスチャがカラーの場合、`R`チャンネルからデータを抽出するか、`METALNESS_MAP_SWIZZLE`を変更して他のチャンネルを指定可能
//----------------------------------------
// テクスチャを指定。詳細はソース部分を参照。1に設定すると画像ファイルを指定
#define METALNESS_MAP_FROM 0

// UV座標反転機能
// 1 : X軸水平反転
// 2 : Y軸垂直反転
// 3 : 水平+垂直同時反転
#define METALNESS_MAP_UV_FLIP 0

// 異なるパラメータマップに使用するカラーチャンネルを指定（R=0,G=1,B=2,A=3,グレースケールマップは指定不要）
#define METALNESS_MAP_SWIZZLE 0

// テクスチャ色を変更。詳細はf1oat3値設定を参照
// 1：乗算 
// 2：指数乗算
#define METALNESS_MAP_APPLY_SCALE 0

// テクスチャパス
#define METALNESS_MAP_FILE "metalness.png"

// 色範囲(0~1) 
const float metalness = 0.0;

// 繰り返し回数
const float metalnessMapLoopNum = 1.0;

//----------------------------------------
//	スペキュラを反射率に変換
// このオプションは、metalnessが0以下の場合に基本反射率を提供する。そのため、このオプションはMMDのハイライトマップではない。マップはカラーとグレーの両方をサポートするが、カラーのRGBマップはCUSTOM_ENABLEと一緒に使用できない
// ただし、`SPECULAR_MAP_TYPE`を変更してグレースケールマップに設定可能。モデルがスペキュラ色を反射しないようにしたい場合、`const float3 specular = 0.0;`に0を設定可能
//----------------------------------------
// テクスチャを指定。詳細はソース部分を参照。1に設定すると画像ファイルを指定
#define SPECULAR_MAP_FROM 0

// スペキュラを反射率に変換するタイプ
// 0：スペキュラカラーから反射係数を計算 by F(x)=0.08*(x) (UE4から)
// 1：スペキュラカラーから反射係数を計算 by F(x)=0.16*(x^2) (Frostbiteエンジンから)
// 2：スペキュラグレーから反射係数を計算 by F(x)=0.08*(x) (UE4から)
// 3：スペキュラグレーから反射係数を計算 by F((x)=0.16*(x2) (Frostbiteエンジンから)
// 4：反射係数 (0.04) を使用してスペキュラ値 (0.5) を置換。スペキュラマップが0から始まる場合に使用可能
// ヒント：ハイライトマップは環境マップではなく、モデルの基本反射率を変更し、環境反射色を変更する。環境反射光が強い場合、このパラメータの寄与は小さい
// モデルが空の色を反射しないようにしたい場合、`const float3 specular=0.0`を設定してモデルがスペキュラ反射を持たないようにする
#define SPECULAR_MAP_TYPE 0

// UV座標反転機能
// 1 : X軸水平反転
// 2 : Y軸垂直反転
// 3 : 水平+垂直同時反転
#define SPECULAR_MAP_UV_FLIP 0

// 異なるパラメータマップに使用するカラーチャンネルを指定（R=0,G=1,B=2,A=3,グレースケールマップは指定不要）
#define SPECULAR_MAP_SWIZZLE 0

// テクスチャ色を変更。詳細はf1oat3値設定を参照
// 1：乗算 
// 2：指数乗算
#define SPECULAR_MAP_APPLY_SCALE 0

// テクスチャパス
#define SPECULAR_MAP_FILE "specular.png"

// 色範囲(0~1)  デフォルト値0.5
const float3 specular = 0.5;

// 繰り返し回数
const float2 specularMapLoopNum = 1.0;

//----------------------------------------
//	アンビエントオクルージョン
// 天頂光源は無数の方向から発せられる光線であるため、リアルタイムでアンビエントオクルージョンを計算することはできない。簡単な方法は`SSAO`または`アンビエントオクルージョンマップ`を使用すること
// リアルタイムの`SSAO`は小さな範囲のオクルージョンのみをシミュレートできるため、オフラインで`アンビエントオクルージョンマップ`をベイクする必要がある場合がある。このマップは、アンビエントライトの広範囲のオクルージョンをシミュレートするための非常に近似した手法
// そのため、よりリアルな効果を生み出すことができる。あるオブジェクトが空からのディフューズ反射およびスペキュラ反射を反射しないようにしたい場合、このパラメータを0に設定可能
//----------------------------------------
// テクスチャを指定。詳細はソース部分を参照。1に設定すると画像ファイルを指定
#define OCCLUSION_MAP_FROM 0

// アンビエントオクルージョンタイプ
// 0：sRGBカラースペースから抽出
// 1：リニアカラースペースから抽出
// 2：sRGBカラースペースおよびモデルの第2UVセットを使用
// 3：リニアカラースペースおよびモデルの第2UVセットを使用
#define OCCLUSION_MAP_TYPE 0

// UV座標反転機能
// 1 : X軸水平反転
// 2 : Y軸垂直反転
// 3 : 水平+垂直同時反転
#define OCCLUSION_MAP_UV_FLIP 0

// 異なるパラメータマップに使用するカラーチャンネルを指定（R=0,G=1,B=2,A=3,グレースケールマップは指定不要）
#define OCCLUSION_MAP_SWIZZLE 0

// テクスチャ色を変更。詳細はf1oat3値設定を参照
// 1：乗算 
// 2：指数乗算
#define OCCLUSION_MAP_APPLY_SCALE 0 

// テクスチャパス
#define OCCLUSION_MAP_FILE "occlusion.png"

// 色範囲(0~1) 
const float occlusion = 1.0;

// 繰り返し回数
const float occlusionMapLoopNum = 1.0;

//----------------------------------------
//	パララックスマップ
// 高さマップをここで使用可能。ただし、DX9ではパララックスマップと頂点ディスプレイスメントを同時に使用できない
//----------------------------------------
// テクスチャを指定。詳細はソース部分を参照。1に設定すると画像ファイルを指定
#define PARALLAX_MAP_FROM 0

// タイプ
// 0：透明度なしで計算
// 1：透明度と最高のSSDOを使用して計算
#define PARALLAX_MAP_TYPE 0

// UV座標反転機能
// 1 : X軸水平反転
// 2 : Y軸垂直反転
// 3 : 水平+垂直同時反転
#define PARALLAX_MAP_UV_FLIP 0

// 異なるパラメータマップに使用するカラーチャンネルを指定（R=0,G=1,B=2,A=3,グレースケールマップは指定不要）
#define PARALLAX_MAP_SWIZZLE 0

// テクスチャパス
#define PARALLAX_MAP_FILE "height.png"

// 必須≥0
const float parallaxMapScale = 1.0;

// 繰り返し回数
const float parallaxMapLoopNum = 1.0;

//----------------------------------------
// エミッシブ
// シーンに光源を追加し、その光源をエミッシブマテリアルのボーンにバインドして、エミッシブ照明を作成可能
//----------------------------------------
// 有効化スイッチ
// 0：無効
// 1：有効
#define EMISSIVE_ENABLE 0

// テクスチャを指定。詳細はソース部分を参照。1に設定すると画像ファイルを指定
#define EMISSIVE_MAP_FROM 0

// UV座標反転機能
// 1 : X軸水平反転
// 2 : Y軸垂直反転
// 3 : 水平+垂直同時反転
#define EMISSIVE_MAP_UV_FLIP 0

// テクスチャ色を変更。詳細はf1oat3値設定を参照
// 1：乗算 
// 2：指数乗算
#define EMISSIVE_MAP_APPLY_SCALE 0

// テクスチャに乗算して色を変更
// 表情に（R+/G+/B+）コントローラーがある場合
#define EMISSIVE_MAP_APPLY_MORPH_COLOR 0

// テクスチャの強度をモーフィングで変更（Intensity+/-）コントローラー
#define EMISSIVE_MAP_APPLY_MORPH_INTENSITY 0

// 点滅頻度
// 0：無効
// 1：emissiveBlinkを設定
// エミッシブ詳細説明
// 1：色に頻度を乗算して点滅。例：const float3 emissiveBlink = float3(1.0,20,3.0):
// 2：色にモーフィングコントローラーの頻度を乗算。点光源コントローラーの点滅モーフィングを参照
// コントローラー設定色範囲例：=mEmissiveColor + f1loat3(10, 10, 10);
#define EMISSIVE_MAP_APPLY_BLINK 0

// テクスチャパス
#define EMISSIVE_MAP_FILE "emissive.png"

// 色範囲（0~1）エミッシブ色
const float3 emissive = 1.0;
// 点滅色範囲（0~10）頻度
const float3 emissiveBlink = 1.0;
// エミッシブ強度、大きいほどBloomが大きくなる（0~100）またはそれ以上
const float  emissiveIntensity = 1.0;
// 繰り返し回数
const float2 emissiveMapLoopNum = 1.0;

//----------------------------------------
//	カスタム（スキンなど）
//----------------------------------------
// カスタム設定
//     | ID | マテリアル     | CustomA   | CustomB |
//     | :- |:------------|:----------|:--------|
//     | 0  | デフォルト | 無効 | 無効 |
//     | 1   | スキン | 曲率 | 散乱色 |
//     | 2  | エミッシブ | 無効 | 無効 |
//     | 3  | 異方性 | 異方性度 | 接線乱れ |
//     | 4  | ガラス | 曲率 | 散乱色 |
//     | 5  | 布 | 光沢度 | パイルカラー |
//     | 6  | クリアコート | 滑らかさ | 無効 |
//     | 7  | サブサーフェス（玉、スキン） | 曲率 | 散乱色 |
//     | 8  | トゥーンシェーディング | 影の閾値  | 影色 |
//     | 9  | トゥーン 影ベース | 影の閾値  | 影色 |
// ヒント:  
//		Subsurface : `曲率`は`不透明度`とも呼ばれ、マテリアルのぼかし強度および透過強度を定義。詳細はUE4 [docs]を参照
//		Glass : マテリアルの屈折を動作させるためには、`PMX`の透明度を`0.999`未満にする必要あり  
//		Cloth : `光沢度`は`GGX`と`InvGGX`の間の補間係数。詳細は[link]を参照
//		Cloth : `パイルカラー`はfresnelのf0パラメータで、マテリアルの基本反射色をsRGBカラーで定義
//		Toon : 詳細は[link]を参照
#define CUSTOM_ENABLE 0

// テクスチャを指定。詳細はソース部分を参照。1に設定すると画像ファイルを指定
#define CUSTOM_A_MAP_FROM 0

// UV座標反転機能
// 1 : X軸水平反転
// 2 : Y軸垂直反転
// 3 : 水平+垂直同時反転
#define CUSTOM_A_MAP_UV_FLIP 0

// 0：通常
// 1：テクスチャ色を反転
#define CUSTOM_A_MAP_COLOR_FLIP 0

// 異なるパラメータマップに使用するカラーチャンネルを指定（R=0,G=1,B=2,A=3,グレースケールマップは指定不要）
#define CUSTOM_A_MAP_SWIZZLE 0

// テクスチャ色を変更。詳細はf1oat3値設定を参照
// 1：乗算 
// 2：指数乗算
#define CUSTOM_A_MAP_APPLY_SCALE 0

// テクスチャパス
#define CUSTOM_A_MAP_FILE "custom.png"

// リニアカラースペースで動作。色範囲（0~1）
const float customA = 0.0;
// 繰り返し回数
const float customAMapLoopNum = 1.0;

// テクスチャを指定
#define CUSTOM_B_MAP_FROM 0
// UV座標反転機能
// 1 : X軸水平反転
// 2 : Y軸垂直反転
// 3 : 水平+垂直同時反転
#define CUSTOM_B_MAP_UV_FLIP 0
// 0：通常
// 1：テクスチャ色を反転
#define CUSTOM_B_MAP_COLOR_FLIP 0
// テクスチャ色を変更。詳細はf1oat3値設定を参照
// 1：乗算 
// 2：指数乗算
#define CUSTOM_B_MAP_APPLY_SCALE 0
// テクスチャパス
#define CUSTOM_B_MAP_FILE "custom.png"

//----------------------------------------
// SSS（サブサーフェススキャタリング）
//
// この現象は、マテリアル内部で光線が散乱することによって形成される。例えば、ワックス、人の肌、ゼリー、皮を剥いたブドウなどのマテリアル
// 光線が物体内部で散乱することによって生じる半透明効果を指す。
// 直感的な例：暗い環境で懐中電灯の光を手のひらに向けると、手のひらが半透明になり、手のひら内の血管がうっすらと見える。これが3Sマテリアルで、通常、ろうそく、玉、肌などの半透明マテリアルを表現するために使用される。
// コントローラーがある場合：static const float.3 customB = mCustomBColor+SSS_SKIN_TRANSMITTANCE(O.75);
// #define SSS_SKIN_TRANSMITTANCE(x) exp((1-saturate(x))*float3(-8,-40,-64))
//----------------------------------------

// リニアカラースペースで動作。色範囲（0~1）
const float3 customB = 0.0;
// 繰り返し回数
const float2 customBMapLoopNum = 1.0;

// rayマテリアル制御メインファイル
#include "material_common_2.0.fxsub"