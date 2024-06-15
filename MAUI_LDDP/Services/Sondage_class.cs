using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_LDDP.Services
{
	public class Sondage_class
	{
		public string Createur { get; set; }
		public DateTime DateJ { get; set; }
		public bool Fini { get; set; }
		public string NameJ { get; set; }
		public string Token { get; set; }
		public bool IsCurrentUserCreator { get; set; }
	}

}
