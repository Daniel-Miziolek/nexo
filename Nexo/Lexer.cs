﻿namespace Nexo
{
    public class Lexer(string source)
    {
        private readonly string _source = source;
        private readonly List<Token> _tokens = [];

        private int _start = 0;
        private int _current = 0;
        private int _line = 1;

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                _start = _current;
                ScanToken(); 
            }

         
            return _tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(':
                    AddToken(TokenType.LeftParen);
                    break;
                case ')':
                    AddToken(TokenType.RightParen);
                    break;
                case '{':
                    AddToken(TokenType.LeftBrace);
                    break;
                case '}':
                    AddToken(TokenType.RightBrace);
                    break;
                case '[':
                    AddToken(TokenType.LeftBracket);
                    break;
                case ']':
                    AddToken(TokenType.RightBracket);
                    break;
                case '"':
                    ScanString();                  
                    break;
                case '+':
                    AddToken(TokenType.Plus);
                    break;
                case '-':
                    AddToken(TokenType.Minus);
                    break;
                case '*':
                    AddToken(TokenType.Mul);
                    break;
                case '/':
                    AddToken(TokenType.Div);
                    break;
                case '.':
                    AddToken(TokenType.Dot);
                    break;
                case '=':
                    AddToken(TokenType.Equal);
                    break;
                case '<':
                    AddToken(TokenType.LessThan);
                    break;
                case '>':
                    AddToken(TokenType.GreaterThan);
                    break;
                case ',':
                    AddToken(TokenType.Comma);
                    break;
                case ';':
                    AddToken(TokenType.SemiColon);
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace
                    break;
                case '\n':
                    _line++;
                    break;
                case '#':
                    while (Peek() != '\n' && !IsAtEnd())
                    {
                        Advance();
                    }
                    break;
                default:
                    if (char.IsDigit(c))
                    {
                        ScanNumber();
                    }
                    else if (char.IsLetter(c))
                    {
                        ScanIdentifier();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Unexpected character '{c}' at line {_line}.");
                        Console.ResetColor();                    
                    }
                    break;
            }
        }

        private void ScanString()
        {
            string value = "";
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') _line++;
                value += Advance();
            }


            if (IsAtEnd())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unterminated string.");
                Console.ResetColor();
            }


            Advance();

            AddToken(TokenType.String, value);
        }

        private void ScanNumber()
        {
            while (char.IsDigit(Peek()))
            {
                Advance();
            }

            string number = _source[_start.._current];
            _tokens.Add(new Token(TokenType.Number, number, int.Parse(number)));
        }

        private void ScanIdentifier()
        {
            while (char.IsLetterOrDigit(Peek()))
            {
                Advance();
            }

            string text = _source[_start.._current];
            TokenType type = TokenType.Identifier;
            if (text == "print") type = TokenType.Print;
            else if (text == "if") type = TokenType.If;
            else if (text == "else") type = TokenType.Else;
            else if (text == "let") type = TokenType.Variables;
            else if (text == "const") type = TokenType.Constant;
            _tokens.Add(new Token(type, text, null));
        }

        private char Advance()
        {
            _current++;
            return _source[_current - 1];
        }

        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[_current];
        }

        private void AddToken(TokenType type, object? literal = null)
        {
            string text = _source[_start.._current];
            if (type == TokenType.String)
            {
                text = _source.Substring(_start + 1, _current - _start - 2);
            }
            _tokens.Add(new Token(type, text, literal));
        }
    }
}
