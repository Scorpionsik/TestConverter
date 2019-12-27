using CoreWPF.Utilites;
using System.IO;
using System.Xml;
using System;

namespace TestConverter.Modules
{
    public class ForMobileViewModel : ModuleViewModel
    {
        public override string Title { get { return "Конвертер *.xml в *.qst"; } }
        public override string SavefileFilter { get { return "Тест QST|*.qst"; } }

        private string Test;

        public ForMobileViewModel() : base("Экспортированный тест MTX|*.xml") { }

        public override void SaveFile(string path)
        {
            if (path == null || path.Length == 0) return;
            File.WriteAllText(path, Test);
        }

        public override bool CheckFile()
        {
            int question = 0;
            try
            {
                XmlDocument tmp_loadTest = new XmlDocument();
                tmp_loadTest.Load(this.Filepath);
            
                XmlNodeList tmp_list = tmp_loadTest.GetElementsByTagName("Task");
                if (tmp_list == null || tmp_list.Count == 0) throw new System.Exception("Структура файла неверна: тег Task отсутствует");
                else
                {
                    foreach (XmlNode node in tmp_list)
                    {
                        question++;
                        if (node.SelectSingleNode("QuestionText/PlainText") == null) throw new System.Exception("Структура файла неверна: тег QuestionText/PlainText отсутствует");
                        foreach (XmlNode tmp_variant in node.SelectNodes("Variants/VariantText"))
                        {
                            if (tmp_variant.Attributes.GetNamedItem("CorrectAnswer") == null) throw new System.Exception("Структура файла неверна: в теге VariantText нет аттрибута CorrectAnswer");
                            if (tmp_variant.ChildNodes == null || tmp_variant.ChildNodes.Count == 0 || tmp_variant.SelectSingleNode("PlainText") == null) throw new System.Exception("Структура файла неверна: тег VariantText неверно оформлен");
                        }

                    }
                }
                if (this.ErrorString != null) this.ErrorString = null;
                return true;
            }
            catch(Exception ex)
            {
                this.ErrorString = ex.Message;
                if (question > 0) this.ErrorString += ", вопрос: " + question.ToString();
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
                    if (!File.Exists(App.AppPath + "1.chm")) File.WriteAllBytes("1.chm", Properties.Resources._1);
                    System.Diagnostics.Process.Start(App.AppPath + "1.chm");
                });
            }
        }
    }
}
