//============================
//      ray Material Translation v1.5.2
//============================

//----------------------------------------
//      albedo Object Texture Color
//----------------------------------------
// Basic color modification section
// ALBEDO MAP FROM specifies a single color or texture map. By default, it retrieves the base color from the texture slot in the PMX model.
// 0: Set an RGB color in the "const float.3 albedo=1.0" below to define the model's color.
// 1: Use bmp, png, jpg, tga, dds, gif, apng image paths.
// 2: Use GIF/APNG paths.
// 3: Use the texture image from the PMX model's tex.
// 4: Use the specular image from the PMX model's sp.
// 5: Use the Toon image from the PMX model.
// 6: Use AVI video or rendered results as the model's texture. Place DummyScreen.X in Extension/DummyScreen/ first.
// 7: Use the PMX model's ambient color to replace the model's color.
// 8: Use the PMX model's specular color to replace the model's color.
// 9: Use the PMX model's glossiness to replace the model's color (only usable for smoothness).
// Tip 1: ALBEDO describes the base color of an object after eliminating non-metallic specular reflections. In UE4 or other engines, it is also referred to as base color or inherent color.
// Tip 2: HDR images operate in a high dynamic range linear color space, so do not use an HDR file for ALBEDO, as it may cause data loss and issues.
// Tip 3: Some (bmp, png, jpg, tga, dds, gif, apng) may not operate in the sRGB color space, causing issues. However, most images operate in sRGB.
// Tip 4: GIF and APNG consume CPU resources.
#define ALBEDO_MAP_FROM 3

// UV coordinate flipping function
// 1: Flip horizontally on the X-axis
// 2: Flip vertically on the Y-axis
// 3: Flip both horizontally and vertically
#define ALBEDO_MAP_UV_FLIP 0

// Modify the color mode in the "const float3 albedo = 1.0;" statement
// 0: Disabled
// 1: Multiply
// 2: Exponential multiply
#define ALBEDO_MAP_APPLY_SCALE 0

// Multiply the diffuse color from the PMX model onto the texture.
#define ALBEDO_MAP_APPLY_DIFFUSE 1

// If there are (R+/G+/B+) controllers in the expression, they can be used to multiply onto the texture to modify the model's color.
#define ALBEDO_MAP_APPLY_MORPH_COLOR 0

// Texture path. If 1 or 2 is set in `ALBEDO_MAP_FROM`, input a relative or absolute image path here.
// Tips: Parent folder is "../", change all "\" to "/".
// If xxx.png and material.fx are in the same folder: #define ALBEDO_MAP_FILE "xxx.png"
// If xxx.png is in a subfolder of material.fx: #define ALBEDO_MAP_FILE "../xxx.png"
// If xxx.png is in another directory: #define ALBEDO_MAP_FILE "../other path/xxx.png"
// If xxx.png is anywhere on the disk: #define ALBEDO_MAP_FILE "C:/Users/User Name/Desktop/xxx.png"
#define ALBEDO_MAP_FILE "albedo.png"

// Change texture color settings
// When ALBEDO_MAP_FROM is `0` or ALBEDO_MAP_APPLY_SCALE is `1`, you need to set a color to albedo, with the color range between 0 and 1.
// Example:
// 1. If red is a [normalized] value, set albedo to:
//      const float3 albedo = float3(1.0, 0.0, 0.0);
// 2. If red is a [non-normalized] value, set albedo to:
//      const float3 albedo = float3(255, 0.0, 0.0) / 255.0;
// 3. If the color is extracted from a display, convert [sRGB] to [linear color-space] using `color ^ gamma`.
//      Convert a normalized color to linear color space:
//      const float3 albedo = pow(float3(r, g, b), 2.2);
//      Convert a non-normalized color to linear color space:
//      const float3 albedo = pow(float3(r, g, b) / 255.0, 2.2);
const float3 albedo = 1.0;

