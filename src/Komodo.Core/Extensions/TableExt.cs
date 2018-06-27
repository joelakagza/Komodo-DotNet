using Komodo.Core.Domain;
using Komodo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace Komodo.Core.Extensions
{
    public static class tableExt
    {
        public static T CreateInstance<T>(this Table table, IEnumerable<Locator> locators, string page)
        {
            var type = typeof(T);
            locators = locators.Where(t => t.Page.ToLower() == page.ToLower());
            var TT = (T)Activator.CreateInstance(type,null);
            var props = TT.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.PropertyType == typeof(Locator))
                {
                    var loc = from i in table.Rows.ToList().Select(t => t[0].ToString())
                              join l in locators on i.ToLower().Trim() equals l.KeyName.ToLower().Trim()
                              where Regex.Replace(l.KeyName, "[^0-9a-zA-Z]+", "").ToLower().Replace(" ","") == prop.Name.ToLower()
                              select l;

                    if (loc.Any())
                        prop.SetValue(TT, loc.FirstOrDefault());
                }
            }

            return TT;
        }

        public static string TryGetValueByQuestion(this Table table, string keyname)
        {
            var trs = table.Rows;
            for (int i = 0; i < table.RowCount; i++)
            {
                var rw = trs.FirstOrDefault(t => t["question"][i].ToString().ToLower() == keyname.ToLower());
                if (rw != null)
                    return rw[0][i].ToString();
            }

            return null;
        }

        public static string TryGetValueByQuestion(this Table table,  IEnumerable<Locator> locators, string keyname)
        {
            var trs = table.Rows;
            for (int i = 0; i < table.RowCount; i++)
            {
                var rw = trs.FirstOrDefault(t => t["question"][i].ToString().ToLower() == keyname.ToLower());
                if (rw != null)
                    return rw[0][i].ToString();
            }

            return null;
        }

        public static List<QuestionAndAnswers> ToQuestionAndAnswers(this Table table)
        {
            var qnas = new List<QuestionAndAnswers>();
            foreach (var rw in table.Rows)
            {
                qnas.Add(new QuestionAndAnswers { Question = rw["question"], Answer = rw["answer"] });
            }

            return qnas;
        }

        public static string TryGetValueByQuestion(this IEnumerable<TableRow> trs, string keyname)
        {
            for (int i = 0; i < trs.Count(); i++)
            {
                var rw = trs.FirstOrDefault(t => t["question"][i].ToString().ToLower() == keyname.ToLower());
                if (rw != null)
                    return rw[0][i].ToString();
            }

            return null;           
        }
    }
}