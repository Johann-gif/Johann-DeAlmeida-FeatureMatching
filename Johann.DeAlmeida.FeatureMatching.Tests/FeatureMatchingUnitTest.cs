using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Johann.DeAlmeida.FeatureMatching.Tests
{
    public class FeatureMatchingUnitTest
    {
        [Fact]
        public async Task ObjectShouldBeDetectedCorrectly()
        {
            var executingPath = GetExecutingPath();
            var imageScenesData = new List<byte[]>();
            foreach (var imagePath in
                Directory.EnumerateFiles(Path.Combine(executingPath, "Scenes")))
            {
                var imageBytes = await File.ReadAllBytesAsync(imagePath);
                imageScenesData.Add(imageBytes);
            }

            var objectImageData = await
                File.ReadAllBytesAsync(Path.Combine(executingPath, "Johann-DeAlmeida-object.jpg"));
            var detectObjectInScenesResults = new ObjectDetection().DetectObjectInScenes(objectImageData, imageScenesData);
            Assert.Equal("[{\"X\":2243,\"Y\":2635},{\"X\":1698,\"Y\":1835},{\"X\":426,\"Y\":2529},{\"X\":1079,\"Y\":3580}]",JsonSerializer.Serialize(detectObjectInScenesResults[0].Points));
            Assert.Equal("[{\"X\":1934,\"Y\":1230},{\"X\":1990,\"Y\":1429},{\"X\":2945,\"Y\":3797},{\"X\":2499,\"Y\":2674}]",JsonSerializer.Serialize(detectObjectInScenesResults[1].Points));
        }
        private static string GetExecutingPath()
        {
            var executingAssemblyPath =
                Assembly.GetExecutingAssembly().Location;
            var executingPath = Path.GetDirectoryName(executingAssemblyPath);
            return executingPath;
        }
    }
}