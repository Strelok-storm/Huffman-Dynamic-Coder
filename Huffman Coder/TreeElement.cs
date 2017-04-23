using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman_Coder
{
	class TreeElement:ICloneable
	{
		TreeElement _parent;
		int _weight;
		string _symbol;
		TreeElement _right;
		TreeElement _left;

		public TreeElement Parent
		{
			get { return _parent; }
			set { _parent = value; }
		}

		public int Weight
		{
			get { return _weight; }
			set
			{
				if (value >= 0)
					_weight = value;
			}
		}

		public string Symbol
		{
			get{ return _symbol; }
			set{ _symbol = value; }
		}

		public TreeElement Right
		{
			get{ return _right; }
			set{ _right = value; }
		}

		public TreeElement Left
		{
			get{ return _left; }
			set{ _left = value; }
		}

		public TreeElement(TreeElement parent, int weight, string symbol, TreeElement left, TreeElement right)
		{
			Parent = parent;
			Weight = weight;
			Symbol = symbol;
			Left = left;
			Right = right;
		}

		public void SetValues(TreeElement target)
		{
			Weight = target.Weight;
			Symbol = target.Symbol;
			Left = target.Left;
			Right = target.Right;
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
