
public abstract class BearState : IBearState
{
    protected BearController _bear;

    public BearState(BearController bear)
    {
        this._bear = bear;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
