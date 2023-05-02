using Ecommerce_API.Data;
using Ecommerce_API.Models;
using Ecommerce_API.Models.Dto;
using Ecommerce_API.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Ecommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOrders(string? userId)
        {
            try
            {
                var orderHeaders = _db.OrderHeaders.Include(u => u.OrderDetails)
                   .ThenInclude(u => u.MenuItem)
                   .OrderByDescending(u => u.OrderHeaderId);

                // Get orders match with userId (customers)
                if (!string.IsNullOrEmpty(userId))
                {
                    _response.Result = orderHeaders.Where(u => u.ApplicationUserId == userId);
                }
                // Get all orders (admin)
                else
                {
                    _response.Result = orderHeaders;
                }

                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetOrder(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var orderHeaders = _db.OrderHeaders.Include(u => u.OrderDetails)
                    .ThenInclude(u => u.MenuItem)
                    .Where(u => u.OrderHeaderId == id);

                if (orderHeaders == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = orderHeaders;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDTO orderHeaderDTO)
        {
            try
            {
                // Convert OrderHeaderDTO to OrderHeader Model
                OrderHeader order = new()
                {
                    ApplicationUserId = orderHeaderDTO.ApplicationUserId,
                    PickupEmail = orderHeaderDTO.PickupEmail,
                    PickupName = orderHeaderDTO.PickupName,
                    PickupPhoneNumber = orderHeaderDTO.PickupPhoneNumber,
                    OrderTotal = orderHeaderDTO.OrderTotal,
                    OrderDate = DateTime.Now,
                    StripePaymentIntentID = orderHeaderDTO.StripePaymentIntentID,
                    TotalItems = orderHeaderDTO.TotalItems,
                    Status = String.IsNullOrEmpty(orderHeaderDTO.Status) ? SD.status_pending : orderHeaderDTO.Status,
                };

                if (ModelState.IsValid)
                {
                    _db.OrderHeaders.Add(order);
                    _db.SaveChanges();

                    // Loop each single orderDetailDTO
                    foreach (var orderDetailDTO in orderHeaderDTO.OrderDetailsDTO)
                    {
                        // Create new OrderDetail to add to Db
                        OrderDetails orderDetails = new()
                        {
                            OrderHeaderId = order.OrderHeaderId,
                            ItemName = orderDetailDTO.ItemName,
                            MenuItemId = orderDetailDTO.MenuItemId,
                            Price = orderDetailDTO.Price,
                            Quantity = orderDetailDTO.Quantity,
                        };

                        _db.OrderDetails.Add(orderDetails);
                    }
                    _db.SaveChanges();

                    _response.Result = order;
                    order.OrderDetails = null;
                    _response.StatusCode = HttpStatusCode.Created;
                    return Ok(_response);
                }
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateOrderHeader(int id, [FromBody] OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {
            try
            {
                if (orderHeaderUpdateDTO == null || id != orderHeaderUpdateDTO.OrderHeaderId)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }

                OrderHeader orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.OrderHeaderId == id);

                if (orderFromDb == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }

                // Check what values need to be updated
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickupName))
                {
                    orderFromDb.PickupName = orderHeaderUpdateDTO.PickupName;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickupPhoneNumber))
                {
                    orderFromDb.PickupPhoneNumber = orderHeaderUpdateDTO.PickupPhoneNumber;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickupEmail))
                {
                    orderFromDb.PickupEmail = orderHeaderUpdateDTO.PickupEmail;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.Status))
                {
                    orderFromDb.Status = orderHeaderUpdateDTO.Status;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.StripePaymentIntentID))
                {
                    orderFromDb.StripePaymentIntentID = orderHeaderUpdateDTO.StripePaymentIntentID;
                }

                _db.SaveChanges();

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}
