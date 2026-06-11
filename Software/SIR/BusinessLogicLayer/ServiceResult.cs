namespace BusinessLogicLayer
{
    public class ServiceResult
    {
        private ServiceResult(bool isSuccessful, string errorMessage)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccessful { get; private set; }
        public string ErrorMessage { get; private set; }

        public static ServiceResult Success()
        {
            return new ServiceResult(true, null);
        }

        public static ServiceResult Failure(string errorMessage)
        {
            return new ServiceResult(false, errorMessage);
        }
    }
}
