using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_LDDP.Services
{
	public class Proposition_class
	{
		public int IdP { get; set; }
		public int Pourcentage { get; set; }
		public string NameP { get; set; }
		public string Token { get; set; }
		public Color RandomColor { get; set; }

		public Proposition_class()
		{
			Random random = new Random();
			RandomColor = Color.FromRgb(random.Next(256), random.Next(256), random.Next(256));
		}
	}


}
