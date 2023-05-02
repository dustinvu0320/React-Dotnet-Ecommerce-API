using Ecommerce_API.Data;
using Ecommerce_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Ecommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly ApplicationDbContext _db;
        public ShoppingCartController(ApplicationDbContext db)
        {
            _response = new();
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetShoppingCart(string userId)
        {
            try
            {
                ShoppingCart shoppingCart;
                if (string.IsNullOrEmpty(userId))
                {
                    shoppingCart = new();
                }
                else
                {
                    // Get shoppingCart that contains all MenuItems from Cart
                    shoppingCart = _db.ShoppingCarts
                        .Include(u => u.CartItems).ThenInclude(u => u.MenuItem)
                        .FirstOrDefault(u => u.UserId == userId);
                }

                // Calculate CartTotal
                if (shoppingCart.CartItems != null && shoppingCart.CartItems.Count > 0)
                {
                    shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);
                }

                _response.Result = shoppingCart;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
                _response.StatusCode = HttpStatusCode.BadRequest;
            }

            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateItemInCart(string userId, int menuItemId, int updateQuantityBy)
        {
            // Shopping cart will have one entry per user id, even if a user has many items in cart.
            // Cart items will have all the items in shopping cart for a user
            // updatequantityby will have count by with an items quantity needs to be updated
            // if it is -1 that means we have lower a count if it is 5 it means we have to add 5 count to existing count.
            // if updatequantityby by is 0, item will be removed

            // * Scenario might happens
            // 1- when a user adds a new item to a new shopping cart for the first time
            // 2- when a user adds a new item to an existing shopping cart (basically user has other items in cart)
            // 3- when a user updates an existing item count
            // 4- when a user removes an existing item

            // Retrieve shoppingCart based on userId & menuItem based on menuItemId
            ShoppingCart shoppingCart = _db.ShoppingCarts.Include(u => u.CartItems).FirstOrDefault(u => u.UserId == userId);
            MenuItem menuItem = _db.MenuItems.FirstOrDefault(u => u.Id == menuItemId);

            if (menuItem == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            // 1st scenario - no shopping cart exists
            if (shoppingCart == null && updateQuantityBy > 0)
            {
                // Create a new shoppingCart
                ShoppingCart newCart = new() { UserId = userId };
                _db.ShoppingCarts.Add(newCart);
                _db.SaveChanges();

                // Create a new cartItem
                CartItem newCartItem = new()
                {
                    MenuItemId = menuItemId,
                    Quantity = updateQuantityBy,
                    ShoppingCartId = newCart.Id,
                    MenuItem = null
                };
                _db.CartItems.Add(newCartItem);
                _db.SaveChanges();
            }

            // ShoppingCart exists
            else
            {
                CartItem cartItemInCart = shoppingCart.CartItems.FirstOrDefault(u => u.MenuItemId == menuItemId);

                // 2nd scenario - no cart item exists
                if (cartItemInCart == null)
                {
                    // Create a new cartItem
                    CartItem newCartItem = new()
                    {
                        MenuItemId = menuItemId,
                        Quantity = updateQuantityBy,
                        ShoppingCartId = shoppingCart.Id,
                        MenuItem = null
                    };
                    _db.CartItems.Add(newCartItem);
                    _db.SaveChanges();
                }

                // cart item already exist in shoppingCart
                else
                {
                    int newQuantity = cartItemInCart.Quantity + updateQuantityBy;

                    // 4th scenario - remove item from cart
                    if (updateQuantityBy == 0 || newQuantity <= 0)
                    {
                        _db.CartItems.Remove(cartItemInCart);

                        if (shoppingCart.CartItems.Count == 1)
                        {
                            _db.ShoppingCarts.Remove(shoppingCart);
                        }
                        _db.SaveChanges();
                    }
                    
                    // 3rd scenario - update quantity
                    else
                    {
                        cartItemInCart.Quantity = newQuantity;
                        _db.SaveChanges();
                    }
                }
            }

            return _response;
        }
    }
}
