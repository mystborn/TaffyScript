using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myst.LexicalAnalysis;

namespace GmParser
{
    public class TokenStream
    {
        private IEnumerator<Token> _stream;
        private bool _finished = false;

        public bool Finished => _finished;

        public TokenStream(Lexer lexer, string input)
        {
            _stream = lexer.Tokenize(input).Where(t => t.Type != Lexer.EoF).GetEnumerator();
            Advance();
        }

        public Token Peek()
        {
            return _stream.Current;
        }

        public Token Read()
        {
            var current = Peek();
            Advance();
            return current;
        }

        public string ReadValue()
        {
            var current = Peek();
            Advance();
            return current.Value;
        }

        private void Advance()
        {
            _finished = !_stream.MoveNext();
        }
    }
}
