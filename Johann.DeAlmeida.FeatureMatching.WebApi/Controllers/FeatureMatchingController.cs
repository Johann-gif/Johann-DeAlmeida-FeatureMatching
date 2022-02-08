using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Johann.DeAlmeida.FeatureMatching.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeatureMatchingController : ControllerBase
    {
        private readonly ObjectDetection _objectDetection;

        public FeatureMatchingController(ObjectDetection objectDetection)
        {
            _objectDetection = objectDetection;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            if (files.Count != 2) return BadRequest();
            using var objectSourceStream = files[0].OpenReadStream();
            using var objectMemoryStream = new MemoryStream();
            objectSourceStream.CopyTo(objectMemoryStream);
            var objectData = objectMemoryStream.ToArray();
            using var sceneSourceStream = files[1].OpenReadStream();
            using var sceneMemoryStream = new MemoryStream();
            sceneSourceStream.CopyTo(sceneMemoryStream);
            var imageData = sceneMemoryStream.ToArray();
            var detectObjectInScenesResults =
                new ObjectDetection().DetectObjectInScenes(objectData, new List<byte[]> {imageData});
            var newImageData = detectObjectInScenesResults.FirstOrDefault();
            return File(newImageData.ImageData, "image/png");
        }
    }
}