// Modify this value to tile the texture, increasing the number of iterations. The default value is 1, meaning a 1x1 tile texture.
// Example:
// 1. If `X` and `Y` are the same, simply set the same value:
//      const float albedoMapLoopNum = 2;
//      or
//      const float2 albedoMapLoopNum = 2;
// 2. Otherwise, the left value represents the X-axis, and the right value represents the Y-axis:
//      const float2 albedoMapLoopNum = float2(2, 3);
const float2 albedoMapLoopNum = 1.0;

//----------------------------------------
//      albedo sub Secondary Texture Color
//----------------------------------------
// Further modify the model's color by setting different values in `ALBEDO_SUB_ENABLE`.
// 0: Disabled
// 1: albedo * albedoSub multiply
// 2: albedo ^ albedoSub exponential multiply
// 3: albedo + albedoSub add
// 4: melanin
// 5: Alpha Blend transparent texture blend
#define ALBEDO_SUB_ENABLE 0

// Specify texture map. See the source section for details. Set to 1 to specify an image file.
#define ALBEDO_SUB_MAP_FROM 0

// UV coordinate flipping function
// 1: Flip horizontally on the X-axis
// 2: Flip vertically on the Y-axis
// 3: Flip both horizontally and vertically
#define ALBEDO_SUB_MAP_UV_FLIP 0

// Modify the color mode in the "const float3 albedo = 1.0;" statement
// 0: Disabled
// 1: Multiply
// 2: Exponential multiply
#define ALBEDO_SUB_MAP_APPLY_SCALE 0

// Texture path
#define ALBEDO_SUB_MAP_FILE "albedo.png"

// Color range (0~1)
const float3 albedoSub = 1.0;

// Texture iteration count
const float2 albedoSubMapLoopNum = 1.0;

//----------------------------------------
//      Alpha Transparency Map (Invalid for opaque objects)
//----------------------------------------
// Specify texture map. See the source section for details. Set to 1 to specify an image file.
#define ALPHA_MAP_FROM 3

// UV coordinate flipping function
// 1: Flip horizontally on the X-axis
// 2: Flip vertically on the Y-axis
// 3: Flip both horizontally and vertically
#define ALPHA_MAP_UV_FLIP 0

// Extract data from different channels of the texture (R=0, G=1, B=2, A=3)
// Input different values to extract data from different channels of the texture.
// 0: Extract data from the `R` channel
// 1: Extract data from the `G` channel
// 2: Extract data from the `B` channel
// 3: Extract data from the `A` channel
#define ALPHA_MAP_SWIZZLE 3

// Texture path
#define ALPHA_MAP_FILE "alpha.png"

// Color range (0~1)
const float alpha = 1.0;

// Texture iteration count
const float alphaMapLoopNum = 1.0;

//----------------------------------------
//      Normal Map
//      Normal maps modify the surface凹凸 of the model to change lighting and add more shadow details. By default, it uses tangent-space normal maps with three channels, but it also supports other types of maps.
//      You can modify `NORMAL_MAP_TYPE` to change the default behavior. Since models must have normal information to calculate lighting, all rendered models must have normals; otherwise, white edges may appear on the model or scene edges.
//      You can try placing the scene model in PMXEditor and check if all normals' XYZ values are not zero.
//----------------------------------------
// Specify texture map. See the source section for details. Set to 1 to specify an image file.
#define NORMAL_MAP_FROM 0

// Normal map type
// 0: Use RGB tangent-space normal maps for the model's normals.
// 1: Use compressed tangent-space normal maps with only RG for the model's normals.
// 2: Use PerturbNormalLQ to calculate bump maps as the model's normals. May not work well on small objects.
// 3: Use PerturbNormalHQ to calculate bump maps as the model's normals (High Quality).
// 4: Use RGB world-space normal maps for the model's normals.
#define NORMAL_MAP_TYPE 0

// UV coordinate flipping function
// 1: Flip horizontally on the X-axis
// 2: Flip vertically on the Y-axis
// 3: Flip both horizontally and vertically
#define NORMAL_MAP_UV_FLIP 0

// Normal map path
#define NORMAL_MAP_FILE "normal.png"

