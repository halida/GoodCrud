namespace GoodCrud.Contract.Dtos
{
    public enum ResultStatus
    {
        Succeed,
        NotFound,
        Failed,
    }
    public class ResultDto<T>
    where T : class
    {
        public ResultStatus Status { get; set; }
        public string Description { get; set; }
        public T Data { get; set; }

        public static ResultDto<T> Succeed(T obj, string description = "")
        {
            return new ResultDto<T> { Status = ResultStatus.Succeed, Description = description, Data = obj };
        }
        public static ResultDto<T> NotFound()
        {
            return new ResultDto<T> { Status = ResultStatus.NotFound, Description = null, Data = null };
        }

        public static ResultDto<T> Failed(string description = "")
        {
            return new ResultDto<T> { Status = ResultStatus.Failed, Description = description, Data = null };
        }
    }

}
