using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using Exam_2018.GameEntities;

namespace Exam_2018.LevelCreation {
    public class Level : IGameEventProcessor<object> {
        private GameEventBus<object> spaceTaxibus;
        private EntityContainer portals;
        private EntityContainer<Obstacle> obstacles;
        private Dictionary<char, Platform> platformDictionary;
        private List<Customer> allCustomersInLevel;
        
        private EntityContainer<Customer> activeCustomersInlevel;
        private TimedEventContainer customerSpawnEvents;
        public TimedEventContainer CustomerDespawnEvents;
        public Customer CurrentlyCarriedCustomer;
        
        private Player player;
        public int PlayerScore;

        private Vec2F playerStartingPosition;
        public string Name;

        private LevelManager levelManager;
        
        private Text scoreText;
        private Text customerDestination;

        private Entity background;

        public Level(
            string name,
            EntityContainer portals,
            EntityContainer<Obstacle> obstacles,
            Dictionary<char, Platform> platformDictionary,
            List<Customer> allCustomersInLevel,
            Player player,
            Vec2F playerStartingPosition
            ) {
            
            InitializeLevel();
            
            Name = name;
            this.portals = portals;
            this.obstacles = obstacles;
            this.platformDictionary = platformDictionary;
            
            this.allCustomersInLevel = allCustomersInLevel;
            activeCustomersInlevel = new EntityContainer<Customer>(allCustomersInLevel.Count);

            customerSpawnEvents = new TimedEventContainer(allCustomersInLevel.Count);
            customerSpawnEvents.AttachEventBus(spaceTaxibus);
            GenerateCustomerSpawnEvents();
            CustomerDespawnEvents = new TimedEventContainer(allCustomersInLevel.Count);
            CustomerDespawnEvents.AttachEventBus(spaceTaxibus);
            CurrentlyCarriedCustomer = null;

            this.player = player;
            this.playerStartingPosition = playerStartingPosition;

        }
            
        /// <summary>
        /// Initialize level properties
        /// </summary>
        public void InitializeLevel() {
            spaceTaxibus = SpaceTaxiBus.GetBus();
            spaceTaxibus.Subscribe(GameEventType.TimedEvent, this);
            levelManager = LevelManager.GetInstance();
            
            //setting text sizes
            scoreText = new Text("", new Vec2F(0.43f, -0.26f),
                new Vec2F(Constants.EXTENT_X * 17, Constants.EXTENT_Y * 14));
            customerDestination = new Text("", new Vec2F(0.37f, -0.30f),
                new Vec2F(Constants.EXTENT_X * 17, Constants.EXTENT_Y * 14));
            scoreText.SetFontSize(23);
            scoreText.SetColor(new Vec3I(255, 255, 255));
            customerDestination.SetFontSize(23);
            customerDestination.SetColor(new Vec3I(255, 255, 255));
            
            background = new Entity(
                new StationaryShape(0.0f, 0.0f, 1f, 1f),
                ImageContainer.GetInstance().GetImageByName("SpaceBackground.png"));
            PlayerScore = 0;
        }

        /// <summary>
        /// Render our game objects
        /// </summary>
        public void Render() {
            background.RenderEntity();
            obstacles.RenderEntities();
            foreach (var platformEntry in platformDictionary) {
                platformEntry.Value.Render();
            }
            activeCustomersInlevel.Iterate(customer => customer.RenderCustomer());
            scoreText.RenderText();
            customerDestination.RenderText();
            player.RenderPlayer();
        }

        /// <summary>
        /// Updates level object logic
        /// </summary>
        public void Update() {
            Collision();
            player.Update();
            activeCustomersInlevel.Iterate(customer => {
                var platform = platformDictionary[customer.PlatformId].GetBoundingBox();
                customer.Update(platform);
            });
            UpdateCustomerPlatformText();
            customerSpawnEvents.ProcessTimedEvents();
            CustomerDespawnEvents.ProcessTimedEvents();
        }

        /// <summary>
        /// Updates the customer text field containing information about where
        /// the currently carried customer should be taken as well as updating the
        /// player score text field
        /// </summary>
        private void UpdateCustomerPlatformText() {
            scoreText.SetText("Current score: " + PlayerScore);
            
            if (CurrentlyCarriedCustomer == null) {
                customerDestination.SetText("No customer aboard");
                return;
            }
            var id = CurrentlyCarriedCustomer.DestinationPlatformId;
            if (CurrentlyCarriedCustomer.NextLevel) {
                if (id == '^') {
                    customerDestination.SetText("Go to any platform in the next level");
                    return;
                }
                customerDestination.SetText("Go to platform " + id +" in the next level");
                return;
            }
            if (id == '^') {
                customerDestination.SetText("Go to any platform");
                return;
            }
            customerDestination.SetText(
                "Go to Platform " + CurrentlyCarriedCustomer.DestinationPlatformId);
        }

        /// <summary>
        /// Checks for collision for each of the GameEntity types
        /// </summary>
        private void Collision() {
            PortalCollision();
            ObstacleCollision();
            PlatformCollision();
            CustomerCollision();
        }

