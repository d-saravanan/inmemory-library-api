using LibMS.Models;
using LibMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LibMS.Controllers
{
    [RoutePrefix("api/media"), EnableCors("*", "*", "*")]
    public class MediaController : ApiController
    {
        private readonly MediaServices mediaServices;
        public MediaController()
        {
            mediaServices = new MediaServices();
        }

        public HttpResponseMessage Get()
        {
            try
            {
                var mediaData = mediaServices.GetAllMedia();

                HttpResponseMessage response = TransformDataToHttpResponse(mediaData);
                return response;
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        private HttpResponseMessage TransformDataToHttpResponse(IEnumerable<Media> mediaData)
        {
            var content = new ObjectContent<IEnumerable<Media>>(mediaData, new JsonMediaTypeFormatter());
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = content;
            return response;
        }

        [Route("ByCategory")]
        public HttpResponseMessage GetMediaByCategory(string catName)
        {
            try
            {
                var mediaData = mediaServices.GetMediaByCategoryName(catName);
                var response = TransformDataToHttpResponse(mediaData);
                return response;
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        public HttpResponseMessage GetById(Guid id)
        {
            try
            {
                var mediaData = mediaServices.GetById(id);

                var content = new ObjectContent<Media>(mediaData, new JsonMediaTypeFormatter());
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = content;
                return response;
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }
    }
}
