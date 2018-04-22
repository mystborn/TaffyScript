using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScriptCompiler.Backend
{
    /// <summary>
    /// Helper class used to generate the BCL of TaffyScript.
    /// </summary>
    public static class BaseClassLibrary
    {
        /// <summary>
        /// Generates the TaffyScript base class library as a string.
        /// </summary>
        /// <returns></returns>
        public static string Generate()
        {
            var sb = new StringBuilder();

            sb.AppendLine("import Math.Abs(float) as abs;");
            sb.AppendLine("import Bcl.AngleDifference(float, float) as angle_difference;");
            sb.AppendLine("import Math.Acos(double) as arccos;");
            sb.AppendLine("import Math.Asin(double) as arcsin;");
            sb.AppendLine("import Math.Atan(double) as arctan;");
            sb.AppendLine("import Math.Atan2(double, double) as arctan2;");
            sb.AppendLine("import Bcl.ArrayCopy(object, int, object, int, int) as array_copy;");
            sb.AppendLine("import Bcl.ArrayCreate(instance, array) as array_create;");
            sb.AppendLine("import Bcl.ArrayEquals(array, array) as array_equals");
            sb.AppendLine("import Bcl.ArrayHeight2D(array2d) as array_height_2d;");
            sb.AppendLine("import Bcl.ArrayLength1D(array) as array_length_1d");
            sb.AppendLine("import Bcl.ArrayLength2D(array2d, int) as array_length_2d");

            sb.AppendLine("import Bcl.Base64Decode(string) as base64_decode");
            sb.AppendLine("import Bcl.Base64Encode(string) as base64_encode");

            //Todo: buffer

            sb.AppendLine("import Math.Ceiling(double) as ceil");
            sb.AppendLine("import Bcl.Choose(instance, array) as choose");
            sb.AppendLine("import Bcl.Clamp(float, float, float) as clamp");

            //Todo: datetime handling.
            //Todo: File-Handling.

            sb.AppendLine("import Bcl.DotProduct(float, float, float, float) as dot_product");

            sb.AppendLine("import DsGrid.DsGridAdd(int, int, int, object) as ds_grid_add");
            sb.AppendLine("import DsGrid.DsGridAddDisk(int, int, int, int, object) as ds_grid_add_disk");
            sb.AppendLine("import DsGrid.DsGridAddGridRegion(int, int, int, int, int, int, int, int) as ds_grid_add_grid_region");
            sb.AppendLine("import DsGrid.DsGridAddRegion(int, int, int, int, int, object) as ds_grid_add_region");
            sb.AppendLine("import DsGrid.DsGridClear(int, object) as ds_grid_clear");
            sb.AppendLine("import DsGrid.DsGridCopy(int, int) as ds_grid_copy");
            sb.AppendLine("import DsGrid.DsGridCreate(int, int) as ds_grid_create");
            sb.AppendLine("import DsGrid.DsGridDestroy(int) as ds_grid_destroy");
            sb.AppendLine("import DsGrid.DsGridGet(int, int, int) as ds_grid_get");
            sb.AppendLine("import DsGrid.DsGridGetDiskMax(int, int, int, int) as ds_grid_get_disk_max");
            sb.AppendLine("import DsGrid.DsGridGetDiskMean(int, int, int, int) as ds_grid_get_disk_mean");
            sb.AppendLine("import DsGrid.DsGridGetDiskMin(int, int, int, int) as ds_grid_get_disk_min");
            sb.AppendLine("import DsGrid.DsGridGetDiskSum(int, int, int, int) as ds_grid_get_disk_sum");
            sb.AppendLine("import DsGrid.DsGridGetMax(int, int, int, int, int) as ds_grid_get_max");
            sb.AppendLine("import DsGrid.DsGridGetMean(int, int, int, int, int) as ds_grid_get_mean");
            sb.AppendLine("import DsGrid.DsGridGetMin(int, int, int, int, int) as ds_grid_get_min");
            sb.AppendLine("import DsGrid.DsGridGetSum(int, int, int, int, int) as ds_grid_get_sum");
            sb.AppendLine("import DsGrid.DsGridHeight(int) as ds_grid_height");
            sb.AppendLine("import DsGrid.DsGridMultiply(int, int, int, object) as ds_grid_multiply");
            sb.AppendLine("import DsGrid.DsGridMultiplyDisk(int, int, int, int, object) as ds_grid_multiply_disk");
            sb.AppendLine("import DsGrid.DsGridMultiplyGridRegion(int, int, int, int, int, int, int, int) as ds_grid_multiply_grid_region");
            sb.AppendLine("import DsGrid.DsGridMultiplyRegion(int, int, int, int, int, object) as ds_grid_multiply_region");
            sb.AppendLine("import DsGrid.DsGridResize(int, int, int) as ds_grid_resize");
            sb.AppendLine("import DsGrid.DsGridSet(int, int, int, object) as ds_grid_set");
            sb.AppendLine("import DsGrid.DsGridSetDisk(int, int, int, int, object) as ds_grid_set_disk");
            sb.AppendLine("import DsGrid.DsGridSetGridRegion(int, int, int, int, int, int, int, int) as ds_grid_set_grid_region");
            sb.AppendLine("import DsGrid.DsGridSetRegion(int, int, int, int, int, object) as ds_grid_set_region");
            sb.AppendLine("import DsGrid.DsGridShuffle(int) as ds_grid_shuffle");
            sb.AppendLine("import DsGrid.DsGridSort(int, int, bool) as ds_grid_sort");
            sb.AppendLine("import DsGrid.DsGridValueDiskExists(int, int, int, int, object) as ds_grid_value_disk_exists");
            sb.AppendLine("import DsGrid.DsGridValueDiskX(int, int, int, int, object) as ds_grid_value_disk_x");
            sb.AppendLine("import DsGrid.DsGridValueDiskY(int, int, int, int, object) as ds_grid_value_disk_y");
            sb.AppendLine("import DsGrid.DsGridValueExists(int, int, int, int, int, object) as ds_grid_value_exists");
            sb.AppendLine("import DsGrid.DsGridValueX(int, int, int, int, int, object) as ds_grid_value_x");
            sb.AppendLine("import DsGrid.DsGridValueY(int, int, int, int, int, object) as ds_grid_value_y");
            sb.AppendLine("import DsGrid.DsGridWidth(int) as ds_grid_width");

            sb.AppendLine("import DsList.DsListAdd(instance, array) as ds_list_add");
            sb.AppendLine("import DsList.DsListClear(int) as ds_list_clear");
            sb.AppendLine("import DsList.DsListCopy(int, int) as ds_list_copy");
            sb.AppendLine("import DsList.DsListCreate() as ds_list_create");
            sb.AppendLine("import DsList.DsListDelete(int, int) as ds_list_delete");
            sb.AppendLine("import DsList.DsListDestroy(int) as ds_list_destroy");
            sb.AppendLine("import DsList.DsListEmpty(int) as ds_list_empty");
            sb.AppendLine("import DsList.DsListFindIndex(int, object) as ds_list_find_index");
            sb.AppendLine("import DsList.DsListFindValue(int, int) as ds_list_find_value");
            sb.AppendLine("import DsList.DsListInsert(int, int, object) as ds_list_insert");
            sb.AppendLine("import DsList.DsListReplace(int, int, object) as ds_list_replace");
            sb.AppendLine("import DsList.DsListSet(instance, array) as ds_list_set");
            sb.AppendLine("import DsList.DsListShuffle(int) as ds_list_shuffle");
            sb.AppendLine("import DsList.DsListSize(int) as ds_list_size");
            sb.AppendLine("import DsList.DsListSort(int) as ds_list_sort");

            sb.AppendLine("import DsMap.DsMapAdd(int, object, object) as ds_map_add");
            sb.AppendLine("import DsMap.DsMapClear(int) as ds_map_clear");
            sb.AppendLine("import DsMap.DsMapCopy(int, int) as ds_map_copy");
            sb.AppendLine("import DsMap.DsMapCreate() as ds_map_create");
            sb.AppendLine("import DsMap.DsMapDelete(int, object) as ds_map_delete");
            sb.AppendLine("import DsMap.DsMapDestroy(int) as ds_map_destroy");
            sb.AppendLine("import DsMap.DsMapEmpty(int) as ds_map_empty");
            sb.AppendLine("import DsMap.DsMapExists(int, object) as ds_map_exists");
            sb.AppendLine("import DsMap.DsMapFindValue(int, object) as ds_map_find_value");
            sb.AppendLine("import DsMap.DsMapKeys(int) as ds_map_keys");
            sb.AppendLine("import DsMap.DsMapReplace(int, object, object) as ds_map_replace");
            sb.AppendLine("import DsMap.DsMapSize(int) as ds_map_size");

            sb.AppendLine("import Bcl.EnvironmentGetVariable(string) as environment_get_variable");
            sb.AppendLine("import Bcl.EventInherited(instance, array) as event_inherited");
            sb.AppendLine("import Bcl.EventPerform(instance, array) as event_perform");
            sb.AppendLine("import Bcl.EventPerformObject(instance, array) as event_perform_object");

            sb.AppendLine("import Math.Exp(double) as exp");
            sb.AppendLine("import Math.Floor(double) as floor");

            sb.AppendLine("import TsInstance.InstanceChange(instance, array) as instance_change");
            sb.AppendLine("import TsInstance.InstanceCopy(instance, array) as instance_copy");
            sb.AppendLine("import TsInstance.InstanceCreate(instance, array) as instance_create");
            sb.AppendLine("import TsInstance.InstanceDestroy(instance, array) as instance_destroy");
            sb.AppendLine("import TsInstance.InstanceExists(int) as instance_exists");
            sb.AppendLine("import TsInstance.InstanceFind(string, int) as instance_find");
            sb.AppendLine("import TsInstance.InstanceNumber(string) as instance_number");

            sb.AppendLine("import TsObject.IsArray(object) as is_array");
            sb.AppendLine("import TsObject.IsReal(object) as is_real");
            sb.AppendLine("import TsObject.IsString(object) as is_string");
            sb.AppendLine("import TsObject.IsDelegate(object) as is_delegate");
            sb.AppendLine("import TsObject.IsUndefined(object) as is_undefined");

            //Todo: lengthdir, lerp

            sb.AppendLine("import Math.Log(double) as ln");
            sb.AppendLine("import Math.Log10(double) as log10");
            sb.AppendLine("import Math.Log(double, double) as logn");
            sb.AppendLine("import Bcl.Max(instance, array) as max");
            sb.AppendLine("import Bcl.Min(instance, array) as min");

            sb.AppendLine("import TsInstance.ObjectGetName(instance) as object_get_name");
            sb.AppendLine("import TsInstance.ObjectGetParent(instance) as object_get_parent");
            sb.AppendLine("import TsInstance.ObjectIsAncestor(string, string) as object_is_ancestor");

            sb.AppendLine("import Bcl.Random(float) as random");
            sb.AppendLine("import Bcl.RandomGetSeed() as random_get_seed");
            sb.AppendLine("import Bcl.RandomRange(float, float) as random_range");
            sb.AppendLine("import Bcl.RandomSetSeed(int) as random_set_seed");
            sb.AppendLine("import Bcl.Randomise() as randomise");

            sb.AppendLine("import Bcl.Real(string) as real");
            sb.AppendLine("import Bcl.Round(float) as round");

            sb.AppendLine("import Bcl.ScriptExecute(instance, array) as script_execute");
            sb.AppendLine("import Bcl.ScriptExists(string) as script_exists");
            sb.AppendLine("import Bcl.ShowError(string, bool) as show_error");
            sb.AppendLine("import System.Console.WriteLine(object) as show_debug_message;");

            sb.AppendLine("import Math.Sign(float) as sign");
            sb.AppendLine("import Bcl.Square(float) as sqr");
            sb.AppendLine("import Math.Sqrt(double) as sqrt");

            sb.AppendLine("import Bcl.ToString(object) as string;");
            sb.AppendLine("import Bcl.StringByteAt(string, int) as string_byte_at");
            sb.AppendLine("import Bcl.StringByteLength(string) as string_byte_length");
            sb.AppendLine("import Bcl.StringCharAt(string, int) as string_char_at");
            sb.AppendLine("import Bcl.StringCopy(string, int, int) as string_copy");
            sb.AppendLine("import Bcl.StringCount(string, string) as string_count");
            sb.AppendLine("import Bcl.StringDelete(string, int, int) as string_delete");
            sb.AppendLine("import Bcl.StringDigits(string) as string_digits");
            sb.AppendLine("import Bcl.StringInsert(string, string, int) as string_insert");
            sb.AppendLine("import Bcl.StringLength(string) as string_length");
            sb.AppendLine("import Bcl.StringLetters(string) as string_letters");
            sb.AppendLine("import Bcl.StringLettersDigits(string) as string_lettersdigits");
            sb.AppendLine("import Bcl.StringLower(string) as string_lower");
            sb.AppendLine("import Bcl.StringOrdAt(string, int) as string_ord_at");
            sb.AppendLine("import Bcl.StringPos(string, string) as string_pos");
            sb.AppendLine("import Bcl.StringRepeat(string, int) as string_repeat");
            sb.AppendLine("import Bcl.StringReplace(string, string, string) as string_replace");
            sb.AppendLine("import Bcl.StringReplaceAll(string, string, string) as string_replace_all");
            sb.AppendLine("import Bcl.StringSetByte(string, int, int) as string_set_byte");
            sb.AppendLine("import Bcl.StringUpper(string) as string_upper");

            sb.AppendLine("import TsObject.Typeof(object) as typeof");

            sb.AppendLine("import TsInstance.VariableGlobalExists(string) as variable_global_exists");
            sb.AppendLine("import TsInstance.VariableGlobalGet(string) as variable_global_get");
            sb.AppendLine("import TsInstance.VariableGlobalGetNames() as variable_global_get_names");
            sb.AppendLine("import TsInstance.VariableGlobalSet(string, object) as variable_global_set");
            sb.AppendLine("import TsInstance.VariableInstanceExists(instance, string) as variable_instance_exists");
            sb.AppendLine("import TsInstance.VariableInstanceGet(instance, string) as variable_instance_get");
            sb.AppendLine("import TsInstance.VariableInstanceGetNames(instance) as variable_instance_get_names");
            sb.AppendLine("import TsInstance.VariableInstanceSet(instance, string, object) as variable_instance_set");

            return sb.ToString();
        }
    }
}
