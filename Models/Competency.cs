namespace LitteraCore.Models
{
    public class Competency
    {
        public string TestID { get; set; }
        public decimal Totalmarks { get; set; }
        public string TrainingCategoryID { get; set; }
        public string PartcipantID { get; set; }
        public string QuestionID { get; set; }
        public int IsCorrect { get; set; }
        public decimal tesQmarksobtained { get; set; }
        public decimal QuestionMarksPercentage { get; set; }
        public string TestDescription { get; set; }
        public string TrainingCategoryName { get; set; }
        public string Skilltag { get; set; }

        public decimal CategoryMarksPercentage { get; set; }

        public decimal testMarksPercentage { get; set; }

        public decimal skillMarksPercentage { get; set; }

        public string Youranswer { get; set; }
        public string correctAnswerDescription { get; set; }

        public string Question { get; set; }

        public string mark_per_question { get; set; }
    }
    public class CompentencyLevel
    {
        public string categoryid { get; set; }
        public string testid { get; set; }
        public string name { get; set; }
        public decimal percentage { get; set; }
    }
}
