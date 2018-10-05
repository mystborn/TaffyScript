using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TaffyScript.Compiler.FrontEnd
{
    public class Tokenizer
    {
        private IErrorLogger _logger;
        private string _source;
        private int _line = 1;
        private int _column = 0;
        private int _start = 0;
        private int _index = 0;
        private string _fname;

        private static Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>()
        {
            { "and", TokenType.LogicalAnd },
            { "argument", TokenType.Argument },
            { "argument_count", TokenType.ReadOnly },
            { "as", TokenType.As },
            { "base", TokenType.Base },
            { "break", TokenType.Break },
            { "case", TokenType.Case },
            { "continue", TokenType.Continue },
            { "default", TokenType.Default },
            { "do", TokenType.Do },
            { "else", TokenType.Else },
            { "enum", TokenType.Enum },
            { "false", TokenType.Bool },
            { "for", TokenType.For },
            { "global", TokenType.ReadOnly },
            { "if", TokenType.If },
            { "import", TokenType.Import },
            { "namespace", TokenType.Namespace },
            { "new", TokenType.New },
            { "noone", TokenType.ReadOnly },
            { "null", TokenType.ReadOnly },
            { "object", TokenType.Object },
            { "or", TokenType.LogicalOr },
            { "public", TokenType.AccessModifier },
            { "private", TokenType.AccessModifier },
            { "protected", TokenType.AccessModifier },
            { "repeat", TokenType.Repeat },
            { "return", TokenType.Return },
            { "script", TokenType.Script },
            { "sealed", TokenType.AccessModifier },
            { "self", TokenType.ReadOnly },
            { "static", TokenType.Static },
            { "switch", TokenType.Switch },
            { "true", TokenType.Bool },
            { "until", TokenType.Until },
            { "using", TokenType.Using },
            { "var", TokenType.Var },
            { "while", TokenType.While }
        };

        // The Tokenizer needs to determine if the stream is finished
        // before outside code does because the last Token will still need
        // to be read if there is no whitespace at the end of a file.

        private bool InternalFinished => _index >= _source.Length;

        public bool Finished => Current != null && Current.Type == TokenType.EoF;
        public TokenPosition Position => new TokenPosition(_index, _line, _column, _fname);
        public Token Previous { get; private set; }
        public Token Current { get; private set; }

        public Tokenizer(string input, IErrorLogger errorLogger)
        {
            _logger = errorLogger;
            _source = input;
            _fname = null;
            Read();
        }

        public Tokenizer(string input, string fname, IErrorLogger errorLogger)
        {
            _logger = errorLogger;
            _source = input;
            _fname = fname;
            Read();
        }

        public Token Read()
        {
            Previous = Current;
            Current = InternalRead();
            return Previous;
        }

        private Token InternalRead()
        {
            SkipWhitespace();

            _start = _index;

            if (_index >= _source.Length)
                return MakeToken(TokenType.EoF);

            var c = Advance();

            switch(c)
            {
                case '(': return MakeToken(TokenType.OpenParen);
                case ')': return MakeToken(TokenType.CloseParen);
                case '{': return MakeToken(TokenType.OpenBrace);
                case '}': return MakeToken(TokenType.CloseBrace);
                case '[': return MakeToken(TokenType.OpenBracket);
                case ']': return MakeToken(TokenType.CloseBracket);
                case '~': return MakeToken(TokenType.Complement);
                case ';': return MakeToken(TokenType.SemiColon);
                case ':': return MakeToken(TokenType.Colon);
                case ',': return MakeToken(TokenType.Comma);
                
                //<, <=, <<, <>
                case '<': return MakeToken(Match('=') ? TokenType.LessThanOrEqual : Match('<') ? TokenType.ShiftLeft : Match('>') ? TokenType.NotEqual : TokenType.LessThan);

                case '&': return MakeToken(Match('=') ? TokenType.AndEquals : Match('&') ? TokenType.LogicalAnd : TokenType.BitwiseAnd);
                case '|': return MakeToken(Match('=') ? TokenType.OrEquals : Match('|') ? TokenType.LogicalOr : TokenType.BitwiseOr);
                case '>': return MakeToken(Match('=') ? TokenType.GreaterThanOrEqual : Match('>') ? TokenType.ShiftRight : TokenType.GreaterThan);
                case '+': return MakeToken(Match('=') ? TokenType.PlusEquals : Match('+') ? TokenType.Increment : TokenType.Plus);
                case '-': return MakeToken(Match('=') ? TokenType.SubEquals : Match('-') ? TokenType.Decrement : TokenType.Minus);

                case '=': return MakeToken(Match('=') ? TokenType.Equal : TokenType.Assign);
                case '*': return MakeToken(Match('=') ? TokenType.MulEquals : TokenType.Multiply);
                case '/': return MakeToken(Match('=') ? TokenType.DivEquals : TokenType.Divide);
                case '%': return MakeToken(Match('=') ? TokenType.ModEquals : TokenType.Modulo);
                case '^': return MakeToken(Match('=') ? TokenType.XorEquals : TokenType.Xor);
                case '!': return MakeToken(Match('=') ? TokenType.NotEqual : TokenType.Not);

                case '\'':
                case '"':
                    return StringLiteral(c);

                case '.':
                    if(IsDigit(PeekChar()))
                        return NumberLiteral(false);
                    return MakeToken(TokenType.Dot);
                case '?':
                    if(IsHex(PeekChar()))
                    {
                        Advance();
                        while (IsHex(PeekChar()))
                            Advance();
                        return MakeToken(TokenType.Number);
                    }
                    else
                        return MakeToken(TokenType.QuestionMark);
                case '0':
                    if (Match('x'))
                    {
                        while (IsHex(PeekChar()))
                            Advance();
                        return MakeToken(TokenType.Number);
                    }
                    else
                        return NumberLiteral(true);
                default:
                    if (IsAlpha(c))
                        return Identifier();
                    if (IsDigit(c))
                        return NumberLiteral(true);
                    break;
            }

            _logger.Error("Unexpected character", new TokenPosition(_index, _line, _column, _fname));
            return null;
        }

        private Token MakeToken(TokenType type)
        {
            return new Token(type, _source.Substring(_start, _index - _start), new TokenPosition(_start, _line, _column, _fname));
        }

        private bool IsHex(char c)
        {
            return (c >= 'a' && c <= 'f') ||
                   (c >= 'A' && c <= 'Z') ||
                   IsDigit(c);
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   c == '_';
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private char Advance()
        {
            _column++;
            return _source[_index++];
        }

        private char PeekChar()
        {
            return _source[_index];
        }

        private char PeekNext()
        {
            if (InternalFinished)
                return '\0';

            return _source[_index + 1];
        }

        private bool Match(char expected)
        {
            if (InternalFinished || _source[_index] != expected)
                return false;

            Advance();
            return true;
        }

        private void SkipWhitespace()
        {
            while(!InternalFinished)
            {
                switch(PeekChar())
                {
                    case ' ':
                    case '\r':
                    case '\t':
                    case '\v':
                    case '\f':
                        Advance();
                        break;
                    case '\n':
                        _line++;
                        _column = 0;
                        Advance();
                        break;
                    case '/':
                        switch(PeekNext())
                        {
                            case '/':
                                while (PeekChar() != '\n' && !InternalFinished)
                                    Advance();
                                break;
                            case '*':
                                Advance();
                                Advance();
                                while(!InternalFinished && !(PeekChar() == '*' && PeekNext() == '/'))
                                {
                                    if (PeekChar() == '\n')
                                    {
                                        _line++;
                                        _column = 0;
                                    }
                                    Advance();
                                }
                                if (!InternalFinished)
                                {
                                    Advance();
                                    Advance();
                                }
                                break;
                            default:
                                return;
                        }
                        break;
                    default:
                        return;
                }
            }
        }

        private Token Identifier()
        {
            var c = PeekChar();
            while(IsAlpha(c) || IsDigit(c))
            {
                Advance();
                c = PeekChar();
            }

            var text = _source.Substring(_start, _index - _start);
            if(text.StartsWith("argument"))
            {
                if (text == "argument_count")
                    return MakeToken(TokenType.ReadOnly);

                if(text.Length > 8)
                    for (var i = 8; i < text.Length; i++)
                        if (!IsDigit(text[i]))
                            return MakeToken(TokenType.Identifier);

                return MakeToken(TokenType.Argument);
            }

            if (_keywords.TryGetValue(text, out var type))
                return MakeToken(type);

            return MakeToken(TokenType.Identifier);
        }

        private Token NumberLiteral(bool decimalAllowed)
        {
            while (IsDigit(PeekChar()))
                Advance();

            if(decimalAllowed && PeekChar() == '.' && IsDigit(PeekNext()))
            {
                Advance();

                while (IsDigit(PeekChar()))
                    Advance();
            }

            return MakeToken(TokenType.Number);
        }

        private Token StringLiteral(char start)
        {
            while(PeekChar() != start && !InternalFinished)
            {
                switch(PeekChar())
                {
                    case '\n':
                        _line++;
                        _column = 0;
                        break;
                    case '\\':
                        Advance();
                        if(InternalFinished)
                        {
                            _logger.Error("Encountered an unterminated string", new TokenPosition(_start, _line, _column, _fname));
                            return MakeToken(TokenType.String);
                        }
                        break;
                }
                Advance();
            }

            if (InternalFinished)
            {
                _logger.Error("Encountered an unterminated string", new TokenPosition(_start, _line, _column, _fname));
                return MakeToken(TokenType.String);
            }

            Advance();
            return MakeToken(TokenType.String);
        }
    }
}