// Normal strength ≥ 0
const float normalMapScale = 1.0;

// Texture iteration count
const float normalMapLoopNum = 1.0;

//-----------------------------------
//      Sub Normal
//      Sub normals are mainly used to add additional details to the original base normals, combining two normal maps into one without modifying the original map.
//-----------------------------------
// Specify texture map. See the source section for details. Set to 1 to specify an image file.
#define NORMAL_SUB_MAP_FROM 0

// Normal map type
// 0: Use RGB tangent-space normal maps for the model's normals.
// 1: Use compressed tangent-space normal maps with only RG for the model's normals.
// 2: Use PerturbNormalLQ to calculate bump maps as the model's normals. May not work well on small objects.
// 3: Use PerturbNormalHQ to calculate bump maps as the model's normals (High Quality).
// 4: Use RGB world-space normal maps for the model's normals.
#define NORMAL_SUB_MAP_TYPE 0

// UV coordinate flipping function
// 1: Flip horizontally on the X-axis
// 2: Flip vertically on the Y-axis
// 3: Flip both horizontally and vertically
#define NORMAL_SUB_MAP_UV_FLIP 0

// Normal map path
#define NORMAL_SUB_MAP_FILE "normal.png"

// Normal strength ≥ 0
const float normalSubMapScale = 1.0;

// Texture iteration count
const float normalSubMapLoopNum = 1.0;

//----------------------------------------
//      Smoothness, Roughness
//      Smoothness describes the unevenness of the model's surface. The higher the value, the clearer the reflection of the surroundings. This map is always a grayscale map.
//      If the input map is colored, data will be extracted from the `R` channel, or you can specify other channels by modifying `SMOOTHNESS_MAP_SWIZZLE`.
//      By default, smoothness is calculated by converting the PMX model's specular into smoothness, so you need to modify `SMOOTHNESS_MAP_FROM` first.
//----------------------------------------
// Specify texture map, noise map, etc.
#define SMOOTHNESS_MAP_FROM 9

// Smoothness/Roughness type
// Describes whether the smoothness map uses smoothness or roughness and how to convert roughness to smoothness.
// 0: Smooth (from Frostbite Engine/CE5)
// 1: Rough~Smooth by 1.0-roughness ^ 0.5 (from UE4/GGX/SubstancePainter2)
// 2: Rough~Smooth by 1.0-roughness (from UE4/GGX/SubstancePainter.2 with linear roughness)
#define SMOOTHNESS_MAP_TYPE 0

// UV coordinate flipping function
// 1: Flip horizontally on the X-axis
// 2: Flip vertically on the Y-axis
// 3: Flip both horizontally and vertically
#define SMOOTHNESS_MAP_UV_FLIP 0

// Specify the color channel used for different parameter maps (R=0, G=1, B=2, A=3, grayscale maps do not need to specify)
#define SMOOTHNESS_MAP_SWIZZLE 0

// Change texture color, see f1oat3 value settings
// 1: Multiply
// 2: Exponential multiply
#define SMOOTHNESS_MAP_APPLY_SCALE 0

// Texture path
#define SMOOTHNESS_MAP_FILE "smoothness.png"

// Color range (0~1), varies by type
const float smoothness = 0.0;

// Iteration count
const float smoothnessMapLoopNum = 1.0;

//----------------------------------------
//      Metalness
//      Metalness only modifies the model's reflectivity, replacing the old rendering pipeline. It is an interpolation between insulators and conductors. The higher the value, the more the reflection color depends on the `Albedo` map color. This map is always a grayscale map.
//      If the input map is colored, data will be extracted from the `R` channel, or you can specify other channels by modifying `METALNESS_MAP_SWIZZLE`.
//----------------------------------------
// Specify texture map. See the source section for details. Set to 1 to specify an image file.
#define METALNESS_MAP_FROM 0

// UV coordinate flipping function
// 1: Flip horizontally on the X-axis
// 2: Flip vertically on the Y-axis
// 3: Flip both horizontally and vertically
#define METALNESS_MAP_UV_FLIP 0

