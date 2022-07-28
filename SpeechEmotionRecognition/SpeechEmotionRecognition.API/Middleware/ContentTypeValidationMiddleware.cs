using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpeechEmotionRecognition.API.Middleware
{
    internal class ContentTypeValidationMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var headerString = context.BindingContext.BindingData["Headers"] as string;

            var headers = JsonSerializer.Deserialize<Dictionary<string, string>>(headerString);

            if (Convert.ToInt32(headers["Content-Length"]) <= 0)
            {
                //If no file is attached, return bad request.
                string errorMessage = "No audio file attached.";
                await SendErrorResponse(context, errorMessage, HttpStatusCode.BadRequest).ConfigureAwait(false);
            }
            else if (string.Compare(headers["Content-Type"], "audio/wave", StringComparison.InvariantCultureIgnoreCase) != 0
                && string.Compare(headers["Content-Type"], "application/octet-stream", StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                //If wav file or octect-stream file is not attached, return unsupported media type.
                string errorMessage = "Invalid file. Only .wav file type is supported.";
                await SendErrorResponse(context, errorMessage, HttpStatusCode.UnsupportedMediaType).ConfigureAwait(false);
            }
            else
            {
                await next(context).ConfigureAwait(false);
            }
        }

        private async Task SendErrorResponse(FunctionContext context, string errorMessage, HttpStatusCode statusCode)
        {
            var httpReqData = await context.GetHttpRequestDataAsync();
            if (httpReqData != null)
            {
                HttpResponseData newHttpResponse = httpReqData.CreateResponse();
                await newHttpResponse.WriteAsJsonAsync(new { ErrorStatus = errorMessage }, statusCode);

                // Update invocation result.
                context.GetInvocationResult().Value = newHttpResponse;
            }
        }
    }
}