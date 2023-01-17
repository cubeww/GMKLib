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
        
        private static int ParseStatement(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            int i = _index;
            switch (tokens[i].Token)
            {
                case Token.EOF:
                    AddError("unexpected EOF encountered", Script, tokens[i]);
                    break;
                case Token.Var:
                    i = ParseVar(ast, tokens, _index);
                    break;
                case Token.GlobalVar:
                    i = ParseGlobalVar(ast, tokens, _index);
                    break;
                case Token.Begin:
                    i = ParseBlock(ast, tokens, _index);
                    break;
                case Token.Repeat:
                    i = ParseRepeat(ast, tokens, _index);
                    break;
                case Token.If:
                    i = ParseIf(ast, tokens, _index);
                    break;
                case Token.While:
                    i = ParseWhile(ast, tokens, _index);
                    break;
                case Token.For:
                    i = ParseFor(ast, tokens, _index);
                    break;
                case Token.Do:
                    i = ParseDo(ast, tokens, _index);
                    break;
                case Token.With:
                    i = ParseWith(ast, tokens, _index);
                    break;
                case Token.Switch:
                    i = ParseSwitch(ast, tokens, _index);
                    break;
                case Token.Case:
                    i = ParseCase(ast, tokens, _index);
                    break;
                case Token.Default:
                    i = ParseDefault(ast, tokens, _index);
                    break;
                case Token.Return:
                    i = ParseReturn(ast, tokens, _index);
                    break;
                case Token.Function:
                    i = ParseFunction(ast, tokens, _index);
                    break;
                case Token.Exit:
                case Token.Break:
                case Token.Continue:
                    ast.Add(new AST(tokens[_index].Token, tokens[_index], tokens[_index].Id, tokens[_index].Value));
                    i++;
                    break;
                default:
                    i = ParseAssignment(ast, tokens, _index);
                    break;
                case Token.SepStatement:
                    break;
            }
            for (; tokens[i].Token == Token.SepStatement; i++)
            {
            }
            return i;
        }

        private static int ParseVar(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            AST astNode = new AST(Token.Var, tokens[_index], 0);
            int num = _index + 1;
            while (tokens[num].Token == Token.Variable)
            {
                if (tokens[num].Id < 100000)
                {
                    AddError("cannot redeclare a builtin varable", Script, tokens[num]);
                }
                AST astNode2 = new AST(tokens[num]);
                astNode2.Token = Token.Constant;
                astNode.Children.Add(astNode2);
                num++;
                if (tokens[num].Token == Token.SepArgument)
                {
                    num++;
                }
            }
            ast.Add(astNode);
            return num;
        }

        private static int ParseGlobalVar(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            AST astNode = new AST(Token.GlobalVar, tokens[_index], 0);
            int num = _index + 1;
            while (tokens[num].Token == Token.Variable)
            {
                if (tokens[num].Id < 100000)
                {
                    AddError("cannot redeclare a builtin varable", Script, tokens[num]);
                }
                AST astNode2 = new AST(tokens[num]);
                astNode2.Token = Token.Constant;
                astNode.Children.Add(astNode2);
                num++;
                if (tokens[num].Token == Token.SepArgument)
                {
                    num++;
                }
            }
            ast.Add(astNode);
            return num;
        }

        private static int ParseBlock(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            int num = _index + 1;
            AST astNode = new AST(tokens[_index]);
            ast.Add(astNode);
            while (!ParseError && tokens[num].Token != Token.EOF && tokens[num].Token != Token.End)
            {
                num = ParseStatement(astNode.Children, tokens, num);
            }
            if (tokens[num].Token != Token.End)
            {
                AddError("symbol } expected", Script, tokens[num]);
            }
            else
            {
                num++;
            }
            return num;
        }

        private static int ParseRepeat(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            int index = _index + 1;
            AST astNode = new AST(tokens[_index]);
            ast.Add(astNode);
            index = ParseExpression1(astNode.Children, tokens, index);
            return ParseStatement(astNode.Children, tokens, index);
        }

        private static int ParseIf(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            AST astNode = new AST(tokens[_index]);
            ast.Add(astNode);
            int num = ParseExpression1(astNode.Children, tokens, _index + 1);
            if (tokens[num].Token == Token.Then)
            {
                num++;
            }
            num = ParseStatement(astNode.Children, tokens, num);
            if (tokens[num].Token == Token.Else)
            {
                num = ParseStatement(astNode.Children, tokens, num + 1);
            }
            return num;
        }

        private static int ParseWhile(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            int index = _index + 1;
            AST astNode = new AST(tokens[_index]);
            ast.Add(astNode);
            index = ParseExpression1(astNode.Children, tokens, index);
            if (tokens[index].Token == Token.Do)
            {
                index++;
            }
            return ParseStatement(astNode.Children, tokens, index);
        }

        private static int ParseFor(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            int num = _index + 1;
            AST astNode = new AST(tokens[_index]);
            ast.Add(astNode);
            if (tokens[num].Token != Token.Open)
            {
                AddError("symbol ( expected", Script, tokens[num]);
            }
            num++;
            num = ParseStatement(astNode.Children, tokens, num);
            if (tokens[num].Token == Token.SepStatement)
            {
                num++;
            }
            num = ParseExpression1(astNode.Children, tokens, num);
            if (tokens[num].Token == Token.SepStatement)
            {
                num++;
            }
            num = ParseStatement(astNode.Children, tokens, num);
            if (tokens[num].Token != Token.Close)
            {
                AddError("Symbol ) expected", Script, tokens[num]);
            }
            num++;
            return ParseStatement(astNode.Children, tokens, num);
        }

        private static int ParseDo(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            int index = _index + 1;
            AST astNode = new AST(tokens[_index]);
            ast.Add(astNode);
            index = ParseStatement(astNode.Children, tokens, index);
            if (tokens[index].Token != Token.Until)
            {
                AddError("keyword Until expected", Script, tokens[index]);
            }
            return ParseExpression1(astNode.Children, tokens, index + 1);
        }

        private static int ParseWith(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            int index = _index + 1;
            AST astNode = new AST(tokens[_index]);
            ast.Add(astNode);
            index = ParseExpression1(astNode.Children, tokens, index);
            if (tokens[index].Token == Token.Do)
            {
                index++;
            }
            return ParseStatement(astNode.Children, tokens, index);
        }

        private static int ParseSwitch(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            int index = _index + 1;
            AST astNode = new AST(tokens[_index]);
            ast.Add(astNode);
            index = ParseExpression1(astNode.Children, tokens, index);
            if (tokens[index].Token != Token.Begin)
            {
                AddError("Symbol { expected", Script, tokens[index]);
            }
            index++;
            while (tokens[index].Token != Token.End && tokens[index].Token != Token.EOF)
            {
                index = ParseStatement(astNode.Children, tokens, index);
            }
            if (tokens[index].Token != Token.End)
            {
                AddError("Symbol } expected", Script, tokens[index]);
            }
            return index + 1;
        }

        private static int ParseCase(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            AST astNode = new AST(tokens[_index]);
            int num = ParseExpression1(astNode.Children, tokens, _index + 1);
            if (tokens[num].Token != Token.Label)
            {
                AddError("Symbol : expected", Script, tokens[num]);
            }
            ast.Add(astNode);
            return num + 1;
        }

        private static int ParseDefault(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            int num = _index + 1;
            AST item = new AST(tokens[_index]);
            if (tokens[num].Token != Token.Label)
            {
                AddError("Symbol : expected", Script, tokens[num]);
            }
            ast.Add(item);
            return num + 1;
        }

        private static int ParseReturn(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            AST astNode = new AST(Token.Return, tokens[_index], 0);
            int index = _index + 1;
            index = ParseExpression1(astNode.Children, tokens, index);
            ast.Add(astNode);
            return index;
        }

        private static int ParseFunction(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            if (tokens[_index].Token != Token.Function)
            {
                AddError("Function name expected", Script, tokens[_index]);
            }
            AST astNode = new AST(tokens[_index]);
            ast.Add(astNode);
            int num = _index + 1;
            if (tokens[num].Token != Token.Open)
            {
                AddError("Symbol ( expected", Script, tokens[num]);
            }
            num++;
            while (!ParseError && tokens[num].Token != Token.EOF && tokens[num].Token != Token.Close)
            {
                num = ParseExpression1(astNode.Children, tokens, num);
                if (tokens[num].Token == Token.SepArgument)
                {
                    num++;
                }
                else if (tokens[num].Token != Token.Close)
                {
                    AddError("Symbol , or ) expected", Script, tokens[num]);
                }
            }
            if (tokens[num].Token != Token.Close)
            {
                AddError("Symbol ) expected", Script, tokens[num]);
            }
            else
            {
                num++;
            }
            return num;
        }

        private static int ParseAssignment(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            AST astNode = new AST(Token.Assign, tokens[_index], 0);
            ast.Add(astNode);
            int num = ParseVariable2(astNode.Children, tokens, _index);
            switch (tokens[num].Token)
            {
                case Token.Assign:
                case Token.AssignPlus:
                case Token.AssignMinus:
                case Token.AssignTimes:
                case Token.AssignDivide:
                case Token.AssignOr:
                case Token.AssignAnd:
                case Token.AssignXor:
                    astNode.Children.Add(new AST(tokens[num]));
                    num = ParseExpression1(astNode.Children, tokens, num + 1);
                    break;
                default:
                    AddError("Assignment operator expected", Script, tokens[num]);
                    break;
            }
            return num;
        }

        private static int ParseExpression1(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            AST astNode = new AST(Token.Binary, tokens[_index], tokens[_index].Id, tokens[_index].Value);
            int num = ParseExpression2(astNode.Children, tokens, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (tokens[num].Token == Token.And || tokens[num].Token == Token.Or || tokens[num].Token == Token.Xor)
                {
                    flag = false;
                    astNode.Children.Add(new AST(tokens[num]));
                    num = ParseExpression2(astNode.Children, tokens, num + 1);
                }
                if (flag)
                {
                    ast.AddRange(astNode.Children);
                }
                else
                {
                    ast.Add(astNode);
                }
            }
            return num;
        }

        private static int ParseExpression2(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            AST astNode = new AST(Token.Binary, tokens[_index], tokens[_index].Id, tokens[_index].Value);
            int num = ParseExpression3(astNode.Children, tokens, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (tokens[num].Token == Token.Less || tokens[num].Token == Token.LessEqual || tokens[num].Token == Token.Equal || tokens[num].Token == Token.NotEqual || tokens[num].Token == Token.Assign || tokens[num].Token == Token.Greater || tokens[num].Token == Token.GreaterEqual)
                {
                    flag = false;
                    astNode.Children.Add(new AST(tokens[num]));
                    num = ParseExpression3(astNode.Children, tokens, num + 1);
                }
                if (flag)
                {
                    ast.AddRange(astNode.Children);
                }
                else
                {
                    ast.Add(astNode);
                }
            }
            return num;
        }

        private static int ParseExpression3(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            AST astNode = new AST(Token.Binary, tokens[_index], tokens[_index].Id, tokens[_index].Value);
            int num = ParseExpression4(astNode.Children, tokens, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (tokens[num].Token == Token.BitOr || tokens[num].Token == Token.BitAnd || tokens[num].Token == Token.BitXor)
                {
                    flag = false;
                    astNode.Children.Add(new AST(tokens[num]));
                    num = ParseExpression4(astNode.Children, tokens, num + 1);
                }
                if (flag)
                {
                    ast.AddRange(astNode.Children);
                }
                else
                {
                    ast.Add(astNode);
                }
            }
            return num;
        }

        private static int ParseExpression4(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            AST astNode = new AST(Token.Binary, tokens[_index], tokens[_index].Id, tokens[_index].Value);
            int num = ParseExpression5(astNode.Children, tokens, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (tokens[num].Token == Token.BitShiftLeft || tokens[num].Token == Token.BitShiftRight)
                {
                    flag = false;
                    astNode.Children.Add(new AST(tokens[num]));
                    num = ParseExpression5(astNode.Children, tokens, num + 1);
                }
                if (flag)
                {
                    ast.AddRange(astNode.Children);
                }
                else
                {
                    ast.Add(astNode);
                }
            }
            return num;
        }

        private static int ParseExpression5(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            AST astNode = new AST(Token.Binary, tokens[_index], tokens[_index].Id, tokens[_index].Value);
            int num = ParseExpression6(astNode.Children, tokens, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (tokens[num].Token == Token.Plus || tokens[num].Token == Token.Minus)
                {
                    flag = false;
                    astNode.Children.Add(new AST(tokens[num]));
                    num = ParseExpression6(astNode.Children, tokens, num + 1);
                }
                if (flag)
                {
                    ast.AddRange(astNode.Children);
                }
                else
                {
                    ast.Add(astNode);
                }
            }
            return num;
        }

        private static int ParseExpression6(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            AST astNode = new AST(Token.Binary, tokens[_index], tokens[_index].Id, tokens[_index].Value);
            int num = ParseVariable2(astNode.Children, tokens, _index);
            if (!ParseError)
            {
                bool flag = true;
                while (tokens[num].Token == Token.Time || tokens[num].Token == Token.Divide || tokens[num].Token == Token.Div || tokens[num].Token == Token.Mod)
                {
                    flag = false;
                    astNode.Children.Add(new AST(tokens[num]));
                    num = ParseVariable2(astNode.Children, tokens, num + 1);
                }
                if (flag)
                {
                    ast.AddRange(astNode.Children);
                }
                else
                {
                    ast.Add(astNode);
                }
            }
            return num;
        }

        private static int ParseVariable2(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            List<AST> list = new List<AST>();
            int num = ParseTerm(list, tokens, _index);
            if (!ParseError)
            {
                if (tokens[num].Token == Token.Dot)
                {
                    AST astNode = new AST(Token.Dot, tokens[num], 0);
                    astNode.Children.AddRange(list);
                    ast.Add(astNode);
                    while (tokens[num].Token == Token.Dot)
                    {
                        num = ParseVariable(astNode.Children, tokens, num + 1);
                    }
                }
                else
                {
                    ast.AddRange(list);
                }
            }
            return num;
        }

        private static int ParseTerm(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            int num = _index;
            switch (tokens[num].Token)
            {
                case Token.Function:
                    num = ParseFunction(ast, tokens, num);
                    break;
                case Token.Constant:
                    ast.Add(new AST(tokens[num]));
                    num++;
                    break;
                case Token.Open:
                    num = ParseExpression1(ast, tokens, num + 1);
                    if (tokens[num].Token != Token.Close)
                    {
                        AddError("Symbol ) expected", Script, tokens[num]);
                    }
                    num++;
                    break;
                case Token.Variable:
                    num = ParseVariable(ast, tokens, num);
                    break;
                case Token.Not:
                case Token.Plus:
                case Token.Minus:
                case Token.BitNegate:
                    {
                        AST astNode = new AST(Token.Unary, tokens[num], (int)tokens[num].Token);
                        num = ParseVariable2(astNode.Children, tokens, num + 1);
                        ast.Add(astNode);
                        break;
                    }
                default:
                    AddError("unexpected symbol in expression", Script, tokens[num]);
                    break;
            }
            return num;
        }

        private static int ParseVariable(List<AST> ast, List<GMLToken> tokens, int _index)
        {
            if (tokens[_index].Token != Token.Variable)
            {
                AddError("variable name expected", Script, tokens[_index]);
            }
            AST astNode = new AST(tokens[_index]);
            ast.Add(astNode);
            int num = _index + 1;
            GMLVariable value;
            if (tokens[num].Token == Token.ArrayOpen)
            {
                num++;
                while (tokens[num].Token != Token.ArrayClose && tokens[num].Token != Token.EOF)
                {
                    num = ParseExpression1(astNode.Children, tokens, num);
                    if (tokens[num].Token == Token.SepArgument)
                    {
                        num++;
                    }
                    else if (tokens[num].Token != Token.ArrayClose)
                    {
                        AddError("symbol , or ] expected", Script, tokens[num]);
                    }
                }
                if (tokens[num].Token == Token.EOF)
                {
                    AddError("symbol ] expected", Script, tokens[num]);
                }
                num++;
                if (astNode.Children.Count >= 3)
                {
                    AddError("only 1 or 2 dimensional arrays are supported", Script, tokens[num]);
                }
            }
            else if (Builtins.GlobalArrays.TryGetValue(astNode.Text, out value) || Builtins.LocalArrays.TryGetValue(astNode.Text, out value))
            {
                AST astNode2 = new AST(Token.Constant, -1, "0");
                astNode2.Value = new GMLValue(0.0);
                astNode.Children.Add(astNode2);
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
