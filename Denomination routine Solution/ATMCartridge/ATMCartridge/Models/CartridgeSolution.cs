namespace ATMCartridge.Models
{
    internal class CartridgeSolution
    {
        public List<CartridgeFrequency> PossibleCombination { get; set; }
        public CartridgeSolution()
        {
            PossibleCombination = new List<CartridgeFrequency>();
        }
    }

}