// Specify the color channel used for different parameter maps (R=0, G=1, B=2, A=3, grayscale maps do not need to specify)
#define METALNESS_MAP_SWIZZLE 0

// Change texture color, see f1oat3 value settings
// 1: Multiply
// 2: Exponential multiply
#define METALNESS_MAP_APPLY_SCALE 0

// Texture path
#define METALNESS_MAP_FILE "metalness.png"

// Color range (0~1)
const float metalness = 0.0;

// Iteration count
const float metalnessMapLoopNum = 1.0;

//----------------------------------------
//      Specular to Reflectivity
//      This option provides a base reflectivity when metalness is not greater than 0. Therefore, this option is not the MMD specular map. The map supports both colored and grayscale maps, but colored RGB maps cannot be used with CUSTOM_ENABLE.
//      However, you can modify SPECULAR_MAP_TYPE to set it as a grayscale map. If you do not want the model to reflect specular color, set the value to 0 in `const float3 specular = 0.0;`
//----------------------------------------
// Specify texture map. See the source section for details. Set to 1 to specify an image file.
#define SPECULAR_MAP_FROM 0

// Specular to Reflectivity type
// 0: Calculate reflectivity coefficient from specular color by F(x)=0.08*(x) (from UE4)
// 1: Calculate reflectivity coefficient from specular color by F(x)=0.16*(x^2) (from Frostbite Engine)
// 2: Calculate reflectivity coefficient from specular grayscale by F(x)=0.08*(x) (from UE4)
// 3: Calculate reflectivity coefficient from specular grayscale by F((x)=0.16*(x2) (from Frostbite Engine)
// 4: Use reflectivity coefficient (0.04) instead of specular value (0.5) — usable when the specular map starts from 0.
// Tip: The specular map is not an environment map. It only modifies the model's base reflectivity, changing the environment reflection color. When the reflected light intensity is greater than the environment reflection, this parameter contributes little.
// If you do not want your model to reflect the sky's color, set const float3 specular=0.0 to make the model non-reflective.
#define SPECULAR_MAP_TYPE 0

// UV coordinate flipping function
// 1: Flip horizontally on the X-axis
// 2: Flip vertically on the Y-axis
// 3: Flip both horizontally and vertically
#define SPECULAR_MAP_UV_FLIP 0

// Specify the color channel used for different parameter maps (R=0, G=1, B=2, A=3, grayscale maps do not need to specify)
#define SPECULAR_MAP_SWIZZLE 0

// Change texture color, see f1oat3 value settings
// 1: Multiply
// 2: Exponential multiply
#define SPECULAR_MAP_APPLY_SCALE 0

// Texture path
#define SPECULAR_MAP_FILE "specular.png"

// Color range (0~1), default value 0.5
const float3 specular = 0.5;

// Iteration count
const float2 specularMapLoopNum = 1.0;

//----------------------------------------
//      Ambient Occlusion
//      Since zenith light is emitted from countless directions, it is impossible to calculate ambient occlusion in real-time. A simple way is to use `SSAO` or `Ambient Occlusion Maps` instead.
//      Real-time `SSAO` can only simulate small-scale occlusion, so you may need to bake `Ambient Occlusion Maps` offline. This map is a very approximate method to simulate large-scale ambient occlusion.
//      Therefore, it can produce more realistic effects. If you do not want an object to reflect the sky's diffuse and specular reflections, you can set this parameter to 0.
//----------------------------------------
// Specify texture map. See the source section for details. Set to 1 to specify an image file.
#define OCCLUSION_MAP_FROM 0

// Ambient Occlusion Type
// 0: Extracted from the sRGB color space
// 1: Extracted from the linear color space
// 2: Extracted from the sRGB color space using the model's second UV set
// 3: Extracted from the linear color space using the model's second UV set
#define OCCLUSION_MAP_TYPE 0

// UV Coordinate Flip Function
// 1: Flip horizontally along the X-axis
// 2: Flip vertically along the Y-axis
// 3: Flip both horizontally and vertically
#define OCCLUSION_MAP_UV_FLIP 0

