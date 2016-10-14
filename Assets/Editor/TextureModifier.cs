//
// Copyright (c) 2016 Wooram Yang
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

//
// Copyright (c) Keijiro Takahashi
//
// The script files in this repository are in the public domain. You can copy and paste it without permission or attribution.
//


using UnityEngine;
using UnityEditor;
using System.Collections;

class TextureModifier : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        var importer = (assetImporter as TextureImporter);

        importer.textureType = TextureImporterType.GUI;

		if (assetPath.EndsWith ("Dither.png") || assetPath.EndsWith ("Dither.jpg") || assetPath.EndsWith ("Dither.tif")) {
            importer.textureFormat = TextureImporterFormat.RGBA32;
        }
    }

    void OnPostprocessTexture (Texture2D texture)
    {
		if (!(assetPath.EndsWith ("Dither.png") || assetPath.EndsWith ("Dither.jpg") || assetPath.EndsWith ("Dither.tif"))) {
            return;
        }

//		DoFloydSteinBurgDithering (texture);
//		DoBillAtkinsonDithering (texture);
		DoSierraLiteDithering (texture);
//		DoTwoRowSierraDithering (texture);

        EditorUtility.CompressTexture (texture, TextureFormat.RGBA4444, TextureCompressionQuality.Best);
    }


	void DoFloydSteinBurgDithering(Texture2D texture)
	{
		var texw = texture.width;
		var texh = texture.height;

		var pixels = texture.GetPixels ();
		var offs = 0;

		float weight = 1.0f / 16.0f;
		float weight1 = 7.0f / 16.0f;
		float weight2 = 3.0f / 16.0f;
		float weight3 = 5.0f / 16.0f;
		float weight4 = 1.0f / 16.0f;

		for (var y = 0; y < texh; y++) {
			for (var x = 0; x < texw; x++) {
				float a = pixels [offs].a;
				float r = pixels [offs].r;
				float g = pixels [offs].g;
				float b = pixels [offs].b;

				var a2 = Mathf.Clamp01 (Mathf.Floor (a * 16) * weight);
				var r2 = Mathf.Clamp01 (Mathf.Floor (r * 16) * weight);
				var g2 = Mathf.Clamp01 (Mathf.Floor (g * 16) * weight);
				var b2 = Mathf.Clamp01 (Mathf.Floor (b * 16) * weight);

				var ae = a - a2;
				var re = r - r2;
				var ge = g - g2;
				var be = b - b2;

				pixels [offs].a = a2;
				pixels [offs].r = r2;
				pixels [offs].g = g2;
				pixels [offs].b = b2;

				var n1 = offs + 1;
				var n2 = offs + texw - 1;
				var n3 = offs + texw;
				var n4 = offs + texw + 1;

				if (x < texw - 1) {
					pixels [n1].a += ae * weight1;
					pixels [n1].r += re * weight1;
					pixels [n1].g += ge * weight1;
					pixels [n1].b += be * weight1;
				}

				if (y < texh - 1) {
					pixels [n3].a += ae * weight3;
					pixels [n3].r += re * weight3;
					pixels [n3].g += ge * weight3;
					pixels [n3].b += be * weight3;

					if (x > 0) {
						pixels [n2].a += ae * weight2;
						pixels [n2].r += re * weight2;
						pixels [n2].g += ge * weight2;
						pixels [n2].b += be * weight2;
					}

					if (x < texw - 1) {
						pixels [n4].a += ae * weight4;
						pixels [n4].r += re * weight4;
						pixels [n4].g += ge * weight4;
						pixels [n4].b += be * weight4;
					}
				}

				offs++;
			}
		}

		texture.SetPixels (pixels);
	}

	void DoBillAtkinsonDithering(Texture2D texture)
	{
        var texw = texture.width;
        var texh = texture.height;

        var pixels = texture.GetPixels ();
        var offs = 0;

		var weight = 1.0f / 8.0f;

        for (var y = 0; y < texh; y++) {
            for (var x = 0; x < texw; x++) {
                float a = pixels [offs].a;
                float r = pixels [offs].r;
                float g = pixels [offs].g;
                float b = pixels [offs].b;

				var a2 = Mathf.Clamp01 (Mathf.Floor (a * 8) * weight);
				var r2 = Mathf.Clamp01 (Mathf.Floor (r * 8) * weight);
				var g2 = Mathf.Clamp01 (Mathf.Floor (g * 8) * weight);
				var b2 = Mathf.Clamp01 (Mathf.Floor (b * 8) * weight);

                var ae = a - a2;
                var re = r - r2;
                var ge = g - g2;
                var be = b - b2;

                pixels [offs].a = a2;
                pixels [offs].r = r2;
                pixels [offs].g = g2;
                pixels [offs].b = b2;


				var n1 = offs + 1;
				var n2 = offs + 2;
				var n3 = offs + texw - 1;
				var n4 = offs + texw;
				var n5 = offs + texw + 1;
				var n6 = offs + texw + texw;

				if (x < texw - 2) {
					pixels [n2].a += ae * weight;
					pixels [n2].r += re * weight;
					pixels [n2].g += ge * weight;
					pixels [n2].b += be * weight;
				}
				if (x < texw - 1) {
					pixels [n1].a += ae * weight;
					pixels [n1].r += re * weight;
					pixels [n1].g += ge * weight;
					pixels [n1].b += be * weight;
				}

				if (y < texh - 2) {
					pixels [n6].a += ae * weight;
					pixels [n6].r += re * weight;
					pixels [n6].g += ge * weight;
					pixels [n6].b += be * weight;
				} 
				if (y < texh - 1) {
					pixels [n4].a += ae * weight;
					pixels [n4].r += re * weight;
					pixels [n4].g += ge * weight;
					pixels [n4].b += be * weight;

					if (x > 0) {
						pixels [n3].a += ae * weight;
						pixels [n3].r += re * weight;
						pixels [n3].g += ge * weight;
						pixels [n3].b += be * weight;
					}
					if (x < texw - 1) {
						pixels [n5].a += ae * weight;
						pixels [n5].r += re * weight;
						pixels [n5].g += ge * weight;
						pixels [n5].b += be * weight;
					}
				}

                offs++;
            }
        }

        texture.SetPixels (pixels);
	}

	private void DoSierraLiteDithering (Texture2D texture)
	{
		var texw = texture.width;
		var texh = texture.height;

		var pixels = texture.GetPixels ();
		var offs = 0;

		var weight = 1.0f / 4.0f;
		var weight1 = 2.0f / 4.0f;
		var weight2 = 1.0f / 4.0f;
		var weight3 = 1.0f / 4.0f;

		for (var y = 0; y < texh; y++) {
			for (var x = 0; x < texw; x++) {
				float a = pixels [offs].a;
				float r = pixels [offs].r;
				float g = pixels [offs].g;
				float b = pixels [offs].b;

				var a2 = Mathf.Clamp01 (Mathf.Floor (a * 4) * weight);
				var r2 = Mathf.Clamp01 (Mathf.Floor (r * 4) * weight);
				var g2 = Mathf.Clamp01 (Mathf.Floor (g * 4) * weight);
				var b2 = Mathf.Clamp01 (Mathf.Floor (b * 4) * weight);

				var ae = a - a2;
				var re = r - r2;
				var ge = g - g2;
				var be = b - b2;

				pixels [offs].a = a2;
				pixels [offs].r = r2;
				pixels [offs].g = g2;
				pixels [offs].b = b2;


				var n1 = offs + 1;
				var n2 = offs + texw - 1;
				var n3 = offs + texw;


				if (x < texw - 1) {
					pixels [n1].a += ae * weight1;
					pixels [n1].r += re * weight1;
					pixels [n1].g += ge * weight1;
					pixels [n1].b += be * weight1;
				}
					
				if (y < texh - 1) {
					pixels [n3].a += ae * weight3;
					pixels [n3].r += re * weight3;
					pixels [n3].g += ge * weight3;
					pixels [n3].b += be * weight3;

					if (x > 0) {
						pixels [n2].a += ae * weight2;
						pixels [n2].r += re * weight2;
						pixels [n2].g += ge * weight2;
						pixels [n2].b += be * weight2;
					}
				}

				offs++;
			}
		}

		texture.SetPixels (pixels);
	}

	private void DoTwoRowSierraDithering (Texture2D texture)
	{
		var texw = texture.width;
		var texh = texture.height;

		var pixels = texture.GetPixels ();
		var offs = 0;

		var weight = 1.0f / 16.0f;
		var weight1 = 4.0f / 16.0f;
		var weight2 = 3.0f / 16.0f;
		var weight3 = 1.0f / 16.0f;
		var weight4 = 2.0f / 16.0f;
		var weight5 = 3.0f / 16.0f;
		var weight6 = 2.0f / 16.0f;
		var weight7 = 1.0f / 16.0f;

		for (var y = 0; y < texh; y++) {
			for (var x = 0; x < texw; x++) {
				float a = pixels [offs].a;
				float r = pixels [offs].r;
				float g = pixels [offs].g;
				float b = pixels [offs].b;

				var a2 = Mathf.Clamp01 (Mathf.Floor (a * 16) * weight);
				var r2 = Mathf.Clamp01 (Mathf.Floor (r * 16) * weight);
				var g2 = Mathf.Clamp01 (Mathf.Floor (g * 16) * weight);
				var b2 = Mathf.Clamp01 (Mathf.Floor (b * 16) * weight);

				var ae = a - a2;
				var re = r - r2;
				var ge = g - g2;
				var be = b - b2;

				pixels [offs].a = a2;
				pixels [offs].r = r2;
				pixels [offs].g = g2;
				pixels [offs].b = b2;


				var n1 = offs + 1;
				var n2 = offs + 2;
				var n3 = offs + texw - 2;
				var n4 = offs + texw - 1;
				var n5 = offs + texw;
				var n6 = offs + texw + 1;
				var n7 = offs + texw + 2;


				if (x < texw - 1) {
					pixels [n1].a += ae * weight2;
					pixels [n1].r += re * weight2;
					pixels [n1].g += ge * weight2;
					pixels [n1].b += be * weight2;
				}
				if (x < texw - 2) {
					pixels [n2].a += ae * weight1;
					pixels [n2].r += re * weight1;
					pixels [n2].g += ge * weight1;
					pixels [n2].b += be * weight1;
				}

				if (y < texh - 1) {
					pixels [n5].a += ae * weight5;
					pixels [n5].r += re * weight5;
					pixels [n5].g += ge * weight5;
					pixels [n5].b += be * weight5;

					if (x - 1 > 0) {
						pixels [n3].a += ae * weight3;
						pixels [n3].r += re * weight3;
						pixels [n3].g += ge * weight3;
						pixels [n3].b += be * weight3;
					}
					if (x > 0) {
						pixels [n4].a += ae * weight4;
						pixels [n4].r += re * weight4;
						pixels [n4].g += ge * weight4;
						pixels [n4].b += be * weight4;
					}
					if (x < texw - 1) {
						pixels [n6].a += ae * weight7;
						pixels [n6].r += re * weight7;
						pixels [n6].g += ge * weight7;
						pixels [n6].b += be * weight7;
					}
					if (x < texw - 2) {
						pixels [n7].a += ae * weight6;
						pixels [n7].r += re * weight6;
						pixels [n7].g += ge * weight6;
						pixels [n7].b += be * weight6;
					}
				}

				offs++;
			}
		}

		texture.SetPixels (pixels);
	}
}
