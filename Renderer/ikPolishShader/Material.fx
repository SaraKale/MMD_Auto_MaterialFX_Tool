//-----------------------------------------------------------------------------
// Material Configuration File
// For non-metals. For metals, assign Material_metal.fx.

#include "Sources/Material_header.fxsub"

// Material Type
#define MATERIAL_TYPE        MT_NORMAL
/*
MT_NORMAL    : Normal (including metals)
MT_FACE      : Skin (for faces)
MT_LEAF      : For materials like leaves and curtains where the back is visible.
MT_MASK      : For skydomes.
*/

// Texture file names to be used
#define TEXTURE_FILENAME_0	"textures/check.png"
#define TEXTURE_FILENAME_1	"textures/check.png"
#define TEXTURE_FILENAME_2	"textures/check.png"
#define TEXTURE_FILENAME_3	"textures/check.png"
#define TEXTURE_FILENAME_4	"textures/check.png"
#define TEXTURE_FILENAME_5	"textures/check.png"
// The numbers in TEXTURE_FILENAME_x are specified in the following xxx_MAP_FILE.

// xxx_VALUE: Directly specify a value.
// xxx_MAP_ENABLE: If 1, read the value from the texture.
// xxx_MAP_LOOPNUM: Number of times the texture repeats. 1 means original size. Larger numbers make it finer.
// xxx_MAP_SCALE, xxx_MAP_OFFSET: The value is calculated as (texture value * scale + offset).

// Override texture color
#define	ALBEDO_MAP_ENABLE		0	// 0: Use original color. 1: Use the texture specified here.
#define ALBEDO_MAP_FILE            1    // Specify the texture file number to use. 0-5
#define ALBEDO_MAP_LOOPNUM        1.0

// Car paint: Change color based on view angle
#define    GRADIENT_MAP_ENABLE        0    // 1: Enable
#define GRADIENT_MAP_FILE        0
#define GRADIENT_POWER            1.0    // -4 to 4. Gradient strength

// Specify texture color blend ratio
// Invalid if ALBEDO_MAP is not used
#define BLEND_VALUE                0.0    // Blend ratio with override texture. 0.0 means use original color.
#define BLEND_MAP_ENABLE        0    // Use blend map to composite with original texture?
#define BLEND_MAP_FILE            0    // Specify the texture file number to use. 0-5
#define BLEND_MAP_CHANNEL        R    // Texture channel to use. R,G,B,A
#define BLEND_MAP_LOOPNUM        1.0
#define BLEND_MAP_SCALE            1.0
#define BLEND_MAP_OFFSET        0.0

// Whether it is metal or not. ≈ Reflection strength
// Higher values increase reflection and the influence of the original color.
// 0: Non-metal, 1: Metal. Gems are around 0.1-0.2.
#define    METALNESS_VALUE            0.0
#define    METALNESS_MAP_ENABLE    0    // 0: Use VALUE, 1: Specify with texture
#define METALNESS_MAP_FILE        0    // Specify the texture file number to use. 0-3
#define METALNESS_MAP_CHANNEL    R    // Texture channel to use. R,G,B,A
#define METALNESS_MAP_LOOPNUM    1.0
#define METALNESS_MAP_SCALE        1.0
#define METALNESS_MAP_OFFSET    0.0

// Surface smoothness
// If SMOOTHNESS_TYPE = 1, 0: Matte, 1: Smooth.
// If SMOOTHNESS_TYPE = 2, 0: Smooth, 1: Matte.

// Smoothness specification method:
// 0: Automatically determined from the model's specular power.
// 1: Use smoothness.
// 2: Use roughness.
#define SMOOTHNESS_TYPE        0

#define    SMOOTHNESS_VALUE        1.0
#define    SMOOTHNESS_MAP_ENABLE    0
#define SMOOTHNESS_MAP_FILE        0
#define SMOOTHNESS_MAP_CHANNEL    R
#define SMOOTHNESS_MAP_LOOPNUM    1.0
#define SMOOTHNESS_MAP_SCALE    1.0
#define SMOOTHNESS_MAP_OFFSET    0.0

// Determine metal reflection color from base color only?
// 0: Determined by base color * specular color. (Pre-0.16 method)
// 1: Determined by base color only.
// 2: Determined by specular color only.
// ※ For non-metals, it will be white regardless of the setting.
#define SPECULAR_COLOR_TYPE        1

// Specular intensity

// Intensity handling:
#define INTENSITY_TYPE        0
// 0: Specular Intensity. Adjusts specular intensity.
// 1: Ambient Occlusion. Masks indirect light.
// 2: Cavity. Blocks all lighting.
// 3: Cavity (View Dependent). Blocks all lighting (view-dependent).

