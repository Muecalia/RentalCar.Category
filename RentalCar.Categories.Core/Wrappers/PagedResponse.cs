namespace RentalCar.Categories.Core.Wrappers;

public class PagedResponse<T> 
{
    public List<T> Datas { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }

    public PagedResponse(List<T> datas, int pageNumber, int pageSize, int totalRecords, string message)
    {
        Datas = datas;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Succeeded = true;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling((decimal)totalRecords / (decimal)pageSize);
        Message = message;
    }

    public PagedResponse(string message)
    {
        Datas = [];
        Succeeded = false;
        Message = message;
    }
    
}