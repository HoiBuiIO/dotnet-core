using Common.ErrorResult;
using Common.Helper;
using Common.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Common.ApiResult
{
    public static class ApiResultExtensions
    {
        public static IActionResult ErrorResult(this ControllerBase ControllerBase, ErrorCode errorCode, HttpCode httpCode = HttpCode.BadRequest)
        {
            return JsonResult(new ApiResponse<object>((int)errorCode, errorCode.GetDescription()), httpCode);
        }

        public static IActionResult ErrorResult<T>(this ControllerBase ControllerBase, ErrorCode errorCode, T result, HttpCode httpCode = HttpCode.BadRequest)
        {
            return JsonResult(new ApiResponse<T>((int)errorCode, errorCode.GetDescription(), result), httpCode);
        }

        public static IActionResult ErrorResult(this ControllerBase ControllerBase, HttpCode httpCode)
        {
            return JsonResult(new ApiResponse<object>((int)httpCode, httpCode.GetDescription()), httpCode);
        }

        public static IActionResult ErrorResult(this ControllerBase ControllerBase, int errorCode, string errorMessage, HttpStatusCode statusCode)
        {
            return JsonResult(new ApiResponse<object>(errorCode, errorMessage), statusCode);
        }

        public static IActionResult ErrorResult(this ControllerBase ControllerBase, int errorCode, string errorMessage)
        {
            return JsonResult(new ApiResponse<object>(errorCode, errorMessage), HttpStatusCode.BadRequest);
        }

        public static IActionResult OkResult<T>(this ControllerBase ControllerBase, T result)
        {
            return JsonResult(new ApiResponse<T>(result));
        }

        public static IActionResult OkResult(this ControllerBase ControllerBase)
        {
            return JsonResult(new ApiResponse<object>(true));
        }

        public static IActionResult OkResult(this ControllerBase ControllerBase, object result)
        {
            return JsonResult(new ApiResponse<object>(result));
        }

        public static IActionResult Result(this ControllerBase ControllerBase, ServiceResult serviceResult)
        {
            return JsonResult(serviceResult.Value, serviceResult.HttpCode);
        }

        private static IActionResult JsonResult(object result, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiJsonResult(result, statusCode);
        }

        private static IActionResult JsonResult(object result, HttpCode statusCode)
        {
            return new ApiJsonResult(result, (HttpStatusCode)statusCode);
        }
    }
}
