using CoreWPF.Utilites;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Linq;
using System.Windows;
using System.Reflection;

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

        static void booksSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                MessageBox.Show("WARNING");
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                MessageBox.Show("ERROR");
            }
            else MessageBox.Show("OK");
        }

        public override bool CheckFile()
        {
            try
            {
                XmlDocument tmp_loadTest = new XmlDocument();
                tmp_loadTest.Load(this.Filepath);
            
                XmlNodeList tmp_list = tmp_loadTest.GetElementsByTagName("Task");
                if (tmp_list == null || tmp_list.Count == 0) throw new System.Exception();
                else
                {
                    foreach (XmlNode node in tmp_list)
                    {
                        if (node.SelectSingleNode("QuestionText/PlainText") == null) throw new System.Exception();
                        foreach (XmlNode tmp_variant in node.SelectNodes("Variants/VariantText"))
                        {
                            if (tmp_variant.Attributes.GetNamedItem("CorrectAnswer") == null) throw new System.Exception();
                            if (tmp_variant.ChildNodes == null || tmp_variant.ChildNodes.Count == 0 || tmp_variant.SelectSingleNode("PlainText") == null) throw new System.Exception();
                        }

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override RelayCommand Command_StartConvert
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    this.Test = "";
                    XmlDocument tmp_loadTest = new XmlDocument();

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
                (obj) => base.Command_StartConvert.CanExecute());
            }
        }

        public override RelayCommand Command_HelpLink
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    App.KillHelpWindows();
                    if (!File.Exists(App.AppPath + "2.chm")) File.WriteAllBytes("2.chm", Properties.Resources._1);
                    System.Diagnostics.Process.Start(App.AppPath + "2.chm");
                });
            }
        }
    }
}
