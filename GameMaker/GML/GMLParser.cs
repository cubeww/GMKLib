using GameMaker.ProjectCommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameMaker.GML
{
    public static class GMLParser
    {
        public const int OBJECT_SELF = -1;

        public const int OBJECT_OTHER = -2;

        public const int OBJECT_ALL = -3;

        public const int OBJECT_NOONE = -4;

        public const int OBJECT_GLOBAL = -5;

        public const int OBJECT_LOCAL = -7;

        public const int OBJECT_NOTSPECIFIED = -6;

        public static Dictionary<string, double> Constants { get; set; } = new Dictionary<string, double>();
        public static Dictionary<string, int> ConstantCount { get; set; } = new Dictionary<string, int>();
        public static Dictionary<string, GMLFunction> Functions { get; set; } = new Dictionary<string, GMLFunction>();
        public static Dictionary<string, GMLVariable> Builtins { get; set; } = new Dictionary<string, GMLVariable>();
        public static Dictionary<string, GMLVariable> BuiltinArray { get; set; } = new Dictionary<string, GMLVariable>();
        public static Dictionary<string, GMLVariable> BuiltinsLocal { get; set; } = new Dictionary<string, GMLVariable>();
        public static Dictionary<string, GMLVariable> BuiltinsLocalArray { get; set; } = new Dictionary<string, GMLVariable>();
        private static Dictionary<string, GMLVariable> Variables { get; set; } = new Dictionary<string, GMLVariable>();
        
        public static int ID { get; set; } = 0;

        private static Project ProjectContext { get; set; } = null;

        private static string Script { get; set; } = string.Empty;

        private static List<GMLError> ParseErrors { get; set; } = null;

        private static bool ParseError { get; set; } = false;

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
                                    ParseErrors.Add(new GMLError(ErrorKind.Warning_Unclosed_Comment, "unclosed comment (/*) at tend of script", null, _index));
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

            ParseError = true;
        }

        public static void Function_Add(string _string, int _numArgs7, int _numArgs8, bool _Pro)
        {
            GMLFunction gMLFunction = new GMLFunction();
            gMLFunction.Name = _string;
            gMLFunction.Id = ID++;
            gMLFunction.NumArgs7 = _numArgs7;
            gMLFunction.NumArgs8 = _numArgs8;
            gMLFunction.Pro = _Pro;
            gMLFunction.InstanceFirstParam = false;
            gMLFunction.OtherSecondParam = false;
            try
            {
                Functions.Add(_string, gMLFunction);
            }
            catch (Exception)
            {
                //MessageBox.Show(_string);
            }
        }

        public static void Function_Add(string _string, int _numArgs7, int _numArgs8, bool _Pro, bool _InstanceFirstParam)
        {
            GMLFunction gMLFunction = new GMLFunction();
            gMLFunction.Name = _string;
            gMLFunction.Id = ID++;
            gMLFunction.NumArgs7 = _numArgs7;
            gMLFunction.NumArgs8 = _numArgs8;
            gMLFunction.Pro = _Pro;
            gMLFunction.InstanceFirstParam = _InstanceFirstParam;
            gMLFunction.OtherSecondParam = false;
            Functions.Add(_string, gMLFunction);
        }

        public static void Function_Add(string _string, int _numArgs7, int _numArgs8, bool _Pro, bool _InstanceFirstParam, bool _OtherSecondParam)
        {
            GMLFunction gMLFunction = new GMLFunction();
            gMLFunction.Name = _string;
            gMLFunction.Id = ID++;
            gMLFunction.NumArgs7 = _numArgs7;
            gMLFunction.NumArgs8 = _numArgs8;
            gMLFunction.Pro = _Pro;
            gMLFunction.InstanceFirstParam = _InstanceFirstParam;
            gMLFunction.OtherSecondParam = _OtherSecondParam;
            Functions.Add(_string, gMLFunction);
        }

        public static void AddRealConstant(string _name, double _value)
        {
            Constants.Add(_name, _value);
        }

        public static void Variable_BuiltIn_Add(string _name, bool _get, bool _set, bool _pro, string _setFunc, string _getFunc)
        {
            GMLVariable gMLVariable = new GMLVariable();
            gMLVariable.Name = _name;
            gMLVariable.Id = ID++;
            gMLVariable.Get = _get;
            gMLVariable.Set = _set;
            gMLVariable.Pro = _pro;
            gMLVariable.SetFunction = _setFunc;
            gMLVariable.GetFunction = _getFunc;
            Builtins.Add(_name, gMLVariable);
        }

        public static void Variable_BuiltIn_Array_Add(string _name, bool _get, bool _set, bool _pro)
        {
            GMLVariable gMLVariable = new GMLVariable();
            gMLVariable.Name = _name;
            gMLVariable.Id = ID++;
            gMLVariable.Get = _get;
            gMLVariable.Set = _set;
            gMLVariable.Pro = _pro;
            gMLVariable.SetFunction = null;
            gMLVariable.GetFunction = null;
            Builtins.Add(_name, gMLVariable);
            BuiltinArray.Add(_name, gMLVariable);
        }

        public static void Variable_BuiltIn_Local_Add(string _name, bool _get, bool _set, bool _pro, string _setFunc, string _getFunc)
        {
            GMLVariable gMLVariable = new GMLVariable();
            gMLVariable.Name = _name;
            gMLVariable.Id = ID++;
            gMLVariable.Get = _get;
            gMLVariable.Set = _set;
            gMLVariable.Pro = _pro;
            gMLVariable.SetFunction = _setFunc;
            gMLVariable.GetFunction = _getFunc;
            BuiltinsLocal.Add(_name, gMLVariable);
        }

        public static void Variable_BuiltIn_Local_Array_Add(string _name, bool _get, bool _set, bool _pro)
        {
            GMLVariable gMLVariable = new GMLVariable();
            gMLVariable.Name = _name;
            gMLVariable.Id = ID++;
            gMLVariable.Get = _get;
            gMLVariable.Set = _set;
            gMLVariable.Pro = _pro;
            gMLVariable.SetFunction = null;
            gMLVariable.GetFunction = null;
            BuiltinsLocal.Add(_name, gMLVariable);
            BuiltinsLocalArray.Add(_name, gMLVariable);
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
            if (!Functions.TryGetValue(_name, out value))
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
            ConstantCount.TryGetValue(_name, out value);
            ConstantCount[_name] = value + 1;
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
            if (!Constants.TryGetValue(_name, out value3))
            {
                return false;
            }
            _val.ValueI = value3;
            return true;
        }

        private static int Code_Variable_Find(string _name)
        {
            GMLVariable value = null;
            if (!Builtins.TryGetValue(_name, out value) && !BuiltinsLocal.TryGetValue(_name, out value) && !Variables.TryGetValue(_name, out value))
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

        private static int ParseStatement(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            int i = _index;
            switch (_pass2[i].Token)
            {
                case Token.EOF:
                    AddError("unexpected EOF encountered", Script, _pass2[i]);
                    break;
                case Token.Var:
                    i = ParseVar(_pass3, _pass2, _index);
                    break;
                case Token.GlobalVar:
                    i = ParseGlobalVar(_pass3, _pass2, _index);
                    break;
                case Token.Begin:
                    i = ParseBlock(_pass3, _pass2, _index);
                    break;
                case Token.Repeat:
                    i = ParseRepeat(_pass3, _pass2, _index);
                    break;
                case Token.If:
                    i = ParseIf(_pass3, _pass2, _index);
                    break;
                case Token.While:
                    i = ParseWhile(_pass3, _pass2, _index);
                    break;
                case Token.For:
                    i = ParseFor(_pass3, _pass2, _index);
                    break;
                case Token.Do:
                    i = ParseDo(_pass3, _pass2, _index);
                    break;
                case Token.With:
                    i = ParseWith(_pass3, _pass2, _index);
                    break;
                case Token.Switch:
                    i = ParseSwitch(_pass3, _pass2, _index);
                    break;
                case Token.Case:
                    i = ParseCase(_pass3, _pass2, _index);
                    break;
                case Token.Default:
                    i = ParseDefault(_pass3, _pass2, _index);
                    break;
                case Token.Return:
                    i = ParseReturn(_pass3, _pass2, _index);
                    break;
                case Token.Function:
                    i = ParseFunction(_pass3, _pass2, _index);
                    break;
                case Token.Exit:
                case Token.Break:
                case Token.Continue:
                    _pass3.Add(new GMLToken(_pass2[_index].Token, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value));
                    i++;
                    break;
                default:
                    i = ParseAssignment(_pass3, _pass2, _index);
                    break;
                case Token.SepStatement:
                    break;
            }
            for (; _pass2[i].Token == Token.SepStatement; i++)
            {
            }
            return i;
        }

        private static int ParseVar(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            GMLToken gMLToken = new GMLToken(Token.Var, _pass2[_index], 0);
            int num = _index + 1;
            while (_pass2[num].Token == Token.Variable)
            {
                if (_pass2[num].Id < 100000)
                {
                    AddError("cannot redeclare a builtin varable", Script, _pass2[num]);
                }
                GMLToken gMLToken2 = new GMLToken(_pass2[num]);
                gMLToken2.Token = Token.Constant;
                gMLToken.Children.Add(gMLToken2);
                num++;
                if (_pass2[num].Token == Token.SepArgument)
                {
                    num++;
                }
            }
            _pass3.Add(gMLToken);
            return num;
        }

        private static int ParseGlobalVar(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            GMLToken gMLToken = new GMLToken(Token.GlobalVar, _pass2[_index], 0);
            int num = _index + 1;
            while (_pass2[num].Token == Token.Variable)
            {
                if (_pass2[num].Id < 100000)
                {
                    AddError("cannot redeclare a builtin varable", Script, _pass2[num]);
                }
                GMLToken gMLToken2 = new GMLToken(_pass2[num]);
                gMLToken2.Token = Token.Constant;
                gMLToken.Children.Add(gMLToken2);
                num++;
                //GML2JavaScript.ms_globals[gMLToken2.Text] = gMLToken2.Text;
                if (_pass2[num].Token == Token.SepArgument)
                {
                    num++;
                }
            }
            _pass3.Add(gMLToken);
            return num;
        }

        private static int ParseBlock(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            int num = _index + 1;
            GMLToken gMLToken = new GMLToken(_pass2[_index]);
            _pass3.Add(gMLToken);
            while (!ParseError && _pass2[num].Token != Token.EOF && _pass2[num].Token != Token.End)
            {
                num = ParseStatement(gMLToken.Children, _pass2, num);
            }
            if (_pass2[num].Token != Token.End)
            {
                AddError("symbol } expected", Script, _pass2[num]);
            }
            else
            {
                num++;
            }
            return num;
        }

        private static int ParseRepeat(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            int index = _index + 1;
            GMLToken gMLToken = new GMLToken(_pass2[_index]);
            _pass3.Add(gMLToken);
            index = ParseExpression1(gMLToken.Children, _pass2, index);
            return ParseStatement(gMLToken.Children, _pass2, index);
        }

        private static int ParseIf(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            GMLToken gMLToken = new GMLToken(_pass2[_index]);
            _pass3.Add(gMLToken);
            int num = ParseExpression1(gMLToken.Children, _pass2, _index + 1);
            if (_pass2[num].Token == Token.Then)
            {
                num++;
            }
            num = ParseStatement(gMLToken.Children, _pass2, num);
            if (_pass2[num].Token == Token.Else)
            {
                num = ParseStatement(gMLToken.Children, _pass2, num + 1);
            }
            return num;
        }

        private static int ParseWhile(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            int index = _index + 1;
            GMLToken gMLToken = new GMLToken(_pass2[_index]);
            _pass3.Add(gMLToken);
            index = ParseExpression1(gMLToken.Children, _pass2, index);
            if (_pass2[index].Token == Token.Do)
            {
                index++;
            }
            return ParseStatement(gMLToken.Children, _pass2, index);
        }

        private static int ParseFor(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            int num = _index + 1;
            GMLToken gMLToken = new GMLToken(_pass2[_index]);
            _pass3.Add(gMLToken);
            if (_pass2[num].Token != Token.Open)
            {
                AddError("symbol ( expected", Script, _pass2[num]);
            }
            num++;
            num = ParseStatement(gMLToken.Children, _pass2, num);
            if (_pass2[num].Token == Token.SepStatement)
            {
                num++;
            }
            num = ParseExpression1(gMLToken.Children, _pass2, num);
            if (_pass2[num].Token == Token.SepStatement)
            {
                num++;
            }
            num = ParseStatement(gMLToken.Children, _pass2, num);
            if (_pass2[num].Token != Token.Close)
            {
                AddError("Symbol ) expected", Script, _pass2[num]);
            }
            num++;
            return ParseStatement(gMLToken.Children, _pass2, num);
        }

        private static int ParseDo(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            int index = _index + 1;
            GMLToken gMLToken = new GMLToken(_pass2[_index]);
            _pass3.Add(gMLToken);
            index = ParseStatement(gMLToken.Children, _pass2, index);
            if (_pass2[index].Token != Token.Until)
            {
                AddError("keyword Until expected", Script, _pass2[index]);
            }
            return ParseExpression1(gMLToken.Children, _pass2, index + 1);
        }

        private static int ParseWith(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            int index = _index + 1;
            GMLToken gMLToken = new GMLToken(_pass2[_index]);
            _pass3.Add(gMLToken);
            index = ParseExpression1(gMLToken.Children, _pass2, index);
            if (_pass2[index].Token == Token.Do)
            {
                index++;
            }
            return ParseStatement(gMLToken.Children, _pass2, index);
        }

        private static int ParseSwitch(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            int index = _index + 1;
            GMLToken gMLToken = new GMLToken(_pass2[_index]);
            _pass3.Add(gMLToken);
            index = ParseExpression1(gMLToken.Children, _pass2, index);
            if (_pass2[index].Token != Token.Begin)
            {
                AddError("Symbol { expected", Script, _pass2[index]);
            }
            index++;
            while (_pass2[index].Token != Token.End && _pass2[index].Token != Token.EOF)
            {
                index = ParseStatement(gMLToken.Children, _pass2, index);
            }
            if (_pass2[index].Token != Token.End)
            {
                AddError("Symbol } expected", Script, _pass2[index]);
            }
            return index + 1;
        }

        private static int ParseCase(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            GMLToken gMLToken = new GMLToken(_pass2[_index]);
            int num = ParseExpression1(gMLToken.Children, _pass2, _index + 1);
            if (_pass2[num].Token != Token.Label)
            {
                AddError("Symbol : expected", Script, _pass2[num]);
            }
            _pass3.Add(gMLToken);
            return num + 1;
        }

        private static int ParseDefault(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            int num = _index + 1;
            GMLToken item = new GMLToken(_pass2[_index]);
            if (_pass2[num].Token != Token.Label)
            {
                AddError("Symbol : expected", Script, _pass2[num]);
            }
            _pass3.Add(item);
            return num + 1;
        }

        private static int ParseReturn(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            GMLToken gMLToken = new GMLToken(Token.Return, _pass2[_index], 0);
            int index = _index + 1;
            index = ParseExpression1(gMLToken.Children, _pass2, index);
            _pass3.Add(gMLToken);
            return index;
        }

        private static int ParseFunction(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            if (_pass2[_index].Token != Token.Function)
            {
                AddError("Function name expected", Script, _pass2[_index]);
            }
            GMLToken gMLToken = new GMLToken(_pass2[_index]);
            _pass3.Add(gMLToken);
            int num = _index + 1;
            if (_pass2[num].Token != Token.Open)
            {
                AddError("Symbol ( expected", Script, _pass2[num]);
            }
            num++;
            while (!ParseError && _pass2[num].Token != Token.EOF && _pass2[num].Token != Token.Close)
            {
                num = ParseExpression1(gMLToken.Children, _pass2, num);
                if (_pass2[num].Token == Token.SepArgument)
                {
                    num++;
                }
                else if (_pass2[num].Token != Token.Close)
                {
                    AddError("Symbol , or ) expected", Script, _pass2[num]);
                }
            }
            if (_pass2[num].Token != Token.Close)
            {
                AddError("Symbol ) expected", Script, _pass2[num]);
            }
            else
            {
                num++;
            }
            return num;
        }

        private static int ParseAssignment(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            GMLToken gMLToken = new GMLToken(Token.Assign, _pass2[_index], 0);
            _pass3.Add(gMLToken);
            int num = ParseVariable2(gMLToken.Children, _pass2, _index);
            switch (_pass2[num].Token)
            {
                case Token.Assign:
                case Token.AssignPlus:
                case Token.AssignMinus:
                case Token.AssignTimes:
                case Token.AssignDivide:
                case Token.AssignOr:
                case Token.AssignAnd:
                case Token.AssignXor:
                    gMLToken.Children.Add(_pass2[num]);
                    num = ParseExpression1(gMLToken.Children, _pass2, num + 1);
                    break;
                default:
                    AddError("Assignment operator expected", Script, _pass2[num]);
                    break;
            }
            return num;
        }

        private static int ParseExpression1(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            GMLToken gMLToken = new GMLToken(Token.Binary, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value);
            int num = ParseExpression2(gMLToken.Children, _pass2, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (_pass2[num].Token == Token.And || _pass2[num].Token == Token.Or || _pass2[num].Token == Token.Xor)
                {
                    flag = false;
                    gMLToken.Children.Add(_pass2[num]);
                    num = ParseExpression2(gMLToken.Children, _pass2, num + 1);
                }
                if (flag)
                {
                    _pass3.AddRange(gMLToken.Children);
                }
                else
                {
                    _pass3.Add(gMLToken);
                }
            }
            return num;
        }

        private static int ParseExpression2(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            GMLToken gMLToken = new GMLToken(Token.Binary, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value);
            int num = ParseExpression3(gMLToken.Children, _pass2, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (_pass2[num].Token == Token.Less || _pass2[num].Token == Token.LessEqual || _pass2[num].Token == Token.Equal || _pass2[num].Token == Token.NotEqual || _pass2[num].Token == Token.Assign || _pass2[num].Token == Token.Greater || _pass2[num].Token == Token.GreaterEqual)
                {
                    flag = false;
                    gMLToken.Children.Add(_pass2[num]);
                    num = ParseExpression3(gMLToken.Children, _pass2, num + 1);
                }
                if (flag)
                {
                    _pass3.AddRange(gMLToken.Children);
                }
                else
                {
                    _pass3.Add(gMLToken);
                }
            }
            return num;
        }

        private static int ParseExpression3(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            GMLToken gMLToken = new GMLToken(Token.Binary, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value);
            int num = ParseExpression4(gMLToken.Children, _pass2, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (_pass2[num].Token == Token.BitOr || _pass2[num].Token == Token.BitAnd || _pass2[num].Token == Token.BitXor)
                {
                    flag = false;
                    gMLToken.Children.Add(_pass2[num]);
                    num = ParseExpression4(gMLToken.Children, _pass2, num + 1);
                }
                if (flag)
                {
                    _pass3.AddRange(gMLToken.Children);
                }
                else
                {
                    _pass3.Add(gMLToken);
                }
            }
            return num;
        }

        private static int ParseExpression4(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            GMLToken gMLToken = new GMLToken(Token.Binary, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value);
            int num = ParseExpression5(gMLToken.Children, _pass2, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (_pass2[num].Token == Token.BitShiftLeft || _pass2[num].Token == Token.BitShiftRight)
                {
                    flag = false;
                    gMLToken.Children.Add(_pass2[num]);
                    num = ParseExpression5(gMLToken.Children, _pass2, num + 1);
                }
                if (flag)
                {
                    _pass3.AddRange(gMLToken.Children);
                }
                else
                {
                    _pass3.Add(gMLToken);
                }
            }
            return num;
        }

        private static int ParseExpression5(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            GMLToken gMLToken = new GMLToken(Token.Binary, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value);
            int num = ParseExpression6(gMLToken.Children, _pass2, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (_pass2[num].Token == Token.Plus || _pass2[num].Token == Token.Minus)
                {
                    flag = false;
                    gMLToken.Children.Add(_pass2[num]);
                    num = ParseExpression6(gMLToken.Children, _pass2, num + 1);
                }
                if (flag)
                {
                    _pass3.AddRange(gMLToken.Children);
                }
                else
                {
                    _pass3.Add(gMLToken);
                }
            }
            return num;
        }

        private static int ParseExpression6(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            GMLToken gMLToken = new GMLToken(Token.Binary, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value);
            int num = ParseVariable2(gMLToken.Children, _pass2, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (_pass2[num].Token == Token.Time || _pass2[num].Token == Token.Divide || _pass2[num].Token == Token.Div || _pass2[num].Token == Token.Mod)
                {
                    flag = false;
                    gMLToken.Children.Add(_pass2[num]);
                    num = ParseVariable2(gMLToken.Children, _pass2, num + 1);
                }
                if (flag)
                {
                    _pass3.AddRange(gMLToken.Children);
                }
                else
                {
                    _pass3.Add(gMLToken);
                }
            }
            return num;
        }

        private static int ParseVariable2(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            List<GMLToken> list = new List<GMLToken>();
            int num = ParseTerm(list, _pass2, _index);
            if (!ParseError)
            {
                if (_pass2[num].Token == Token.Dot)
                {
                    GMLToken gMLToken = new GMLToken(Token.Dot, _pass2[num], 0);
                    gMLToken.Children.AddRange(list);
                    _pass3.Add(gMLToken);
                    while (_pass2[num].Token == Token.Dot)
                    {
                        num = ParseVariable(gMLToken.Children, _pass2, num + 1);
                    }
                }
                else
                {
                    _pass3.AddRange(list);
                }
            }
            return num;
        }

        private static int ParseTerm(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            int num = _index;
            switch (_pass2[num].Token)
            {
                case Token.Function:
                    num = ParseFunction(_pass3, _pass2, num);
                    break;
                case Token.Constant:
                    _pass3.Add(_pass2[num]);
                    num++;
                    break;
                case Token.Open:
                    num = ParseExpression1(_pass3, _pass2, num + 1);
                    if (_pass2[num].Token != Token.Close)
                    {
                        AddError("Symbol ) expected", Script, _pass2[num]);
                    }
                    num++;
                    break;
                case Token.Variable:
                    num = ParseVariable(_pass3, _pass2, num);
                    break;
                case Token.Not:
                case Token.Plus:
                case Token.Minus:
                case Token.BitNegate:
                    {
                        GMLToken gMLToken = new GMLToken(Token.Unary, _pass2[num], (int)_pass2[num].Token);
                        num = ParseVariable2(gMLToken.Children, _pass2, num + 1);
                        _pass3.Add(gMLToken);
                        break;
                    }
                default:
                    AddError("unexpected symbol in expression", Script, _pass2[num]);
                    break;
            }
            return num;
        }

        private static int ParseVariable(List<GMLToken> _pass3, List<GMLToken> _pass2, int _index)
        {
            if (_pass2[_index].Token != Token.Variable)
            {
                AddError("variable name expected", Script, _pass2[_index]);
            }
            GMLToken gMLToken = new GMLToken(_pass2[_index]);
            _pass3.Add(gMLToken);
            int num = _index + 1;
            GMLVariable value;
            if (_pass2[num].Token == Token.ArrayOpen)
            {
                num++;
                while (_pass2[num].Token != Token.ArrayClose && _pass2[num].Token != Token.EOF)
                {
                    num = ParseExpression1(gMLToken.Children, _pass2, num);
                    if (_pass2[num].Token == Token.SepArgument)
                    {
                        num++;
                    }
                    else if (_pass2[num].Token != Token.ArrayClose)
                    {
                        AddError("symbol , or ] expected", Script, _pass2[num]);
                    }
                }
                if (_pass2[num].Token == Token.EOF)
                {
                    AddError("symbol ] expected", Script, _pass2[num]);
                }
                num++;
                if (gMLToken.Children.Count >= 3)
                {
                    AddError("only 1 or 2 dimensional arrays are supported", Script, _pass2[num]);
                }
            }
            else if (BuiltinArray.TryGetValue(gMLToken.Text, out value) || BuiltinsLocalArray.TryGetValue(gMLToken.Text, out value))
            {
                GMLToken gMLToken2 = new GMLToken(Token.Constant, -1, "0");
                gMLToken2.Value = new GMLValue(0.0);
                gMLToken.Children.Add(gMLToken2);
            }
            return num;
        }

        static GMLParser()
        {
            Builtin.AddBuiltin();
        }
        public static List<GMLToken> Tokenize(Project _assets, string _name, string _script)
        {
            ParseError = false;
            ParseErrors = new List<GMLError>();
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
        public static GMLToken Parse(Project _assets, string _name, string _script)
        {
            var tokens = Tokenize(_assets, _name, _script);

            List<GMLToken> astTokens = new List<GMLToken>();

            int index = 0;
            while (!ParseError && tokens[index].Token != Token.EOF)
            {
                index = ParseStatement(astTokens, tokens, index);
            }

            GMLToken ast = new GMLToken(Token.Block, 0, "");
            ast.Children = astTokens;
            return ast;
        }
    }
}
