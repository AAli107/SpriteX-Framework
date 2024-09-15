using OpenTK.Mathematics;
using SpriteX_Framework.FrameworkContents.Structs;

namespace SpriteX_Framework.FrameworkContents.Components
{
    /// <summary>
    /// Can be used on GameObjects to enable physics simulation.
    /// </summary>
    public class PhysicsComponent : Component
    {
        /// <summary>
        /// velocity of parent game object
        /// </summary>
        public Vector2d velocity = new Vector2(0, 0);
        /// <summary>
        /// The mass of the parent game object
        /// </summary>
        public float mass = 10f;
        /// <summary>
        /// movement friction that slows down parent game object the higher the value
        /// </summary>
        public float friction = 0.1f;
        /// <summary>
        /// controls the behavior of the parent game object friction so that it behaves airborne when true
        /// </summary>
        public bool isAirborne = true;
        /// <summary>
        /// controls which axis (X,Y) cannot move
        /// </summary>
        public Constraint2D movementConstraint = new();
        /// <summary>
        /// Controls whether parent game object has gravity or not
        /// </summary>
        public bool gravityEnabled = true;
        /// <summary>
        /// controls the direction of gravity
        /// </summary>
        public Vector2d gravityVector = Vector2.UnitY;
        /// <summary>
        /// controls the strength of gravity affecting the parent game object
        /// </summary>
        public float gravityMultiplier = 1;

        public PhysicsComponent(GameObject parent) : base(parent) { }

        public override void UpdateTick(MainWindow win)
        {
            base.UpdateTick(win);

            if (gravityEnabled) velocity += gravityVector * gravityMultiplier;

            velocity *= 1 / (((isAirborne ? (friction * (1 / mass)) : friction) + 1) >= 1 ? ((isAirborne ? (friction * (1 / mass)) : friction) + 1) : 1);

            if (velocity.X == 0 && velocity.Y == 0) return;

            if (movementConstraint.X) velocity.X = 0;
            if (movementConstraint.Y) velocity.Y = 0;

            parent.SetPosition(parent.GetPosition() + new Vector2d(movementConstraint.X ? 0 : velocity.X, movementConstraint.Y ? 0 : velocity.Y));
        }

        /// <summary>
        /// Will override the current Velocity
        /// </summary>
        /// <param name="velocity"></param>
        public void OverrideVelocity(Vector2d velocity)
        {
            if (isEnabled)
                this.velocity = velocity;
        }
        /// <summary>
        /// Adds velocity into current Velocity
        /// </summary>
        /// <param name="velocity"></param>
        public void AddVelocity(Vector2d velocity)
        {
            if (isEnabled)
                this.velocity += velocity;
        }
    }
}
