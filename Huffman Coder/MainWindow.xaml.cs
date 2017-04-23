using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Huffman_Coder
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		List<TreeElement> tree;
		string symbolString = "";
		public MainWindow()
		{
			InitializeComponent();
			tree = new List<TreeElement>();
			InitTree();
		}
		
		private void InitTree()
		{
			tree.Add(new TreeElement(null, 0, "", null, null));
			tree.Add(new TreeElement(tree[0], 0, "ESC", null, null));
			tree.Add(new TreeElement(tree[0], 0, "EOF", null, null));
			tree[0].Left = tree[1];
			tree[0].Right = tree[2];
		}
		
		private void ReBuildTree(int currIndex)
		{
			while(true)
			{
				int num = 0;
				tree[currIndex].Weight++;
				if (tree[currIndex].Parent == null)
				{
					break;
				}
				else
				{
					int i = currIndex;
					while(tree[currIndex].Weight>tree[i-1].Weight)
					{
						num = i - 1;
						i--;
					}
					if (num!=0)
					{
						
						Swap(currIndex, num);
					}
					else
					{
						num = currIndex;
					}
				}
				currIndex = tree.FindIndex((e) => e == tree[num].Parent);
			}
		}

		private void Swap(int current, int second)//Обмен ссылок реализован неверно
		{
			TreeElement k = new TreeElement(null, tree[current].Weight, tree[current].Symbol, tree[current].Left, tree[current].Right);//Ссылки родитля ставятся не на объекты, а на элементы списка!
			if (tree[current].Right != null && tree[current].Left != null)
			{
				tree[current].Right.Parent = tree[second];
				tree[current].Left.Parent = tree[second];
			}
			tree[current].Left = tree[second].Left;
			tree[current].Right = tree[second].Right;
			tree[current].Weight = tree[second].Weight;
			tree[current].Symbol = tree[second].Symbol;

			//Необходимо поменять ссылки у детей (если они есть) на перемещенный объект. 
			if(tree[second].Left!=null && tree[second].Right!=null)
			{
				tree[second].Left.Parent = tree[current];
				tree[second].Right.Parent = tree[current];
			}
			tree[second].Left = k.Left;
			tree[second].Right = k.Right;
			tree[second].Weight = k.Weight;
			tree[second].Symbol = k.Symbol;
		}

		private void AddNewSymbol(string symbol)
		{
			tree.Insert(tree.Count - 3, new TreeElement(tree[tree.Count - 3].Parent, 0, "", tree[tree.Count - 3], null));//Установка левого элемена и родителя нового родителя
			tree.Insert(tree.Count - 3, new TreeElement(tree[tree.Count - 4], 1, symbol, null, null));//Полная устаовка нового символа
			tree[tree.Count - 5].Right = tree[tree.Count - 4];//Правый элемент нового родителя
			if(tree[tree.Count-5].Parent!=null)
				tree[tree.Count - 5].Parent.Left = tree[tree.Count - 5];
			tree[tree.Count - 3].Parent = tree[tree.Count - 5];//Родитель 0-символа
			ReBuildTree(tree.Count-5);
		}

		private bool SearchSymbol(string symbol)
		{
			foreach(TreeElement e in tree)
			{
				if (e.Symbol.Equals(symbol))
					return true;
			}
			return false;
		}

		private void Coder()
		{
			//string out1 = GetWayToSymbol("ESC");
			//string out2 = GetWayToSymbol("EOF");
			//Console.WriteLine(out1 + "    " + out2);
			symbolString = inputFlow.Text;
			while (symbolString.Count<char>() != 0)
			{
				string symbol = symbolString[0].ToString();
				symbolString=symbolString.Remove(0, 1);
				if(SearchSymbol(symbol)==false)
				{
					//string exitCode = GetWayToSymbol("ESC");
					//exitCode += (int)symbol[0];
					//Console.WriteLine(exitCode);
					
					AddNewSymbol(symbol);
				}
				else
				{
					//string exitCode = GetWayToSymbol(symbol);
					ReBuildTree(GetSymbolIndex(symbol));
				}
			}
			foreach (TreeElement e in tree)
			{
				if (e.Parent != null)
				{
					if (e.Right != null)
						Console.WriteLine("Symbol^: " + e.Symbol + "  Weight:  " + e.Weight + "  parentHash:  " + e.Parent.GetHashCode() + "  LeftHash: " + e.Left.GetHashCode() + "  RightHashCode: " + e.Right.GetHashCode() + "       " + e.GetHashCode());
					else
						Console.WriteLine("Symbol^: " + e.Symbol + "  Weight:  " + e.Weight + "  parentHash:  " + e.Parent.GetHashCode() + " Left: " + e.Left + " Right: " + e.Right + "       " + e.GetHashCode());
				}
				else
				{
					if (e.Right != null)
						Console.WriteLine("Symbol^: " + e.Symbol + "  Weight:  " + e.Weight + "  parentHash:  " + e.Parent + "  LeftHash: " + e.Left.GetHashCode() + "  RightHashCode: " + e.Right.GetHashCode() + "       " + e.GetHashCode());
					else
						Console.WriteLine("Symbol^: " + e.Symbol + "  Weight:  " + e.Weight + "  parentHash:  " + e.Parent + " Left: " + e.Left + " Right: " + e.Right + "       " + e.GetHashCode());

				}

			}
		}

		private int GetSymbolIndex(string symbol)
		{
			return tree.FindIndex((e) => e.Symbol.Equals(symbol));
		}

		private void GetWayToSymbol(string symbol, ref string findedWay, TreeElement current)
		{
			string way="";
			if(symbol.Equals(current.Symbol))
			{
				findedWay = way;
				return;
			}
			if(current.Left!=null)
			{
				way += "0";
				GetWayToSymbol(symbol, ref findedWay, current.Left);
			}
			if(current.Right!=null)
			{
				way += "1";
				GetWayToSymbol(symbol, ref findedWay, current.Right);
			}
		}

		private void CoderClick(object sender, EventArgs e)
		{
			Coder();
		}
	}
}
