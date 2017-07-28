using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GomeOfLife
{
	public class Cell
	{
		private bool _isAlive;

		public bool IsAlive
		{
			get { return _isAlive; }
			set
			{
				_isAlive = value;

				if (_isAlive)
				{
					TurnCellOn();
				}
				else
				{
					TurnCellOff();
				}
			}
		}

		public bool NextState { get; set; }
		public Rectangle CellShape { get; set; }

		public double PositionX { get; set; }

		public double PositionY { get; set; }

		public Cell(Rectangle shape, double positionX, double positionY, LifeContainer lifeContainer)
		{
			CellShape = shape;
			PositionX = positionX;
			PositionY = positionY;
			CellShape.MouseDown += CellShape_MouseDown;
			lifeContainer.OnLifeCycleCompleted += LifeContainer_OnLifeCycleCompleted;
		}

		private void LifeContainer_OnLifeCycleCompleted(object sender, EventArgs e)
		{
			IsAlive = NextState;
		}

		private void CellShape_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			IsAlive = !IsAlive;
		}

		private void TurnCellOff()
		{
			CellShape.Fill = new SolidColorBrush(Colors.Black);
		}

		private void TurnCellOn()
		{
			CellShape.Fill = new SolidColorBrush(Colors.GreenYellow);
		}
	}
}