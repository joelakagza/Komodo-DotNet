﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Komodo.Data;
using System;

namespace Komodo.Core.Domain
{
    public partial class ResultsReal : EntityBase
    {
        public ResultsReal()
        {
          
        }

        public string TestId { get; set; }
        public string Env { get; set; }
        public string Browser { get; set; }
        public string Version { get; set; }
        public string Source { get; set; }
        public string Feature { get; set; }
        public string Scenario { get; set; }
        public string Result { get; set; }
        public string Error { get; set; }
        public string OutPut { get; set; }
        public string Tags { get; set; }
        public Nullable<System.DateTime> FeatureDate { get; set; }
        public Nullable<System.DateTime> ScenarioDate { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string UserName { get; set; }
        public string TestName { get; set; }
        public Nullable<int> retries { get; set; }
        public string JobAssets { get; set; }
        public string config { get; set; }
    }
}
