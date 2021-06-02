using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Service
{
	public interface IPonudaService
	{
		ObservableCollection<Ponuda> svePonude();

		Ponuda getPonuda(int id);
	}
}
