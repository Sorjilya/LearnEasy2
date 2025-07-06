using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LearnEasy2
{
	internal class CardsButtonContext
	{
		public TextBox txtbox { get; set; }
		public int MyIndex { get; set; }
		public Label lblPoints { get; set; }
		public Timer roundTimer { get; set; }
		public ProgressBar progressBar { get; set; }
		public int difficulty { get; set; }
		public int group { get; set; }
	}
}
