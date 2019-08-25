# GlyphLister

This application was build to ease [TextMesh Pro](https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126)'s [font creation procedure](http://digitalnativestudios.com/textmeshpro/docs/font/).
Will display every glyphs' Unicode value from given fonts.
Then put those value into `Custom Range(Dec)` or `Unicode Range(Hex)` filed of `Font Asset Creator` window.

## Usage

	GlyphLister.exe -p <path to the font> [-f d|h|c]

	-p, --path      Required. Path to the font directory or complete path to the single font.

	-f, --format    Set output format. <mode> can be d(ec), h(ex), c(har). default is dec.

	--help          Display this help screen.

	--version       Display version information.


## Example 

	> GlyphLister.exe "PathToTheFontFile"

	Found 1 font.

	Font Name: Bowlby One
	Total Glyphs: 288

	32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125,126,160,161,162,163,164,165,166,167,168,169,170,171,172,174,175,176,177,178,179,180,181,182,183,184,185,186,187,188,189,190,191,192,193,194,195,196,197,198,199,200,201,202,203,204,205,206,207,208,209,210,211,212,213,214,215,216,217,218,219,220,221,222,223,224,225,226,227,228,229,230,231,232,233,234,235,236,237,238,239,240,241,242,243,244,245,246,247,248,249,250,251,252,253,254,255,305,321,322,338,339,352,353,376,381,382,402,512,513,514,515,516,517,518,519,520,521,522,523,524,525,526,527,528,529,530,531,532,533,534,535,536,537,538,539,710,711,728,729,730,731,732,733,783,785,806,937,960,8211,8212,8216,8217,8218,8220,8221,8222,8224,8225,8226,8230,8240,8249,8250,8260,8308,8364,8482,8706,8710,8719,8721,8722,8730,8734,8747,8776,8800,8804,8805,9674,10030,10031,10122,10123,10124,10125,10126,10127,10128,10129,10130,10131,64257,64258

## Todo

- [X] Value as Decimal
- [X] Value as Hexadecimal
- [X] Value as Character 
