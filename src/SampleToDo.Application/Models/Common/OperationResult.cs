﻿namespace SampleToDo.Application.Models.Common;

public class OperationResult<TResult>
{
    public TResult Result { get; private init; }

    public bool IsSuccess { get; private init; }
    public string ErrorMessage { get; private init; }
    public bool IsException { get; set; }
    public bool IsNotFound { get; private init; }
    public int CustomCode { get; }

    public static OperationResult<TResult> SuccessResult(TResult result)
    {
        return new OperationResult<TResult> { Result = result, IsSuccess = true };
    }

    public static OperationResult<TResult> FailureResult(string message, TResult result = default)
    {
        return new OperationResult<TResult> { Result = result, ErrorMessage = message, IsSuccess = false };
    }

    public static OperationResult<TResult> NotFoundResult(string message)
    {
        return new OperationResult<TResult> { ErrorMessage = message, IsSuccess = false, IsNotFound = true };
    }
}