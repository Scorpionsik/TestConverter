using CoreWPF.Utilites;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace TestConverter.Modules
{
    public class ForPCViewModel : ModuleViewModel
    {
        public override string Title { get { return "Конвертер *.qst в *.xml"; } }
        public override string SavefileFilter { get { return "xml"; } }

        private XmlDocument Test;

        public ForPCViewModel() : base("Тест QST|*.qst") { }

        public override void SaveFile(string path)
        {
            if (path == null || path.Length == 0) return;
            Test.Save(path);
        }

        public override RelayCommand Command_StartConvert
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    //load qst
                    string tmp_qst = File.ReadAllText(this.Filepath);

                    Test = new XmlDocument();
                    Test.LoadXml(Properties.Resources.temp_test);
                    //add title
                    XmlNode tmp_elem = Test.GetElementsByTagName("Title")[0];
                    XmlNode tmp_elem2;
                    XmlNode tmp_elem3;
                    tmp_elem.InnerText = this.Filename ?? "Без названия";

                    //add questions
                    tmp_elem = Test.GetElementsByTagName("Tasks")[0];

                    foreach (Match m in new Regex(@"(\n)?[ \t]*\?.+(\r\n[ \t]*[+-]{1}[^\r]+)+").Matches(tmp_qst))
                    {
                        XmlNode tmp_question = Test.CreateElement("Task");
                        XmlAttribute tmp_attr = Test.CreateAttribute("Type");
                        tmp_attr.InnerText = "TYPE_TASK_CHOICE_SINGLE";
                        tmp_question.Attributes.Append(tmp_attr);
                        tmp_attr = Test.CreateAttribute("Score");
                        tmp_attr.InnerText = "1";
                        tmp_question.Attributes.Append(tmp_attr);
                        tmp_question.AppendChild(Test.CreateElement("Title"));
                        //add question text
                        tmp_elem2 = Test.CreateElement("QuestionText");
                        tmp_elem2.AppendChild(Test.CreateElement("PlainText"));

                        string Q_title = new Regex(@"\?[^\r]+").Match(m.Value).Value;
                        Q_title = new Regex(@"^\?{1,2} ").Replace(Q_title, "");

                        tmp_elem2.FirstChild.InnerText = new Regex(@"[ \t\r\n]*$").Replace(Q_title, "");

                        tmp_question.AppendChild(tmp_elem2);

                        //add options
                        tmp_elem2 = Test.CreateElement("Options");
                        tmp_elem2.AppendChild(Test.CreateElement("IsAllowRandom"));
                        tmp_elem2.LastChild.InnerText = "true";
                        tmp_elem2.AppendChild(Test.CreateElement("IsCompulsory"));
                        tmp_elem2.LastChild.InnerText = "false";
                        tmp_elem2.AppendChild(Test.CreateElement("IsOnlyForEduMode"));
                        tmp_elem2.LastChild.InnerText = "false";
                        tmp_elem2.AppendChild(Test.CreateElement("IsDenyPartially"));
                        tmp_elem2.LastChild.InnerText = "false";

                        tmp_question.AppendChild(tmp_elem2);

                        //add variants
                        tmp_elem2 = Test.CreateElement("Variants");

                        foreach (Match v in new Regex("\r\n[ \t]*[+-]{1}[^\r]+").Matches(m.Value))
                        {
                            tmp_elem3 = Test.CreateElement("VariantText");
                            tmp_attr = Test.CreateAttribute("CorrectAnswer");
                            if (new Regex("\r\n[ \t]*[+]{1}").IsMatch(v.Value))
                            {
                                tmp_attr.InnerText = "True";
                            }
                            else if (new Regex("\r\n[ \t]*[-]{1}").IsMatch(v.Value))
                            {
                                tmp_attr.InnerText = "False";
                            }
                            tmp_elem3.Attributes.Append(tmp_attr);
                            tmp_elem3.AppendChild(Test.CreateElement("PlainText"));
                            tmp_elem3.LastChild.InnerText = new Regex(@"[ \t\r\n]*$").Replace(new Regex("\r\n[ \t]*[+-]{1}[ ]*").Replace(v.Value, ""),"");
                            tmp_elem2.AppendChild(tmp_elem3);
                        }

                        tmp_question.AppendChild(tmp_elem2);
                        //add to main test
                        tmp_elem.AppendChild(tmp_question);
                    }

                    this.SaveFile(this.GetPathForSavefile());
                },
                (obj) => this.Filepath != null && this.Filepath.Length > 0
                );
            }
        }
    }
}
