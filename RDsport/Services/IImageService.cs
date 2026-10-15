namespace RDsport.Services
{
    public interface IImageService
    {
        Task<string> UploadImage(IFormFile file,string folderNameToCreate);
        Task<bool> DeleteImage(string image, string folderNameToCreate);
        bool ValidateImage(IFormFile file);
    }
}
