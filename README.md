unity-dither4444
================

I added a few of dithering method. 


<img src="http://i.imgur.com/NKDcU8y.png" width="400"/><br>
Original<br>

<img src="http://i.imgur.com/L4vb709.png" width="400"/><br>
Floyd-Steinburg<br>

<img src="http://i.imgur.com/18UpkUx.png" width="400"/><br>
Two row Sierra<br>

<img src="http://i.imgur.com/zBjI7u8.png" width="400"/><br>
Bill Atkinson<br>

<img src="http://i.imgur.com/m7DhDXH.png" width="400"/><br>
Sierra Lite<br>



This example shows how to make a high-quality 16-bit color texture in Unity.

Abstract
--------

Unity supports 16-bit color as a texture color format, however it introduces significant color banding.

![Image A (original)](http://keijiro.github.io/unity-dither4444/a-original.png)![Image A (default)](http://keijiro.github.io/unity-dither4444/a-default.png)

![Image B (original)](http://keijiro.github.io/unity-dither4444/b-original.png)![Image B (default)](http://keijiro.github.io/unity-dither4444/b-default.png)

*(Left: original image, Right: 16-bit converted image)*

It’s mainly because of lack of dither -- Unity simply quantizes the image to 16-bit without any fancy algorithms. It can be improved by dithering the image before quantization. This example does it in the AssetPostProcessor script.

![Image A (original)](http://keijiro.github.io/unity-dither4444/a-original.png)![Image A (dithered)](http://keijiro.github.io/unity-dither4444/a-dither.png)

![Image B (original)](http://keijiro.github.io/unity-dither4444/b-original.png)![Image B (dithered)](http://keijiro.github.io/unity-dither4444/b-dither.png)

*(Left: original image, Right: 16-bit image with dithering)*

Usage
-----

Add “Dither” to the end of the filename, or import an image with the “Dither” suffix. The AssetPostProcessor script automatically detects and convert it. You can change the behavior by modifying the script. See [the script](https://github.com/keijiro/unity-dither4444/blob/master/Assets/Editor/TextureModifier.cs) for further details.

License
-------

The script files in this repository are in the public domain. You can copy and paste it without permission or attribution.
