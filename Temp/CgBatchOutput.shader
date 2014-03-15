Shader "Hidden/GlowConeTap" {

Properties {
	_Color ("Color", color) = (1,1,1,0)
	_MainTex ("", 2D) = "white" {}
}

Category {
	ZTest Always Cull Off ZWrite Off Fog { Mode Off }

	Subshader {
		Pass {
			Program "vp" {
// Vertex combos: 1
//   opengl - ALU: 20 to 20
//   d3d9 - ALU: 21 to 21
//   d3d11 - ALU: 12 to 12, TEX: 0 to 0, FLOW: 1 to 1
//   d3d11_9x - ALU: 12 to 12, TEX: 0 to 0, FLOW: 1 to 1
SubProgram "opengl " {
Keywords { }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 9 [_MainTex_TexelSize]
Vector 10 [_BlurOffsets]
"!!ARBvp1.0
# 20 ALU
PARAM c[11] = { { 0 },
		state.matrix.mvp,
		state.matrix.texture[0],
		program.local[9..10] };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R0.xy, c[9];
MUL R0.zw, R0.xyxy, c[10].xyxy;
ADD R1.xy, vertex.texcoord[0], -R0.zwzw;
MOV R1.zw, c[0].x;
DP4 R2.y, R1, c[6];
DP4 R2.x, R1, c[5];
MOV R0.xy, c[10];
MUL R1.y, R0, c[9];
MUL R1.x, R0, c[9];
MOV R0.y, R1;
MOV R0.x, -R1;
MOV R1.y, -R1;
ADD result.texcoord[0].zw, R2.xyxy, R0.xyxy;
ADD result.texcoord[0].xy, R2, R0.zwzw;
ADD result.texcoord[1].zw, R2.xyxy, -R0;
ADD result.texcoord[1].xy, R2, R1;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 20 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [glstate_matrix_texture0]
Vector 8 [_MainTex_TexelSize]
Vector 9 [_BlurOffsets]
"vs_2_0
; 21 ALU
def c10, 0.00000000, 0, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c9
mul r1.xy, c8, r0
mov r0.zw, c10.x
add r0.xy, v1, -r1
dp4 r1.w, r0, c5
dp4 r1.z, r0, c4
mov r0.y, c8
mul r0.y, c9, r0
mov r0.w, r0.y
mov r0.x, c8
mul r0.x, c9, r0
mov r0.z, -r0.x
mov r0.y, -r0
add oT0.zw, r1, r0
add oT0.xy, r1.zwzw, r1
add oT1.zw, r1, -r1.xyxy
add oT1.xy, r1.zwzw, r0
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}

SubProgram "d3d11 " {
Keywords { }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 64 // 48 used size, 4 vars
Vector 16 [_MainTex_TexelSize] 4
Vector 32 [_BlurOffsets] 4
ConstBuffer "UnityPerDraw" 336 // 64 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
ConstBuffer "UnityPerDrawTexMatrices" 768 // 576 used size, 5 vars
Matrix 512 [glstate_matrix_texture0] 4
BindCB "$Globals" 0
BindCB "UnityPerDraw" 1
BindCB "UnityPerDrawTexMatrices" 2
// 13 instructions, 2 temp regs, 0 temp arrays:
// ALU 12 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedbkgibiakkbnphdafkdebngcmjafkjdaiabaaaaaadaadaaaaadaaaaaa
cmaaaaaaiaaaaaaapaaaaaaaejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklkl
epfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaa
fmaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaaapaaaaaafdfgfpfagphdgjhe
gjgpgoaafeeffiedepepfceeaaklklklfdeieefcdiacaaaaeaaaabaaioaaaaaa
fjaaaaaeegiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaa
fjaaaaaeegiocaaaacaaaaaaccaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaad
dcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaa
abaaaaaagfaaaaadpccabaaaacaaaaaagiaaaaacacaaaaaadiaaaaaipcaabaaa
aaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaacaaaaaakgbkbaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaaegiocaaaabaaaaaaadaaaaaa
pgbpbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaamdcaabaaaaaaaaaaaegiacaia
ebaaaaaaaaaaaaaaabaaaaaaegiacaaaaaaaaaaaacaaaaaaegbabaaaabaaaaaa
diaaaaaigcaabaaaaaaaaaaafgafbaaaaaaaaaaaagibcaaaacaaaaaacbaaaaaa
dcaaaaakdcaabaaaaaaaaaaaegiacaaaacaaaaaacaaaaaaaagaabaaaaaaaaaaa
jgafbaaaaaaaaaaadiaaaaajdcaabaaaabaaaaaaegiacaaaaaaaaaaaabaaaaaa
egiacaaaaaaaaaaaacaaaaaadgaaaaagmcaabaaaabaaaaaaagaebaiaebaaaaaa
abaaaaaaaaaaaaahpccabaaaabaaaaaaegaebaaaaaaaaaaaegagbaaaabaaaaaa
aaaaaaahdccabaaaacaaaaaaegaabaaaaaaaaaaamgaabaaaabaaaaaadcaaaaam
mccabaaaacaaaaaaagiecaiaebaaaaaaaaaaaaaaabaaaaaaagiecaaaaaaaaaaa
acaaaaaaagaebaaaaaaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { }
"!!GLES


#ifdef VERTEX

varying mediump vec4 xlv_TEXCOORD0_1;
varying mediump vec4 xlv_TEXCOORD0;
uniform highp vec4 _BlurOffsets;
uniform highp vec4 _MainTex_TexelSize;
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  mediump vec4 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp float tmpvar_3;
  tmpvar_3 = (_MainTex_TexelSize.x * _BlurOffsets.x);
  highp float tmpvar_4;
  tmpvar_4 = (_MainTex_TexelSize.y * _BlurOffsets.y);
  highp vec2 tmpvar_5;
  tmpvar_5.x = tmpvar_3;
  tmpvar_5.y = tmpvar_4;
  highp vec2 inUV_6;
  inUV_6 = (_glesMultiTexCoord0.xy - tmpvar_5);
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(0.0, 0.0);
  tmpvar_7.x = inUV_6.x;
  tmpvar_7.y = inUV_6.y;
  highp vec4 tmpvar_8;
  tmpvar_8 = (glstate_matrix_texture0 * tmpvar_7);
  highp vec2 tmpvar_9;
  tmpvar_9.x = tmpvar_3;
  tmpvar_9.y = tmpvar_4;
  highp vec2 tmpvar_10;
  tmpvar_10 = (tmpvar_8.xy + tmpvar_9);
  tmpvar_1.xy = tmpvar_10;
  highp vec2 tmpvar_11;
  tmpvar_11.x = -(tmpvar_3);
  tmpvar_11.y = tmpvar_4;
  highp vec2 tmpvar_12;
  tmpvar_12 = (tmpvar_8.xy + tmpvar_11);
  tmpvar_1.zw = tmpvar_12;
  highp vec2 tmpvar_13;
  tmpvar_13.x = tmpvar_3;
  tmpvar_13.y = -(tmpvar_4);
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_8.xy + tmpvar_13);
  tmpvar_2.xy = tmpvar_14;
  highp vec2 tmpvar_15;
  tmpvar_15.x = -(tmpvar_3);
  tmpvar_15.y = -(tmpvar_4);
  highp vec2 tmpvar_16;
  tmpvar_16 = (tmpvar_8.xy + tmpvar_15);
  tmpvar_2.zw = tmpvar_16;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD0_1 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

varying mediump vec4 xlv_TEXCOORD0_1;
varying mediump vec4 xlv_TEXCOORD0;
uniform lowp vec4 _Color;
uniform sampler2D _MainTex;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (((texture2D (_MainTex, xlv_TEXCOORD0.xy) + texture2D (_MainTex, xlv_TEXCOORD0.zw)) + texture2D (_MainTex, xlv_TEXCOORD0_1.xy)) + texture2D (_MainTex, xlv_TEXCOORD0_1.zw));
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * _Color.xyz);
  gl_FragData[0] = (c_1 * _Color.w);
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { }
"!!GLES


#ifdef VERTEX

varying mediump vec4 xlv_TEXCOORD0_1;
varying mediump vec4 xlv_TEXCOORD0;
uniform highp vec4 _BlurOffsets;
uniform highp vec4 _MainTex_TexelSize;
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  mediump vec4 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp float tmpvar_3;
  tmpvar_3 = (_MainTex_TexelSize.x * _BlurOffsets.x);
  highp float tmpvar_4;
  tmpvar_4 = (_MainTex_TexelSize.y * _BlurOffsets.y);
  highp vec2 tmpvar_5;
  tmpvar_5.x = tmpvar_3;
  tmpvar_5.y = tmpvar_4;
  highp vec2 inUV_6;
  inUV_6 = (_glesMultiTexCoord0.xy - tmpvar_5);
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(0.0, 0.0);
  tmpvar_7.x = inUV_6.x;
  tmpvar_7.y = inUV_6.y;
  highp vec4 tmpvar_8;
  tmpvar_8 = (glstate_matrix_texture0 * tmpvar_7);
  highp vec2 tmpvar_9;
  tmpvar_9.x = tmpvar_3;
  tmpvar_9.y = tmpvar_4;
  highp vec2 tmpvar_10;
  tmpvar_10 = (tmpvar_8.xy + tmpvar_9);
  tmpvar_1.xy = tmpvar_10;
  highp vec2 tmpvar_11;
  tmpvar_11.x = -(tmpvar_3);
  tmpvar_11.y = tmpvar_4;
  highp vec2 tmpvar_12;
  tmpvar_12 = (tmpvar_8.xy + tmpvar_11);
  tmpvar_1.zw = tmpvar_12;
  highp vec2 tmpvar_13;
  tmpvar_13.x = tmpvar_3;
  tmpvar_13.y = -(tmpvar_4);
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_8.xy + tmpvar_13);
  tmpvar_2.xy = tmpvar_14;
  highp vec2 tmpvar_15;
  tmpvar_15.x = -(tmpvar_3);
  tmpvar_15.y = -(tmpvar_4);
  highp vec2 tmpvar_16;
  tmpvar_16 = (tmpvar_8.xy + tmpvar_15);
  tmpvar_2.zw = tmpvar_16;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD0_1 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

varying mediump vec4 xlv_TEXCOORD0_1;
varying mediump vec4 xlv_TEXCOORD0;
uniform lowp vec4 _Color;
uniform sampler2D _MainTex;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (((texture2D (_MainTex, xlv_TEXCOORD0.xy) + texture2D (_MainTex, xlv_TEXCOORD0.zw)) + texture2D (_MainTex, xlv_TEXCOORD0_1.xy)) + texture2D (_MainTex, xlv_TEXCOORD0_1.zw));
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * _Color.xyz);
  gl_FragData[0] = (c_1 * _Color.w);
}



#endif"
}

