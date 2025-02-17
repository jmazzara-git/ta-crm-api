using FluentValidation.Results;

namespace TACRM.Services.Dtos
{
	public class ApiResponse
	{
		public bool Ok => !Errors.Any();
		public object Data { get; set; }
		public ApiErrors Errors { get; set; } = [];
	}

	public class ApiResponse<T>
	{
		public ApiResponse() { }

		public ApiResponse(T data)
		{
			Data = data;
		}

		public bool Ok => !Errors.Any();
		public T Data { get; set; }
		public ApiErrors Errors { get; set; } = [];
	}

	public class ApiErrors : List<string>
	{
		public void Add(IEnumerable<ValidationFailure> errors)
		{
			AddRange(errors.Select(e => e.ErrorMessage));
		}
	}
}
