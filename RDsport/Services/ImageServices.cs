namespace RDsport.Services
{
    public class ImageServices : IImageService
    {
        private readonly int maxLength = 5 * 1024 * 1024;
        private readonly string[] allowedExtension = { ".jpg", ".jpej", ".png", ".gif", ".web" };
        private readonly IWebHostEnvironment _webHost;
        public ImageServices(IWebHostEnvironment webHost)
        {
            _webHost = webHost;

        }
        public async Task<bool> DeleteImage(string image, string folderNameToCreate)
        {
            if (image != null)
            {
                var pathFileName = Path.GetFileName(image);

                var folder = Path.Combine(_webHost.WebRootPath, "Images", $"{folderNameToCreate}", pathFileName);
                if (File.Exists(folder))
                {
                  await  Task.Run(() => File.Delete(folder));
                    return true;
                }
               
            }
            return false;
        }

        public async Task<string> UploadImage(IFormFile file,string folderNameToCreate)
        {
            try
            {
                if (!ValidateImage(file)) return "File is not valid";
                if (file != null)
                {
                    var folderUpload = Path.Combine(_webHost.WebRootPath, "Images", $"{folderNameToCreate}");
                    if (!Directory.Exists(folderUpload))
                    {
                        Directory.CreateDirectory(folderUpload);
                    }
                    var extension= Path.GetExtension(file.FileName);
                    var fileUniqueName=$"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(folderUpload, fileUniqueName);

                    using (var fileStream=new FileStream(filePath, FileMode.Create))
                    {
                      await file.CopyToAsync(fileStream);
                    }
                    return $"Images/{folderNameToCreate}/{fileUniqueName}";
                }
                return null;
                
            }
            catch (Exception ex) 
            {

                throw new NotImplementedException(ex.Message);
            }
        }

        public bool ValidateImage(IFormFile file)
        {
            try
            {
                if (file.Length > maxLength || file.Length == 0)
                {
                    return false;
                }
                var extension = Path.GetExtension(file.FileName);
                if (!allowedExtension.Contains(extension.ToLower())) return false;
                return true;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);

            }
        }
    }
}