SubProgram "flash " {
Keywords { }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [glstate_matrix_texture0]
Vector 8 [_MainTex_TexelSize]
Vector 9 [_BlurOffsets]
"agal_vs
c10 0.0 0.0 0.0 0.0
[bc]
aaaaaaaaaaaaadacajaaaaoeabaaaaaaaaaaaaaaaaaaaaaa mov r0.xy, c9
adaaaaaaabaaadacaiaaaaoeabaaaaaaaaaaaafeacaaaaaa mul r1.xy, c8, r0.xyyy
aaaaaaaaaaaaamacakaaaaaaabaaaaaaaaaaaaaaaaaaaaaa mov r0.zw, c10.x
acaaaaaaaaaaadacadaaaaoeaaaaaaaaabaaaafeacaaaaaa sub r0.xy, a3, r1.xyyy
bdaaaaaaabaaaiacaaaaaaoeacaaaaaaafaaaaoeabaaaaaa dp4 r1.w, r0, c5
bdaaaaaaabaaaeacaaaaaaoeacaaaaaaaeaaaaoeabaaaaaa dp4 r1.z, r0, c4
aaaaaaaaaaaaacacaiaaaaoeabaaaaaaaaaaaaaaaaaaaaaa mov r0.y, c8
adaaaaaaaaaaacacajaaaaoeabaaaaaaaaaaaaffacaaaaaa mul r0.y, c9, r0.y
aaaaaaaaaaaaaiacaaaaaaffacaaaaaaaaaaaaaaaaaaaaaa mov r0.w, r0.y
aaaaaaaaaaaaabacaiaaaaoeabaaaaaaaaaaaaaaaaaaaaaa mov r0.x, c8
adaaaaaaaaaaabacajaaaaoeabaaaaaaaaaaaaaaacaaaaaa mul r0.x, c9, r0.x
bfaaaaaaaaaaaeacaaaaaaaaacaaaaaaaaaaaaaaaaaaaaaa neg r0.z, r0.x
bfaaaaaaaaaaacacaaaaaaffacaaaaaaaaaaaaaaaaaaaaaa neg r0.y, r0.y
abaaaaaaaaaaamaeabaaaaopacaaaaaaaaaaaaopacaaaaaa add v0.zw, r1.wwzw, r0.wwzw
abaaaaaaaaaaadaeabaaaapoacaaaaaaabaaaafeacaaaaaa add v0.xy, r1.zwww, r1.xyyy
acaaaaaaabaaamaeabaaaaopacaaaaaaabaaaaefacaaaaaa sub v1.zw, r1.wwzw, r1.yyxy
abaaaaaaabaaadaeabaaaapoacaaaaaaaaaaaafeacaaaaaa add v1.xy, r1.zwww, r0.xyyy
bdaaaaaaaaaaaiadaaaaaaoeaaaaaaaaadaaaaoeabaaaaaa dp4 o0.w, a0, c3
bdaaaaaaaaaaaeadaaaaaaoeaaaaaaaaacaaaaoeabaaaaaa dp4 o0.z, a0, c2
bdaaaaaaaaaaacadaaaaaaoeaaaaaaaaabaaaaoeabaaaaaa dp4 o0.y, a0, c1
bdaaaaaaaaaaabadaaaaaaoeaaaaaaaaaaaaaaoeabaaaaaa dp4 o0.x, a0, c0
"
}

