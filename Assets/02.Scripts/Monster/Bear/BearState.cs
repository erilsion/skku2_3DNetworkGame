using Unity.VisualScripting;

public abstract class BearState : IBearState
{
    protected BearController bear;

    public BearState(BearController bear)
    {
        this.bear = bear;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
