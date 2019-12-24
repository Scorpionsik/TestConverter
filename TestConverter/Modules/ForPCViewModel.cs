using CoreWPF.MVVM;
using CoreWPF.Utilites;
using CoreWPF.Utilites.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConverter.Modules
{
    public class ForPCViewModel : ModuleViewModel
    {
        public override string Title { get { return "Конвертер *.qst в *.xml"; } }
        public override string SavefileFilter { get { return "xml"; } }

        public ForPCViewModel() : base("Тест QST|*.qst") { }

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
