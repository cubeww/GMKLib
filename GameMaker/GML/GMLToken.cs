using System.Collections.Generic;
using System.Text;

namespace GameMaker.GML
{
	public class GMLToken
	{
		//private static int TabCount { get; set; }

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

		public GMLToken(Token _tok, int _index, string _text)
		{
			Token = _tok;
			Index = _index;
			Text = _text;
			//Children = new List<GMLToken>();
		}

		public GMLToken(Token _tok, GMLToken _pass1, int _id)
		{
			Token = _tok;
			Index = _pass1.Index;
			Text = _pass1.Text;
			Id = _id;
			Value = new GMLValue();
			//Children = new List<GMLToken>();
		}

		public GMLToken(Token _tok, GMLToken _pass1, int _id, GMLValue _value)
		{
			Token = _tok;
			Index = _pass1.Index;
			Text = _pass1.Text;
			Id = _id;
			Value = new GMLValue(_value);
			//Children = new List<GMLToken>();
		}

		public GMLToken(GMLToken _tok)
		{
			Token = _tok.Token;
			Index = _tok.Index;
			Text = _tok.Text;
			Id = _tok.Id;
			Value = new GMLValue(_tok.Value);
			//Children = new List<GMLToken>(_tok.Children);
		}

		//public override string ToString()
		//{
		//	StringBuilder stringBuilder = new StringBuilder();
		//	stringBuilder.AppendFormat("< tok={0:G}, index={1}, id={3}, text=\"{2}\", value={4} ", Token, Index, Text, Id, (Value != null) ? Value.ToString() : "null");
		//	if (Children.Count > 0)
		//	{
		//		stringBuilder.Append("Children=[ \n");
		//		TabCount++;
		//		foreach (GMLToken child in Children)
		//		{
		//			for (int i = 0; i < TabCount; i++)
		//			{
		//				stringBuilder.AppendFormat("  ");
		//			}
		//			stringBuilder.Append(child.ToString());
		//			stringBuilder.Append(",\n");
		//		}
		//		TabCount--;
		//		for (int j = 0; j < TabCount; j++)
		//		{
		//			stringBuilder.AppendFormat("  ");
		//		}
		//		stringBuilder.Append("]");
		//	}
		//	stringBuilder.Append('>');
		//	return stringBuilder.ToString();
		//}
	}
}
