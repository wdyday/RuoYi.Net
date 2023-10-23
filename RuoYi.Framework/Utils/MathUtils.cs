namespace RuoYi.Framework.Utils
{
    public static class MathUtils
    {
        public static double Round(double value, int decimals)
        {
            return Math.Round(value, decimals,  MidpointRounding.AwayFromZero);
        }

        public static decimal Round(decimal value, int decimals)
        {
            return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
        }
    }
}
