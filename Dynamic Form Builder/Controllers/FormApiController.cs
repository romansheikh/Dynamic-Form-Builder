using Dynamic_Form_Builder.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Dynamic_Form_Builder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FormApiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("submit")]
        public IActionResult SubmitForm([FromBody] Form form)
            {
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

            return Ok(new { message = "Form saved successfully" });
        }


    }
}
