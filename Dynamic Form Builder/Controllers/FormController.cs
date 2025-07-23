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

        [HttpPost]
        public IActionResult Create(Form form)
        {
            //foreach (var field in form.Fields)
            //{
            //    Console.WriteLine($"Label: {field.Label}, Required: {field.IsRequired}, Option: {field.SelectedOption}");
            //}

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Forms (Title) VALUES (@title); SELECT SCOPE_IDENTITY();", conn);
                cmd.Parameters.AddWithValue("@title", form.Title);
                int formId = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (var field in form.Fields)
                {
                    SqlCommand fieldCmd = new SqlCommand("INSERT INTO FormFields (FormId, Label, IsRequired, SelectedOption) VALUES (@formId, @label, @isRequired, @selectedOption)", conn);
                    fieldCmd.Parameters.AddWithValue("@formId", formId);
                    fieldCmd.Parameters.AddWithValue("@label", field.Label);
                    fieldCmd.Parameters.AddWithValue("@isRequired", field.IsRequired);
                    fieldCmd.Parameters.AddWithValue("@selectedOption", field.SelectedOption);
                    fieldCmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
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
