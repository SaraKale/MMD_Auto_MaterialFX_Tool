////////////////////////////////////////////////////////////////////////////////////////////////
// 1. Shadow Settings //////////////////////////////////////////////////////////////////////////

// Light intensity multiplier
#define LightPower 2.0

// Shadow intensity multiplier
#define ShadowPower 0.75

// Glow suppression when the light source is strong
#define Saturate 0.0



////////////////////////////////////////////////////////////////////////////////////////////////
// 2. Emission Settings ////////////////////////////////////////////////////////////////////////

// Emissive texture reference source
// 0: Monochrome (ignores texture emission)
// 1: Model texture
// 2: Specified on the effect side
#define EMISSIVE_FROM 0

// Emissive texture
#define EMISSIVE_TEXTURE "emissive.png"

// Emission intensity multiplier
#define EmissiveScale 0.0



////////////////////////////////////////////////////////////////////////////////////////////////
// 3. Normal Mapping Settings //////////////////////////////////////////////////////////////////

// Source of normal map
// 0: None
// 1: Spherical map
// 2: Specified on the effect side
#define NORMAL_FROM 0

// Normal map texture
#define NORMAL_TEXTURE "normal.png"

// Normal mapping intensity multiplier
#define NormalScale 1.0

// Texture tiling factor
#define NormalLoop 1.0



////////////////////////////////////////////////////////////////////////////////////////////////
// 4. Shading Settings /////////////////////////////////////////////////////////////////////////

// Enable toon shading
// 0: Disabled
// 1: Enabled
#define TOON_ENABLE 1

// Shadow threshold
#define ToonThreshold 0.2

// Shadow hint texture
// Uses the same shading hint format as AlternativeFull, can be replaced
#define THRESHOLD_TEXTURE "shading_hint_toon.png"

// Enable shadows
// 0: No shadows
// 1: Shadows enabled
#define SHADOW_ENABLE 1



#include "Common_Shader.fxsub"
