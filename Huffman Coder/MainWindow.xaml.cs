using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using Microsoft.Win32;
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
		List<char> outputString;
		
		public MainWindow()
		{
			InitializeComponent();
			tree = new List<TreeElement>();
			outputString = new List<char>();
			InitTree();
		}
		
		private void InitTree()
		{
			tree.Add(new TreeElement(null, 0, null, null, null, false, false));
			tree.Add(new TreeElement(tree[0], 0, null, null, null, false, true));
			tree.Add(new TreeElement(tree[0], 0, null, null, null, true, false));
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
			TreeElement k = new TreeElement(null, tree[current].Weight, tree[current].Symbol, tree[current].Left, tree[current].Right, tree[current].IsEOF, tree[current].IsESC);//Ссылки родитля ставятся не на объекты, а на элементы списка!
			if (tree[current].Right != null && tree[current].Left != null)
			{
				tree[current].Right.Parent = tree[second];
				tree[current].Left.Parent = tree[second];
			}
			tree[current].Left = tree[second].Left;
			tree[current].Right = tree[second].Right;
			tree[current].Weight = tree[second].Weight;
			tree[current].Symbol = tree[second].Symbol;
			tree[current].IsEOF = tree[second].IsEOF;
			tree[current].IsESC = tree[current].IsESC;

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
			tree[second].IsEOF = k.IsEOF;
			tree[second].IsESC = k.IsESC;
		}

		private void AddNewSymbol(Byte symbol)
		{
			tree.Insert(tree.Count - 3, new TreeElement(tree[tree.Count - 3].Parent, 0, null, tree[tree.Count - 3], null, false, false));//Установка левого элемена и родителя нового родителя
			tree.Insert(tree.Count - 3, new TreeElement(tree[tree.Count - 4], 1, symbol, null, null, false, false));//Полная устаовка нового символа
			tree[tree.Count - 5].Right = tree[tree.Count - 4];//Правый элемент нового родителя
			if(tree[tree.Count-5].Parent!=null)
				tree[tree.Count - 5].Parent.Left = tree[tree.Count - 5];
			tree[tree.Count - 3].Parent = tree[tree.Count - 5];//Родитель 0-символа
			ReBuildTree(tree.Count-5);
		}

		private bool SearchSymbol(Byte symbol)
		{
			foreach(TreeElement e in tree)
			{
				if (e.Symbol!=null && e.Symbol==symbol)
					return true;
			}
			return false;
		}

		private void Coder(string filePath)
		{
			BinaryReader inputFileReader = new BinaryReader(File.Open(filePath, FileMode.Open));
			BinaryWriter outputFileWriter = new BinaryWriter(File.Open("code.package", FileMode.Create));
			while (true)//Пока в файле есть символы
			{
				try
				{
					Byte symbol = inputFileReader.ReadByte();
				
				if(SearchSymbol(symbol)==false)
				{
					string exitCode = "";
					GetESCCode(ref exitCode, tree[0], "");
					/*if(exitCode.Length>8)//Разбиение строки на частички по 8 бит
					{
						
					}
					Byte b = Convert.ToByte(exitCode, 2);
					outputFileWriter.Write(b);
					outputFileWriter.Write(symbol);
					outputFileWriter.Flush();*/
					foreach(char c in exitCode)
					{
							outputString.Add(c);
					}
					//string symbolString = Convert.ToString(symbol, 2);
					foreach(char c in Convert.ToString(symbol, 2))
					{
							outputString.Add(c);
					}
					AddNewSymbol(symbol);
				}
				else
				{
					string exitCode = "";
					GetWayToSymbol(symbol, ref exitCode, tree[0], "");
					/*Byte b = Convert.ToByte(exitCode);
					outputFileWriter.Write(b);
					outputFileWriter.Flush();*/
					foreach(char c in exitCode)
					{
						outputString.Add(c);
					}
					ReBuildTree(GetSymbolIndex(symbol));
				}
				}
				catch (EndOfStreamException ex)
				{
					break;
				}
			}
			string output = "";
			//ЗАписать по 8 бит в байты и писать их в файл.
			for (int i=0; i<outputString.Count; i++)
			{
				output += outputString[i];
				if(output.Length==8)
				{
					Byte b = Convert.ToByte(output, 2);
					outputFileWriter.Write(b);
					outputFileWriter.Flush();
					output = "";
				}
			}
		}

		private int GetSymbolIndex(Byte? symbol)
		{
			return tree.FindIndex((e) => e.Symbol != null && e.Symbol==symbol);
		}
		
		private void GetESCCode(ref string way, TreeElement current, string outputWay)
		{
			
			if (current.IsESC == true)
			{
				way = outputWay;
				return;
			}
			if(current.Left!=null)
			{
				way += "0";
				GetESCCode(ref way, current.Left, outputWay);
			}
			if(current.Right!=null)
			{
				way += "1";
				GetESCCode(ref way, current.Right, outputWay);
			}
		}

		private void GetWayToSymbol(Byte? symbol, ref string findedWay, TreeElement current, string way)
		{
			if(current.Symbol!=null && current.Symbol==symbol)
			{
				findedWay = way;
				return;
			}
			if(current.Left!=null)
			{
				way += "0";
				GetWayToSymbol(symbol, ref findedWay, current.Left, way);
			}
			if(current.Right!=null)
			{
				way += "1";
				GetWayToSymbol(symbol, ref findedWay, current.Right, way);
			}
		}

		private void CoderClick(object sender, EventArgs e)
		{
			string filePath = "";
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.ShowDialog();
			try
			{
				filePath = fileDialog.FileName;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Невозможно прочитать файл!" + ex.Message);
			}
			Console.WriteLine(filePath);
			Coder(filePath);
		}
	}
}
