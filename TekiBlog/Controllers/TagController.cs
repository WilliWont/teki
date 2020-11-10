using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TekiBlog.Controllers
{
    public class TagController : Controller
    {
        private readonly ILogger<TagController> _logger;
        private readonly IService _service;

        public TagController(ILogger<TagController> logger,
            IService service)
        {
            _service = service;
            _logger = logger;
        }
        public IActionResult Index(int? id)
        {
            return View();
        }

        [HttpGet]
        public JsonResult Get()
        {
            var tags = _service.GetAllActiveTags();
            var result = new List<Object>();
            foreach( var tag in tags)
            {
                var tagJson = new
                {
                    value = tag.Id,
                    text = tag.Name,
                    continent = "tag"
                };
                result.Add(tagJson);
            }
            return Json(result);
        }
    }
}
