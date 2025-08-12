using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers.apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceAgentController : ControllerBase
    {
        [HttpGet("TestEndpoint/{id}")]
        public ActionResult<string> TestEndpoint(int id)
        {
            return Ok("Test");
        }

        #region Example API endpoint calls
        // GET: api/DeviceAgent/TestEndpoint/5
        [HttpGet("TestEndpoint/{id:int}")]
        public ActionResult<string> TestEndpointWithRoute(int id)
        {
            return Ok($"Test with ID from route: {id}");
        }

        // GET: api/DeviceAgent/TestEndpoint?id=5
        [HttpGet("TestEndpoint")]
        public ActionResult<string> TestEndpointWithQuery([FromQuery] int id)
        {
            return Ok($"Test with ID from query: {id}");
        }

        // GET: api/DeviceAgent/TestEndpoint/5?name=SomeName
        [HttpGet("TestEndpoint/{id:int}/WithName")]
        public ActionResult<string> TestEndpointWithRouteAndQuery(int id, [FromQuery] string name)
        {
            return Ok($"Test with ID: {id}, Name: {name}");
        }
        #endregion Example API endpoint calls

    }
}