SubProgram "d3d11_9x " {
Keywords { }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 64 // 48 used size, 4 vars
Vector 16 [_MainTex_TexelSize] 4
Vector 32 [_BlurOffsets] 4
ConstBuffer "UnityPerDraw" 336 // 64 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
ConstBuffer "UnityPerDrawTexMatrices" 768 // 576 used size, 5 vars
Matrix 512 [glstate_matrix_texture0] 4
BindCB "$Globals" 0
BindCB "UnityPerDraw" 1
BindCB "UnityPerDrawTexMatrices" 2
// 13 instructions, 2 temp regs, 0 temp arrays:
// ALU 12 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0_level_9_1
eefiecedpgliijocgjamdeiipiehaobnogmnaeodabaaaaaakiaeaaaaaeaaaaaa
daaaaaaakeabaaaaoeadaaaadiaeaaaaebgpgodjgmabaaaagmabaaaaaaacpopp
caabaaaaemaaaaaaadaaceaaaaaaeiaaaaaaeiaaaaaaceaaabaaeiaaaaaaabaa
acaaabaaaaaaaaaaabaaaaaaaeaaadaaaaaaaaaaacaacaaaacaaahaaaaaaaaaa
aaaaaaaaaaacpoppbpaaaaacafaaaaiaaaaaapjabpaaaaacafaaabiaabaaapja
abaaaaacaaaaadiaabaaoekaaeaaaaaeaaaaamiaaaaaeeiaacaaeekbabaaeeja
afaaaaadabaaadiaaaaappiaaiaaoekaaeaaaaaeaaaaamiaahaaeekaaaaakkia
abaaeeiaafaaaaadabaaadiaaaaaoeiaacaaoekaabaaaaacabaaamiaabaaeeib
acaaaaadaaaaapoaaaaaooiaabaageiaacaaaaadabaaadoaaaaaooiaabaaomia
aeaaaaaeabaaamoaaaaaeeiaacaaeekbaaaaoeiaafaaaaadaaaaapiaaaaaffja
aeaaoekaaeaaaaaeaaaaapiaadaaoekaaaaaaajaaaaaoeiaaeaaaaaeaaaaapia
afaaoekaaaaakkjaaaaaoeiaaeaaaaaeaaaaapiaagaaoekaaaaappjaaaaaoeia
aeaaaaaeaaaaadmaaaaappiaaaaaoekaaaaaoeiaabaaaaacaaaaammaaaaaoeia
ppppaaaafdeieefcdiacaaaaeaaaabaaioaaaaaafjaaaaaeegiocaaaaaaaaaaa
adaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaafjaaaaaeegiocaaaacaaaaaa
ccaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaae
pccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaagfaaaaadpccabaaa
acaaaaaagiaaaaacacaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaa
egiocaaaabaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
aaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaa
egiocaaaabaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaak
pccabaaaaaaaaaaaegiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaa
aaaaaaaadcaaaaamdcaabaaaaaaaaaaaegiacaiaebaaaaaaaaaaaaaaabaaaaaa
egiacaaaaaaaaaaaacaaaaaaegbabaaaabaaaaaadiaaaaaigcaabaaaaaaaaaaa
fgafbaaaaaaaaaaaagibcaaaacaaaaaacbaaaaaadcaaaaakdcaabaaaaaaaaaaa
egiacaaaacaaaaaacaaaaaaaagaabaaaaaaaaaaajgafbaaaaaaaaaaadiaaaaaj
dcaabaaaabaaaaaaegiacaaaaaaaaaaaabaaaaaaegiacaaaaaaaaaaaacaaaaaa
dgaaaaagmcaabaaaabaaaaaaagaebaiaebaaaaaaabaaaaaaaaaaaaahpccabaaa
abaaaaaaegaebaaaaaaaaaaaegagbaaaabaaaaaaaaaaaaahdccabaaaacaaaaaa
egaabaaaaaaaaaaamgaabaaaabaaaaaadcaaaaammccabaaaacaaaaaaagiecaia
ebaaaaaaaaaaaaaaabaaaaaaagiecaaaaaaaaaaaacaaaaaaagaebaaaaaaaaaaa
doaaaaabejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaa
adadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklklepfdeheogiaaaaaa
adaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaa
fmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaafmaaaaaaabaaaaaa
aaaaaaaaadaaaaaaacaaaaaaapaaaaaafdfgfpfagphdgjhegjgpgoaafeeffied
epepfceeaaklklkl"
}

