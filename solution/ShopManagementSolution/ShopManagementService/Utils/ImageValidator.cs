namespace ShopManagementService.Utils;

public class ImageValidator
{
    private static readonly string[] AllowedImageTypes = { "image/jpeg", "image/png", "image/gif", "image/webp" };
    private const long MaxFileSize = 50 * 1024 * 1024;

    public static bool IsValidImage(IFormFile file, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (!AllowedImageTypes.Contains(file.ContentType.ToLower()))
        {
            errorMessage = "Invalid file type! Only JPG, PNG, GIF, and WEBP are allowed.";
            return false;
        }

        if (file.Length > MaxFileSize)
        {
            errorMessage = "File size exceeds the 50MB limit.";
            return false;
        }

        return true;
    }
}
