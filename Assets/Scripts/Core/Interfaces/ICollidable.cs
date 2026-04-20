namespace Game.Core
{
    public interface ICollidable
    {
        PhysicsBody Body { get; }
        float CollisionRadius { get; }
        void OnCollision(ICollidable other);
    }
}