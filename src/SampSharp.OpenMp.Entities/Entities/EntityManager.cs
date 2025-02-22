namespace SampSharp.Entities;

internal class EntityManager : IEntityManager
{
    public void Create(EntityId entity, EntityId parent = default)
    {
        throw new NotImplementedException();
    }

    public T AddComponent<T>(EntityId entity, params object[] args) where T : Component
    {
        throw new NotImplementedException();
    }

    public T AddComponent<T>(EntityId entity) where T : Component
    {
        throw new NotImplementedException();
    }

    public void AddComponent<T>(EntityId entity, T component) where T : notnull, Component
    {
        throw new NotImplementedException();
    }

    public void Destroy(Component component)
    {
        throw new NotImplementedException();
    }

    public void Destroy(EntityId entity)
    {
        throw new NotImplementedException();
    }

    public void Destroy<T>(EntityId entity) where T : Component
    {
        throw new NotImplementedException();
    }

    public EntityId[] GetChildren(EntityId entity)
    {
        throw new NotImplementedException();
    }

    public EntityId[] GetRootEntities()
    {
        throw new NotImplementedException();
    }

    public T GetComponent<T>() where T : Component
    {
        throw new NotImplementedException();
    }

    public T GetComponent<T>(EntityId entity) where T : Component
    {
        throw new NotImplementedException();
    }

    public T GetComponentInChildren<T>(EntityId entity) where T : Component
    {
        throw new NotImplementedException();
    }

    public T GetComponentInParent<T>(EntityId entity) where T : Component
    {
        throw new NotImplementedException();
    }

    public T[] GetComponents<T>() where T : Component
    {
        throw new NotImplementedException();
    }

    public T[] GetComponents<T>(EntityId entity) where T : Component
    {
        throw new NotImplementedException();
    }

    public T[] GetComponentsInChildren<T>(EntityId entity) where T : Component
    {
        throw new NotImplementedException();
    }

    public T[] GetComponentsInParent<T>(EntityId entity) where T : Component
    {
        throw new NotImplementedException();
    }

    public EntityId GetParent(EntityId entity)
    {
        throw new NotImplementedException();
    }

    public bool Exists(EntityId entity)
    {
        throw new NotImplementedException();
    }
}