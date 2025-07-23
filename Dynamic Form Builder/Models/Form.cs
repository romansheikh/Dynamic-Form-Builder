using Microsoft.AspNetCore.Http;

namespace Dynamic_Form_Builder.Models
{
    public class Form
    {
        public int FormId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<FormField> Fields { get; set; } = new List<FormField>();
    }

}
