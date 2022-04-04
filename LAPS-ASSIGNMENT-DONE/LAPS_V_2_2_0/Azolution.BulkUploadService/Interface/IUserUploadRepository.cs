namespace Azolution.BulkUploadService.Interface
{
    public interface IUserUploadRepository
    {
        string ImportUserUplodedData(string importFilePath, int userId);
    }
}
