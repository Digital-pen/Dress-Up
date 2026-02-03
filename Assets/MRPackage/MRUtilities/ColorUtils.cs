using UnityEngine;
using System.Collections;
using System.Globalization;


public static class ColorUtils {

	public static Color GetColorFromHex(string hexColor) {
		return ParseColor (hexColor);
	}

	static Color ParseColor(string hexstring)
	{
		if (hexstring.StartsWith("#"))
			hexstring = hexstring.Substring(1);

		if (hexstring.StartsWith("0x"))
			hexstring = hexstring.Substring(2);

		byte r = byte.Parse(hexstring.Substring(0, 2), NumberStyles.HexNumber);
		byte g = byte.Parse(hexstring.Substring(2, 2), NumberStyles.HexNumber);
		byte b = byte.Parse(hexstring.Substring(4, 2), NumberStyles.HexNumber);

		return new Color32(r, g, b, 255);
	}
}
