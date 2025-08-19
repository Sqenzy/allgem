using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;

public class GoogleDriveService
{
    private readonly DriveService _driveService;
    private readonly string _folderId = "1mppRN8CAyRncCAKhYwAxURczOxp5ZrxT";

    public GoogleDriveService()
    {
        // načtení service account credentials.json
        var credential = GoogleCredential.FromFile("allgem-dec82d5012c2.json")
            .CreateScoped(DriveService.Scope.DriveFile);

        _driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "MojeWebApp",
        });
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        {
            Name = fileName,
            Parents = new List<string> { _folderId }
        };

        var request = _driveService.Files.Create(fileMetadata, fileStream, contentType);
        request.Fields = "id";

        var result = await request.UploadAsync();

        if (result.Status == UploadStatus.Failed)
            throw new Exception($"Upload selhal: {result.Exception}");

        return request.ResponseBody.Id; // ID souboru na Google Drive
    }
}
