namespace LitteraCore.Models
{
    public class MaskInfo
    {
        public class Form
        {
            public string Formid { get; set; }
            public string Description { get; set; }
            public List<string> Columns { get; set; }
        }
        public class MaskDataWrapper
        {
            public List<Form> MaskData { get; set; }
        }
    }
}
