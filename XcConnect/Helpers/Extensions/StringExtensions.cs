namespace XcConnect.Helpers.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Cast String To Int
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int ToInt(this string Value, int DefaultValue = 0)
        {
            int NewValue = DefaultValue;

            if (!string.IsNullOrEmpty(Value))
            {   
                int.TryParse(Value, out NewValue);
            }

            return NewValue;
        }

        /// <summary>
        /// Cast String To Long
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static long ToLong(this string Value, long DefaultValue = 0)
        {
            long NewValue = DefaultValue;

            if (!string.IsNullOrEmpty(Value))
            {
                long.TryParse(Value, out NewValue);
            }

            return NewValue;
        }
    }
}