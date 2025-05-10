using ApartmentFinishingServices.Core.Services.Contract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Service
{
    public class FileService(IWebHostEnvironment environment) : IFileService
    {
        public void DeleteFile(string fileNameWithExtension)
        {
            if(string.IsNullOrEmpty(fileNameWithExtension))
            {
                throw new ArgumentNullException(nameof(fileNameWithExtension));
            }
            var contentPath = environment.ContentRootPath;
            var path = Path.Combine(contentPath, $"uploads" , fileNameWithExtension);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Invalid file path");
            }
            File.Delete(path);
        }

        public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions)
        {
            if(imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile));
            }
            var contentPath = environment.ContentRootPath;
            var path = Path.Combine(contentPath, "uploads");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            var extension = Path.GetExtension(imageFile.FileName);
            if(!allowedFileExtensions.Contains(extension))
            {
                throw new ArgumentException($"only {string.Join(",", allowedFileExtensions)} are allowed");
            }

            var fileName = $"{Guid.NewGuid().ToString()}{extension}";
            var fileNameWithPath = Path.Combine(path, fileName);
            using var stream = new FileStream(fileNameWithPath, FileMode.Create);

            await imageFile.CopyToAsync(stream);
            return fileName;


        }
    }
}
