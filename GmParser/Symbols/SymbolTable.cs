using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser
{
    public class SymbolTable
    {
        private SymbolNode _root;
        private SymbolNode _current;

        public IEnumerable<ISymbol> Symbols => _current.Children.Values;

        public SymbolTable()
        {
            _root = new SymbolNode(null, "__0Root", SymbolType.Block);
            _current = _root;
            InitBaseClassLibrary();
        }

        public void Enter(string branchName)
        {
            var symbol = _current.Children[branchName];
            if (!symbol.IsLeaf)
                _current = (SymbolNode)symbol;
            else
                throw new InvalidOperationException($"Could not enter scope {branchName}. Was a leaf node.");
        }

        public void Exit()
        {
            _current = _current.Parent;
        }

        public void EnterNew(string branchName, SymbolType type)
        {
            _current = _current.EnterNew(branchName, type);
        }

        public bool Defined(string name, out ISymbol symbol)
        {
            var current = _current;
            symbol = default(ISymbol);
            while(current != null)
            {
                if(current.Children.TryGetValue(name, out symbol))
                    return true;
                current = current.Parent;
            }
            return false;
        }

        public bool AddLeaf(string name, SymbolType type, SymbolScope scope)
        {
            if(!Defined(name, out var overwrite))
            {
                _current.Children.Add(name, new SymbolLeaf(_current, name, type, scope));
                return true;
            }
            return false;
        }

        public void AddPending(string name)
        {
            _current.AddPending(name);
        }

        public void PrintTable()
        {
            var sb = new StringBuilder();
            PrintNode(sb, _root, 0);
            Console.WriteLine(sb);
        }

        private void PrintNode(StringBuilder sb, ISymbol symbol, int indent)
        {
            sb.Append(' ', indent);
            sb.AppendLine(symbol.Name);
            if(symbol is SymbolNode node)
                foreach(var child in node.Children.Values)
                    PrintNode(sb, child, indent + 2);
        }

        public void PrintPending()
        {
            var sb = new StringBuilder();
            PrintPending(sb, _root, 0);
            Console.WriteLine(sb);
        }

        private void PrintPending(StringBuilder sb, ISymbol symbol, int indent)
        {
            if(symbol is SymbolNode node)
            {
                foreach(var pending in node.Pending)
                {
                    sb.Append(' ', indent);
                    sb.AppendLine(pending);
                }
                foreach (var child in node.Children.Values)
                    PrintPending(sb, child, indent);
            }
        }

        private void InitBaseClassLibrary()
        {
            AddGlobalMethod("string");
            /*AddGlobalMethod("ds_grid_add");
            AddGlobalMethod("ds_grid_add_disk");
            AddGlobalMethod("ds_grid_add_grid_region");
            AddGlobalMethod("ds_grid_add_region");
            AddGlobalMethod("ds_grid_clear");
            AddGlobalMethod("ds_grid_copy");
            AddGlobalMethod("ds_grid_create");
            AddGlobalMethod("ds_grid_destroy");
            AddGlobalMethod("ds_grid_get");
            AddGlobalMethod("ds_grid_get_disk_max");
            AddGlobalMethod("ds_grid_get_disk_mean");
            AddGlobalMethod("ds_grid_get_disk_min");
            AddGlobalMethod("ds_grid_get_disk_sum");
            AddGlobalMethod("ds_grid_get_max");
            AddGlobalMethod("ds_grid_get_mean");
            AddGlobalMethod("ds_grid_get_min");
            AddGlobalMethod("ds_grid_get_sum");
            AddGlobalMethod("ds_grid_height");
            AddGlobalMethod("ds_grid_mulitply");
            AddGlobalMethod("ds_grid_multiply_disk");
            AddGlobalMethod("ds_grid_multiply_grid_region");
            AddGlobalMethod("ds_grid_multiply_region");
            AddGlobalMethod("ds_grid_read");
            AddGlobalMethod("ds_grid_resize");
            AddGlobalMethod("ds_grid_set");
            AddGlobalMethod("ds_grid_set_disk");
            AddGlobalMethod("ds_grid_set_grid_region");
            AddGlobalMethod("ds_grid_region");
            AddGlobalMethod("ds_grid_shuffle");
            AddGlobalMethod("ds_grid_sort");
            AddGlobalMethod("ds_grid_value_disk_exists");
            AddGlobalMethod("ds_grid_value_disk_x");
            AddGlobalMethod("ds_grid_value_disk_y");
            AddGlobalMethod("ds_grid_value_exists");
            AddGlobalMethod("ds_grid_value_x");
            AddGlobalMethod("ds_grid_value_y");
            AddGlobalMethod("ds_grid_width");
            AddGlobalMethod("ds_grid_write");
            AddGlobalMethod("ds_list_add");
            AddGlobalMethod("ds_list_clear");
            AddGlobalMethod("ds_list_copy");
            AddGlobalMethod("ds_list_create");
            AddGlobalMethod("ds_list_delete");
            AddGlobalMethod("ds_list_destroy");
            AddGlobalMethod("ds_list_empty");
            AddGlobalMethod("ds_list_find_index");
            AddGlobalMethod("ds_list_find_value");
            AddGlobalMethod("ds_list_insert");
            AddGlobalMethod("ds_list_mask_as_list");
            AddGlobalMethod("ds_list_mark_as_value");
            AddGlobalMethod("ds_list_read");
            AddGlobalMethod("ds_list_replace");
            AddGlobalMethod("ds_list_set");
            AddGlobalMethod("ds_list_shuffle");
            AddGlobalMethod("ds_list_size");
            AddGlobalMethod("ds_list_sort");
            AddGlobalMethod("ds_list_write");
            AddGlobalMethod("ds_map_add");
            AddGlobalMethod("ds_map_add_list");
            AddGlobalMethod("ds_map_add_map");
            AddGlobalMethod("ds_map_clear");
            AddGlobalMethod("ds_map_copy");
            AddGlobalMethod("ds_map_create");
            AddGlobalMethod("ds_map_delete");
            AddGlobalMethod("ds_map_destroy");
            AddGlobalMethod("ds_map_empty");
            AddGlobalMethod("ds_map_exists");
            AddGlobalMethod("ds_map_find_first");
            AddGlobalMethod("ds_map_find_last");
            AddGlobalMethod("ds_map_find_next");
            AddGlobalMethod("ds_map_find_previous");
            AddGlobalMethod("ds_map_find_value");
            AddGlobalMethod("ds_map_read");
            AddGlobalMethod("ds_map_replace");
            AddGlobalMethod("ds_map_replace_list");
            AddGlobalMethod("ds_map_replace_map");
            AddGlobalMethod("ds_map_secure_load");
            AddGlobalMethod("ds_map_secure_write");
            AddGlobalMethod("ds_map_size");
            AddGlobalMethod("ds_map_write");
            AddGlobalMethod("ds_queue_clear");
            AddGlobalMethod("ds_queue_copy");
            AddGlobalMethod("ds_queue_create");
            AddGlobalMethod("ds_queue_dequeue");
            AddGlobalMethod("ds_queue_destroy");
            AddGlobalMethod("ds_queue_empty");
            AddGlobalMethod("ds_queue_enqueue");
            AddGlobalMethod("ds_queue_head");
            AddGlobalMethod("ds_queue_read");
            AddGlobalMethod("ds_queue_size");
            AddGlobalMethod("ds_queue_tail");
            AddGlobalMethod("ds_queue_write");
            AddGlobalMethod("ds_stack_clear");
            AddGlobalMethod("ds_stack_copy");
            AddGlobalMethod("ds_stack_create");
            AddGlobalMethod("ds_stack_destroy");
            AddGlobalMethod("ds_stack_empty");
            AddGlobalMethod("ds_stack_pop");
            AddGlobalMethod("ds_stack_push");
            AddGlobalMethod("ds_stack_read");
            AddGlobalMethod("ds_stack_size");
            AddGlobalMethod("ds_stack_top");
            AddGlobalMethod("ds_stack_write");
            AddGlobalMethod("show_debug_message");*/
        }

        private void AddGlobalMethod(string name)
        {
            AddLeaf(name, SymbolType.Script, SymbolScope.Global);
        }
    }
}
