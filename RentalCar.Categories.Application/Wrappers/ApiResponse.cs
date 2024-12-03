namespace RentalCar.Categories.Application.Wrappers
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;

        public ApiResponse() { }

        public ApiResponse(T data, string message)
        {
            Data = data;
            Succeeded = true;
            Message = message;
        }

        public ApiResponse(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public static ApiResponse<T> Error(string message) => new(message);

        public static ApiResponse<T> Success(T data, string message) => new(data, message);

    }
}
