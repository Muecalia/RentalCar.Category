namespace RentalCar.Categories.Core.MessageBus;

public class RabbitQueue
{
    public static string CATEGORY_MODEL_NEW_REQUEST_QUEUE = "CategoryModelNewRequestQueue";
    public static string? CATEGORY_MODEL_NEW_RESPONSE_QUEUE = "CategoryModelNewResponseQueue";
    
    public static string CATEGORY_MODEL_FIND_REQUEST_QUEUE = "CategoryModelFindRequestQueue";
    public static string CATEGORY_MODEL_FIND_RESPONSE_QUEUE = "CategoryModelFindResponseQueue";
    
    public static string CATEGORY_MODEL_UPDATE_REQUEST_QUEUE = "CategoryModelUpdateRequestQueue";
    public static string CATEGORY_MODEL_UPDATE_RESPONSE_QUEUE = "CategoryModelUpdateResponseQueue";
}