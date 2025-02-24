//-----------------------------------------------------------------------------
// General presets.

//----------------------------------------------------------
// Settings for SSS (Subsurface Scattering)

// Enable velvet effect?
#define ENABLE_VELVET	0
const float VelvetExponent = 2.0;			// Size of the edge
const float VelvetBaseReflection = 0.01;	// Brightness at the front 
#define VELVET_MUL_COLOR		float3(0.50, 0.50, 0.50)	// Front color (multiplicative)
#define VELVET_MUL_RIM_COLOR	float3(1.00, 1.00, 1.00)	// Edge color (multiplicative)
#define VELVET_ADD_COLOR		float3(0.00, 0.00, 0.00)	// Front color (additive)
#define VELVET_ADD_RIM_COLOR	float3(0.00, 0.00, 0.00)	// Edge color (additive)

//----------------------------------------------------------
// Specular-related settings

// Use the model's specular color?
#define ENABLE_SPECULAR_COLOR	0	// 0: Disabled, 1: Enabled

// Clearcoat effect
// Adds a transparent layer on top of the model.
#define ENABLE_CLEARCOAT		0			// 0: Disabled, 1: Enabled

const float USE_POLYGON_NORMAL = 1.0;		// Ignore normal map for the clearcoat layer?
const float ClearcoatSmoothness =  0.95;		// The closer to 1, the sharper the specular highlight. (0~1)
const float ClearcoatIntensity = 0.5;		// Specular intensity. 0 turns it off. (0~1.0)
const float3 ClearcoatF0 = float3(0.05,0.05,0.05);	// Specular reflectivity
const float4 ClearcoatColor = float4(1,1,1, 0.0);	// Clearcoat color


// Add specular specifically for hair
#define ENABLE_HAIR_SPECULAR	0
// Hair glossiness
const float HairSmoothness = 0.5;	// (0~1)
// Hair specular intensity
const float HairSpecularIntensity = 1.0;	// (0~1)
// Bone name used as the reference for hair direction
// #define HAIR_CENTER_BONE_NAME	"Head"


// Disable sphere map.
#define IGNORE_SPHERE	1

// Sphere map intensity
float3 SphereScale = float3(1.0, 1.0, 1.0) * 0.1;

// Increase opacity based on specular.
// When enabled, highlights reflected on glass, etc., will appear stronger.
// For objects with alpha cutouts like grass, strong highlights may appear on the edges.
#define ENABLE_SPECULAR_ALPHA	0


//----------------------------------------------------------
// Other settings

#define ToonColor_Scale			0.5			// Intensity of toon color emphasis. (0.0~1.0)

// Alpha cutout
// Used when edges of cutout textures like leaves appear dirty.
#define Enable_Cutout	0
#define CutoutThreshold	0.5		// Threshold value for transparent/opaque boundaries

#define IS_LIGHT	0		// For additional lights

// Ignore emissive
// When 0, (diffuse color + emissive color) * texture color becomes the base color.
// When 1, diffuse color * texture color becomes the base color.
#define IGNORE_EMISSIVE		0

// Fetch color from g-buffer.
// When using POM (Parallax Occlusion Mapping), the color position changes due to height, so color must be fetched from the g-buffer.
// When 0, fetch color from the model's texture.
#define USE_ALBEDO_MAP		0


//----------------------------------------------------------
// Load common processing
#include "Sources/PolishMain_common.fxsub"