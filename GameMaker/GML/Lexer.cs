using GameMaker.ProjectCommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker.GML
{
    public class Lexer
    {
        private static Dictionary<string, GMLVariable> Variables { get; set; } = new Dictionary<string, GMLVariable>();
        private static Project ProjectContext { get; set; } = null;
        private static string Script { get; set; } = string.Empty;
        private static List<GMLError> TokenizeErrors { get; set; } = null;
        private static bool TokenizeError { get; set; } = false;
        private static string ScriptName { get; set; } = "";
        private static void SkipWhitespace(string _script, ref int _index)
        {
            bool flag = false;
            while (!flag)
            {
                while (_index < _script.Length && char.IsWhiteSpace(_script[_index]))
                {
                    _index++;
                }
                if (_index < _script.Length && _script[_index] == '/')
                {
                    if (_index + 1 < _script.Length)
                    {
                        switch (_script[_index + 1])
                        {
                            case '*':
                                _index += 2;
                                while (_index < _script.Length && (_script[_index] != '*' || _script[_index + 1] != '/'))
                                {
                                    _index++;
                                }
                                if (_index >= _script.Length)
                                {
                                    TokenizeErrors.Add(new GMLError(ErrorKind.Warning_Unclosed_Comment, "unclosed comment (/*) at tend of script", null, _index));
                                }
                                else
                                {
                                    _index += 2;
                                }
                                break;
                            case '/':
                                _index += 2;
                                while (_index < _script.Length && _script[_index] != '\n' && _script[_index] != '\r')
                                {
                                    _index++;
                                }
                                _index++;
                                break;
                            default:
                                flag = true;
                                break;
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                }
                else
                {
                    flag = true;
                }
            }
        }

        private static GMLToken NextName(string _script, ref int _index)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int index = _index;
            stringBuilder.Append(_script[_index]);
            _index++;
            while (_index < _script.Length && (char.IsLetterOrDigit(_script[_index]) || _script[_index] == '_'))
            {
                stringBuilder.Append(_script[_index]);
                _index++;
            }
            string text = stringBuilder.ToString();
            switch (text)
            {
                case "var":
                    return new GMLToken(Token.Var, index, text);
                case "if":
                    return new GMLToken(Token.If, index, text);
                case "end":
                    return new GMLToken(Token.End, index, text);
                case "else":
                    return new GMLToken(Token.Else, index, text);
                case "while":
                    return new GMLToken(Token.While, index, text);
                case "do":
                    return new GMLToken(Token.Do, index, text);
                case "for":
                    return new GMLToken(Token.For, index, text);
                case "begin":
                    return new GMLToken(Token.Begin, index, text);
                case "then":
                    return new GMLToken(Token.Then, index, text);
                case "with":
                    return new GMLToken(Token.With, index, text);
                case "until":
                    return new GMLToken(Token.Until, index, text);
                case "repeat":
                    return new GMLToken(Token.Repeat, index, text);
                case "exit":
                    return new GMLToken(Token.Exit, index, text);
                case "return":
                    return new GMLToken(Token.Return, index, text);
                case "break":
                    return new GMLToken(Token.Break, index, text);
                case "continue":
                    return new GMLToken(Token.Continue, index, text);
                case "switch":
                    return new GMLToken(Token.Switch, index, text);
                case "case":
                    return new GMLToken(Token.Case, index, text);
                case "default":
                    return new GMLToken(Token.Default, index, text);
                case "and":
                    return new GMLToken(Token.And, index, text);
                case "or":
                    return new GMLToken(Token.Or, index, text);
                case "not":
                    return new GMLToken(Token.Not, index, text);
                case "div":
                    return new GMLToken(Token.Div, index, text);
                case "mod":
                    return new GMLToken(Token.Mod, index, text);
                case "xor":
                    return new GMLToken(Token.BitXor, index, text);
                case "globalvar":
                    return new GMLToken(Token.GlobalVar, index, text);
                default:
                    return new GMLToken(Token.Name, index, text);
            }
        }

        private static GMLToken NextValue(string _script, ref int _index)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int index = _index;
            stringBuilder.Append(_script[_index]);
            _index++;
            while (_index < _script.Length && (char.IsDigit(_script[_index]) || _script[_index] == '.'))
            {
                stringBuilder.Append(_script[_index]);
                _index++;
            }
            return new GMLToken(Token.Number, index, stringBuilder.ToString());
        }

        private static GMLToken NextHex(string _script, ref int _index)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int index = _index;
            stringBuilder.Append(_script[_index]);
            _index++;
            while (_index < _script.Length && (char.IsDigit(_script[_index]) || (char.ToLower(_script[_index]) >= 'a' && char.ToLower(_script[_index]) <= 'f')))
            {
                stringBuilder.Append(_script[_index]);
                _index++;
            }
            return new GMLToken(Token.Number, index, stringBuilder.ToString());
        }

        private static GMLToken NextString(string _script, ref int _index)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int index = _index;
            char c = _script[_index];
            _index++;
            while (_index < _script.Length && c != _script[_index])
            {
                stringBuilder.Append(_script[_index]);
                _index++;
            }
            if (_index < _script.Length)
            {
                _index++;
            }
            return new GMLToken(Token.String, index, stringBuilder.ToString());
        }

        private static GMLToken NextToken(string _script, ref int _index)
        {
            SkipWhitespace(_script, ref _index);
            if (_index >= _script.Length)
            {
                return new GMLToken(Token.EOF, _index, string.Empty);
            }
            char c = _script[_index];
            if (char.IsLetter(c) || c == '_')
            {
                return NextName(_script, ref _index);
            }
            if (char.IsDigit(c))
            {
                return NextValue(_script, ref _index);
            }
            switch (c)
            {
                case '$':
                    return NextHex(_script, ref _index);
                case '"':
                case '\'':
                    return NextString(_script, ref _index);
                default:
                    if (_index + 1 < _script.Length && _script[_index] == '.' && char.IsDigit(_script[_index + 1]))
                    {
                        return NextValue(_script, ref _index);
                    }
                    switch (c)
                    {
                        case '{':
                            return new GMLToken(Token.Begin, _index++, "{");
                        case '}':
                            return new GMLToken(Token.End, _index++, "}");
                        case '(':
                            return new GMLToken(Token.Open, _index++, "(");
                        case ')':
                            return new GMLToken(Token.Close, _index++, ")");
                        case '[':
                            return new GMLToken(Token.ArrayOpen, _index++, "[");
                        case ']':
                            return new GMLToken(Token.ArrayClose, _index++, "]");
                        case ';':
                            return new GMLToken(Token.SepStatement, _index++, ";");
                        case ',':
                            return new GMLToken(Token.SepArgument, _index++, ",");
                        case '.':
                            return new GMLToken(Token.Dot, _index++, ".");
                        case '~':
                            return new GMLToken(Token.BitNegate, _index++, "~");
                        case '!':
                            _index++;
                            if (_index >= _script.Length || _script[_index] != '=')
                            {
                                return new GMLToken(Token.Not, _index - 1, "!");
                            }
                            return new GMLToken(Token.NotEqual, _index++ - 2, "!=");
                        case '=':
                            _index++;
                            if (_index >= _script.Length || _script[_index] != '=')
                            {
                                return new GMLToken(Token.Assign, _index - 1, "=");
                            }
                            return new GMLToken(Token.Equal, _index++ - 2, "==");
                        case ':':
                            _index++;
                            if (_index >= _script.Length || _script[_index] != '=')
                            {
                                return new GMLToken(Token.Label, _index - 1, ":");
                            }
                            return new GMLToken(Token.Assign, _index++ - 2, ":=");
                        case '+':
                            _index++;
                            if (_index >= _script.Length || _script[_index] != '=')
                            {
                                return new GMLToken(Token.Plus, _index - 1, "+");
                            }
                            return new GMLToken(Token.AssignPlus, _index++ - 2, "+=");
                        case '-':
                            _index++;
                            if (_index >= _script.Length || _script[_index] != '=')
                            {
                                return new GMLToken(Token.Minus, _index - 1, "-");
                            }
                            return new GMLToken(Token.AssignMinus, _index++ - 2, "-=");
                        case '*':
                            _index++;
                            if (_index >= _script.Length || _script[_index] != '=')
                            {
                                return new GMLToken(Token.Time, _index - 1, "*");
                            }
                            return new GMLToken(Token.AssignTimes, _index++ - 2, "*=");
                        case '/':
                            _index++;
                            if (_index >= _script.Length || _script[_index] != '=')
                            {
                                return new GMLToken(Token.Divide, _index - 1, "/");
                            }
                            return new GMLToken(Token.AssignDivide, _index++ - 2, "/=");
                        case '<':
                            if (_index + 1 < _script.Length)
                            {
                                switch (_script[_index + 1])
                                {
                                    case '>':
                                        _index += 2;
                                        return new GMLToken(Token.NotEqual, _index, "<>");
                                    case '<':
                                        _index += 2;
                                        return new GMLToken(Token.BitShiftLeft, _index, "<<");
                                    case '=':
                                        _index += 2;
                                        return new GMLToken(Token.LessEqual, _index, "<=");
                                    default:
                                        return new GMLToken(Token.Less, _index++, "<");
                                }
                            }
                            return new GMLToken(Token.Less, _index++, "<");
                        case '>':
                            if (_index + 1 < _script.Length)
                            {
                                switch (_script[_index + 1])
                                {
                                    case '>':
                                        _index += 2;
                                        return new GMLToken(Token.BitShiftRight, _index, ">>");
                                    case '=':
                                        _index += 2;
                                        return new GMLToken(Token.GreaterEqual, _index, ">=");
                                    default:
                                        return new GMLToken(Token.Greater, _index++, ">");
                                }
                            }
                            return new GMLToken(Token.Greater, _index++, ">");
                        case '|':
                            if (_index + 1 < _script.Length)
                            {
                                switch (_script[_index + 1])
                                {
                                    case '|':
                                        _index += 2;
                                        return new GMLToken(Token.Or, _index, "||");
                                    case '=':
                                        _index += 2;
                                        return new GMLToken(Token.AssignOr, _index, "|=");
                                    default:
                                        return new GMLToken(Token.BitOr, _index++, "|");
                                }
                            }
                            return new GMLToken(Token.BitOr, _index++, "|");
                        case '&':
                            if (_index + 1 < _script.Length)
                            {
                                switch (_script[_index + 1])
                                {
                                    case '&':
                                        _index += 2;
                                        return new GMLToken(Token.And, _index, "&&");
                                    case '=':
                                        _index += 2;
                                        return new GMLToken(Token.AssignAnd, _index, "&=");
                                    default:
                                        return new GMLToken(Token.BitAnd, _index++, "&");
                                }
                            }
                            return new GMLToken(Token.BitAnd, _index++, "&");
                        case '^':
                            if (_index + 1 < _script.Length)
                            {
                                switch (_script[_index + 1])
                                {
                                    case '^':
                                        _index += 2;
                                        return new GMLToken(Token.Xor, _index, "^^");
                                    case '=':
                                        _index += 2;
                                        return new GMLToken(Token.AssignXor, _index, "^=");
                                    default:
                                        return new GMLToken(Token.BitXor, _index++, "^");
                                }
                            }
                            return new GMLToken(Token.BitXor, _index++, "^");
                        default:
                            return new GMLToken(Token.Error, _index, string.Empty);
                    }
            }
        }

        public static void AddError(string _errorMessage, string _script, GMLToken _token)
        {
            int row = 1;
            for (int i = 0; i < _token.Index; i++)
            {
                if (_script[i] == '\n')
                {
                    row++;
                }
            }
            Console.WriteLine("Error : {0}({1}) : {2}", ScriptName, row, _errorMessage);

            TokenizeError = true;
        }

        public static int Find<T>(IList<T> _list, string _name)
            where T : Resource
        {
            int i;
            for (i = 0; i < _list.Count && !(_list[i].Name == _name); i++)
            {
            }
            if (i >= _list.Count)
            {
                return -1;
            }
            return i;
        }

        public static int FindTriggerConstName(IList<Trigger> _list, string _name)
        {
            int result = -1;
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].Constant == _name)
                {
                    result = i + 1;
                    break;
                }
            }
            return result;
        }

        private static int Code_Function_Find(string _name)
        {
            if (ProjectContext != null)
            {
                // TODO: Find in extensions.
                //int num = 0;
                //foreach (GMExtension extension in ms_assets.Extensions)
                //{
                //    int num2 = 0;
                //    foreach (GMExtensionInclude include in extension.Includes)
                //    {
                //        int num3 = 0;
                //        foreach (GMExtensionFunction function in include.Functions)
                //        {
                //            if (function.Name == _name)
                //            {
                //                return ms_id + num * 256 + num2 * 4096 + num3;
                //            }
                //            num3++;
                //        }
                //        num2++;
                //    }
                //    num++;
                //}
                int num4 = Find(ProjectContext.Scripts, _name);
                if (num4 >= 0)
                {
                    return num4 + 100000;
                }
            }
            GMLFunction value = null;
            if (!Builtins.Functions.TryGetValue(_name, out value))
            {
                return -1;
            }
            return value.Id;
        }

        private static int FindResourceIndexFromName(string _name)
        {
            int result = -1;
            if (ProjectContext != null)
            {
                int num = Find(ProjectContext.Objects, _name);
                if (num >= 0)
                {
                    return num;
                }
                num = Find(ProjectContext.Sprites, _name);
                if (num >= 0)
                {
                    return num;
                }
                num = Find(ProjectContext.Sounds, _name);
                if (num >= 0)
                {
                    return num;
                }
                num = Find(ProjectContext.Backgrounds, _name);
                if (num >= 0)
                {
                    return num;
                }
                num = Find(ProjectContext.Paths, _name);
                if (num >= 0)
                {
                    return num;
                }
                num = Find(ProjectContext.Fonts, _name);
                if (num >= 0)
                {
                    return num;
                }
                num = Find(ProjectContext.Timelines, _name);
                if (num >= 0)
                {
                    return num;
                }
                num = Find(ProjectContext.Scripts, _name);
                if (num >= 0)
                {
                    return num;
                }
                num = Find(ProjectContext.Rooms, _name);
                if (num >= 0)
                {
                    return num;
                }
                num = FindTriggerConstName(ProjectContext.Triggers, _name);
                if (num >= 0)
                {
                    return num;
                }
            }
            return result;
        }

        private static bool Code_Constant_Find(string _name, out GMLValue _val)
        {
            _val = new GMLValue();
            _val.Kind = Kind.Number;
            int value = 0;
            Builtins.ConstantCount.TryGetValue(_name, out value);
            Builtins.ConstantCount[_name] = value + 1;
            int num = FindResourceIndexFromName(_name);
            if (num >= 0)
            {
                _val.ValueI = num;
                return true;
            }
            string value2;
            var con = ProjectContext.Settings.Constants.Find(c => c.Name == _name);
            if (con != null)
            {
                value2 = con.Name;
                double result = 0.0;
                if (double.TryParse(value2, out result))
                {
                    _val.ValueI = result;
                    return true;
                }
                num = FindResourceIndexFromName(value2);
                if (num >= 0)
                {
                    _val.ValueI = num;
                    return true;
                }
                _val.Kind = Kind.Constant;
                _val.ValueS = _name;
                return true;
            }
            double value3 = 0.0;
            if (!Builtins.Constants.TryGetValue(_name, out value3))
            {
                return false;
            }
            _val.ValueI = value3;
            return true;
        }

        private static int Code_Variable_Find(string _name)
        {
            GMLVariable value = null;
            if (!Builtins.GlobalVariables.TryGetValue(_name, out value) && !Builtins.LocalVariables.TryGetValue(_name, out value) && !Variables.TryGetValue(_name, out value))
            {
                value = new GMLVariable();
                value.Name = _name;
                value.Id = 100000 + Variables.Count;
                Variables.Add(_name, value);
            }
            return value.Id;
        }

        private static void CreateFunctionsToken(string _script, List<GMLToken> _pass1, List<GMLToken> _pass2, int _index)
        {
            int num = Code_Function_Find(_pass1[_index].Text);
            if (num < 0)
            {
                AddError(string.Format("unknown function or script {0}", _pass1[_index].Text), _script, _pass1[_index]);
            }
            _pass2.Add(new GMLToken(Token.Function, _pass1[_index], num));
        }

        private static void CreateNameToken(string _script, List<GMLToken> _pass1, List<GMLToken> _pass2, int _index)
        {
            GMLValue _val = null;
            if (!Code_Constant_Find(_pass1[_index].Text, out _val))
            {
                int id = Code_Variable_Find(_pass1[_index].Text);
                _pass2.Add(new GMLToken(Token.Variable, _pass1[_index], id));
            }
            else
            {
                _pass2.Add(new GMLToken(Token.Constant, _pass1[_index], 0, _val));
            }
        }

        private static void CreateValueToken(string _script, List<GMLToken> _pass1, List<GMLToken> _pass2, int _index)
        {
            string text = _pass1[_index].Text;
            GMLValue gMLValue = null;
            if (text[0] == '$')
            {
                long num = Convert.ToInt64(text.Substring(1), 16);
                gMLValue = new GMLValue(num);
            }
            else
            {
                double result = 0.0;
                if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
                {
                    AddError(string.Format("Number {0} in incorrect format", text), _script, _pass1[_index]);
                }
                gMLValue = new GMLValue(result);
            }
            _pass2.Add(new GMLToken(Token.Constant, _pass1[_index], 0, gMLValue));
        }

        private static void CreateStringToken(string _script, List<GMLToken> _pass1, List<GMLToken> _pass2, int _index)
        {
            _pass2.Add(new GMLToken(Token.Constant, _pass1[_index], 0, new GMLValue(_pass1[_index].Text)));
        }

        private static void CreateNormalToken(string _script, List<GMLToken> _pass1, List<GMLToken> _pass2, int _index)
        {
            _pass2.Add(new GMLToken(_pass1[_index].Token, _pass1[_index], 0));
        }
        public static List<GMLToken> Tokenize(Project _assets, string _name, string _script)
        {
            TokenizeError = false;
            TokenizeErrors = new List<GMLError>();
            ProjectContext = _assets;
            Script = _script;
            ScriptName = _name;

            List<GMLToken> tokens = new List<GMLToken>();
            int _index = 0;

            // First, convert the GML code to tokens.
            bool flag = false;
            while (!flag)
            {
                GMLToken gMLToken = NextToken(_script, ref _index);
                tokens.Add(gMLToken);
                flag = (gMLToken.Token == Token.EOF);
            }

            // Then, search the built-in function and project context, redirect the name token.
            List<GMLToken> newTokens = new List<GMLToken>();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Token == Token.Name && tokens[i + 1].Token == Token.Open)
                {
                    CreateFunctionsToken(_script, tokens, newTokens, i);
                }
                else if (tokens[i].Token == Token.Name)
                {
                    CreateNameToken(_script, tokens, newTokens, i);
                }
                else if (tokens[i].Token == Token.Number)
                {
                    CreateValueToken(_script, tokens, newTokens, i);
                }
                else if (tokens[i].Token == Token.String)
                {
                    CreateStringToken(_script, tokens, newTokens, i);
                }
                else
                {
                    CreateNormalToken(_script, tokens, newTokens, i);
                }
            }

            return newTokens;
        }
    }
}
