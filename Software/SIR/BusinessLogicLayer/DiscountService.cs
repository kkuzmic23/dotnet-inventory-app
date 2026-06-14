namespace BusinessLogicLayer
{
    public class DiscountService
    {
        public double CalculatePrice(double originalPrice)
        {
            return originalPrice * 0.9; 
        }
    }
}
