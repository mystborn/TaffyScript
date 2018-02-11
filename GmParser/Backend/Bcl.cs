using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser.Backend
{
    public class Bcl
    {
        public static string Generate()
        {
            var sb = new StringBuilder();
            sb.AppendLine("import Bcl.ToString(array) as string;");
            sb.AppendLine("import System.Console.WriteLine(object) as show_debug_message;");

            sb.AppendLine("import Math.Abs(double) as abs;");
            sb.AppendLine("import Bcl.AngleDifference(array) as angle_difference;");
            sb.AppendLine("import Bcl.ArcCos(array) as arccos;");
            sb.AppendLine("import Bcl.ArcSin(array) as arcsin;");
            sb.AppendLine("import Bcl.ArcTan(array) as arctan;");
            sb.AppendLine("import Bcl.ArcTan2(array) as arctan2;");
            sb.AppendLine("import Bcl.ArrayCopy(array) as array_copy;");
            sb.AppendLine("import Bcl.ArrayCreate(array) as array_create;");
            sb.AppendLine("import Bcl.ArrayEquals(array) as array_equals");
            sb.AppendLine("import Bcl.ArrayHeight2D(array) as array_height_2d;");
            sb.AppendLine("import Bcl.ArrayLength1D(array) as array_length_1d");
            sb.AppendLine("import Bcl.ArrayLength2D(array) as array_length_2d");
            //Todo: asset_get_type, asset_get_index
            sb.AppendLine("import Bcl.Base64Decode(array) as base64_decode");
            sb.AppendLine("import Bcl.Base64Encode(array) as base64_encode");
            //Todo: buffer
            sb.AppendLine("import Bcl.Ceil(double) as ceil");
            sb.AppendLine("import Bcl.Choose(array) as choose");
            sb.AppendLine("import Bcl.Clamp(double, double, double) as clamp");
            sb.AppendLine("import Bcl.CodeIsCompiled() as code_is_compiled");
            //Todo: colour_get_hsv
            //Todo: d-trig funcs
            //Todo: datetime handling.
            //Todo: File-Handling
            sb.AppendLine("import Bcl.DotProduct(double, double, double, double) as dot_product");
            sb.AppendLine("import GmInstance.InstanceCreate(string) as instance_create");
            sb.AppendLine("import GmInstance.InstanceDestroy(double) as instance_destroy");

            return sb.ToString();
        }
    }
}
