using System;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Komodo.Core.Support;
using Komodo.Core.Extensions;

namespace Komodo.Core.Steps
{

    [Binding]
    public class DiffPlex : SeleniumStepsBase
    {

        [Then(@"in the diff web tool compare the followng files '(.*)' vs '(.*)'")]
        public void ThenInTheDiffWebToolCompareTheFollowngFilesXxVsXx(string p0, string p1)
        {
            //HttpContext.Current.Server.
            string url = "=" +
                         System.Web.HttpUtility.UrlEncode(p0.StrVar().Replace("%{resultsPath}".StrVar(), "~/results")) +
                         "&fileTwo="
                         + System.Web.HttpUtility.UrlEncode(p1.StrVar().Replace("%{resultsPath}".ReplaceStrVar(),
                                                                                "~/results"));
            var f1 = FileStorage.ReadFileText(p0.StrVar());
            var f2 = FileStorage.ReadFileText(p0.StrVar());
            Console.WriteLine(url);
        }

        [Then(@"compare with diffplex the following files '(.*)' vs '(.*)'")]
        [Then(@"compare with diff plex the following files '(.*)' vs '(.*)'")]
        public void ThenCompareWithDiffPlexTheFollowngFilesVs(string fileOne, string fileTwo)
        {
            string str1 = FileStorage.ReadFileText(fileOne.StrVar());
            string str2 = FileStorage.ReadFileText(fileTwo.StrVar());

            var d1 = new Differ();
            var sidebYSide = new SideBySideDiffBuilder(d1);
            var result1 = sidebYSide.BuildDiffModel(str1, str2);

            foreach (var line in result1.NewText.Lines)
            {
                if (line.Type == ChangeType.Modified)
                    Assert.Fail(line.Position + " " +  line.Text);
            }
        }

        [Then(@"compare with diffplex the following text '(.*)' vs '(.*)'")]
        [Then(@"compare with diff plex the following string '(.*)' vs '(.*)'")]
        public void ThenCompareWithDiffPlexTheFollowingText(string fileOne, string fileTwo)
        {
            var d1 = new Differ();
            var sidebYSide = new SideBySideDiffBuilder(d1);
            var result1 = sidebYSide.BuildDiffModel(fileOne.StrVar(), fileTwo.StrVar());

            foreach (var line in result1.NewText.Lines)
            {
                if (line.Type == ChangeType.Inserted)
                    Assert.Fail(line.Position + " " + line.Text);
                if (line.Type == ChangeType.Modified)
                    Assert.Fail(line.Position + " " + line.Text);
                if (line.Type == ChangeType.Deleted)
                    Assert.Fail(line.Position + " " + line.Text);
                if (line.Type == ChangeType.Imaginary)
                    Assert.Fail(line.Position + " " + line.Text);
            }
        }

        [Then(@"compare and verify with diffplex the result is equal to expected result")]
        [Then(@"compare and verify with diffplex the string1 is equal to string2")]
        public void ThenCompareAndVerifyWithDiffplexTheStringXIsEqualToStringX( Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                ThenCompareWithDiffPlexTheFollowingText(row[0].StrVar(), row[1].StrVar());
            }
        }

        [Then(@"compare and verify with diffplex the fileone is equal to filetwo")]
        public void ThenCompareAndVerifyWithDiffplexTheFileoneIsEqualToFiletwo(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                ThenCompareWithDiffPlexTheFollowingText(Common.ReadFileText(row[0].StrVar()), Common.ReadFileText(row[1].StrVar()));
            }
        }

    }
}
