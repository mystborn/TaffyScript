using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TaffyScriptCompiler.FrontEnd
{
    // This class uses many switch statements and a few goto statements in an effort to be as swift as possible.

    /// <summary>
    /// Used to convert TaffyScript code into a stream of <see cref="Token"/>s that can be consumed by a TokenParser.
    /// </summary>
    public class Tokenizer : IDisposable
    {
        private TextReader _reader;
        private int _line = 1;
        private int _column;
        private int _index;
        private bool _finished;
        private char _current;
        private HashSet<char> _whitespace;
        private Dictionary<string, string> _definitions;
        private Token _token;
        private string _fname = null;

        /// <summary>
        /// Determines whether the current document has been fully read.
        /// </summary>
        public bool Finished => _current == '\0';

        /// <summary>
        /// Gets invoked when an error occurs during the lexical analysis phase.
        /// </summary>
        public Action<Exception> ErrorEncountered;

        /// <summary>
        /// Creates a Tokenizer using a string as the input code.
        /// </summary>
        /// <param name="input">The code to convert into a stream of <see cref="Token"/>s.</param>
        public Tokenizer(string input)
        {
            _reader = new StringReader(input);
            Init();
        }

        /// <summary>
        /// Creates a Tokenizer using a <see cref="FileStream"/> as the input code.
        /// </summary>
        /// <param name="stream">The code to convert into a stream of <see cref="Token"/>s.</param>
        public Tokenizer(FileStream stream)
        {
            _fname = stream.Name;
            _reader = new StreamReader(stream);
            Init();
        }

        private void Init()
        {
            // Defines the whitespace characters that should be ignored.
            _whitespace = new HashSet<char>() { '\r', '\t', '\v', '\f', ' ', '\n' };
            
            // Defines the language keywords and other constructs.
            // Strings, numbers, and identifiers get processed under a special case.
            _definitions = new Dictionary<string, string>()
            {
                { "true", "bool" },
                { "false", "bool" },
                { "var", "local" },
                { "break", "break" },
                { "continue", "continue" },
                { "while", "while" },
                { "repeat", "repeat" },
                { "do", "do" },
                { "until", "until" },
                { "if", "if" },
                { "else", "else" },
                { "for", "for" },
                { "return", "return" },
                { "exit", "exit" },
                { "switch", "switch" },
                { "case", "case" },
                { "default", "default" },
                { "enum", "enum" },
                { "with", "with" },
                { "import", "import" },
                { "script", "script" },
                { "argument", "argument" },
                { "argument_count", "readonly" },
                { "all", "readonly" },
                { "noone", "readonly" },
                { "id", "readonly" },
                { "self", "readonly" },
                { "pi", "readonly" },
                { "global", "readonly" },
                { "instance_count", "readonly" },
                { "object", "object" },
                { "event", "event" },
                { "as", "as" },
                { "and", "&&" },
                { "or", "||" },
                { ";", ";" },
                { "||", "||" },
                { "&&", "&&" },
                { "!=", "!=" },
                { "<>", "!=" },
                { "==", "==" },
                { "<=", "<=" },
                { ">=", ">=" },
                { "<", "<" },
                { ">", ">" },
                { "!", "!" },
                { "~", "~" },
                { "^", "^" },
                { "|", "|" },
                { "&", "&" },
                { "<<", "<<" },
                { ">>", ">>" },
                { "(", "(" },
                { ")", ")" },
                { "[", "[" },
                { "]", "]" },
                { "{", "{" },
                { "}", "}" },
                { ".", "." },
                { "=", "=" },
                { "++", "++" },
                { "--", "--" },
                { "+=", "+=" },
                { "-=", "-=" },
                { "*=", "*=" },
                { "/=", "/=" },
                { "%=", "%=" },
                { "+", "+" },
                { "-", "-" },
                { "*", "*" },
                { "/", "/" },
                { "%", "%" },
                { ",", "," },
                { "?", "?" },
                { "#", "#" },
                { ":", ":" },
            };
            for (var i = 0; i < 16; i++)
                _definitions.Add($"argument{i}", "argument");

            // Set _current to the first character. Needed to prevent an error when callinf ReadNext for the first time.
            TryReadNext();

            // Set the first token.
            ReadWhiteSpace();
            Advance();
        }

        /// <summary>
        /// Gets the next <see cref="Token"/> without reading it.
        /// </summary>
        public Token Peek()
        {
            return _token;
        }

        /// <summary>
        /// Reads the next token.
        /// </summary>
        public Token Read()
        {
            if (_finished)
            {
                _current = '\0';
                return _token;
            }
            var current = Peek();
            Advance();
            return current;
        }

        /// <summary>
        /// Reads the next token and returns its value.
        /// </summary>
        public string ReadValue()
        {
            if (_finished)
            {
                _current = '\0';
                return _token.Value;
            }
            var current = Peek();
            Advance();
            return current.Value;
        }

        /// <summary>
        /// Advances the stream to the next token.
        /// </summary>
        private void Advance()
        {
            var next = ReadNext();
            var pos = new TokenPosition(_index - next.Length, _line, _column - next.Length, _fname);
            if (_definitions.TryGetValue(next, out var def))
                _token = new Token(def, next, pos);
            else
            {
                switch (next[0])
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '?':
                        _token = new Token("num", next, pos);
                        break;
                    case '\'':
                    case '"':
                        _token = new Token("string", next, pos);
                        break;
                    case '/':
                        ReadWhiteSpace();
                        if (!_finished)
                            Advance();
                        else
                            _current = '\0';
                        break;
                    case '*':
                        ReadWhiteSpace();
                        if (!_finished)
                            Advance();
                        else
                            _current = '\0';
                        Advance();
                        break;
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'T':
                    case 'U':
                    case 'V':
                    case 'W':
                    case 'X':
                    case 'Y':
                    case 'Z':
                    case '_':
                        _token = new Token("id", next, pos);
                        break;
                    default:
                        ErrorEncountered?.Invoke(new UnrecognizedTokenException(next[0], pos));
                        break;
                        
                }
            }

            ReadWhiteSpace();
        }
 
        private string ReadNext()
        {
            StringBuilder sb = new StringBuilder(new string(_current, 1));
            var value = _current;

            // If we've reached the end of the stream, return the final character by itself. Otherwise try and get the rest of the string.
            if (!TryReadNext())
                return sb.ToString();

            switch(value)
            {
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                case '_':
                    sb.Append(ReadCharacters());
                    break;
                case '0':
                    //Reads a hex number.
                    if (_current == 'x')
                    {
                        sb.Append(_current);
                        TryReadNext();
                        sb.Append(ReadHexNumber());
                        //There must be at least one character after the x in a hex number.
                        if (sb.Length == 2)
                            ErrorEncountered?.Invoke(new UnrecognizedTokenException(_current, new TokenPosition(_index, _line, _column, _fname)));
                        return sb.ToString();
                    }
                    else
                        sb.Append(ReadNumber(false));
                    break;
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    sb.Append(ReadNumber(false));
                    break;
                case '.':
                    //Floating point number with no preceding value.
                    sb.Append(ReadNumber(true));
                    break;
                case '|':
                    if (_current == '|')
                    {
                        sb.Append(_current);
                        TryReadNext();
                    }
                    break;
                case '&':
                    if (_current == '&')
                    {
                        sb.Append(_current);
                        TryReadNext();
                    }
                    break;
                case '!':
                    sb.Append(ReadAssignment());
                    break;
                case '=':
                    sb.Append(ReadAssignment());
                    break;
                case '<':
                    switch (_current)
                    {
                        case '>':
                        case '<':
                        case '=':
                            sb.Append(_current);
                            TryReadNext();
                            break;
                        default:
                            break;
                    }
                    break;
                case '>':
                    if(_current == '>' || _current == '=')
                    {
                        sb.Append(_current);
                        TryReadNext();
                    }
                    break;
                case '\'':
                    char prev;
                    do
                    {
                        prev = _current;
                        sb.Append(ReadAny());
                    }
                    while (prev != '\'');
                    break;
                case '"':
                    do
                    {
                        prev = _current;
                        sb.Append(ReadAny());
                    }
                    while (prev != '"');
                    break;
                case '+':
                    if(_current == '+' || _current == '=')
                    {
                        sb.Append(_current);
                        TryReadNext();
                    }
                    break;
                case '-':
                    if(_current == '-' || _current == '=')
                    {
                        sb.Append(_current);
                        TryReadNext();
                    }
                    break;
                case '*':
                    sb.Append(ReadAssignment());
                    break;
                case '%':
                    sb.Append(ReadAssignment());
                    break;
                case '/':
                    if (_current == '/')
                    {
                        sb.Append(_current);
                        char end;
                        do
                        {
                            end = ReadAny();
                            sb.Append(end);
                        }
                        while (end != '\n');
                    }
                    else if (_current == '*')
                    {
                        sb.Append(_current);
                        char end = _current;
                        do
                        {
                            if (_finished)
                            {
                                _current = '\0';
                                goto End;
                            }
                            prev = ReadAny();
                            end = _current;
                            sb.Append(end);
                        }
                        while (prev != '*' || end != '/');
                        TryReadNext();
                    }
                    else
                        sb.Append(ReadAssignment());
                    End:
                    break;
                case '?':
                    sb.Append(ReadHexNumber());
                    break;
                default:
                    break;
            }

            return sb.ToString();
        }

        private string ReadNumber(bool hasDecimalPoint)
        {
            var sb = new StringBuilder();
            while(!_finished)
            {
                switch (_current)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        sb.Append(_current);
                        break;
                    case '.':
                        if (!hasDecimalPoint)
                            sb.Append(_current);
                        else
                            goto End;
                        break;
                    default:
                        return sb.ToString();
                }
                TryReadNext();
            }
            End:
            return sb.ToString();
        }

        private string ReadHexNumber()
        {
            var sb = new StringBuilder();
            while (!_finished)
            {
                switch (_current)
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        sb.Append(_current);
                        break;
                    default:
                        return sb.ToString();
                }
                TryReadNext();
            }
            return sb.ToString();
        }

        private string ReadCharacters()
        {
            var sb = new StringBuilder();
            while(!_finished)
            {
                switch (_current)
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'T':
                    case 'U':
                    case 'V':
                    case 'W':
                    case 'X':
                    case 'Y':
                    case 'Z':
                    case '_':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        sb.Append(_current);
                        break;
                    default:
                        goto End;
                }
                TryReadNext();
            }
            End:
            return sb.ToString();
        }

        private void ReadWhiteSpace()
        {
            while (!_finished)
            {
                switch(_current)
                {
                    case '\n':
                        _line += 1;
                        _column = 0;
                        break;
                    case '\t':
                    case '\r':
                    case '\v':
                    case '\f':
                    case ' ':
                        break;
                    default:
                        goto End;
                }
                TryReadNext();
            }

            End:
            return;
        }

        private char ReadAny()
        {
            var value = _current;
            TryReadNext();
            switch(value)
            {
                case '\n':
                    _line += 1;
                    _column = 0;
                    break;
                default:
                    break;
            }
            return value;
        }

        /// <summary>
        /// Helper method for {operator}= tokens. i.e. +=, -=, etc.
        /// </summary>
        /// <returns></returns>
        private string ReadAssignment()
        {
            if(_current == '=')
            {
                TryReadNext();
                return "=";
            }
            return "";
        }

        private bool TryReadNext()
        {
            if (_finished)
                return false;

            var next = _reader.Read();
            if (next == -1)
                _finished = true;
            else
            {
                _current = (char)next;
                ++_column;
                ++_index;
            }

            return !_finished;
        }

        private void Dispose(bool disposing)
        {
            if (_reader == null || !disposing)
                return;

            _reader.Dispose();
            _reader = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