SubProgram "gles3 " {
Keywords { }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct v2f {
    highp vec4 pos;
    mediump vec4 uv[2];
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform highp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 321
uniform highp vec4 _MainTex_TexelSize;
uniform highp vec4 _BlurOffsets;
uniform sampler2D _MainTex;
#line 337
uniform lowp vec4 _Color;
#line 193
highp vec2 MultiplyUV( in highp mat4 mat, in highp vec2 inUV ) {
    highp vec4 temp = vec4( inUV.x, inUV.y, 0.0, 0.0);
    temp = (mat * temp);
    #line 197
    return temp.xy;
}
#line 323
v2f vert( in appdata_img v ) {
    #line 325
    v2f o;
    highp float offX = (_MainTex_TexelSize.x * _BlurOffsets.x);
    highp float offY = (_MainTex_TexelSize.y * _BlurOffsets.y);
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 329
    highp vec2 uv = MultiplyUV( glstate_matrix_texture0, (v.texcoord.xy - vec2( offX, offY)));
    o.uv[0].xy = (uv + vec2( offX, offY));
    o.uv[0].zw = (uv + vec2( (-offX), offY));
    o.uv[1].xy = (uv + vec2( offX, (-offY)));
    #line 333
    o.uv[1].zw = (uv + vec2( (-offX), (-offY)));
    return o;
}
out mediump vec4 xlv_TEXCOORD0;
out mediump vec4 xlv_TEXCOORD0_1;
void main() {
    v2f xl_retval;
    appdata_img xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.texcoord = vec2(gl_MultiTexCoord0);
    xl_retval = vert( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec4(xl_retval.uv[0]);
    xlv_TEXCOORD0_1 = vec4(xl_retval.uv[1]);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct v2f {
    highp vec4 pos;
    mediump vec4 uv[2];
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform highp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 321
uniform highp vec4 _MainTex_TexelSize;
uniform highp vec4 _BlurOffsets;
uniform sampler2D _MainTex;
#line 337
uniform lowp vec4 _Color;
#line 338
lowp vec4 frag( in v2f i ) {
    lowp vec4 c;
    #line 341
    c = texture( _MainTex, i.uv[0].xy);
    c += texture( _MainTex, i.uv[0].zw);
    c += texture( _MainTex, i.uv[1].xy);
    c += texture( _MainTex, i.uv[1].zw);
    #line 345
    c.xyz *= _Color.xyz;
    return (c * _Color.w);
}
in mediump vec4 xlv_TEXCOORD0;
in mediump vec4 xlv_TEXCOORD0_1;
void main() {
    lowp vec4 xl_retval;
    v2f xlt_i;
    xlt_i.pos = vec4(0.0);
    xlt_i.uv[0] = vec4(xlv_TEXCOORD0);
    xlt_i.uv[1] = vec4(xlv_TEXCOORD0_1);
    xl_retval = frag( xlt_i);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

}
Program "fp" {
// Fragment combos: 1
//   opengl - ALU: 9 to 9, TEX: 4 to 4
//   d3d9 - ALU: 11 to 11, TEX: 4 to 4
//   d3d11 - ALU: 5 to 5, TEX: 4 to 4, FLOW: 1 to 1
//   d3d11_9x - ALU: 5 to 5, TEX: 4 to 4, FLOW: 1 to 1
SubProgram "opengl " {
Keywords { }
Vector 0 [_Color]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 9 ALU, 4 TEX
PARAM c[1] = { program.local[0] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEX R3, fragment.texcoord[1].zwzw, texture[0], 2D;
TEX R2, fragment.texcoord[1], texture[0], 2D;
TEX R1, fragment.texcoord[0].zwzw, texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R0, R0, R1;
ADD R0, R0, R2;
ADD R0, R0, R3;
MUL R0.xyz, R0, c[0];
MUL result.color, R0, c[0].w;
END
# 9 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { }
Vector 0 [_Color]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 11 ALU, 4 TEX
dcl_2d s0
dcl t0
dcl t1
texld r3, t0, s0
mov r1.y, t0.w
mov r1.x, t0.z
mov r2.xy, r1
mov r0.y, t1.w
mov r0.x, t1.z
texld r0, r0, s0
texld r1, t1, s0
texld r2, r2, s0
add_pp r2, r3, r2
add_pp r1, r2, r1
add_pp r0, r1, r0
mul_pp r0.xyz, r0, c0
mul_pp r0, r0, c0.w
mov_pp oC0, r0
"
}

SubProgram "d3d11 " {
Keywords { }
ConstBuffer "$Globals" 64 // 64 used size, 4 vars
Vector 48 [_Color] 4
BindCB "$Globals" 0
SetTexture 0 [_MainTex] 2D 0
// 10 instructions, 2 temp regs, 0 temp arrays:
// ALU 5 float, 0 int, 0 uint
// TEX 4 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedhjnkpodggbfpigaepahjmpghdlaonddaabaaaaaagaacaaaaadaaaaaa
cmaaaaaajmaaaaaanaaaaaaaejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apapaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefciiabaaaaeaaaaaaagcaaaaaa
fjaaaaaeegiocaaaaaaaaaaaaeaaaaaafkaaaaadaagabaaaaaaaaaaafibiaaae
aahabaaaaaaaaaaaffffaaaagcbaaaadpcbabaaaabaaaaaagcbaaaadpcbabaaa
acaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacacaaaaaaefaaaaajpcaabaaa
aaaaaaaaegbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaaefaaaaaj
pcaabaaaabaaaaaaogbkbaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaa
aaaaaaahpcaabaaaaaaaaaaaegaobaaaaaaaaaaaegaobaaaabaaaaaaefaaaaaj
pcaabaaaabaaaaaaegbabaaaacaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaa
aaaaaaahpcaabaaaaaaaaaaaegaobaaaaaaaaaaaegaobaaaabaaaaaaefaaaaaj
pcaabaaaabaaaaaaogbkbaaaacaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaa
aaaaaaahpcaabaaaaaaaaaaaegaobaaaaaaaaaaaegaobaaaabaaaaaadiaaaaai
hcaabaaaaaaaaaaaegacbaaaaaaaaaaaegiccaaaaaaaaaaaadaaaaaadiaaaaai
pccabaaaaaaaaaaaegaobaaaaaaaaaaapgipcaaaaaaaaaaaadaaaaaadoaaaaab
"
}

SubProgram "gles " {
Keywords { }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { }
"!!GLES"
}

SubProgram "flash " {
Keywords { }
Vector 0 [_Color]
SetTexture 0 [_MainTex] 2D
"agal_ps
[bc]
ciaaaaaaadaaapacaaaaaaoeaeaaaaaaaaaaaaaaafaababb tex r3, v0, s0 <2d wrap linear point>
aaaaaaaaabaaacacaaaaaappaeaaaaaaaaaaaaaaaaaaaaaa mov r1.y, v0.w
aaaaaaaaabaaabacaaaaaakkaeaaaaaaaaaaaaaaaaaaaaaa mov r1.x, v0.z
aaaaaaaaacaaadacabaaaafeacaaaaaaaaaaaaaaaaaaaaaa mov r2.xy, r1.xyyy
aaaaaaaaaaaaacacabaaaappaeaaaaaaaaaaaaaaaaaaaaaa mov r0.y, v1.w
aaaaaaaaaaaaabacabaaaakkaeaaaaaaaaaaaaaaaaaaaaaa mov r0.x, v1.z
ciaaaaaaaaaaapacaaaaaafeacaaaaaaaaaaaaaaafaababb tex r0, r0.xyyy, s0 <2d wrap linear point>
ciaaaaaaabaaapacabaaaaoeaeaaaaaaaaaaaaaaafaababb tex r1, v1, s0 <2d wrap linear point>
ciaaaaaaacaaapacacaaaafeacaaaaaaaaaaaaaaafaababb tex r2, r2.xyyy, s0 <2d wrap linear point>
abaaaaaaacaaapacadaaaaoeacaaaaaaacaaaaoeacaaaaaa add r2, r3, r2
abaaaaaaabaaapacacaaaaoeacaaaaaaabaaaaoeacaaaaaa add r1, r2, r1
abaaaaaaaaaaapacabaaaaoeacaaaaaaaaaaaaoeacaaaaaa add r0, r1, r0
adaaaaaaaaaaahacaaaaaakeacaaaaaaaaaaaaoeabaaaaaa mul r0.xyz, r0.xyzz, c0
adaaaaaaaaaaapacaaaaaaoeacaaaaaaaaaaaappabaaaaaa mul r0, r0, c0.w
aaaaaaaaaaaaapadaaaaaaoeacaaaaaaaaaaaaaaaaaaaaaa mov o0, r0
"
}

SubProgram "d3d11_9x " {
Keywords { }
ConstBuffer "$Globals" 64 // 64 used size, 4 vars
Vector 48 [_Color] 4
BindCB "$Globals" 0
SetTexture 0 [_MainTex] 2D 0
// 10 instructions, 2 temp regs, 0 temp arrays:
// ALU 5 float, 0 int, 0 uint
// TEX 4 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0_level_9_1
eefiecedekfdepgpnckfemdkjfnnjmbfcdbjclccabaaaaaajiadaaaaaeaaaaaa
daaaaaaageabaaaapeacaaaageadaaaaebgpgodjcmabaaaacmabaaaaaaacpppp
piaaaaaadeaaaaaaabaaciaaaaaadeaaaaaadeaaabaaceaaaaaadeaaaaaaaaaa
aaaaadaaabaaaaaaaaaaaaaaaaacppppbpaaaaacaaaaaaiaaaaacplabpaaaaac
aaaaaaiaabaacplabpaaaaacaaaaaajaaaaiapkaabaaaaacaaaacbiaaaaakkla
abaaaaacaaaacciaaaaapplaabaaaaacabaacbiaabaakklaabaaaaacabaaccia
abaapplaecaaaaadaaaaapiaaaaaoeiaaaaioekaecaaaaadacaacpiaaaaaoela
aaaioekaecaaaaadadaaapiaabaaoelaaaaioekaecaaaaadabaaapiaabaaoeia
aaaioekaacaaaaadaaaacpiaaaaaoeiaacaaoeiaacaaaaadaaaacpiaadaaoeia
aaaaoeiaacaaaaadaaaacpiaabaaoeiaaaaaoeiaafaaaaadaaaachiaaaaaoeia
aaaaoekaafaaaaadaaaacpiaaaaaoeiaaaaappkaabaaaaacaaaicpiaaaaaoeia
ppppaaaafdeieefciiabaaaaeaaaaaaagcaaaaaafjaaaaaeegiocaaaaaaaaaaa
aeaaaaaafkaaaaadaagabaaaaaaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaa
gcbaaaadpcbabaaaabaaaaaagcbaaaadpcbabaaaacaaaaaagfaaaaadpccabaaa
aaaaaaaagiaaaaacacaaaaaaefaaaaajpcaabaaaaaaaaaaaegbabaaaabaaaaaa
eghobaaaaaaaaaaaaagabaaaaaaaaaaaefaaaaajpcaabaaaabaaaaaaogbkbaaa
abaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaaaaaaaaahpcaabaaaaaaaaaaa
egaobaaaaaaaaaaaegaobaaaabaaaaaaefaaaaajpcaabaaaabaaaaaaegbabaaa
acaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaaaaaaaaahpcaabaaaaaaaaaaa
egaobaaaaaaaaaaaegaobaaaabaaaaaaefaaaaajpcaabaaaabaaaaaaogbkbaaa
acaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaaaaaaaaahpcaabaaaaaaaaaaa
egaobaaaaaaaaaaaegaobaaaabaaaaaadiaaaaaihcaabaaaaaaaaaaaegacbaaa
aaaaaaaaegiccaaaaaaaaaaaadaaaaaadiaaaaaipccabaaaaaaaaaaaegaobaaa
aaaaaaaapgipcaaaaaaaaaaaadaaaaaadoaaaaabejfdeheogiaaaaaaadaaaaaa
aiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapapaaaafmaaaaaaabaaaaaaaaaaaaaa
adaaaaaaacaaaaaaapapaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfcee
aaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklkl"
}

SubProgram "gles3 " {
Keywords { }
"!!GLES3"
}

}

#LINE 57

		}
	}

	Subshader {
		Pass {
			SetTexture [_MainTex] {constantColor [_Color] combine texture * constant alpha}
			SetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}
			SetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}
			SetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}		
		}

	}
}

Fallback off

}
