using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConverter.Modules;

namespace TestConverterTest
{
    [TestClass]
    public class TestForMobileViewModel
    {
        private static string TestPath = new Regex(@"[^\\]+$").Replace(Assembly.GetExecutingAssembly().Location, "") + "TestFiles\\";
        private static string SavePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Desktop";


        [TestMethod]
        public void TestSaveFile()
        {
            ForMobileViewModel Test_model = new ForMobileViewModel();
            string s = Assembly.GetExecutingAssembly().Location;
            string saveFile = "";
            try
            {
                Test_model.Command_GetFilepath.Execute(TestPath + "test.xml");
                Test_model.Command_StartConvert.Execute(true);
                saveFile = SavePath + "\\" + Test_model.Filename + new Regex(@"\.[^.]+$").Match(Test_model.SavefileFilter);
                Test_model.SaveFile(saveFile);
                Assert.IsTrue(File.Exists(saveFile));
            }
            catch(Exception ex) { Assert.Fail(ex.Message); }
            finally
            {
                if (File.Exists(saveFile)) File.Delete(saveFile);
            }
        }

        [TestMethod]
        public void TestCommand_StartConvert()
        {
            try
            {
                ForMobileViewModel Test_model = new ForMobileViewModel();
                Test_model.Command_GetFilepath.Execute(TestPath + "test.xml");
                Test_model.Command_StartConvert.Execute(true);
            }
            catch(Exception ex) { Assert.Fail(ex.Message); }
        }

        [TestMethod]
        public void TestCheckFile_GoodFile()
        {
            ForMobileViewModel Test_model = new ForMobileViewModel();
            try
            {
                Test_model.Command_GetFilepath.Execute(TestPath + "test.xml");
                Assert.IsTrue(Test_model.CheckFile());
            }
            catch (Exception ex) { Assert.Fail(ex.Message +"\r\n\r\n"+ Test_model.ErrorString); }
        }

        [TestMethod]
        public void TestCheckFile_BadFile()
        {
            ForMobileViewModel Test_model = new ForMobileViewModel();
            try
            {
                Test_model.Command_GetFilepath.Execute(TestPath + "test-bad.xml");
                Assert.IsFalse(Test_model.CheckFile());
            }
            catch (Exception ex) { Assert.Fail(ex.Message + "\r\n\r\n" + Test_model.ErrorString); }
        }
    }
}
