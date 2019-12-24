using CoreWPF.Utilites;
using CoreWPF.Utilites.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConverter.Modules
{
    public class ForMobileViewModel : ModuleViewModel
    {
        public override string Title { get { return "Конвертер *.xml в *.qst"; } }
        public override string SavefileFilter { get { return "qst"; } }

        public ForMobileViewModel() : base("Экспортированный тест MTX|*.xml") { }

        public override void SaveFile(string path)
        {

        }

        public override RelayCommand Command_StartConvert
        {
            get
            {
                return new RelayCommand(obj =>
                {

                });
            }
        }
    }
}
