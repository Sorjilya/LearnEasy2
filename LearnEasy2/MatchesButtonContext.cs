using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LearnEasy2
{
	class MatchesButtonContext
	{
		public List<Button> LeftButtons { get; set; }
		public List<Button> RightButtons { get; set; }
		public int MyIndex { get; set; }
		public Label lblPoints { get; set; }
		public Timer roundTimer { get; set; }
		public ProgressBar progressBar { get; set; }
		public int difficulty { get; set; }
		public int group { get; set; }
	}
}
