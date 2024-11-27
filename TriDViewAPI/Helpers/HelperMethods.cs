using TriDViewAPI.DTO;
using TriDViewAPI.Models;

namespace TriDViewAPI.Helpers
{
    public class HelperMethods
    {
        #region Images
        public static async Task<string> SavePhotoToPathAsync(string directoryPath, string fileName, IFormFile image)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException("Directory path cannot be null or empty.", nameof(directoryPath));
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            int fileIndex = 1;
            string fullFilePath = Path.Combine(directoryPath, fileName);

            while (File.Exists(fullFilePath))
            {
                fileName = $"{fileNameWithoutExtension}_{fileIndex++}{extension}";
                fullFilePath = Path.Combine(directoryPath, fileName);
            }

            await using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return fileName;
        }
        public static async Task<string> FindImage(string directoryPath, string fileName)
        {
            string fullPath = Path.Combine(directoryPath, fileName);
            if (File.Exists(fullPath))
            {
                var imageByteArray = System.IO.File.ReadAllBytes(fullPath);
                return Convert.ToBase64String(imageByteArray);
            }
            return null;
        }
        #endregion
    }
}
