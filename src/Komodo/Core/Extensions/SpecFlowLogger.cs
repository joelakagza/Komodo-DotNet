using Komodo.Core.Steps;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Bindings;
using TechTalk.SpecFlow.Bindings.Reflection;
using TechTalk.SpecFlow.Tracing;

namespace Komodo.Core.Extensions
{
    /// <summary>
    /// A class to capture SpecFlow console logging so that we can custom format it.
    /// </summary>
    public class SpecFlowLogger : TechTalk.SpecFlow.Tracing.ITraceListener, ITestTracer
    {
        public const string Name = "SpecFlow";

        /// <summary>
        /// This is called to announce each step.
        /// </summary>
        public void WriteTestOutput(string message)
        {
            Console.WriteLine(message.StrVar());
            KomodoTestSuite.sb.Append(message.StrVar() + Environment.NewLine);
        }

        /// <summary>
        /// This is for SpecFlow tool output, like saying when a step is done or reporting timeout errors.
        /// </summary>
        public void WriteToolOutput(string message)
        {
            Console.WriteLine(message.StrVar());
            KomodoTestSuite.sb.Append(message.StrVar() + Environment.NewLine);
            KomodoTestSuite.stepCount++;
            DateTime sDateTime;
            ScenarioContext.Current.TryGetValue("Date", out  sDateTime);
        }

        #region Implementation of ITestRunnerManager

        public ITestRunner CreateTestRunner(Assembly testAssembly, bool async)
        {
            throw new NotImplementedException();
        }

        public ITestRunner GetTestRunner(Assembly testAssembly, bool async)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void TraceBindingError(BindingException ex)
        {
            throw new NotImplementedException();
        }

        public void TraceDuration(TimeSpan elapsed, IBindingMethod method, object[] arguments)
        {
            throw new NotImplementedException();
        }

        public void TraceDuration(TimeSpan elapsed, string text)
        {
            throw new NotImplementedException();
        }

        public void TraceDuration(TimeSpan elapsed, MethodInfo methodInfo, object[] arguments)
        {
            throw new NotImplementedException();
        }

        public void TraceError(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void TraceNoMatchingStepDefinition(StepInstance stepInstance, ProgrammingLanguage targetLanguage, CultureInfo bindingCulture, List<BindingMatch> matchesWithoutScopeCheck)
        {
            throw new NotImplementedException();
        }

        public void TraceNoMatchingStepDefinition(TechTalk.SpecFlow.Bindings.StepArgumentTransformationBinding stepArgs,
                                                  ProgrammingLanguage targetLanguage,
                                                  List<TechTalk.SpecFlow.Bindings.BindingMatch> matchesWithoutScopeCheck)
        {
            throw new NotImplementedException();
        }

        public void TraceStep(TechTalk.SpecFlow.Bindings.StepArgumentTransformationBinding stepArgs, bool showAdditionalArguments)
        {
            throw new NotImplementedException();
        }

        public void TraceStepDone(TechTalk.SpecFlow.Bindings.BindingMatch match, object[] arguments, TimeSpan duration)
        {
            throw new NotImplementedException();
        }

        public void TraceStepPending(TechTalk.SpecFlow.Bindings.BindingMatch match, object[] arguments)
        {
            throw new NotImplementedException();
        }

        public void TraceStepSkipped()
        {
            throw new NotImplementedException();
        }

        public void TraceStep(StepInstance stepInstance, bool showAdditionalArguments)
        {
            throw new NotImplementedException();
        }

        public void TraceWarning(string text)
        {
            throw new NotImplementedException();
        }
    }

}