        /// <summary>
        /// Collision check for colliding with a portal
        /// </summary>
        private void PortalCollision() {
            var playerShape = player.Shape.AsDynamicShape();
            foreach (Entity portal in portals) {
                var collisionData = CollisionDetection.Aabb(playerShape, portal.Shape);
                if (collisionData.Collision) {
                    levelManager.NextLevel();
                    break;
                }
            }
        }

        /// <summary>
        /// Collision check for Obstacle objects
        /// </summary>
        private void ObstacleCollision() {
            var playerShape = player.Shape.AsDynamicShape();
            foreach (Obstacle obstacle in obstacles) {
                var collisionData = CollisionDetection.Aabb(playerShape, obstacle.Shape);
                if (collisionData.Collision) {
                    spaceTaxibus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_OVER", ""));
                }
            }
        }

        /// <summary>
        /// Checks whether a player has collided with a platform and also checks if a player has
        /// succesfully delivered a customer
        /// </summary>
        private void PlatformCollision() {
            var playerShape = player.Shape.AsDynamicShape();
            foreach (Platform platform in platformDictionary.Values) {
                var platformCollision = platform.Collision(playerShape);
                if (!platformCollision.Item1) {
                    continue;
                }
                //if collision with platform, check if speed is too great
                if (platformCollision.Item2) {
                    spaceTaxibus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this, 
                            "CHANGE_STATE", "GAME_OVER", ""));
                }
                //try to deliver customer at platform
                TryDeliverCustomer(platform);
            }
        }

        /// <summary>
        /// Checks collision between rendered customers and the player
        /// </summary>
        private void CustomerCollision() {
            if (CurrentlyCarriedCustomer != null) { //should only carry one customer at once
                return;
            }
            foreach (Customer customer in activeCustomersInlevel) {
                if (!customer.Collision(player.Shape.AsDynamicShape())) {
                    continue;
                }
                CurrentlyCarriedCustomer = customer;
                CustomerDespawnEvents.AddTimedEvent(
                    TimeSpanType.Seconds, customer.TimeToDeliver, "LEVEL", "DESPAWN_CUSTOMER",
                    customer.Name);
            }
        }

        /// <summary>
        /// Checks if CurrentlyCarriedCustomer is supposed to be dropped off at the
        /// platform supplied as the method argument
        /// </summary>
        /// <param name="platform">Platform to test</param>
        private void TryDeliverCustomer(Platform platform) {
            var customer = CurrentlyCarriedCustomer;
            if (customer == null) {
                return;
            }
            if (!customer.IsDestination(platform)) {
                return;
            }
            CustomerDespawnEvents.ResetContainer();
            PlayerScore += customer.Points;
            CurrentlyCarriedCustomer = null;
        }

        /// <summary>
        /// Restarts the level
        /// </summary>
        public void RestartLevel() {
            player.Reset();
            player.Shape.Position = playerStartingPosition;
            
            //reset all fields required for level restart
            CurrentlyCarriedCustomer = null;
            CustomerDespawnEvents.ResetContainer();
            activeCustomersInlevel.ClearContainer();
            customerSpawnEvents.ResetContainer();
            GenerateCustomerSpawnEvents();
            PlayerScore = 0;
        }

        public void PauseLevel() {
            player.Reset();
        }

        /// <summary>
        /// Creates timed events for spawning customers
        /// </summary>
        public void GenerateCustomerSpawnEvents() {
            foreach (Customer customer in allCustomersInLevel) {
                customerSpawnEvents.AddTimedEvent(
                    TimeSpanType.Seconds, 
                    customer.WhenToSpawn, 
                    "LEVEL", 
                    "SPAWN_CUSTOMER", 
                    customer.Name);
            }
        }

        /// <summary>
        /// Processes incoming events. Right now only timed events are processed
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="gameEvent"></param>
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            switch (eventType) {
            case GameEventType.TimedEvent:
                if (gameEvent.Message == "LEVEL") {
                    switch (gameEvent.Parameter1) {
                    case "SPAWN_CUSTOMER": //Finding correct customer in allCustomersInLevel
                        foreach (Customer customer in allCustomersInLevel) {
                            if (customer.Name != gameEvent.Parameter2) {
                                continue;
                            }
                            //get platform that the customer should spawn on
                            var platform = 
                                platformDictionary[customer.PlatformId].GetBoundingBox();
                            var xVal = platform.Position.X + platform.Extent.X / 2;
                            
                            //set customer position to center of platform
                            customer.Shape.Position.X = xVal;
                            customer.Shape.Position.Y = platform.Position.Y + Constants.EXTENT_Y;
                            activeCustomersInlevel.AddStationaryEntity(customer.Copy());
                        }
                        break;
                    case "DESPAWN_CUSTOMER":
                        spaceTaxibus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, 
                                this, 
                                "CHANGE_STATE", 
                                "GAME_OVER", 
                                ""));
                        break;
                    }
                }

                break;
            }
        }
    }
}