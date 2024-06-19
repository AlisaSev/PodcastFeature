using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Organizations;
using Sabio.Models.Domain.Podcasts;
using Sabio.Models.Requests.Podcasts;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using Stripe;
using System;
using System.Linq.Expressions;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/podcasts")]
    [ApiController]
    public class PodcastsApiController : BaseApiController
    {
        private IPodcastsService _service = null;
        private IAuthenticationService<int> _authService = null;

        public PodcastsApiController(IPodcastsService service,
           ILogger<PodcastsApiController> logger,
            IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        #region HttpPost ("Add")
        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(PodcastsAddRequest model)
        {
            ObjectResult result = null;
            int currentId = _authService.GetCurrentUserId();

            try
            {
                int id = _service.Add(model, currentId);

                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());

                result = StatusCode(500, response);
            }

            return result;
        }
        #endregion

        #region HttpPut ("Update")
        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(PodcastsUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            int currentId = _authService.GetCurrentUserId();

            try
            {
                _service.Update(model, currentId);

                response = new SuccessResponse();

            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"SqlException Error: {ex.Message}");
            }

            return StatusCode(code, response);
        }
        #endregion

        #region HttpDelete ("DeleteById")

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
        #endregion

        #region HttpGet ("Paginate")
        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Podcast>>> GetPage(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<Podcast> page = _service.GetPage(pageIndex, pageSize);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Podcast>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);

        }
        #endregion

        #region HttpGet ("CreatedBy")
        [HttpGet("createdby")]
        public ActionResult<ItemResponse<Paged<Podcast>>> GetByCreatedBy(int pageIndex, int pageSize, int createdBy)
        {
            int code = 200;
            BaseResponse response = null;
            int currentId = _authService.GetCurrentUserId();
            try
            {
                Paged<Podcast> page = _service.GetByCreatedBy(pageIndex, pageSize, createdBy);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Podcast>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }
        #endregion

        #region HttpGet ("Search")
        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Podcast>>> SearchPaginated(int pageIndex, int pageSize, string searchTerm)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Podcast> pagedPodcasts = _service.SearchPaginated(pageIndex, pageSize, searchTerm);

                if (pagedPodcasts == null)
                {
                    code = 404;
                    response = new ErrorResponse("No podcasts found");
                }
                else
                {
                    response = new ItemResponse<Paged<Podcast>> { Item = pagedPodcasts };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse("An error occurred");
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }
        #endregion
    }
}