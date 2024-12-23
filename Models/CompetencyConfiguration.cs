namespace LitteraCore.Models
{
    public class CompetencyConfiguration
    {
        public CompetencyConfiguration()
        {
            no_of_question = 20;
            time_per_question = 2;
            application_type_id = 115;
            mark_per_question = 1;
        }
        public int no_of_question { get; set; }
        public decimal time_per_question { get; set; }

        public int application_type_id { get; set; }
        public int mark_per_question { get; set; }
    }
}
