using Dynamic_Form_Builder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Dynamic_Form_Builder.Controllers
{
    public class FormController : Controller
    {
        private readonly IConfiguration _configuration;

        public FormController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            List<Form> forms = new List<Form>();

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT FormId, Title, CreatedAt FROM Forms", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    forms.Add(new Form
                    {
                        FormId = Convert.ToInt32(reader["FormId"]),
                        Title = reader["Title"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    });
                }

                reader.Close();
            }

            return View(forms);
        }

        public IActionResult Create() => View();

        public IActionResult RenderDropdown(int index)
        {
            return ViewComponent("DropdownField", new { index = index });
        }
        
        public IActionResult Preview(int id)
        {
            Form form = new Form();
            form.Fields = new List<FormField>();

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                // Get form title
                SqlCommand formCmd = new SqlCommand("SELECT Title, CreatedAt FROM Forms WHERE FormId = @formId", conn);
                formCmd.Parameters.AddWithValue("@formId", id);

                using (SqlDataReader reader = formCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        form.FormId = id;
                        form.Title = reader["Title"].ToString();
                        form.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                    }
                }

                // Get form fields
                SqlCommand fieldCmd = new SqlCommand("SELECT FieldId, Label, IsRequired, SelectedOption FROM FormFields WHERE FormId = @formId", conn);
                fieldCmd.Parameters.AddWithValue("@formId", id);

                using (SqlDataReader fieldReader = fieldCmd.ExecuteReader())
                {
                    while (fieldReader.Read())
                    {
                        form.Fields.Add(new FormField
                        {
                            FieldId = Convert.ToInt32(fieldReader["FieldId"]),
                            Label = fieldReader["Label"].ToString(),
                            IsRequired = Convert.ToBoolean(fieldReader["IsRequired"]),
                            SelectedOption = fieldReader["SelectedOption"].ToString()
                        });
                    }
                }
            }

            return View(form);
        }

    }
}
