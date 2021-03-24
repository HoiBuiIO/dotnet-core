using Common.ApiResult;
using Common.ErrorResult;
using Common.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Service
{
	public class ServiceResult
	{
		public HttpCode HttpCode { get; private set; }
		public ApiResponse<object> Value { get; private set; }

		public static ServiceResult Succeeded()
		{
			return new ServiceResult
			{
				HttpCode = HttpCode.OK,
				Value = new ApiResponse<object>(true)
			};
		}

		public static ServiceResult Succeeded(object data)
		{
			return new ServiceResult
			{
				HttpCode = HttpCode.OK,
				Value = new ApiResponse<object>(data)
			};
		}

		public static ServiceResult Failed(HttpCode httpCode, object result = null)
		{
			return new ServiceResult
			{
				HttpCode = httpCode,
				Value = new ApiResponse<object>((int)httpCode, httpCode.GetDescription(), result)
			};
		}

		public static ServiceResult Failed(HttpCode httpCode, string message)
		{
			return new ServiceResult
			{
				HttpCode = httpCode,
				Value = new ApiResponse<object>((int)httpCode, message)
			};
		}

		public static ServiceResult Failed(ErrorCode errorCode, HttpCode httpCode = HttpCode.BadRequest)
		{
			return new ServiceResult
			{
				HttpCode = httpCode,
				Value = new ApiResponse<object>((int)errorCode, errorCode.GetDescription())
			};
		}

		public static ServiceResult Failed(ErrorCode errorCode, object result, HttpCode httpCode = HttpCode.BadRequest)
		{
			return new ServiceResult
			{
				HttpCode = httpCode,
				Value = new ApiResponse<object>((int)errorCode, errorCode.GetDescription(), result)
			};
		}
	}
}