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
    public static class Parser
    {
        private static string Script { get; set; } = string.Empty;
        private static bool ParseError { get; set; } = false;
        private static string ScriptName { get; set; } = "";

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
        
        private static int ParseStatement(List<AST> _pass3, List<GMLToken> _pass2, int _index)
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
                    _pass3.Add(new AST(_pass2[_index].Token, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value));
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

        private static int ParseVar(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            AST gMLToken = new AST(Token.Var, _pass2[_index], 0);
            int num = _index + 1;
            while (_pass2[num].Token == Token.Variable)
            {
                if (_pass2[num].Id < 100000)
                {
                    AddError("cannot redeclare a builtin varable", Script, _pass2[num]);
                }
                AST gMLToken2 = new AST(_pass2[num]);
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

        private static int ParseGlobalVar(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            AST gMLToken = new AST(Token.GlobalVar, _pass2[_index], 0);
            int num = _index + 1;
            while (_pass2[num].Token == Token.Variable)
            {
                if (_pass2[num].Id < 100000)
                {
                    AddError("cannot redeclare a builtin varable", Script, _pass2[num]);
                }
                AST gMLToken2 = new AST(_pass2[num]);
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

        private static int ParseBlock(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            int num = _index + 1;
            AST gMLToken = new AST(_pass2[_index]);
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

        private static int ParseRepeat(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            int index = _index + 1;
            AST gMLToken = new AST(_pass2[_index]);
            _pass3.Add(gMLToken);
            index = ParseExpression1(gMLToken.Children, _pass2, index);
            return ParseStatement(gMLToken.Children, _pass2, index);
        }

        private static int ParseIf(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            AST gMLToken = new AST(_pass2[_index]);
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

        private static int ParseWhile(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            int index = _index + 1;
            AST gMLToken = new AST(_pass2[_index]);
            _pass3.Add(gMLToken);
            index = ParseExpression1(gMLToken.Children, _pass2, index);
            if (_pass2[index].Token == Token.Do)
            {
                index++;
            }
            return ParseStatement(gMLToken.Children, _pass2, index);
        }

        private static int ParseFor(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            int num = _index + 1;
            AST gMLToken = new AST(_pass2[_index]);
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

        private static int ParseDo(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            int index = _index + 1;
            AST gMLToken = new AST(_pass2[_index]);
            _pass3.Add(gMLToken);
            index = ParseStatement(gMLToken.Children, _pass2, index);
            if (_pass2[index].Token != Token.Until)
            {
                AddError("keyword Until expected", Script, _pass2[index]);
            }
            return ParseExpression1(gMLToken.Children, _pass2, index + 1);
        }

        private static int ParseWith(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            int index = _index + 1;
            AST gMLToken = new AST(_pass2[_index]);
            _pass3.Add(gMLToken);
            index = ParseExpression1(gMLToken.Children, _pass2, index);
            if (_pass2[index].Token == Token.Do)
            {
                index++;
            }
            return ParseStatement(gMLToken.Children, _pass2, index);
        }

        private static int ParseSwitch(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            int index = _index + 1;
            AST gMLToken = new AST(_pass2[_index]);
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

        private static int ParseCase(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            AST gMLToken = new AST(_pass2[_index]);
            int num = ParseExpression1(gMLToken.Children, _pass2, _index + 1);
            if (_pass2[num].Token != Token.Label)
            {
                AddError("Symbol : expected", Script, _pass2[num]);
            }
            _pass3.Add(gMLToken);
            return num + 1;
        }

        private static int ParseDefault(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            int num = _index + 1;
            AST item = new AST(_pass2[_index]);
            if (_pass2[num].Token != Token.Label)
            {
                AddError("Symbol : expected", Script, _pass2[num]);
            }
            _pass3.Add(item);
            return num + 1;
        }

        private static int ParseReturn(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            AST gMLToken = new AST(Token.Return, _pass2[_index], 0);
            int index = _index + 1;
            index = ParseExpression1(gMLToken.Children, _pass2, index);
            _pass3.Add(gMLToken);
            return index;
        }

        private static int ParseFunction(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            if (_pass2[_index].Token != Token.Function)
            {
                AddError("Function name expected", Script, _pass2[_index]);
            }
            AST gMLToken = new AST(_pass2[_index]);
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

        private static int ParseAssignment(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            AST gMLToken = new AST(Token.Assign, _pass2[_index], 0);
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
                    gMLToken.Children.Add(new AST(_pass2[num]));
                    num = ParseExpression1(gMLToken.Children, _pass2, num + 1);
                    break;
                default:
                    AddError("Assignment operator expected", Script, _pass2[num]);
                    break;
            }
            return num;
        }

        private static int ParseExpression1(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            AST gMLToken = new AST(Token.Binary, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value);
            int num = ParseExpression2(gMLToken.Children, _pass2, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (_pass2[num].Token == Token.And || _pass2[num].Token == Token.Or || _pass2[num].Token == Token.Xor)
                {
                    flag = false;
                    gMLToken.Children.Add(new AST(_pass2[num]));
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

        private static int ParseExpression2(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            AST gMLToken = new AST(Token.Binary, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value);
            int num = ParseExpression3(gMLToken.Children, _pass2, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (_pass2[num].Token == Token.Less || _pass2[num].Token == Token.LessEqual || _pass2[num].Token == Token.Equal || _pass2[num].Token == Token.NotEqual || _pass2[num].Token == Token.Assign || _pass2[num].Token == Token.Greater || _pass2[num].Token == Token.GreaterEqual)
                {
                    flag = false;
                    gMLToken.Children.Add(new AST(_pass2[num]));
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

        private static int ParseExpression3(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            AST gMLToken = new AST(Token.Binary, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value);
            int num = ParseExpression4(gMLToken.Children, _pass2, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (_pass2[num].Token == Token.BitOr || _pass2[num].Token == Token.BitAnd || _pass2[num].Token == Token.BitXor)
                {
                    flag = false;
                    gMLToken.Children.Add(new AST(_pass2[num]));
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

        private static int ParseExpression4(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            AST gMLToken = new AST(Token.Binary, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value);
            int num = ParseExpression5(gMLToken.Children, _pass2, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (_pass2[num].Token == Token.BitShiftLeft || _pass2[num].Token == Token.BitShiftRight)
                {
                    flag = false;
                    gMLToken.Children.Add(new AST(_pass2[num]));
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

        private static int ParseExpression5(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            AST gMLToken = new AST(Token.Binary, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value);
            int num = ParseExpression6(gMLToken.Children, _pass2, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (_pass2[num].Token == Token.Plus || _pass2[num].Token == Token.Minus)
                {
                    flag = false;
                    gMLToken.Children.Add(new AST(_pass2[num]));
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

        private static int ParseExpression6(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            AST gMLToken = new AST(Token.Binary, _pass2[_index], _pass2[_index].Id, _pass2[_index].Value);
            int num = ParseVariable2(gMLToken.Children, _pass2, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (_pass2[num].Token == Token.Time || _pass2[num].Token == Token.Divide || _pass2[num].Token == Token.Div || _pass2[num].Token == Token.Mod)
                {
                    flag = false;
                    gMLToken.Children.Add(new AST(_pass2[num]));
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

        private static int ParseVariable2(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            List<AST> list = new List<AST>();
            int num = ParseTerm(list, _pass2, _index);
            if (!ParseError)
            {
                if (_pass2[num].Token == Token.Dot)
                {
                    AST gMLToken = new AST(Token.Dot, _pass2[num], 0);
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

        private static int ParseTerm(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            int num = _index;
            switch (_pass2[num].Token)
            {
                case Token.Function:
                    num = ParseFunction(_pass3, _pass2, num);
                    break;
                case Token.Constant:
                    _pass3.Add(new AST(_pass2[num]));
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
                        AST gMLToken = new AST(Token.Unary, _pass2[num], (int)_pass2[num].Token);
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

        private static int ParseVariable(List<AST> _pass3, List<GMLToken> _pass2, int _index)
        {
            if (_pass2[_index].Token != Token.Variable)
            {
                AddError("variable name expected", Script, _pass2[_index]);
            }
            AST gMLToken = new AST(_pass2[_index]);
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
            else if (Builtins.GlobalArrays.TryGetValue(gMLToken.Text, out value) || Builtins.LocalArrays.TryGetValue(gMLToken.Text, out value))
            {
                AST gMLToken2 = new AST(Token.Constant, -1, "0");
                gMLToken2.Value = new GMLValue(0.0);
                gMLToken.Children.Add(gMLToken2);
            }
            return num;
        }

        static Parser()
        {
            Builtins.AddBuiltins();
        }
        public static AST Parse(Project _assets, string _name, string _script)
        {
            var tokens = Lexer.Tokenize(_assets, _name, _script);

            List<AST> astNodes = new List<AST>();

            int index = 0;
            while (!ParseError && tokens[index].Token != Token.EOF)
            {
                index = ParseStatement(astNodes, tokens, index);
            }

            var ast = new AST(Token.Block, 0, "");
            ast.Children = astNodes;
            return ast;
        }
    }
}