// Specify the color channel used for different parameter maps (R=0, G=1, B=2, A=3; grayscale images do not need to be specified)
#define OCCLUSION_MAP_SWIZZLE 0

// Modify texture color, see float3 value settings
// 1: Multiply
// 2: Directed multiplication
#define OCCLUSION_MAP_APPLY_SCALE 0

// Texture Path
#define OCCLUSION_MAP_FILE "occlusion.png"

// Color Range (0~1)
const float occlusion = 1.0;

// Iteration Count
const float occlusionMapLoopNum = 1.0;

//----------------------------------------
// Parallax Mapping
// You can use a height map here, but in DX9, parallax mapping cannot work simultaneously with vertex displacement
//----------------------------------------
// Specify texture map source; refer to the source section. Setting it to 1 designates an image file
#define PARALLAX_MAP_FROM 0

// Type
// 0: Compute without transparency
// 1: Compute using transparency and the best SSDO
#define PARALLAX_MAP_TYPE 0

// UV Coordinate Flip Function
// 1: Flip horizontally along the X-axis
// 2: Flip vertically along the Y-axis
// 3: Flip both horizontally and vertically
#define PARALLAX_MAP_UV_FLIP 0

// Specify the color channel used for different parameter maps (R=0, G=1, B=2, A=3; grayscale images do not need to be specified)
#define PARALLAX_MAP_SWIZZLE 0

// Texture Path
#define PARALLAX_MAP_FILE "height.png"

// Must be ≥0
const float parallaxMapScale = 1.0;

// Iteration Count
const float parallaxMapLoopNum = 1.0;

//----------------------------------------
// Emissive Mapping
// You can add a light source to the scene and bind it to the emissive material bone to simulate emissive lighting
//----------------------------------------
// Enable Switch
// 0: Disabled
// 1: Enabled
#define EMISSIVE_ENABLE 0

// Specify texture map source; refer to the source section. Setting it to 1 designates an image file
#define EMISSIVE_MAP_FROM 0

// UV Coordinate Flip Function
// 1: Flip horizontally along the X-axis
// 2: Flip vertically along the Y-axis
// 3: Flip both horizontally and vertically
#define EMISSIVE_MAP_UV_FLIP 0

// Modify texture color, see float3 value settings
// 1: Multiply
// 2: Directed multiplication
#define EMISSIVE_MAP_APPLY_SCALE 0

// Multiply texture color changes
// Controlled by expressions (R+/G+/B+)
#define EMISSIVE_MAP_APPLY_MORPH_COLOR 0

// Texture intensity controlled by morph (Intensity+/-)
#define EMISSIVE_MAP_APPLY_MORPH_INTENSITY 0

// Blink Frequency
// 0: Disabled
// 1: Enable emissiveBlink
// Emissive detail settings
// 1: Color multiplied by frequency for pulsating emission, e.g., const float3 emissiveBlink = float3(1.0, 20, 3.0);
// 2: Color multiplied by morph controller frequency (refer to point light controller for blinking morph settings)
// Example setting: =mEmissiveColor + float3(10, 10, 10);
#define EMISSIVE_MAP_APPLY_BLINK 0

// Texture Path
#define EMISSIVE_MAP_FILE "emissive.png"

// Color Range (0~1) for emissive color
const float3 emissive = 1.0;
// Blink color range (0~10) for frequency
const float3 emissiveBlink = 1.0;
// Emissive intensity; higher values increase Bloom effect (0~100 or higher)
const float emissiveIntensity = 1.0;
// Iteration Count
const float2 emissiveMapLoopNum = 1.0;

