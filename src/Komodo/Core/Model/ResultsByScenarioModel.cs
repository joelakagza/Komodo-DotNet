using System;

namespace Komodo.Core.Model
{
    public class SmarterRunnerVW
    {
        public string config { get; set; }
        public string Source { get; set; }
        public string Env { get; set; }
        public string Browser { get; set; }
        public string Feature { get; set; }
        public string Scenario { get; set; }
        public string Tags { get; set; }
        public DateTime ScenarioDate { get; set; }
        public int sCount { get; set; }
        public string Result { get; set; }
        public decimal ResultInt { get; set; }
        public string EmailGeneratedDate { get; set; }
        public string TestName { get; set; }
        public int Retries { get; set; }
    }

    public class ResultsRealVw
    {
        public string config { get; set; }
        public string Source { get; set; }
        public string Env { get; set; }
        public string Version { get; set; }
        public string Browser { get; set; }
        public string Feature { get; set; }
        public string Scenario { get; set; }
        public string Tags { get; set; }
        public DateTime ScenarioDate { get; set; }
        public string Result { get; set; }
        public decimal ResultInt { get; set; }
        public string EmailGeneratedDate { get; set; }
        public string TestName { get; set; }
        public int Retries { get; set; }
    }

    public class ResultsByScenarioModel
    {
        public string Source { get; set; }
        public string Env { get; set; }
        public string Browser { get; set; }
        public string Feature { get; set; }
        public string Scenario { get; set; }
        public string UserName { get; set; }
        public DateTime ScenarioDate { get; set; }
        public string Result { get; set; }
        public decimal ResultInt { get; set; }
        public string EmailGeneratedDate { get; set; }
        public string TestName { get; set; }
        public int Retries { get; set; }
        public string config { get; set; }
    }

    public class ResultsByTagModel
    {
        public string Env { get; set; }
        public string Browser { get; set; }
        public string Tag { get; set; }
        public string UserName { get; set; }
        public DateTime ScenarioDate { get; set; }
        public string Result { get; set; }
        public decimal ResultInt { get; set; }
        public string EmailGeneratedDate { get; set; }
        public string TestName { get; set; }
        public int Retries { get; set; }
    }


}
