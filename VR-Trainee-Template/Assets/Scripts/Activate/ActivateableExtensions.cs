namespace ATG.Activator
{
    public static class ActivateableExtensions
    {
        public static void SetFlag(this IActivateable activateable, ActiveStatus flag, bool enable)
        {
            if (enable)
                activateable.Status |= flag;      // включаем флаг
            else
                activateable.Status &= ~flag;     // выключаем флаг
        }
        
        public static bool IsActiveTotal(this IActivateable activateable)
        {
            return activateable.Status.HasFlag(ActiveStatus.ACTIVE);
        }

        public static bool IsDisactiveTotal(this IActivateable activateable)
        {
            return activateable.Status.HasFlag(ActiveStatus.DISACTIVE);
        }

        public static bool IsVisibleActive(this IActivateable activateable)
        {
            return activateable.Status.HasFlag(ActiveStatus.VISUAL_ACTIVE);
        }

        public static bool IsLogicalActive(this IActivateable activateable)
        {
            return activateable.Status.HasFlag(ActiveStatus.LOGIC_ACTIVE);
        }
    }
}