using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.Models;
using Microsoft.Data.SqlClient;
using RazorLight;
using System.Net.Mail;
using System.Net;
using e_commerce_backend.DTO.Order;
using System.Text.Json;

namespace e_commerce_backend.Data.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _config;

        public EmailRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task SendEmailAsync<TModel>(SendEmailRequest request)
        {
            EmailTemplate template = null;

            string connectionString = _config.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                string query = "SELECT TOP 1 Subject, Body FROM EmailTemplates WHERE TemplateName = @TemplateName";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TemplateName", request.TemplateName);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            template = new EmailTemplate
                            {
                                TemplateName = request.TemplateName,
                                Subject = reader.GetString(0),
                                Body = reader.GetString(1)
                            };
                        }
                    }
                }
            }

            if (template == null)
                throw new Exception("Template not found");

            // ✅ RazorLight setup
            var engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(EmailRepository))
                .UseMemoryCachingProvider()
                .Build();

            // ✅ Strongly Typed Deserialization
            TModel model = JsonSerializer.Deserialize<TModel>(request.ModelJson);

            // ✅ Razor compile and render
            string body = await engine.CompileRenderStringAsync("templateKey", template.Body, model);

            using (var client = new SmtpClient())
            {
                client.Host = _config["SmtpSettings:Host"];
                client.Port = int.Parse(_config["SmtpSettings:Port"]);
                client.EnableSsl = bool.Parse(_config["SmtpSettings:EnableSsl"]);
                client.Credentials = new NetworkCredential(
                    _config["SmtpSettings:UserName"],
                    _config["SmtpSettings:Password"]
                );

                var mail = new MailMessage();
                mail.From = new MailAddress(_config["SmtpSettings:UserName"]);
                mail.To.Add(request.ToEmail);
                mail.Subject = template.Subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                await client.SendMailAsync(mail);
            }
        }

    }
}
