/*{
  "DESCRIPTION": "Horizontal blue streaks blooming off bright highlights \u2014 the widescreen lens flare that makes phone footage read as cinema.",
  "CATEGORIES": ["Guillotine", "Stylize"],
  "INPUTS": [
  {
    "NAME": "inputImage",
    "TYPE": "image"
  },
  {
    "NAME": "intensity",
    "TYPE": "float",
    "DEFAULT": 0.7,
    "MIN": 0.0,
    "MAX": 1.5
  },
  {
    "NAME": "threshold",
    "TYPE": "float",
    "DEFAULT": 0.72,
    "MIN": 0.4,
    "MAX": 1.0
  }
]
}*/
void main() {
  vec2 uv = isf_FragNormCoord;
  vec2 t = 1.0 / RENDERSIZE;
  vec4 c = IMG_THIS_PIXEL(inputImage);

  // Sample a long horizontal tail on both sides; only very bright pixels contribute, so the
  // streak grows out of actual highlights rather than smearing the whole frame.
  vec3 streak = vec3(0.0);
  for (int i = 1; i <= 12; i++) {
    float d = float(i) * t.x * 6.0;
    float w = 1.0 - float(i) / 13.0;
    vec3 l = IMG_NORM_PIXEL(inputImage, uv + vec2(d, 0.0)).rgb;
    vec3 r = IMG_NORM_PIXEL(inputImage, uv - vec2(d, 0.0)).rgb;
    streak += (max(l - threshold, 0.0) + max(r - threshold, 0.0)) * w;
  }
  streak /= 12.0;

  // Classic anamorphic tint: cool blue, slightly cyan.
  vec3 flare = streak * vec3(0.35, 0.65, 1.0) * intensity * 3.0;
  gl_FragColor = vec4(c.rgb + flare, c.a);
}