//----------------------------------------
// Custom (e.g., Skin, etc.)
//----------------------------------------
// Custom Settings
//     | ID | Material     | CustomA   | CustomB |
//     | :- |:------------|:----------|:--------|
//     | 0  | Default | Invalid | Invalid |
//     | 1  | Skin | Curvature | Scatter Color |
//     | 2  | Emissive | Invalid | Invalid |
//     | 3  | Anisotropic | Anisotropic Intensity | Tangent Distortion |
//     | 4  | Glass | Curvature | Scatter Color |
//     | 5  | Cloth | Glossiness | Fur Color |
//     | 6  | Varnish | Smoothness | Invalid |
//     | 7  | Subsurface (Jade, Skin) | Curvature | Scatter Color |
//     | 8  | Toon Shading | Shadow Threshold | Shadow Color |
//     | 9  | Toon-Based Shadows | Shadow Threshold | Shadow Color |
// Tips:
//     Subsurface: `Curvature` is also referred to as `Opacity`, defining material blur intensity and transmission strength. More info can be found in UE4 [docs].
//     Glass: For refraction effects to work, `PMX` transparency must be less than `0.999`.
//     Cloth: `Glossiness` is an interpolation coefficient between `GGX` and `InvGGX`. More details at [link].
//     Cloth: `Fur Color` is the f0 parameter in fresnel, defining the base reflection color of the material using the sRGB color space.
//     Toon: More details at [link].
#define CUSTOM_ENABLE 0

// Specify texture map source; refer to the source section. Setting it to 1 designates an image file
#define CUSTOM_A_MAP_FROM 0

// UV Coordinate Flip Function
// 1: Flip horizontally along the X-axis
// 2: Flip vertically along the Y-axis
// 3: Flip both horizontally and vertically
#define CUSTOM_A_MAP_UV_FLIP 0

// 0: Normal
// 1: Use inverted colors from the texture
#define CUSTOM_A_MAP_COLOR_FLIP 0

// Specify the color channel used for different parameter maps (R=0, G=1, B=2, A=3; grayscale images do not need to be specified)
#define CUSTOM_A_MAP_SWIZZLE 0

// Modify texture color, see float3 value settings
// 1: Multiply
// 2: Directed multiplication
#define CUSTOM_A_MAP_APPLY_SCALE 0

// Texture Path
#define CUSTOM_A_MAP_FILE "custom.png"

// Operates in linear color space, color range (0~1)
const float customA = 0.0;
// Iteration Count
const float customAMapLoopNum = 1.0;

// Specify texture map source
#define CUSTOM_B_MAP_FROM 0
// UV Coordinate Flip Function
// 1: Flip horizontally along the X-axis
// 2: Flip vertically along the Y-axis
// 3: Flip both horizontally and vertically
#define CUSTOM_B_MAP_UV_FLIP 0
// 0: Normal
// 1: Use inverted colors from the texture
#define CUSTOM_B_MAP_COLOR_FLIP 0
// Modify texture color, see float3 value settings
// 1: Multiply
// 2: Directed multiplication
#define CUSTOM_B_MAP_APPLY_SCALE 0
// Texture Path
#define CUSTOM_B_MAP_FILE "custom.png"

//----------------------------------------
// SSS (Subsurface Scattering)
//
// This phenomenon occurs due to the scattering of light within a material. When light enters a surface, it refracts multiple times inside the object, creating a soft translucent effect.
// Examples include wax, human skin, jelly, and peeled grapes.
// It refers to the dispersion of light within an object, resulting in a semi-translucent appearance.
//
// Intuitive example: In a dark environment, if you shine a flashlight on your palm, your hand will appear semi-translucent, and the veins inside will be faintly visible. 
// This is the effect of SSS materials, commonly used to simulate semi-transparent materials like candles, jade, and skin.
//
// With a controller, the formula is: 
// static const float3 customB = mCustomBColor + SSS_SKIN_TRANSMITTANCE(0.75);
// #define SSS_SKIN_TRANSMITTANCE(x) exp((1 - saturate(x)) * float3(-8, -40, -64))
//----------------------------------------

// Working in a linear color space, color range (0~1)
const float3 customB = 0.0;
// Iteration count
const float2 customBMapLoopNum = 1.0;

// Main file for Ray material control
#include "material_common_2.0.fxsub"
