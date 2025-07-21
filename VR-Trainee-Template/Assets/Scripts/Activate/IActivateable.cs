using System;

namespace ATG.Activator
{
    [Flags]
    public enum ActiveStatus: byte
    {
        DISACTIVE = 0,
        VISUAL_ACTIVE = 1 << 0,
        LOGIC_ACTIVE = 1 << 1,
        ACTIVE = VISUAL_ACTIVE | LOGIC_ACTIVE
    }
    
    public interface IActivateable
    {
        ActiveStatus Status { get; set; }
        void SetActiveTotal(bool isActive);
        void SetActiveVisual(bool isActive);
        void SetActiveLogical(bool isActive);
    }
}