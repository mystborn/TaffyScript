using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Represents a position within a document/stream where a <see cref="Token"/> was found.
    /// </summary>
    public class TokenPosition
    {
        /// <summary>
        /// The position on the line where the <see cref="Token"/> was found.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// The index in the document/stream where the <see cref="Token"/> was found.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The line number where the <see cref="Token"/> was found.
        /// </summary>
        public int Line { get; }

        public string File { get; }

        public TokenPosition(int index, int line, int column, string file)
        {
            Index = index;
            Line = line;
            Column = column;
            File = file;
        }

        public override string ToString()
        {
            return $"TokenPosition: Line: {Line} | Column: {Column} | Index: {Index}";
        }
    }
}
