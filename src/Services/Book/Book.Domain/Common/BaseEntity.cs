namespace Book.Domain.Common;

public abstract class BaseEntity<TId>
{
    TId _id;

    public virtual TId Id
    {
        get { return _id; }
        protected set { _id = value; }
    }

    public override bool Equals(object? obj)
    {
        var compareTo = obj as BaseEntity<TId>;

        if (ReferenceEquals(this, compareTo))
        {
            return true;
        }

        if (compareTo is null)
        {
            return false;
        }

        return Id.Equals(compareTo.Id);
    }
}
