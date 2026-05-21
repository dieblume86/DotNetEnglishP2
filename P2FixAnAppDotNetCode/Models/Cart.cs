using System.Collections.Generic;
using System.Linq;

namespace P2FixAnAppDotNetCode.Models
{
    /// <summary>
    /// The Cart class
    /// </summary>
    public class Cart : ICart
    {
        /// <summary>
        /// Collection of cartLine, it paired product id with its cartLine
        /// </summary>
        private Dictionary<int, CartLine> _cartLines = new Dictionary<int, CartLine>();

        /// <summary>
        /// Read-only property for dispaly only
        /// </summary>
        public IEnumerable<CartLine> Lines => GetCartLineList();

        /// <summary>
        /// Return the actual cartline list
        /// </summary>
        /// <returns></returns>
        private List<CartLine> GetCartLineList()
        {
            if (_cartLines.Count == 0)
                return new List<CartLine>();

            return _cartLines.Values.ToList();
        }

        /// <summary>
        /// Adds a product in the cart or increment its quantity in the cart if already added
        /// </summary>//
        public void AddItem(Product product, int quantity)
        {
            if (!_cartLines.ContainsKey(product.Id))
            {
                var cartLine = new CartLine
                {
                    Product = product
                };

                _cartLines.Add(product.Id, cartLine);
            }

            if (_cartLines[product.Id].Quantity + quantity < product.Stock)
            {
                _cartLines[product.Id].Quantity += quantity;
            }
            else
            {
                _cartLines[product.Id].Quantity = product.Stock;
            }
        }

        /// <summary>
        /// Removes a product form the cart
        /// </summary>
        public void RemoveLine(Product product) =>
              _cartLines.Remove(product.Id);

        /// <summary>
        /// Get total value of a cart
        /// </summary>
        public double GetTotalValue()
        {
            var cartLines = GetCartLineList();

            if (cartLines.Count == 0)
                return 0.0;

            var totalValue = 0.0;
            foreach (var cartLine in cartLines)
            {
                totalValue += cartLine.Product.Price * cartLine.Quantity;
            }

            return totalValue;
        }

        /// <summary>
        /// Get average value of a cart
        /// </summary>
        public double GetAverageValue()
        {
            var cartLines = GetCartLineList();

            if (cartLines.Count == 0)
                return 0.0;

            var totalQuantity = 0;
            foreach (var cartLine in cartLines)
            {
                totalQuantity += cartLine.Quantity;
            }

            return GetTotalValue() / totalQuantity;
        }

        /// <summary>
        /// Looks after a given product in the cart and returns if it finds it
        /// </summary>
        public Product FindProductInCartLines(int productId)
        {
            return GetCartLineList().FirstOrDefault(x => x.Product.Id == productId)?.Product;
        }

        /// <summary>
        /// Get a specifid cartline by its index
        /// </summary>
        public CartLine GetCartLineByIndex(int index)
        {
            return Lines.ToArray()[index];
        }

        /// <summary>
        /// Clears a the cart of all added products
        /// </summary>
        public void Clear()
        {
            _cartLines.Clear();

            List<CartLine> cartLines = GetCartLineList();
            cartLines.Clear();
        }
    }

    public class CartLine
    {
        public int OrderLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
