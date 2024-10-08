﻿using Microsoft.AspNetCore.Http;

namespace MsfServer.Domain.Shared.Responses
{
    public class ResponseObject<T>
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public ResponseObject() { }

        public ResponseObject(int status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
        public ResponseObject(string message, T data)
        {
            Status = StatusCodes.Status200OK;
            Message = message;
            Data = data;
        }

        public ResponseObject<T> ResponseSuccess(string message, T data)
        {
            return new ResponseObject<T>(StatusCodes.Status200OK, message, data);
        }

        public static ResponseObject<T> CreateResponse(string message, T data)
        {
            return new ResponseObject<T>(message, data);
        }
    }
}
