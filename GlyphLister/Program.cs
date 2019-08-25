using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.IO;
using System.Windows;
using CommandLine;
using CommandLine.Text;

namespace GlyphLister
{
    class Program
    {
        /// <summary>
        /// Get every glyphs' decimal unicode number of a given fonts.
        /// <para>Verify font information <see cref="https://fontdrop.info/"/></para>
        /// </summary>
        /// <param name="args">Directory or File path string, and switches</param>
        /// <returns>0 for success, 1 for error</returns>
        [STAThread]
        private static void Main(string[] args)
        {
            var parserResult = Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(options => RunCommand(options))
                .WithNotParsed(errors => HandleError(errors));
        }

        private static int RunCommand(Options opts)
        {
            if (opts.Path == null) return 1;
            
            GetGlyphs(opts.Path, opts.Format);
            return 0;
        }

        private static int HandleError(IEnumerable<Error> errs)
        {
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
            var output = string.Empty;
            var families = Fonts.GetFontFamilies(path);

            output += $"Found {families.Count} font";
            if (families.Count > 1) output += "s.\n";
            else output += ".\n";

            var formatStr = "{0}";
            
            if (option == "d")
            {
                formatStr = "{0}";
            }
            else if (option == "h")
            {
                formatStr = "{0:x}";
            }
            else if (option == "c")
            {
                output += $"Current code page: {Console.OutputEncoding.CodePage}\n";
                output += "!Warning! Characters may not display properly in console environment.\n";
                formatStr = "{0}";
            }

            foreach (var family in families)
            {
                // There might be multiple typefaces exist based on styles(normal, oblique) and weights(normal, bold).
                var typefaces = family.GetTypefaces();
                output += $"\nFont Name: {family.FamilyNames.Values.ElementAt(0)}\n";
                
                // However I will just grab first typeface only.
                var typeface = typefaces.ElementAt(0); 
                
                typeface.TryGetGlyphTypeface(out var glyph);
                
                var characterMap = glyph.CharacterToGlyphMap;
                
                output += $"Total Glyphs: {characterMap.Count}\n";

                foreach (var kvp in characterMap)
                {
                    if (option == "c")
                        output += (char) kvp.Key;
                    else
                        output += string.Format(formatStr, kvp.Key);

                    if (kvp.Key != characterMap.Keys.Last() && option != "c") output += ",";
                }

                output += "\n";
            }
            
            Console.WriteLine(output);

            if (families.Count == 0) return;
            Clipboard.SetText(output, TextDataFormat.UnicodeText);
            Console.WriteLine("Output copied to clipboard.");
        }
    }

    public class Options
    {
        private string _path;
        private string _format;

        [Option('p', "path", Required = true, HelpText = "Path to the font directory or complete path to the single font.")]
        public string Path
        {
            get => _path;
            set
            {
                if (Directory.Exists(value) || File.Exists(value))
                {
                    value = System.IO.Path.GetFullPath(value);
                    _path = (Directory.Exists(value)) ? PathAddBackslash(value) : value;
                }
                else
                {
                    _path = null;
                    Console.WriteLine("Invalid path: {0}", value);
                }
            }
        }

        [Option('f', "format", Required = false, HelpText = "Set output format. <mode> can be d(ec), h(ex), c(har). default is dec.")]
        public string Format
        {
            get => _format;
            set
            {
                switch (value.ToLower())
                {
                    case "d":
                    case "dex":
                        _format = "d";
                        break;
                    case "h":
                    case "hex":
                        _format = "h";
                        break;
                    case "c":
                    case "char":
                        _format = "c";
                        break;
                    default:
                        _format = "d";
                        break;
                }
            }
        }


        /// <summary>
        /// Check path string and return ends with proper directory separator character.
        /// <see cref="https://stackoverflow.com/questions/20405965/how-to-ensure-there-is-trailing-directory-separator-in-paths"/>
        /// </summary>
        /// <param name="path">String value for directory path</param>
        /// <returns>Return path string ends with directory separator character</returns>
        public static string PathAddBackslash(string path)
        {
            // They're always one character but EndsWith is shorter than
            // array style access to last path character. Change this
            // if performance are a (measured) issue.
            var separator1 = System.IO.Path.DirectorySeparatorChar.ToString();
            var separator2 = System.IO.Path.AltDirectorySeparatorChar.ToString();

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
