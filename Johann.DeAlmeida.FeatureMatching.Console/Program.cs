using Johann.DeAlmeida.FeatureMatching;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

var objectImagePath = args[0];
var scenesPath = args[1];

var imagesData = new List<byte[]>();
foreach (var imagePath in Directory.EnumerateFiles(scenesPath))
{
    var imageBytes = await File.ReadAllBytesAsync(imagePath);
    imagesData.Add(imageBytes);
}
var objectImageData = await File.ReadAllBytesAsync(objectImagePath);
var detectObjectInScenesResults = new ObjectDetection().DetectObjectInScenes(objectImageData, imagesData);

foreach (var objectDetectionResult in detectObjectInScenesResults)
{
    System.Console.WriteLine($"Points:{JsonSerializer.Serialize(objectDetectionResult.Points)}");
}