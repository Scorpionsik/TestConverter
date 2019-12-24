using CoreWPF.Utilites;
using CoreWPF.Utilites.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestConverter.Modules
{
    public class ForMobileViewModel : ModuleViewModel
    {
        public override string Title { get { return "Конвертер *.xml в *.qst"; } }
        public override string SavefileFilter { get { return "qst"; } }

        private string Test;

        public ForMobileViewModel() : base("Экспортированный тест MTX|*.xml") { }

        public override void SaveFile(string path)
        {
            if (path == null || path.Length == 0) return;
            File.WriteAllText(path, Test);
        }

        public override RelayCommand Command_StartConvert
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    this.Test = "";
                    XmlDocument tmp_loadTest = new XmlDocument();
                    XmlNode tmp_elem;
                    tmp_loadTest.Load(this.Filepath);
                    foreach(XmlNode tmp_question in tmp_loadTest.GetElementsByTagName("Task"))
                    {
                        string tmp_writeQuestion = "? ";
                        XmlNode tmp_testTitle = tmp_question.SelectSingleNode("QuestionText/PlainText");
                        if (tmp_testTitle != null) tmp_writeQuestion += tmp_testTitle.InnerText + "\r\n";
                        else tmp_writeQuestion += " " + "\r\n";
                        foreach (XmlNode tmp_variant in tmp_question.SelectNodes("Variants/VariantText"))
                        {
                            if (tmp_variant.Attributes["CorrectAnswer"].InnerText == "True") tmp_writeQuestion += "+ ";
                            else tmp_writeQuestion += "- ";
                            tmp_writeQuestion += tmp_variant.FirstChild.InnerText + "\r\n";
                        }
                        this.Test += tmp_writeQuestion;
                    }
                    this.SaveFile(this.GetPathForSavefile());
                },
                (obj) => this.Filepath != null && this.Filepath.Length > 0);
            }
        }
    }
}
