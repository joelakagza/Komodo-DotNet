using Komodo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komodo.Core.Extensions
{
    public static class QuestionAndAnswersExt
    {
        public static QuestionAndAnswers FindQuestionAndAnswer(this IEnumerable<QuestionAndAnswers> QnA, string question)
        {
            return QnA.FirstOrDefault(q => q.Question.ToLower() == question.ToLower());
        }

        public static string GetAnswer(this IEnumerable<QuestionAndAnswers> QnA, string question)
        {
            return QnA.FirstOrDefault(q => q.Question.ToLower() == question.ToLower()).Answer;
        }
    }
}
