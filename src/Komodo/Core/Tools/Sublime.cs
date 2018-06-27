using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TechTalk.SpecFlow.Reporting;

namespace Komodo.Core.Tools
{
    public class Sublime
    {
        public Sublime()
        {
        }

        public void CreateSublimeFiles()
        {
            List<BindingInfo> lst = new List<BindingInfo>();

            Assembly[] systemAssembly = AppDomain.CurrentDomain.GetAssemblies();
            Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();

            SublimeCompletions allStepDefinitions = new SublimeCompletions();
            allStepDefinitions.scope = "source.feature , text.gherkin.feature";

            string[] nameValueCollection = ConfigurationManager.AppSettings.AllKeys;


            SublimeCompletions slcom = new SublimeCompletions();
            slcom.scope = "source.feature , text.gherkin.feature";

            BindingCollector pCollector = new BindingCollector();
            pCollector.BuildBindingsFromAssembly(ass, lst);

            AddToStepSublimeCompletions(slcom, lst);
            AddToStepSublimeCompletions(allStepDefinitions, lst);

            SaveSubLimeCompletions(ass.GetName().Name, slcom);


            SaveSubLimeCompletions("StepDefinitions", allStepDefinitions);

            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            string name = Path.GetFileName(codeBase);
            string specFlow = ConfigurationManager.AppSettings["specFlow"];
            string projectName = ConfigurationManager.AppSettings["projectName"];

            SublimeBuild buildFile = new SublimeBuild();
            buildFile.shell = "true";
            buildFile.cmds.Add(Environment.CurrentDirectory + "\\" + name);
            buildFile.cmds.Add(specFlow + " generateAll " + projectName + " /force /verbose");
            buildFile.file_regex = "^  (.*)\\(([0-9]*),([0-9]*)";

            buildFile.BuildSublimeBuildPackage();
        }

        private static void SaveSubLimeCompletions(string filename, SublimeCompletions slcom)
        {
            string json = JsonConvert.SerializeObject(slcom);
            FileInfo fi = new FileInfo(Environment.CurrentDirectory + "/" + filename + ".sublime-completions");
            File.WriteAllText(fi.FullName, json);
        }

        private static void AddToStepSublimeCompletions(SublimeCompletions slcom, List<BindingInfo> lst)
        {
            foreach (var bindingInfo in lst)
            {
                slcom.completions.Add(bindingInfo.BindingType + " " + GetSampleText(bindingInfo));
            }
        }

        private static void AddToStepDefinitionTable(DataTable methodsDt, List<BindingInfo> lst)
        {
            foreach (var bindingInfo in lst)
            {
                DataRow dr = methodsDt.NewRow();
                dr["type"] = bindingInfo.BindingType;
                dr["method"] = GetSampleText(bindingInfo);
                methodsDt.Rows.Add(dr);
            }
        }

        private static string GetSampleText(BindingInfo bindingInfo)
        {
            var sampleText = bindingInfo.Regex.ToString().Trim('$', '^');
            Regex re = new Regex(@"\([^\)]+\)");
            int paramIndex = 0;
            sampleText = re.Replace(sampleText, delegate
            {
                return paramIndex >= bindingInfo.ParameterNames.Length
                               ? "{?param?}"
                               : "{"
                                 +
                                 bindingInfo.ParameterNames[paramIndex++]
                                 + "}";
            });
            return sampleText;
        }

        public class SublimeCompletions
        {
            public string scope { get; set; }
            public List<string> completions { get; set; }

            public SublimeCompletions()
            {
                completions = new List<string>();
            }
        }

        public class SublimeBuildModel
        {
            public string shell { get; set; }
            public string cmd { get; set; }
            public string file_regex { get; set; }
        }

        public class SublimeBuild
        {
            public string shell { get; set; }
            public List<string> cmds { get; set; }
            private string cmd { get; set; }
            public string file_regex { get; set; }

            public SublimeBuild()
            {
                cmds = new List<string>();
            }

            public void BuildSublimeBuildPackage()
            {
                SublimeBuildModel sblm = new SublimeBuildModel();
                sblm.shell = shell;
                sblm.cmd = string.Join(" && ", cmds);
                sblm.file_regex = file_regex;

                string json = JsonConvert.SerializeObject(sblm);
                FileInfo fi = new FileInfo(Environment.CurrentDirectory + "/" + "SpecFlow" + ".sublime-build");
                File.WriteAllText(fi.FullName, json);
            }
        }
         
       
    }
}
