using System;
using System.Linq;
using System.Windows.Media;
using System.IO;

namespace GlyphLister
{
    class Program
    {
        /// <summary>
        /// Get every glyphs' decimal unicode number of a given fonts.
        /// <para>Verify font information <see cref="https://fontdrop.info/"/></para>
        /// </summary>
        /// <param name="args">Directory or File path string, and switches</param>
        /// <returns>0 for success, 1 for missing path</returns>
        private static int Main(string[] args)
        {
            // If there's no arguments, print error and help then quit.
            if (args.Length == 0)
            {
                System.Console.WriteLine("\n\nPlease enter a path to the font file(s).");
                System.Console.WriteLine("\nUsage: GlyphLister.exe path[font filename] [/H]");
                System.Console.WriteLine("\n  /H      Return hex values\n\n");
                return 1;
            }

            // If there's only 1 argument and it was help, print help then quit.
            if (args.Length == 1 && args[0] == "/?")
            {
                System.Console.WriteLine("\n\nUsage: GlyphLister.exe path[font filename] [/H]");
                System.Console.WriteLine("\n  /H      Return hex values\n\n");
                return 1;
            }

            // Set up arguments
            var path = args[0];
            var option = args.Length >= 2 ? args[1] : string.Empty;

            // If path was directory
            if (Directory.Exists(path))
            {
                GetGlyphs(PathAddBackslash(path), option);
                return 0;
            }

            // If path was filename
            if (File.Exists(path))
            {
                GetGlyphs(path, option);
                return 0;
            }

            // If path was invalid
            Console.WriteLine("\nPath or font file does not exists...");
            return 1;
        }

        /// <summary>
        /// <para>method based on <see cref="https://stackoverflow.com/questions/1439551/get-supported-characters-of-a-font-in-c-sharp/1439676"/></para>
        /// </summary>
        /// <param name="path">Directory path or font filename</param>
        /// <param name="option">/H for hex value output</param>
        private static void GetGlyphs(string path, string option)
        {
            // Get fonts from path
            var families = Fonts.GetFontFamilies(path);
            foreach (var family in families)
            {
                // There might be multiple typefaces exist based on styles(normal, oblique) and weights(normal, bold).
                var typefaces = family.GetTypefaces();
                Console.WriteLine("\n\nFont Name: {0}", family.FamilyNames.Values.ElementAt(0));
                
                // However I will just grab first typeface only.
                var typeface = typefaces.ElementAt(0); 
                
                typeface.TryGetGlyphTypeface(out var glyph);
                
                var characterMap = glyph.CharacterToGlyphMap;
                
                Console.WriteLine("Total Glyphs: {0}\n", characterMap.Count);

                foreach (var kvp in characterMap)
                {
                    var formatStr = (option.ToUpper() == "/H") ? "{0:x}" : "{0}";
                    Console.Write(formatStr, kvp.Key);
                    if (kvp.Key != characterMap.Keys.Last()) Console.Write(",");
                }
            }

            Console.WriteLine("\n\n");
        }

        /// <summary>
        /// Check path string and return ends with proper directory separator character.
        /// <see cref="https://stackoverflow.com/questions/20405965/how-to-ensure-there-is-trailing-directory-separator-in-paths"/>
        /// </summary>
        /// <param name="path">String value for directory path</param>
        /// <returns>Return path string ends with directory separator character</returns>
        static string PathAddBackslash(string path)
        {
            // They're always one character but EndsWith is shorter than
            // array style access to last path character. Change this
            // if performance are a (measured) issue.
            var separator1 = Path.DirectorySeparatorChar.ToString();
            var separator2 = Path.AltDirectorySeparatorChar.ToString();

            // Trailing white spaces are always ignored but folders may have
            // leading spaces. It's unusual but it may happen. If it's an issue
            // then just replace TrimEnd() with Trim(). Tnx Paul Groke to point this out.
            path = path.TrimEnd();

            // Argument is always a directory name then if there is one
            // of allowed separators then I have nothing to do.
            if (path.EndsWith(separator1) || path.EndsWith(separator2))
                return path;

            // If there is the "alt" separator then I add a trailing one.
            // Note that URI format (file://drive:\path\filename.ext) is
            // not supported in most .NET I/O functions then we don't support it
            // here too. If you have to then simply revert this check:
            // if (path.Contains(separator1))
            //     return path + separator1;
            //
            // return path + separator2;
            if (path.Contains(separator2))
                return path + separator2;

            // If there is not an "alt" separator I add a "normal" one.
            // It means path may be with normal one or it has not any separator
            // (for example if it's just a directory name). In this case I
            // default to normal as users expect.
            return path + separator1;
        }
    }
}
