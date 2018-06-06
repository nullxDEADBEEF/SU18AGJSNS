using System;
using System.Drawing;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;

namespace exam_2018.GameEntities {
    public class Platform {
        public char Id;
        private Text text;
        private EntityContainer platEntity = new EntityContainer();   
        //Simplified bounding box representing the total volume of the entities within platEntity.
        private StationaryShape boundingBox = new StationaryShape(new Vec2F(), new Vec2F());

        public Platform(char id) {
            Id = id;
            text = new Text(
                id.ToString(),
                new Vec2F(
                    boundingBox.Position.X + boundingBox.Extent.X / 2 - Constants.EXTENT_X / 2, 
                    boundingBox.Position.Y),
                new Vec2F(Constants.EXTENT_X, Constants.EXTENT_Y));
            text.SetFontSize(400);
            text.SetColor(Color.Black);
        }

        /// <summary>
        /// Add a new entity to the instance's platEntity EntityContainer. Also updates the
        /// boundingBox shape if the position and dimensions of the new shape exceed the ones of
        /// boundingBox. Also re-centers the platform's text label to the new center of the
        /// bounding box
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="image"></param>
        public void AddPlatform(StationaryShape shape, IBaseImage image) {
            if (platEntity.CountEntities() < 1) {
                boundingBox.Position = shape.Position.Copy();
                boundingBox.Extent = shape.Extent.Copy();
            }
            
            platEntity.AddStationaryEntity(shape, image);
            if (shape.Position.X < boundingBox.Position.X) {
                boundingBox.Position.X = shape.Position.X;
            }

            if (shape.Position.Y < boundingBox.Position.Y) {
                    boundingBox.Position.Y = boundingBox.Position.Y;
            }

            if (shape.Position.X + shape.Extent.X >
                boundingBox.Position.X + boundingBox.Extent.X) {
                boundingBox.Extent.X =
                    (shape.Position.X + shape.Extent.X) - boundingBox.Position.X;
            }

            if (shape.Position.Y + shape.Extent.Y >
                boundingBox.Position.Y + boundingBox.Extent.Y) {
                boundingBox.Extent.Y =
                    (shape.Position.Y + shape.Extent.Y) - boundingBox.Position.Y;
            }

            text.GetShape().Position = new Vec2F(
                boundingBox.Position.X + boundingBox.Extent.X / 2 - Constants.EXTENT_X / 2,
                boundingBox.Position.Y);
        }

        public void Render() {
            platEntity.RenderEntities();
            text.RenderText();
        }

        public Tuple<bool, bool> Collision(DynamicShape shape) {
            var collisionData = CollisionDetection.Aabb(shape, boundingBox);
            if (!collisionData.Collision) {
                return Tuple.Create(false, false);
            }

            if (Math.Abs(shape.Direction.Y) > Constants.CRASHSPEED) {
                return Tuple.Create(true, true);
            }

            shape.Direction = collisionData.DirectionFactor * shape.Direction;
            shape.Direction.X = 0;

            return Tuple.Create(true, false);
        }

        /// <summary>
        /// Returns the bounding box for the platform instance
        /// </summary>
        /// <returns></returns>
        public StationaryShape GetBoundingBox() {
            
            return new StationaryShape(boundingBox.Position.Copy(), boundingBox.Extent.Copy());
            
        }
    }
    
    
}