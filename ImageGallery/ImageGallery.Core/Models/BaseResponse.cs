namespace ImageGallery.Core.Models
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public object Response { get; set; }

        public BaseResponse(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }

        public BaseResponse(object response)
        {
            Success = true;
            Response = response;
        }
    }
}
