namespace ATG.Activator
{
    public abstract class ActivateObject: IActivateable
    {
        public ActiveStatus Status { get; set; }
        
        public virtual void SetActiveTotal(bool isActive)
        {
            SetActiveVisual(isActive);
            SetActiveLogical(isActive);
        }

        public virtual void SetActiveVisual(bool isActive)
        {
            this.SetFlag(ActiveStatus.VISUAL_ACTIVE, isActive);
        }

        public virtual void SetActiveLogical(bool isActive)
        {
            this.SetFlag(ActiveStatus.LOGIC_ACTIVE, isActive);
        }
    }
}