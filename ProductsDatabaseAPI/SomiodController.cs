using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using ProductsDatabaseAPI.Models;
using SomiodAPI.Models;

namespace ProductsDatabaseAPI.Controllers
{
    [Route("api/somiod")]
    [ApiController]
    public class SomiodController : ControllerBase
    {
        private static List<Application> Applications = new();

        //Application Endpoints

        [HttpGet]
        public IActionResult GetAllApplications([FromHeader(Name = "somiod-locate")] string locate)
        {
            if (locate == "application")
            {
                var apps = Applications.Select(app => new
                {
                    app.Id,
                    app.Name,
                    app.CreationDatetime
                }).ToList();
                return Ok(apps);
            }
            return BadRequest("Invalid locate header");
        }

        [HttpPost]
        public IActionResult CreateApplication([FromBody] Application application)
        {
            if (string.IsNullOrEmpty(application.Name))
                return BadRequest("Application name is required");

            application.Id = Applications.Count + 1;
            Applications.Add(application);
            return CreatedAtAction(nameof(GetApplication), new { applicationName = application.Name }, application);
        }

        [HttpGet("{applicationName}")]
        public IActionResult GetApplication(string applicationName)
        {
            var application = Applications.FirstOrDefault(a => a.Name == applicationName);
            if (application == null) return NotFound();
            return Ok(application);
        }

        [HttpPut("{applicationName}")]
        public IActionResult UpdateApplication(string applicationName, [FromBody] Application updatedApplication)
        {
            var application = Applications.FirstOrDefault(a => a.Name == applicationName);
            if (application == null) return NotFound();

            application.Name = updatedApplication.Name ?? application.Name;
            return Ok(application);
        }

        [HttpDelete("{applicationName}")]
        public IActionResult DeleteApplication(string applicationName)
        {
            var application = Applications.FirstOrDefault(a => a.Name == applicationName);
            if (application == null) return NotFound();

            Applications.Remove(application);
            return NoContent();
        }

        

        //Container Endpoints

        [HttpGet("{applicationName}")]
        public IActionResult GetAllContainers(
            string applicationName,
            [FromHeader(Name = "somiod-locate")] string locate)
        {
            if (locate == "container")
            {
                var application = Applications.FirstOrDefault(a => a.Name == applicationName);
                if (application == null) return NotFound();

                var containers = application.Containers?.Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.CreationDatetime,
                    c.Parent
                }).ToList() ?? new List<object>();

                return Ok(containers);
            }

            return BadRequest("Invalid locate header");
        }

        [HttpPost("{applicationName}/")]
        public IActionResult CreateContainer(string applicationName, [FromBody] Container container)
        {
            var application = Applications.FirstOrDefault(a => a.Name == applicationName);
            if (application == null) return NotFound();

            container.Id = (application.Containers?.Count ?? 0) + 1;
            container.Parent = application.Id;
            application.Containers ??= new List<Container>();
            application.Containers.Add(container);

            return CreatedAtAction(nameof(GetContainer), new { applicationName, containerName = container.Name }, container);
        }

        [HttpGet("{applicationName}/{containerName}")]
        public IActionResult GetContainer(string applicationName, string containerName)
        {
            var container = GetContainerByName(applicationName, containerName);
            if (container == null) return NotFound();
            return Ok(container);
        }

        [HttpPut("{applicationName}/{containerName}")]
        public IActionResult UpdateContainer(string applicationName, string containerName, [FromBody] Container updatedContainer)
        {
            var container = GetContainerByName(applicationName, containerName);
            if (container == null) return NotFound();

            container.Name = updatedContainer.Name ?? container.Name;
            return Ok(container);
        }

        [HttpDelete("{applicationName}/{containerName}")]
        public IActionResult DeleteContainer(string applicationName, string containerName)
        {
            var application = Applications.FirstOrDefault(a => a.Name == applicationName);
            if (application == null) return NotFound();

            var container = application.Containers?.FirstOrDefault(c => c.Name == containerName);
            if (container == null) return NotFound();

            application.Containers.Remove(container);
            return NoContent();
        }

        

        //Helper Methods

        private Container GetContainerByName(string applicationName, string containerName)
        {
            var application = Applications.FirstOrDefault(a => a.Name == applicationName);
            return application?.Containers?.FirstOrDefault(c => c.Name == containerName);
        }

       
    }
}
