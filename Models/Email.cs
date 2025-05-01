namespace e_commerce_backend.Models
{
        public class EmailTemplate
        {
            public Guid Id { get; set; }
            public string TemplateName { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }

        public class SendEmailRequest
        {
            public string ToEmail { get; set; }
            public string TemplateName { get; set; }
        //public dynamic Placeholders { get; set; }

        public string ModelJson { get; set; }
    }

}
