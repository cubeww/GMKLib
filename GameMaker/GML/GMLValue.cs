namespace GameMaker.GML
{
	public class GMLValue
	{
		public Kind Kind
		{
			get;
			set;
		}

		public double ValueI
		{
			get;
			set;
		}

		public string ValueS
		{
			get;
			set;
		}

		public GMLValue()
		{
			Kind = Kind.None;
		}

		public GMLValue(double _value)
		{
			ValueI = _value;
			Kind = Kind.Number;
		}

		public GMLValue(string _value)
		{
			Kind = Kind.String;
			ValueS = _value;
		}

		public GMLValue(GMLValue _value)
		{
			Kind = _value.Kind;
			ValueI = _value.ValueI;
			ValueS = _value.ValueS;
		}

		public override string ToString()
		{
			return string.Format("[ kind={0:G}, val={1}]", Kind, (Kind == Kind.None) ? "none" : ((Kind == Kind.Number) ? ValueI.ToString() : ValueS.ToString()));
		}
	}
}
