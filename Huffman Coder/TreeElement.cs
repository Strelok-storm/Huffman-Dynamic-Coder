﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman_Coder
{
	class TreeElement
	{
		TreeElement _parent;
		int _weight;
		Byte? _symbol;
		TreeElement _right;
		TreeElement _left;
		bool _isEOF;
		bool _isESC;

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

		public Byte? Symbol
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

		public bool IsEOF
		{
			get{ return _isEOF; }
			set{ _isEOF = value; }
		}

		public bool IsESC
		{
			get{ return _isESC; }
			set{ _isESC = value; }
		}

		public TreeElement(TreeElement parent, int weight, Byte? symbol, TreeElement left, TreeElement right, bool isEof, bool isEsc)
		{
			Parent = parent;
			Weight = weight;
			Symbol = symbol;
			Left = left;
			Right = right;
			IsEOF = isEof;
			IsESC = isEsc;
		}
	}
}
