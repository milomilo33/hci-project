using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Service
{
	public interface ISaradnikService
	{
		ObservableCollection<Saradnik> sviSaradnici();

		Saradnik getSaradnici(int id);

		void RemoveSaradnik(int id);

		void UpdateSaradnik(Saradnik saradnik);

	}
}
