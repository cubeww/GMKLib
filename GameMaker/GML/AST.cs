using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace GameMaker.GML
{
    public class AST
    {
        private static int TabCount { get; set; }

        public Token Token
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public GMLValue Value
        {
            get;
            set;
        }

        public List<AST> Children
        {
            get;
            set;
        }

        public AST(Token _tok, int _index, string _text)
        {
            Token = _tok;
            Index = _index;
            Text = _text;
            Children = new List<AST>();
        }

        public AST(Token _tok, GMLToken _pass1, int _id)
        {
            Token = _tok;
            Index = _pass1.Index;
            Text = _pass1.Text;
            Id = _id;
            Value = new GMLValue();
            Children = new List<AST>();
        }

        public AST(Token _tok, GMLToken _pass1, int _id, GMLValue _value)
        {
            Token = _tok;
            Index = _pass1.Index;
            Text = _pass1.Text;
            Id = _id;
            Value = new GMLValue(_value);
            Children = new List<AST>();
        }
        public AST(GMLToken _tok)
        {
            Token = _tok.Token;
            Index = _tok.Index;
            Text = _tok.Text;
            Id = _tok.Id;
            Value = new GMLValue(_tok.Value);
            Children = new List<AST>();
        }
        public AST(AST _tok)
        {
            Token = _tok.Token;
            Index = _tok.Index;
            Text = _tok.Text;
            Id = _tok.Id;
            Value = new GMLValue(_tok.Value);
            Children = new List<AST>(_tok.Children);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("< tok={0:G}, index={1}, id={3}, text=\"{2}\", value={4} ", Token, Index, Text, Id, (Value != null) ? Value.ToString() : "null");
            if (Children.Count > 0)
            {
                stringBuilder.Append("Children=[ \n");
                TabCount++;
                foreach (AST child in Children)
                {
                    for (int i = 0; i < TabCount; i++)
                    {
                        stringBuilder.AppendFormat("  ");
                    }
                    stringBuilder.Append(child.ToString());
                    stringBuilder.Append(",\n");
                }
                TabCount--;
                for (int j = 0; j < TabCount; j++)
                {
                    stringBuilder.AppendFormat("  ");
                }
                stringBuilder.Append("]");
            }
            stringBuilder.Append('>');
            return stringBuilder.ToString();
        }

        public string Format()
        {
            string str = "";

            void FormatAST(AST ast)
            {
                int i = 0;
                switch (ast.Token)
                {
                    case Token.Var:
                        str += "var ";
                        foreach (var child in ast.Children)
                        {
                            FormatAST(child);
                            if (i++ != ast.Children.Count - 1)
                                str += ", ";
                        }
                        break;
                    case Token.Variable:
                        str += ast.Text;
                        break;
                    case Token.Assign:
                        FormatAST(ast.Children[0]);
                        str += " = ";
                        FormatAST(ast.Children[2]);
                        break;
                    case Token.Constant:
                        str += ast.Text;
                        break;
                    case Token.Binary:
                        FormatAST(ast.Children[0]);
                        FormatAST(ast.Children[1]);
                        FormatAST(ast.Children[2]);
                        break;
                    case Token.Function:
                        str += ast.Text;
                        str += "(";
                        foreach (var child in ast.Children)
                        {
                            FormatAST(child);
                            if (i++ != ast.Children.Count - 1)
                                str += ", ";
                        }
                        str += ")";
                        break;
                    case Token.Dot:
                        FormatAST(ast.Children[0]);
                        str += ".";
                        FormatAST(ast.Children[1]);
                        break;
                    case Token.Block:
                        str += "{";
                        foreach (var child in ast.Children)
                        {
                            FormatAST(child);
                            str += ";";
                        }
                        str += "}";
                        break;
                    default:
                        str += ast.Text;
                        break;
                }
            }

            foreach (var child in Children)
            {
                FormatAST(child);
                str += ";";
            }

            return str;
        }
    }
}
