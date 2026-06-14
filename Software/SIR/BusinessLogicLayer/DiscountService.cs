namespace BusinessLogicLayer
{
    public class DiscountService
    {
        public double CalculatePrice(double originalPrice)
        {
            if (originalPrice < 0)
            {
                throw new System.ArgumentException("Price cannot be negative.");
            }

            double discount = 0.1; 

            if (originalPrice > 500)
            {
                discount = 0.2;
            }

            return originalPrice * (1 - discount);
        }
    }
}
