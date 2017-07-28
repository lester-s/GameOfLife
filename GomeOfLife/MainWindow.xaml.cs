using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace GomeOfLife
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly int _squareSize = 20;
		private readonly int _speedOfLife = 100;
		private Cell[,] _cellGrid;
		private double _columnAmount;
		private double _rowAmount;
		private LifeContainer _lifeContainer;

		public MainWindow()
		{
			InitializeComponent();
			IsLifeOn = false;
			_lifeContainer = new LifeContainer();
			Loaded += MainWindow_Loaded;
		}

		private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			DrawGrid();
		}

		private void DrawGrid()
		{
			_columnAmount = Math.Floor(MainCanvas.ActualWidth / _squareSize);
			_rowAmount = Math.Floor(MainCanvas.ActualHeight / _squareSize);
			_cellGrid = new Cell[(int)_rowAmount, (int)_columnAmount];

			for (int i = 0; i < _rowAmount; i++)
			{
				for (int j = 0; j < _columnAmount; j++)
				{
					var newCell = new Cell(DrawRectangle(i, j), i, j, _lifeContainer);
					_cellGrid[i, j] = newCell;
				}
			}
		}

		private Rectangle DrawRectangle(int rowFactor, int colFactor)
		{
			Rectangle rec = new Rectangle();
			Canvas.SetTop(rec, rowFactor * _squareSize);
			Canvas.SetLeft(rec, colFactor * _squareSize);
			rec.Width = _squareSize;
			rec.Height = _squareSize;
			rec.Fill = new SolidColorBrush(Colors.Black);
			rec.Stroke = new SolidColorBrush(Colors.Red);
			MainCanvas.Children.Add(rec);
			return rec;
		}

		public class Square
		{
			public double PositionX { get; set; }
			public double PositionY { get; set; }
			public double Size { get; set; }
		}

		public bool IsLifeOn { get; set; }

		private async void StartLife(object sender, RoutedEventArgs e)
		{
			IsLifeOn = !IsLifeOn;
			lifeButton.Content = IsLifeOn ? "Pause life" : "Start life";
			if (IsLifeOn)
			{
				ProceedWithLife();
			}
		}

		private async void ProceedWithLife()
		{
			await Task.Run(ComputeLife);

			_lifeContainer.Tick();

			if (IsLifeOn)
			{
				ProceedWithLife();
			}
		}

		private async Task ComputeLife()
		{
			for (int i = 0; i < _rowAmount; i++)
			{
				for (int j = 0; j < _columnAmount; j++)
				{
					var aliveNeighbour = 0;

					if (i > 0)
					{
						if (j > 0)
						{
							if (_cellGrid[i - 1, j - 1].IsAlive)
							{
								aliveNeighbour++;
							}
						}

						if (_cellGrid[i - 1, j].IsAlive)
						{
							aliveNeighbour++;
						}

						if (j < _columnAmount - 1)
						{
							if (_cellGrid[i - 1, j + 1].IsAlive)
							{
								aliveNeighbour++;
							}
						}
					}

					if (j > 0)
					{
						if (_cellGrid[i, j - 1].IsAlive)
						{
							aliveNeighbour++;
						}
					}

					if (j < _columnAmount - 1)
					{
						if (_cellGrid[i, j + 1].IsAlive)
						{
							aliveNeighbour++;
						}
					}

					if (i < _rowAmount - 1)
					{
						if (j > 0)
						{
							if (_cellGrid[i + 1, j - 1].IsAlive)
							{
								aliveNeighbour++;
							}
						}

						if (_cellGrid[i + 1, j].IsAlive)
						{
							aliveNeighbour++;
						}

						if (j < _columnAmount - 1)
						{
							if (_cellGrid[i + 1, j + 1].IsAlive)
							{
								aliveNeighbour++;
							}
						}
					}

					var currentCell = _cellGrid[i, j];

					if (currentCell.IsAlive)
					{
						if (aliveNeighbour < 2 || aliveNeighbour > 3)
						{
							currentCell.NextState = false;
						}
						else
						{
							currentCell.NextState = true;
						}
					}
					else
					{
						if (aliveNeighbour == 3)
						{
							currentCell.NextState = true;
						}
					}
				}
			}
			Thread.Sleep(_speedOfLife);
		}

		private void OnCleanClick(object sender, RoutedEventArgs e)
		{
			for (int i = 0; i < _rowAmount; i++)
			{
				for (int j = 0; j < _columnAmount; j++)
				{
					_cellGrid[i, j].NextState = false;
				}
			}

			_lifeContainer.Tick();
		}
	}
}