namespace Dynamic_Form_Builder.Models
{
    public class FormField
    {
        public int FieldId { get; set; }
        public int FormId { get; set; }
        public string Label { get; set; }
        public bool IsRequired { get; set; }
        public string SelectedOption { get; set; }
    }
}
