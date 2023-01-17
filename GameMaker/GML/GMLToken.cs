using System.Collections.Generic;
using System.Text;

namespace GameMaker.GML
{
	public class GMLToken
	{
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
		}

		public GMLToken(Token _tok, GMLToken _token, int _id)
		{
			Token = _tok;
			Index = _token.Index;
			Text = _token.Text;
			Id = _id;
			Value = new GMLValue();
		}

		public GMLToken(Token _tok, GMLToken _token, int _id, GMLValue _value)
		{
			Token = _tok;
			Index = _token.Index;
			Text = _token.Text;
			Id = _id;
			Value = new GMLValue(_value);
		}
	}
}