// 0: No highlight, 1: Highlight
#define    INTENSITY_VALUE            1.0
#define    INTENSITY_MAP_ENABLE    0
#define INTENSITY_MAP_FILE        0
#define INTENSITY_MAP_CHANNEL    R
#define INTENSITY_MAP_LOOPNUM    1.0
#define INTENSITY_MAP_SCALE        1.0
#define INTENSITY_MAP_OFFSET    0.0

// Emissive intensity
// ※ Emissive intensity and subsurface scattering cannot be shared.
#define    EMISSIVE_TYPE            0
// 0: AL compatible
// 1: No emission (lightweight)
// 2: Specify here. EMISSIVE_VALUE or EMISSIVE_MAP
// 3: For additional lights
// 4: For additional lights (screen)
// 5: Treat model's emissive as emission color

// The following settings apply when EMISSIVE_TYPE is 2, 5:
#define    EMISSIVE_VALUE            1.0 // 0.0 to 8.0
#define    EMISSIVE_MAP_ENABLE        0
#define EMISSIVE_MAP_FILE        0
#define EMISSIVE_MAP_CHANNEL    R
#define EMISSIVE_MAP_LOOPNUM    1.0
#define EMISSIVE_MAP_SCALE        1.0 // 0.0 to 8.0
#define EMISSIVE_MAP_OFFSET        0.0

// Ignore model's emissive
#define IGNORE_EMISSIVE        0

// Subsurface scattering: For semi-transparent materials like skin and plastic.
// 0: Opaque. 1: Semi-transparent.
// Ignored for metals.
#define    SSS_VALUE            0.0
#define    SSS_MAP_ENABLE        0
#define SSS_MAP_FILE        0
#define SSS_MAP_CHANNEL        R
#define SSS_MAP_LOOPNUM        1.0
#define SSS_MAP_SCALE        1.0
#define SSS_MAP_OFFSET        0.0

// Rim light suppression
#define DISABLE_RIM            0

//-----------------------------------------------------------------------------
// Others

// Ignore sphere map (0: Do not ignore, 1: Ignore)
#define    IGNORE_SPHERE    1
// Use only the center color of the sphere. (Some materials set color with sphere)
#define USE_ONLY_CENTER_OF_SPHERE    0

// Treat materials as transparent if transparency is below this value.
#define AlphaThreshold        0.5

// Enable pseudo-transparency (0: Disable, 1: Enable, 2: Enable + backface rendering)
#define    ENABLE_FAKE_SEMITRANSPARENT    0

// Coefficients for pseudo-transparency color calculation
#define SURFACE_ABSORPTION_RATE            0.1 // Uniform absorption rate (0.0 to 1.0)
#define BODY_ABSORPTION_RATE            1.0 // Absorption rate that changes with thickness (0.0 to )

//-----------------------------------------------------------------------------
// Normal Map

// Use normal map? 0: Disabled. 1: Enabled
#define NORMALMAP_ENABLE		0

// Main normal map
#define NORMALMAP_MAIN_FILENAME  	"textures/dummy_n.bmp"
#define NORMALMAP_MAIN_LOOPNUM    1.0
#define NORMALMAP_MAIN_HEIGHT    1.0
// Direction flip
#define    NORMALMAP_MAIN_FLIP        FLIP_NONE
// FLIP_NONE: No flip
// FLIP_X: Flip x
// FLIP_Y: Flip y
// FLIP_XY: Flip x and y

// Sub normal map
#define NORMALMAP_SUB_ENABLE    0
#define NORMALMAP_SUB_FILENAME "textures/dummy_n.bmp"
#define NORMALMAP_SUB_LOOPNUM    1.0
#define NORMALMAP_SUB_HEIGHT    1.0
#define    NORMALMAP_SUB_FLIP        FLIP_NONE

// Set to 1 if the model uses vertically flipped textures
// Humanoid models often flip horizontally.
#define NORMALMAP_FLIP_VERTICAL    0

// Suppress orientation correction during normal map calculation.
// If lighting looks wrong when using normal maps,
// setting this to 1 may improve it.
#define DISABLE_ALIGNMENT_CORRECTION    0

//-----------------------------------------------------------------------------
// Parallax Occlusion Mapping
// When using parallax occlusion, set main to PolishMain_pom.fx or similar,
// and set USE_ALBEDO_MAP to 1.

#define PARALLAX_ENABLE        0
#define PARALLAX_FILENAME    "textures/white.png"
#define PARALLAX_LOOPNUM    1.0        // Texture repeat count

// Depth adjustment (in mmd units)
// How much height 0-1 in the depth map represents in mmd.
#define PARALLAX_HEIGHT        1.0

// Reference distance on texture
// (Reference pixel / texture size)
#define PARALLAX_LENGTH        (32.0/512.0)

#define PARALLAX_ITERATION    8    // Number of searches (1 to 16)

//-----------------------------------------------------------------------------
#include "Materials/Sources/Material_body.fxsub"