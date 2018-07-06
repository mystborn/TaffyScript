using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    /// <summary>
    /// Helper class used to generate the BCL of TaffyScript.
    /// </summary>
    public static class BaseClassLibraryGenerator
    {
        /// <summary>
        /// Generates the TaffyScript base class library as a string.
        /// </summary>
        /// <returns></returns>
        public static string Generate()
        {
            var sb = new StringBuilder();

            sb.AppendLine("import Math.Abs(float) as abs;");
            sb.AppendLine("import Bcl.ArrayCopy(object, int, object, int, int) as array_copy;");
            sb.AppendLine("import Bcl.ArrayCreate(instance, array) as array_create;");
            sb.AppendLine("import Bcl.ArrayEquals(array, array) as array_equals");
            sb.AppendLine("import Bcl.ArrayHeight2D(array2d) as array_height_2d;");
            sb.AppendLine("import Bcl.ArrayLength1D(array) as array_length_1d");
            sb.AppendLine("import Bcl.ArrayLength2D(array2d, int) as array_length_2d");

            sb.AppendLine("import Bcl.Base64Decode(string) as base64_decode");
            sb.AppendLine("import Bcl.Base64Encode(string) as base64_encode");

            //Todo: buffer

            sb.AppendLine("import Bcl.CallInstanceScript(instance, array) as call_instance_script");
            sb.AppendLine("import Bcl.CallGlobalScript(instance, array) as call_global_script");

            sb.AppendLine("import Math.Ceiling(double) as ceil");
            sb.AppendLine("import Bcl.Choose(instance, array) as choose");
            sb.AppendLine("import Bcl.Clamp(float, float, float) as clamp");

            //Todo: datetime handling.
            //Todo: File-Handling.

            sb.AppendLine("import Bcl.EnvironmentGetVariable(string) as environment_get_variable");

            sb.AppendLine("import Math.Exp(double) as exp");
            sb.AppendLine("import Math.Floor(double) as floor");

            sb.AppendLine("import TsReflection.InstanceCreate(instance, array) as instance_create");

            sb.AppendLine("import TsObject.IsArray(object) as is_array");
            sb.AppendLine("import TsObject.IsReal(object) as is_number");
            sb.AppendLine("import TsObject.IsString(object) as is_string");
            sb.AppendLine("import TsObject.IsDelegate(object) as is_delegate");
            sb.AppendLine("import TsObject.IsNull(object) as is_null");

            sb.AppendLine("import Math.Log(double) as ln");
            sb.AppendLine("import Math.Log10(double) as log10");
            sb.AppendLine("import Math.Log(double, double) as logn");
            sb.AppendLine("import Bcl.Max(instance, array) as max");
            sb.AppendLine("import Bcl.Min(instance, array) as min");

            sb.AppendLine("import TsReflection.ObjectGetName(instance) as object_get_name");
            sb.AppendLine("import TsReflection.ObjectGetParent(instance) as object_get_parent");
            sb.AppendLine("import TsReflection.ObjectIsAncestor(string, string) as object_is_ancestor");

            sb.AppendLine("import Bcl.Random(float) as random");
            sb.AppendLine("import Bcl.RandomGetSeed() as random_get_seed");
            sb.AppendLine("import Bcl.RandomRange(float, float) as random_range");
            sb.AppendLine("import Bcl.RandomSetSeed(int) as random_set_seed");
            sb.AppendLine("import Bcl.Randomise() as randomise");

            sb.AppendLine("import Bcl.Real(string) as real");
            sb.AppendLine("import Bcl.Round(float) as round");

            sb.AppendLine("import Bcl.ScriptExists(string) as script_exists");
            sb.AppendLine("import Bcl.ShowError(string, bool) as show_error");
            sb.AppendLine("import System.Console.WriteLine(object) as print");

            sb.AppendLine("import Math.Sign(float) as sign");
            sb.AppendLine("import Bcl.Square(float) as sqr");
            sb.AppendLine("import Math.Sqrt(double) as sqrt");

            sb.AppendLine("import Bcl.ToString(object) as string;");
            sb.AppendLine("import Bcl.StringCharAt(string, int) as string_char_at");
            sb.AppendLine("import Bcl.StringCopy(string, int, int) as string_copy");
            sb.AppendLine("import Bcl.StringCount(string, string) as string_count");
            sb.AppendLine("import Bcl.StringDelete(string, int, int) as string_delete");
            sb.AppendLine("import Bcl.StringDigits(string) as string_digits");
            sb.AppendLine("import Bcl.StringInsert(string, string, int) as string_insert");
            sb.AppendLine("import Bcl.StringLength(string) as string_length");
            sb.AppendLine("import Bcl.StringLetters(string) as string_letters");
            sb.AppendLine("import Bcl.StringLettersDigits(string) as string_letters_digits");
            sb.AppendLine("import Bcl.StringLower(string) as string_lower");
            sb.AppendLine("import Bcl.StringOrdAt(string, int) as string_ord_at");
            sb.AppendLine("import Bcl.StringPos(string, string) as string_pos");
            sb.AppendLine("import Bcl.StringRepeat(string, int) as string_repeat");
            sb.AppendLine("import Bcl.StringReplace(string, string, string) as string_replace");
            sb.AppendLine("import Bcl.StringReplaceAll(string, string, string) as string_replace_all");
            sb.AppendLine("import Bcl.StringUpper(string) as string_upper");

            sb.AppendLine("import TsObject.Typeof(object) as typeof");

            sb.AppendLine("import TsReflection.VariableGlobalExists(string) as variable_global_exists");
            sb.AppendLine("import TsReflection.VariableGlobalGet(string) as variable_global_get");
            sb.AppendLine("import TsReflection.VariableGlobalGetNames() as variable_global_get_names");
            sb.AppendLine("import TsReflection.VariableGlobalSet(string, object) as variable_global_set");
            sb.AppendLine("import TsReflection.VariableInstanceExists(instance, string) as variable_instance_exists");
            sb.AppendLine("import TsReflection.VariableInstanceGet(instance, string) as variable_instance_get");
            sb.AppendLine("import TsReflection.VariableInstanceGetNames(instance) as variable_instance_get_names");
            sb.AppendLine("import TsReflection.VariableInstanceSet(instance, string, object) as variable_instance_set");
            
            sb.AppendLine("import object TsList as ds_list");
            sb.AppendLine("import object TsMap as ds_map");
            sb.AppendLine("import object TsGrid as ds_grid");

            sb.AppendLine("namespace TaffyScript.Xml {");
            sb.AppendLine("import object TaffyScript.Xml.XmlReader;");
            sb.AppendLine("import object TaffyScript.Xml.XmlReaderSettings;");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
