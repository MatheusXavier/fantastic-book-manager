namespace Book.Domain.Common;

public abstract class BaseEntity<TId>
{
    TId _id;

    public virtual TId Id
    {
        get { return _id; }
        protected set { _id = value; }
    }
}
