using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchemaValidator.Properties
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonSchemaController : ControllerBase
    {
        public class ValidateRequest
        {
            public string Json { get; set; }
            public string Schema { get; set; }
        }

        public class ValidateResponse
        {
            public bool Valid { get; set; }
            public IList<ValidationError> Errors { get; set; }
        }

        // GET: api/<JsonSchemaController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<JsonSchemaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Route("validate")]
        public ValidateResponse Valiate(ValidateRequest request)
        {
            WebClient wget = new System.Net.WebClient();
            var jsonSchema = wget.DownloadString(request.Schema);

            // load schema
            JSchema schema = JSchema.Parse(jsonSchema);
            JToken json = JToken.Parse(request.Json);

            // validate json
            IList<ValidationError> errors;
            bool valid = json.IsValid(schema, out errors);

            // return error messages and line info to the browser
            return new ValidateResponse
            {
                Valid = valid,
                Errors = errors
            };
        }
    }
}
