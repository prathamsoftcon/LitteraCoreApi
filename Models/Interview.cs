namespace LitteraCore.Models
{
    public class Interview
    {
    }
    public class interviewQuestion
    {
        public string questionid { get; set; }
        public string question { get; set; }
        public string idealanswer { get; set; }
        public double[] idealanswer_embedings { get; set; }

        public string candidateanswer { get; set; }

        public double[] candidateanswer_embeddings { get; set; }
        public int? displaysequence { get; set; } = 0;

        public interviewResult result { get; set; }

        public int max_length { get; set; }

        public int min_length { get; set; }


        public string tips { get; set; }



    }
    public class interviewAnswers
    {
        public string tpad_id { get;set; }
        public int is_AI_enabled_required { get; set; }
        public interviewQuestion[] interviewQuestion { get; set; }
    }

    public class interviewResult
    {
        public double Relevance { get; set; }
        public double Completeness { get; set; }
        public double Accuracy { get; set; }
        public double Clarity { get; set; }
        public double Depth { get; set; }

        

    }


    public class AI_Answer
    {
        public string id { get; set; }
        public string useranswer { get; set; }
    }
}
